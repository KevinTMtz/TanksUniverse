using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShotForce : MonoBehaviour
{
    private float force;

    private ShotForceBar shotForceBar;

    private float forceDelta;
    
    void Start()
    {
        force = 50;
        forceDelta = 0.5f;

        shotForceBar = GameObject.Find("ShotForceBar").GetComponent<ShotForceBar>();
        shotForceBar.SetForce(50);
    }

    public void IncreaseForce()
    {
        force = Math.Min(100, force + forceDelta);
        shotForceBar.SetForce(force);
    }

    public void DecreaseForce()
    {
        force = Math.Max(0, force - forceDelta);
        shotForceBar.SetForce(force);
    }

    public float Force
    {
        get { return force; }
    }
}
