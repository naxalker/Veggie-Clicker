using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OfflineEarningsPopup : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private TMP_Text _earningsLabel;
    [SerializeField] private Button _claimButton;

    public void Configure(string earningsText)
    {
        _earningsLabel.text = earningsText;
    }

    public Button GetClaimButton() => _claimButton;
}
