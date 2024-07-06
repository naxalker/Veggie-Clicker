using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(DailyRewardsUI))]
public class DailyRewardsManager : MonoBehaviour
{
    private const string RewardIndexKey = "RewardIndex";
    private const string LastClaimKey = "LastClaimDateTime";

    [Header("Elements")]
    [SerializeField] private DailyRewardContainer[] _dailyRewardContainers;
    [SerializeField] private Button _claimButton;
    private DailyRewardsUI _dailyRewardsUI;

    [Header("Data")]
    [SerializeField] private DailyReward[] _dailyRewardsData;
    private int _rewardIndex;
    private DateTime _lastClaimDateTime;

    private void Awake()
    {
        _dailyRewardsUI = GetComponent<DailyRewardsUI>();

        LoadData();

        if (!CheckIfAllRewardsHaveBeenClaimed())
            _dailyRewardsUI.OpenPanel();

        _claimButton.onClick.AddListener(ClaimButtonHandler);
    }

    private void Start()
    {
        ConfigureRewards();
    }

    private void ConfigureRewards()
    {
        for (int i = 0; i < _dailyRewardContainers.Length; i++)
        {
            Sprite icon = _dailyRewardsData[i].Icon;
            string amount;

            if (_dailyRewardsData[i].RewardType == DailyRewardType.Carrots)
                amount = DoubleUtilities.ToIdleNotation(_dailyRewardsData[i].Amount);
            else
                amount = _dailyRewardsData[i].Amount.ToString();

            string day = "Day " + (i + 1);

            bool claimed = _rewardIndex > i;

            _dailyRewardContainers[i].Configure(icon, amount, day, claimed);
        }
    }

    private void ClaimButtonHandler()
    {
        DailyReward dailyReward = _dailyRewardsData[_rewardIndex];

        RewardPlayer(dailyReward);

        _rewardIndex++;
        SaveData();

        UpdateRewardContainers();

        if (!CheckIfAllRewardsHaveBeenClaimed())
            ResetTimer();

        _dailyRewardsUI.ClosePanel();
    }

    private bool CheckIfAllRewardsHaveBeenClaimed()
    {
        if (_rewardIndex > 6)
        {
            _dailyRewardsUI.AllRewardsClaimed();
        }

        return _rewardIndex > 6;
    }

    private void ResetTimer()
    {
        _dailyRewardsUI.ResetTimer();
    }

    private void UpdateRewardContainers()
    {
        for (int i = 0; i < _dailyRewardContainers.Length; i++)
        {
            if (_rewardIndex > i)
                _dailyRewardContainers[i].Claim();
        }
    }

    private void RewardPlayer(DailyReward dailyReward)
    {
        switch (dailyReward.RewardType)
        {
            case DailyRewardType.Carrots:
                RewardCarrots(dailyReward.Amount);
                break;

            case DailyRewardType.Upgrade:
                RewardUpgrade(dailyReward.UpgradeIndex, dailyReward.Amount);
                break;

            default:
                break;
        }
    }

    private void RewardCarrots(double amount)
    {
        CarrotManager.Instance.AddCarrots(amount);
    }

    private void RewardUpgrade(int upgradeIndex, double amount)
    {
        ShopManager.Instance.RewardUpgrade(upgradeIndex, (int)amount);
    }

    private void CheckIfCanClaim()
    {
        TimeSpan timeSpan = DateTime.Now.Subtract(_lastClaimDateTime);
        double elapsedHours = timeSpan.TotalHours;

        if (elapsedHours < 24)
        {
            int seconds = 60 * 60 * 24 - (int)timeSpan.TotalSeconds;
            _dailyRewardsUI.InitializeTimer(seconds);
        }
    }

    private void LoadData()
    {
        _rewardIndex = PlayerPrefs.GetInt(RewardIndexKey);

        if (LoadLastClaimDateTime())
            CheckIfCanClaim();
    }

    private void SaveData()
    {
        PlayerPrefs.SetInt(RewardIndexKey, _rewardIndex);

        SaveLastDateTime();
    }

    private bool LoadLastClaimDateTime()
    {
        bool validDateTime = DateTime.TryParse(PlayerPrefs.GetString(LastClaimKey), out _lastClaimDateTime);

        return validDateTime;
    }

    private void SaveLastDateTime()
    {
        DateTime now = DateTime.Now;

        PlayerPrefs.SetString(LastClaimKey, now.ToString());
    }
}
