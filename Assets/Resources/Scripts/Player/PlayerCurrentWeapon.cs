using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCurrentWeapon : MonoBehaviour
{
    private int currentWeapon;

    private PlayerTankInfoUI playerTankInfoUI;

    void Start()
    {
        currentWeapon = 1;

        playerTankInfoUI = gameObject.GetComponent<PlayerTankInfoUI>();
        UpdateUI();
    }

    public void SetCurrentWeapon(int weaponNum)
    {
        currentWeapon = weaponNum;
        UpdateUI();
    }

    void UpdateUI()
    {
        playerTankInfoUI.SetCurrentWeaponText(currentWeapon);
    }

    public int CurrentWeapon
    {
        get { return currentWeapon; }
    }
}
