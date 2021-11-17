using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTankInfoUI : MonoBehaviour
{
    public Text canonRotAXText;
    
    public void SetCanonRotAXText(float canonRotAX)
    {
        canonRotAXText.text = $"Canon Rot-X: {(int) canonRotAX * -1}";
    }

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
}
