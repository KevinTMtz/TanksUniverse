using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour 
{   
    public int health;
    private bool isKill;

    [HideInInspector]
    public bool invinsible;

    private PlayerTankInfoUI playerTankInfoUI;
    
    void Start()
    {
        health = 100;
        isKill = false;

        playerTankInfoUI = gameObject.GetComponent<PlayerTankInfoUI>();
        playerTankInfoUI.SetMaxHealth(health);
    }

    void Update()
    {   
        if (health == 0 && !isKill)
        {
            isKill = true;
        }
    }

    public void IncreaseHealth(int heal)
    {
        health = Math.Min(100, health + heal);
        UpdateUI();
    }

    public void DecreaseHealth(int damage)
    {
        if (!invinsible)
        {
            health = Math.Max(0, health - damage);
            UpdateUI();
        }
    }

    void UpdateUI()
    {
        playerTankInfoUI.SetHealth(health);
    }

    public bool IsKill
    {
        get { return isKill; }
    }
}
