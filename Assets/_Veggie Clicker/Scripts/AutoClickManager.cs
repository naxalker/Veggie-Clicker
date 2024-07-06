using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoClickManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Transform rotator;
    [SerializeField] private GameObject bunnyPrefab;

    [Header("Settings")]
    [SerializeField] private float rotatorSpeed;
    [SerializeField] private float rotatorRadius;
    private int currentBunnyIndex;

    [Header("Data")]
    [SerializeField] private int level;
    //[SerializeField] private float carrotsPerSecond;

    private void Awake()
    {
        ShopManager.OnUpgradePurchased += CheckIfCanUpgrade;
    }

    private void Start()
    {
        LoadData();

        //carrotsPerSecond = level * .1f;

        //InvokeRepeating("AddCarrots", 1, 1);

        SpawnBunnies();

        StartAnimatingBunnies();
    }

    private void Update()
    {
        rotator.Rotate(Vector3.forward * Time.deltaTime * rotatorSpeed);
    }

    private void OnDestroy()
    {
        ShopManager.OnUpgradePurchased -= CheckIfCanUpgrade;
    }

    private void CheckIfCanUpgrade(int upgradeIndex)
    {
        if (upgradeIndex == 0)
            Upgrade();
    }

    private void SpawnBunnies()
    {
        while (rotator.childCount > 0)
        {
            Transform bunny = rotator.GetChild(0);
            bunny.SetParent(null);
            Destroy(bunny.gameObject);
        }

        int bunnyCount = Mathf.Min(level, 36);

        for (int i = 0; i < bunnyCount; i++)
        {
            float angle = i * 10;

            Vector2 position = new Vector2();
            position.x = rotatorRadius * Mathf.Cos(angle * Mathf.Deg2Rad);
            position.y = rotatorRadius * Mathf.Sin(angle * Mathf.Deg2Rad);

            GameObject bunnyInstance = Instantiate(bunnyPrefab, position, Quaternion.identity, rotator);
            bunnyInstance.transform.up = position.normalized;
        }
    }


    //private void AddCarrots()
    //{
    //    CarrotManager.instance.AddCarrots(carrotsPerSecond);
    //}

    public void Upgrade()
    {
        level++;
        //carrotsPerSecond = level * .1f;

        if (level <= 36)
        {
            SpawnBunnies();
            StartAnimatingBunnies();
        }
    }

    private void StartAnimatingBunnies()
    {
        if (rotator.childCount <= 0)
            return;

        LeanTween.cancel(gameObject);

        for (int i = 0; i < rotator.childCount; i++)
            LeanTween.cancel(rotator.GetChild(i).gameObject);

        LeanTween.moveLocalY(rotator.GetChild(currentBunnyIndex).GetChild(0).gameObject, -0.5f, .25f)
            .setLoopPingPong(1)
            .setOnComplete(AnimateNextBunny);
    }

    private void AnimateNextBunny()
    {
        currentBunnyIndex++;

        if (currentBunnyIndex >= rotator.childCount)
            ResetBunniesAnimation();
        else
            StartAnimatingBunnies();
    }

    private void ResetBunniesAnimation()
    {
        currentBunnyIndex = 0;

        float delay = Math.Max(10 - rotator.childCount, 0);

        LeanTween.delayedCall(gameObject, delay, StartAnimatingBunnies);
    }

    private void LoadData()
    {
        level = ShopManager.Instance.GetUpgradeLevel(0);
    }
}
