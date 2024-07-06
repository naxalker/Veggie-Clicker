using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;

    [Header("Settings")]
    [Tooltip("Value in hertz")]
    [SerializeField] private int _addCarrotsFrequency;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        InvokeRepeating("AddCarrots", 1, 1f / _addCarrotsFrequency);
    }

    private void AddCarrots()
    {
        double totalCarrots = GetCarrotsPerSecond();

        CarrotManager.Instance.AddCarrots(totalCarrots / _addCarrotsFrequency);
    }

    public double GetCarrotsPerSecond()
    {
        UpgradeSO[] upgrades = ShopManager.Instance.GetUpgrades();

        if (upgrades.Length < 1)
            return 0;

        double totalCarrots = 0;

        for (int i = 0; i < upgrades.Length; i++)
        {
            double upgradeCarrots = upgrades[i].cpsPerLevel * ShopManager.Instance.GetUpgradeLevel(i);
            totalCarrots += upgradeCarrots;
        }

        return totalCarrots;
    }
}
