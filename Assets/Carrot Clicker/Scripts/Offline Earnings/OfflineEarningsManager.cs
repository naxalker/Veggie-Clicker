using System;
using UnityEngine;

[RequireComponent(typeof(OfflineEarningsUI))]
public class OfflineEarningsManager : MonoBehaviour
{
    [Header("Elements")]
    private OfflineEarningsUI _offlineEarningsUI;

    [Header("Settings")]
    [SerializeField] private int _maxOfflineSeconds;
    private DateTime _lastDateTime;

    private void Start()
    {
        _offlineEarningsUI = GetComponent<OfflineEarningsUI>();

        if (LoadLastDateTime())
            CalculateOfflineSeconds();
        else
            Debug.LogError("Не удалось получить время окончания фокусировки");
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus)
            return;

        SaveCurrentDateTime();
    }

    private void OnApplicationQuit()
    {
        SaveCurrentDateTime();
    }

    private void CalculateOfflineSeconds()
    {
        TimeSpan timeSpan = DateTime.Now.Subtract(_lastDateTime);
        //Debug.Log("Тебя не было " + timeSpan.TotalSeconds + " секунд.");

        int offlineSeconds = (int)timeSpan.TotalSeconds;
        offlineSeconds = Mathf.Min(offlineSeconds, _maxOfflineSeconds);

        CalculateOfflineEarnings(offlineSeconds);
    }

    private void CalculateOfflineEarnings(int offlineSeconds)
    {
        double offlineEarnings = offlineSeconds * UpgradeManager.Instance.GetCarrotsPerSecond();

        if (offlineEarnings <= 0)
            return;

        Debug.Log("You've earned " + offlineEarnings);

        _offlineEarningsUI.DisplayPopup(offlineEarnings);
    }

    private bool LoadLastDateTime()
    {
        bool validDateTime = DateTime.TryParse(PlayerPrefs.GetString("LastDateTime"), out _lastDateTime);

        return validDateTime;
    }

    private void SaveCurrentDateTime()
    {
        DateTime now = DateTime.Now;

        PlayerPrefs.SetString("LastDateTime", now.ToString());
    }
}
