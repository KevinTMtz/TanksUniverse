using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCurrentWeapon : MonoBehaviour
{
    private int currentWeapon;

    public Text currentGunText;

    public void SetCurrentWeapon(int weaponNum)
    {
        currentWeapon = weaponNum;
        currentGunText.text = $"{currentWeapon}";
    }

    public int CurrentWeapon
    {
        get { return currentWeapon; }
    }
}
