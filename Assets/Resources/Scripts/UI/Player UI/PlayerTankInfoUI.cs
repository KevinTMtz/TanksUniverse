using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTankInfoUI : MonoBehaviour
{
    // Health Bar
    public Slider sliderHealthBar;

    public void SetMaxHealth(int health)
    {
        sliderHealthBar.maxValue = health;
        sliderHealthBar.value = health;
    }
    
    public void SetHealth(int health)
    {
        sliderHealthBar.value = health;
    }

    // Shot Force Bar
    public Slider sliderShotForce;

    public void SetForce(int force)
    {
        sliderShotForce.value = force;
    }

    // Canon Rotation
    public Text canonRotAXText;
    
    public void SetCanonRotAXText(float canonRotAX)
    {
        canonRotAXText.text = $"Canon Rot-X: {(int) canonRotAX * -1}";
    }

    // Current Weapon
    public Text currentGunText;

    public void SetCurrentWeaponText(int currentWeapon)
    {
        currentGunText.text = $"Weapon {currentWeapon}";
    }

    // Tower Rotation
    public Text towerRotationText;

    public void SetTowerRotationText(float towerRotAY)
    {
        towerRotationText.text = $"Tower Rot-Y: {(int) ((towerRotAY > 0 ? 0 : 365) + towerRotAY % 365)}";
    }

    // Tank Position
    public Text tankPositionText;

    public void SetTankPositionText(Vector3 tankPos)
    {
        tankPositionText.text = $"Tank Pos: {tankPos.ToString("F2")}";
    }

    // Tank Rotation
    public Text tankRotationText;

    public void SetTankRotationText(float tankRotAY)
    {
        tankRotationText.text = $"Tower Rot-Y: {(int) ((tankRotAY > 0 ? 0 : 365) + tankRotAY % 365)}";
    }
}
