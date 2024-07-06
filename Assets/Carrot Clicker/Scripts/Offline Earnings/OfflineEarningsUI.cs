using System;
using UnityEngine;

public class OfflineEarningsUI : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private OfflineEarningsPopup _popup;

    public void DisplayPopup(double earnings)
    {
        _popup.Configure(DoubleUtilities.ToIdleNotation(earnings));

        _popup.GetClaimButton().onClick.AddListener(() => ClaimButtonClickedHandler(earnings));
        
        _popup.gameObject.SetActive(true);
    }

    private void ClaimButtonClickedHandler(double earnings)
    {
        Debug.Log("Give the player " + earnings + " carrots!");

        _popup.gameObject.SetActive(false);

        CarrotManager.Instance.AddCarrots(earnings);
    }
}
