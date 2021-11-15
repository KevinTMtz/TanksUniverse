using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour 
{   
    private int health;
    private bool isKill;

    [HideInInspector]
    public bool invinsible;

    private HealthBar healthBar;
    
    void Start()
    {
        health = 100;
        isKill = false;

        healthBar = GameObject.Find("HealthBar").GetComponent<HealthBar>();
        healthBar.SetMaxHealth(health);
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
        healthBar.SetHealth(health);
    }

    public void DecreaseHealth(int damage)
    {
        if (!invinsible)
        {
            health = Math.Max(0, health - damage);
            healthBar.SetHealth(health);
        }
    }

    public bool IsKill
    {
        get { return isKill; }
    }
}
