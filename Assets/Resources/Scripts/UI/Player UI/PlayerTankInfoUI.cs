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

    public void SetTowerRotationText(float towerRotAY, float tankRotAY)
    {
        float totalAngle = towerRotAY + tankRotAY;
        towerRotationText.text = $"Tower Rot-Y: {(int) ((totalAngle > 0 ? 0 : 360) + totalAngle % 360)}";
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
        tankRotationText.text = $"Tank Rot-Y: {(int) ((tankRotAY > 0 ? 0 : 360) + tankRotAY % 360)}";
    }

    // Wind Rotation
    public Text windRotationText;

    public void SetWindRotationText(float windRotAY)
    {
        windRotationText.text = $"Wind Rot-Y: {windRotAY}";
    }

    // Wind Force
    public Text windForceText;

    public void SetWindForceText(float windForce)
    {
        windForceText.text = $"Wind Force: {windForce}";
    }
}
