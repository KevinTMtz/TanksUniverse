using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        // Left Shift + WASD
        if (Input.GetKey("left shift") && 
            (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0))
        {
            Debug.Log("Tank Y Rotation");
        }
        // WASD
        else if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            Debug.Log("WASD Movement");
        }
        // 1, 2, 3, 4, 5
        else if (Input.GetKey("1") || Input.GetKey("2") || Input.GetKey("3") || 
                Input.GetKey("4") || Input.GetKey("5"))
        {
            Debug.Log("Change Weapon");
        }
        // Space
        else if (Input.GetKey("space"))
        {
            Debug.Log("Shoot");
        }
        // Left Shift + (+, -)
        else if (Input.GetKey("left shift") && (Input.GetKey("[+]") || Input.GetKey("[-]")))
        {
            Debug.Log("Vertical Angle");
        }
        // +, -
        else if (Input.GetKey("[+]") || Input.GetKey("[-]"))
        {
            Debug.Log("Initial Velocity Change");
        }
    }
}
