using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DailyRewardContainer : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Image _iconImage;
    [SerializeField] private TMP_Text _amountText;
    [SerializeField] private TMP_Text _dayText;
    [SerializeField] private GameObject _claimElements;

    public void Configure(Sprite icon, string amount, string day, bool claimed)
    {
        _iconImage.sprite = icon;
        _amountText.text = amount;
        _dayText.text = day;

        if (claimed)
            Claim();
    }

    public void Claim()
    {
        _claimElements.SetActive(true);
    }
}
