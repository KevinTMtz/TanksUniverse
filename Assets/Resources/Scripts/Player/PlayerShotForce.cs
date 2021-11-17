using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShotForce : MonoBehaviour
{
    private float force;

    private float forceDelta;
    
    private PlayerTankInfoUI PlayerTankInfoUI;
    public Text shotForceText;
    
    void Start()
    {
        force = 50;
        forceDelta = 0.1f;

        PlayerTankInfoUI = gameObject.GetComponent<PlayerTankInfoUI>();
        
        UpdateUI();
    }

    public void IncreaseForce()
    {
        force = Math.Min(100, force + forceDelta);

        UpdateUI();
    }

    public void DecreaseForce()
    {
        force = Math.Max(0, force - forceDelta);
        
        UpdateUI();
    }

    void UpdateUI()
    {
        PlayerTankInfoUI.SetForce((int) force);
        shotForceText.text = ((int) force).ToString();
    }

    public float Force
    {
        get { return force; }
    }
}
