using System;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;

    public static Action<int> OnUpgradePurchased;

    [Header("Elements")]
    [SerializeField] private GameObject _upgradeButton;
    [SerializeField] private Transform _upgradeButtonParent;

    [Header("Data")]
    [SerializeField] private UpgradeSO[] _upgrades;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        SpawnButtons();
    }

    public void RewardUpgrade(int upgradeIndex, int levels)
    {
        for (int i = 0; i < levels; i++)
        {
            IncreaseUpgradeLevel(upgradeIndex);
        }
    }

    public int GetUpgradeLevel(int upgradeIndex) => PlayerPrefs.GetInt("Upgrade" + upgradeIndex);

    public UpgradeSO[] GetUpgrades() => _upgrades;

    private string GetUpgradePriceString(int upgradeIndex) 
        => DoubleUtilities.ToIdleNotation(GetUpgradePrice(upgradeIndex));

    private double GetUpgradePrice(int upgradeIndex)
        => _upgrades[upgradeIndex].GetPrice(GetUpgradeLevel(upgradeIndex));

    private void SpawnButtons()
    {
        for (int i = 0; i < _upgrades.Length; i++)
        {
            SpawnButton(i);
        }
    }

    private void SpawnButton(int index)
    {
        UpgradeButton upgradeButtonInstance = Instantiate(_upgradeButton, _upgradeButtonParent).GetComponent<UpgradeButton>();

        UpgradeSO upgrade = _upgrades[index];

        int upgradeLevel = PlayerPrefs.GetInt("Upgrade" + index);

        Sprite icon = upgrade.icon;
        string title = upgrade.title;
        string subtitle = string.Format("”р. {0} (+{1} нажатие/сек)", upgradeLevel + 1, upgrade.cpsPerLevel);
        string price = GetUpgradePriceString(index);

        upgradeButtonInstance.Configure(icon, title, subtitle, price);

        upgradeButtonInstance.GetButton().onClick.AddListener(() => UpgradeButtonClickedHandler(index));
    }

    private void UpgradeButtonClickedHandler(int upgradeIndex)
    {
        if (CarrotManager.Instance.TryPurchase(GetUpgradePrice(upgradeIndex)))
            IncreaseUpgradeLevel(upgradeIndex);
    }

    private void IncreaseUpgradeLevel(int upgradeIndex)
    {
        int currentUpgradeLevel = GetUpgradeLevel(upgradeIndex);
        currentUpgradeLevel++;

        SaveUpgradeLevel(upgradeIndex, currentUpgradeLevel);

        UpdateVisuals(upgradeIndex);

        OnUpgradePurchased?.Invoke(upgradeIndex);
    }

    private void UpdateVisuals(int upgradeIndex)
    {
        UpgradeButton upgradeButton = _upgradeButtonParent.GetChild(upgradeIndex).GetComponent<UpgradeButton>();

        UpgradeSO upgrade = _upgrades[upgradeIndex];

        int upgradeLevel = PlayerPrefs.GetInt("Upgrade" + upgradeIndex);

        string subtitle = string.Format("”р. {0} (+{1} нажатие/сек)", upgradeLevel + 1, upgrade.cpsPerLevel);
        string price = GetUpgradePriceString(upgradeIndex);

        upgradeButton.UpdateVisuals(subtitle, price);
    }

    private void SaveUpgradeLevel(int upgradeIndex, int upgradeLevel)
    {
        PlayerPrefs.SetInt("Upgrade" + upgradeIndex, upgradeLevel);
    }
}
