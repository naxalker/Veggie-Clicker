using UnityEngine;
using TMPro;

public class CarrotManager : MonoBehaviour
{
    public static CarrotManager Instance;

    [Header("Elements")]
    [SerializeField] private TextMeshProUGUI _carrotsText;
    [SerializeField] private TextMeshProUGUI _cpsText;

    [Header("Data")]
    [SerializeField] private double _totalCarrotsCount;
    [SerializeField] private int _frenzyModeMultiplier;
    private int _carrotIncrement = 1;
    private double _previousCarrotCount;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        LoadData();

        InputManager.OnCarrotClicked += CarrotClickedCallback;

        Carrot.OnFrenzyModeStarted += FrenzyModeStartedCallback;
        Carrot.OnFrenzyModeStopped += FrenzyModeStoppedCallback;
    }

    private void Start()
    {
        InvokeRepeating("UpdateCpsText", 0, 1f);
    }

    private void OnDestroy()
    {
        InputManager.OnCarrotClicked -= CarrotClickedCallback;

        Carrot.OnFrenzyModeStarted -= FrenzyModeStartedCallback;
        Carrot.OnFrenzyModeStopped -= FrenzyModeStoppedCallback;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.C))
        {
            AddCarrots((_totalCarrotsCount + 1000000) * 2);
        }
    }

    public bool TryPurchase(double price)
    {
        if (price <= _totalCarrotsCount)
        {
            _totalCarrotsCount -= price;
            UpdateCarrotsText();
            return true;
        }
        return false;
    }

    public void AddCarrots(double value)
    {
        _totalCarrotsCount += value;

        UpdateCarrotsText();

        SaveData();
    }

    public void AddCarrots(float value)
    {
        AddCarrots((double)value);
    }

    public int GetCurrentMultiplier()
    {
        return _carrotIncrement;
    }

    private void FrenzyModeStartedCallback()
    {
        _carrotIncrement = _frenzyModeMultiplier;
    }

    private void FrenzyModeStoppedCallback()
    {
        _carrotIncrement = 1;
    }

    private void CarrotClickedCallback()
    {
        _totalCarrotsCount += _carrotIncrement;

        UpdateCarrotsText();

        SaveData();
    }

    private void UpdateCarrotsText()
    {
        _carrotsText.text = DoubleUtilities.ToIdleNotation(_totalCarrotsCount) + " Морковок!";
    }

    private void UpdateCpsText()
    {
        double cps = _totalCarrotsCount - _previousCarrotCount;

        if (cps < 0)
            cps = UpgradeManager.Instance.GetCarrotsPerSecond();

        _cpsText.text = DoubleUtilities.ToIdleNotation(cps) + " м/c";

        _previousCarrotCount = _totalCarrotsCount;
    }

    private void SaveData()
    {
        PlayerPrefs.SetString("Carrots", _totalCarrotsCount.ToString());
    }

    private void LoadData()
    {
        double.TryParse(PlayerPrefs.GetString("Carrots"), out _totalCarrotsCount);

        UpdateCarrotsText();
    }
}
