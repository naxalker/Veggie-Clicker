using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DailyRewardsUI : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private GameObject _panel;
    [SerializeField] private Button _dailyRewardsButton;

    [Header("Timer Elements")]
    [SerializeField] private GameObject _claimButton;
    [SerializeField] private GameObject _timerContainer;
    [SerializeField] private TMP_Text _timerText;

    private int _seconds;

    private void Awake()
    {
        _dailyRewardsButton.onClick.AddListener(OpenPanel);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            _seconds /= 2;
    }

    public void InitializeTimer(int seconds)
    {
        _claimButton.SetActive(false);
        _timerContainer.SetActive(true);

        _seconds = seconds;

        UpdateTimerText();

        InvokeRepeating("UpdateTimer", 0, 1f);
    }

    public void ResetTimer()
    {
        int timerSeconds = 60 * 60 * 24 - 1;
        InitializeTimer(timerSeconds);
    }

    public void OpenPanel()
    {
        _panel.SetActive(true);
    }

    public void ClosePanel()
    {
        _panel.SetActive(false);
    }

    public void AllRewardsClaimed()
    {
        _claimButton.SetActive(false);
        _timerContainer.SetActive(false);
    }

    private void UpdateTimer()
    {
        _seconds--;
        UpdateTimerText();

        if (_seconds <= 0)
            StopTimer();
    }

    private void StopTimer()
    {
        CancelInvoke("UpdateTimer");

        _claimButton.SetActive(true);
        _timerContainer.SetActive(false);
    }

    private void UpdateTimerText()
    {
        _timerText.text = TimeSpan.FromSeconds(_seconds).ToString();
    }
}
