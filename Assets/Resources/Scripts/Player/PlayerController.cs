using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject Body;
    public GameObject Tower;
    public GameObject Canon;
    public GameObject RFrontWheel;
    public GameObject LFrontWheel;
    public GameObject RBackWheel;
    public GameObject LBackWheel;

    List<Vector3[]> originals;

    float tankRotAY;

    float towerRotAY;
    float canonRotAX;

    float lWheelRotAX;
    float rWheelRotAX;

    enum TankParts
    {
        Body,
        Tower,
        Canon,
        RFrontWheel,
        LFrontWheel,
        RBackWheel,
        LBackWheel
    }

    void Start()
    {
        originals = new List<Vector3[]>();

        MeshUtils.ExtractVertices(Body, originals);
        MeshUtils.ExtractVertices(Tower, originals);
        MeshUtils.ExtractVertices(Canon, originals);
        MeshUtils.ExtractVertices(RFrontWheel, originals);
        MeshUtils.ExtractVertices(LFrontWheel, originals);
        MeshUtils.ExtractVertices(RBackWheel, originals);
        MeshUtils.ExtractVertices(LBackWheel, originals);

        tankRotAY = 0;
        towerRotAY = 0;
        canonRotAX = 0;
        lWheelRotAX = 0;
        rWheelRotAX = 0;
    }

    void Update()
    {
        // Rotate canon
        if (Input.GetAxis("Vertical") != 0) {
            if (canonRotAX >= 30) canonRotAX = 30;
            if (canonRotAX <= -35) canonRotAX = -35;

            canonRotAX -= Input.GetAxis("Vertical") * 0.5f;
        }

        // Rotate tower
        if (Input.GetKey("q") || Input.GetKey("e")) {
            towerRotAY += (Input.GetKey("q") ? -1 : 1) * 0.5f;
        }

        if (Input.GetKey("a") || Input.GetKey("d")) {
            // Move tank forward
            if (Input.GetKey("a") && Input.GetKey("d"))
            {
                lWheelRotAX += 0.75f;
                rWheelRotAX += 0.75f;
            }
            // Rotate tank left
            else if (Input.GetKey("a"))
            {
                lWheelRotAX -= 0.75f;
                rWheelRotAX += 0.75f;

                tankRotAY -= 0.5f;
            }
            // Rotate tank right
            else if (Input.GetKey("d"))
            {
                lWheelRotAX += 0.75f;
                rWheelRotAX -= 0.75f;

                tankRotAY += 0.5f;
            }
        }

        // Change projectile force
        if (Input.GetKey("f") || Input.GetKey("r"))
        {
            Debug.Log("Change Projectile Force");
        }

        // Chance weapon
        if (Input.GetKey("1") || Input.GetKey("2") || Input.GetKey("3") || 
                Input.GetKey("4") || Input.GetKey("5"))
        {
            Debug.Log("Change Weapon");
        }
        
        // Shoot
        if (Input.GetKey("space"))
        {
            Debug.Log("Shoot");
        }

        // Apply transformation to tank
        Matrix4x4 tankRotY = Transformations.RotateM(tankRotAY, Transformations.AXIS.AX_Y);

        Matrix4x4 bodyToTowerPivot1 = Transformations.TranslateM(-(Tower.transform.position.x-Body.transform.position.x), 0, -(Tower.transform.position.z-Body.transform.position.z));
        Matrix4x4 bodyToTowerPivot2 = Transformations.TranslateM(Tower.transform.position.x-Body.transform.position.x, 0, (Tower.transform.position.z-Body.transform.position.z));

        Matrix4x4 bodyT = bodyToTowerPivot2 * tankRotY * bodyToTowerPivot1;

        MeshUtils.ApplyTransformations(Body, bodyT, originals[(int) TankParts.Body]);

        // Apply transformations to tower and canon
        Matrix4x4 canonRotationX = Transformations.RotateM(canonRotAX, Transformations.AXIS.AX_X);
        Matrix4x4 towerRotationY = Transformations.RotateM(towerRotAY, Transformations.AXIS.AX_Y);

        Matrix4x4 canonToTowerPivot1 = Transformations.TranslateM(-(Tower.transform.position.x-Canon.transform.position.x), 0, -(Tower.transform.position.z-Canon.transform.position.z));
        Matrix4x4 canonToTowerPivot2 = Transformations.TranslateM(Tower.transform.position.x-Canon.transform.position.x, 0, (Tower.transform.position.z-Canon.transform.position.z));

        Matrix4x4 towerT = towerRotationY * tankRotY;
        Matrix4x4 canonT = canonToTowerPivot2 * towerRotationY * canonToTowerPivot1 * canonToTowerPivot2 * tankRotY * canonToTowerPivot1 * canonRotationX;

        MeshUtils.ApplyTransformations(Tower, towerT, originals[(int) TankParts.Tower]);
        MeshUtils.ApplyTransformations(Canon, canonT, originals[(int) TankParts.Canon]);

        // Apply transformations to wheels
        Matrix4x4 lWheelRotX = Transformations.RotateM(lWheelRotAX, Transformations.AXIS.AX_X);
        Matrix4x4 rWheelRotX = Transformations.RotateM(rWheelRotAX, Transformations.AXIS.AX_X);

        Matrix4x4 lBackWheelToTowerPivot1 = Transformations.TranslateM(-(Tower.transform.position.x-LBackWheel.transform.position.x), 0, -(Tower.transform.position.z-LBackWheel.transform.position.z));
        Matrix4x4 lBackWheelToTowerPivot2 = Transformations.TranslateM(Tower.transform.position.x-LBackWheel.transform.position.x, 0, (Tower.transform.position.z-LBackWheel.transform.position.z));

        Matrix4x4 lFrontWheelToTowerPivot1 = Transformations.TranslateM(-(Tower.transform.position.x-LFrontWheel.transform.position.x), 0, -(Tower.transform.position.z-LFrontWheel.transform.position.z));
        Matrix4x4 lFrontWheelToTowerPivot2 = Transformations.TranslateM(Tower.transform.position.x-LFrontWheel.transform.position.x, 0, (Tower.transform.position.z-LFrontWheel.transform.position.z));

        Matrix4x4 rBackWheelToTowerPivot1 = Transformations.TranslateM(-(Tower.transform.position.x-RBackWheel.transform.position.x), 0, -(Tower.transform.position.z-RBackWheel.transform.position.z));
        Matrix4x4 rBackWheelToTowerPivot2 = Transformations.TranslateM(Tower.transform.position.x-RBackWheel.transform.position.x, 0, (Tower.transform.position.z-RBackWheel.transform.position.z));

        Matrix4x4 rFrontWheelToTowerPivot1 = Transformations.TranslateM(-(Tower.transform.position.x-RFrontWheel.transform.position.x), 0, -(Tower.transform.position.z-RFrontWheel.transform.position.z));
        Matrix4x4 rFrontWheelToTowerPivot2 = Transformations.TranslateM(Tower.transform.position.x-RFrontWheel.transform.position.x, 0, (Tower.transform.position.z-RFrontWheel.transform.position.z));

        Matrix4x4 lBackWheelT = lBackWheelToTowerPivot2 * tankRotY * lBackWheelToTowerPivot1 * lWheelRotX;

        Matrix4x4 lFrontWheelT = lFrontWheelToTowerPivot2 * tankRotY * lFrontWheelToTowerPivot1 * lWheelRotX;
        
        Matrix4x4 rBackWheelT = rBackWheelToTowerPivot2 * tankRotY * rBackWheelToTowerPivot1 * rWheelRotX;
        
        Matrix4x4 rFrontWheelT = rFrontWheelToTowerPivot2 * tankRotY * rFrontWheelToTowerPivot1 * rWheelRotX;

        MeshUtils.ApplyTransformations(LBackWheel, lBackWheelT, originals[(int) TankParts.LBackWheel]);
        MeshUtils.ApplyTransformations(LFrontWheel, lFrontWheelT, originals[(int) TankParts.LFrontWheel]);

        MeshUtils.ApplyTransformations(RBackWheel, rBackWheelT, originals[(int) TankParts.RBackWheel]);
        MeshUtils.ApplyTransformations(RFrontWheel, rFrontWheelT, originals[(int) TankParts.RFrontWheel]);
    }
}
