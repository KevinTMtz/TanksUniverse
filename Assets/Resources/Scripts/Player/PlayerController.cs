using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public GameObject Body;
    public GameObject Tower;
    public GameObject Canon;
    public GameObject RFrontWheel;
    public GameObject LFrontWheel;
    public GameObject RBackWheel;
    public GameObject LBackWheel;

    public Camera playerCamera;
    public GameObject shootPoint;
    public GameObject canonPoint;

    private List<Vector3[]> originals;

    private Vector3 tankPosDelta;
    private Vector3 towerPos;

    private float tankRotAY;
    private float towerRotAY;
    private float canonRotAX;
    private float lWheelRotAX;
    private float rWheelRotAX;

    private PlayerShootController playerShootController;

    private PlayerShotForce playerShotForce;
    private PlayerCurrentWeapon playerCurrentWeapon;
    private PlayerTankInfoUI playerTankInfoUI;

    Vector3 directionRotCanonPoint;

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

        tankPosDelta = new Vector3(0, 0, 0);
        towerPos = Tower.transform.position;

        tankRotAY = 0;
        towerRotAY = 0;
        canonRotAX = 0;
        lWheelRotAX = 0;
        rWheelRotAX = 0;

        playerShootController = GetComponent<PlayerShootController>();

        playerShotForce = GetComponent<PlayerShotForce>();
        playerCurrentWeapon = GetComponent<PlayerCurrentWeapon>();
        playerTankInfoUI = GetComponent<PlayerTankInfoUI>();

        playerTankInfoUI.SetTankPositionText(new Vector3(
            transform.position.x + tankPosDelta.x,
            transform.position.y,
            transform.position.z + tankPosDelta.z
        ));

        directionRotCanonPoint = canonPoint.transform.position - Tower.transform.position;
    }

    void Update()
    {
        if (Time.timeScale != 0)
        {
            UpdateInputValues();
            ApplyTankTransformations();
        }
    }

    void UpdateInputValues()
    {
        if (Input.GetKey("a") || Input.GetKey("d"))
        {
            // Move tank forward
            if (Input.GetKey("a") && Input.GetKey("d"))
            {
                lWheelRotAX += 0.75f;
                rWheelRotAX += 0.75f;

                float deltaMoveX = Mathf.Sin(Mathf.Deg2Rad * tankRotAY) * 0.01f;
                float deltaMoveZ = Mathf.Cos(Mathf.Deg2Rad * tankRotAY) * 0.01f;

                tankPosDelta = new Vector3(
                    tankPosDelta.x + deltaMoveX, 
                    0, 
                    tankPosDelta.z + deltaMoveZ
                );

                playerTankInfoUI.SetTankPositionText(new Vector3(
                    transform.position.x + tankPosDelta.x,
                    transform.position.y,
                    transform.position.z + tankPosDelta.z
                ));

                // Move player camera
                playerCamera.transform.position = new Vector3(
                    playerCamera.transform.position.x + deltaMoveX,
                    playerCamera.transform.position.y,
                    playerCamera.transform.position.z + deltaMoveZ
                );

                // Move shoot point
                shootPoint.transform.position = new Vector3(
                    shootPoint.transform.position.x + deltaMoveX,
                    shootPoint.transform.position.y,
                    shootPoint.transform.position.z + deltaMoveZ
                );
            }
            // Rotate tank left
            else if (Input.GetKey("a"))
            {
                lWheelRotAX -= 0.75f;
                rWheelRotAX += 0.75f;

                tankRotAY -= 0.5f;

                RotateGOWithPivot(playerCamera.gameObject, towerPos, -0.5f, Vector3.up);
                RotateGOWithPivot(shootPoint, towerPos, -0.5f, Vector3.up);
            }
            // Rotate tank right
            else if (Input.GetKey("d"))
            {
                lWheelRotAX += 0.75f;
                rWheelRotAX -= 0.75f;

                tankRotAY += 0.5f;

                RotateGOWithPivot(playerCamera.gameObject, towerPos, 0.5f, Vector3.up);
                RotateGOWithPivot(shootPoint, towerPos, 0.5f, Vector3.up);
            }

            playerTankInfoUI.SetTankRotationText(tankRotAY);
        }

        // Rotate tower
        if (Input.GetKey("q") || Input.GetKey("e"))
        {
            float deltaTowerRotAY = (Input.GetKey("q") ? -1 : 1) * 0.5f;

            towerRotAY += deltaTowerRotAY;

            playerTankInfoUI.SetTowerRotationText(towerRotAY);

            RotateGOWithPivot(playerCamera.gameObject, towerPos, deltaTowerRotAY, Vector3.up);
            RotateGOWithPivot(shootPoint, towerPos, deltaTowerRotAY, Vector3.up);
        }

        // Rotate canon base point around the pivot
        Quaternion rotCanonPointAY = Quaternion.AngleAxis(towerRotAY + tankRotAY, Vector3.up);
        canonPoint.transform.position = Tower.transform.position + rotCanonPointAY * directionRotCanonPoint;
        canonPoint.transform.localRotation = rotCanonPointAY;
        
        // Rotate canon
        float deltaCanonRotAX = -Input.GetAxis("Vertical") * 0.5f;
        if (
            Input.GetAxis("Vertical") != 0 && 
            !(canonRotAX + deltaCanonRotAX > 31) && !(canonRotAX + deltaCanonRotAX < -36)
        )
        {
            canonRotAX += deltaCanonRotAX;

            playerTankInfoUI.SetCanonRotAXText(canonRotAX);

            RotateGOWithPivot(
                shootPoint,
                canonPoint.transform.position,
                deltaCanonRotAX,
                shootPoint.transform.right
            );
        }

        // Change projectile force
        if (Input.GetKey("f") || Input.GetKey("r"))
        {
            if(Input.GetKey("f"))
                playerShotForce.DecreaseForce();
            else
                playerShotForce.IncreaseForce();
        }

        // Chance weapon
        if (Input.GetKey("1") || Input.GetKey("2") || Input.GetKey("3") || 
                Input.GetKey("4") || Input.GetKey("5"))
        {
            if (Input.GetKey("1"))
                playerCurrentWeapon.SetCurrentWeapon(1);
            else if (Input.GetKey("2"))
                playerCurrentWeapon.SetCurrentWeapon(2);
            else if (Input.GetKey("3"))
                playerCurrentWeapon.SetCurrentWeapon(3);
            else if (Input.GetKey("4"))
                playerCurrentWeapon.SetCurrentWeapon(4);
            else if (Input.GetKey("5"))
                playerCurrentWeapon.SetCurrentWeapon(5);
        }
        
        // Shoot
        if (Input.GetKey("space"))
        {
            playerShootController.ShootProjectile(
                shootPoint.transform.position,
                shootPoint.transform.forward,
                playerShotForce.Force
            );
        }
    }

    void ApplyTankTransformations()
    {
        Matrix4x4 tankRotY = Transformations.RotateM(tankRotAY, Transformations.AXIS.AX_Y);
        Matrix4x4 tankMove = Transformations.TranslateM(tankPosDelta.x, 0, tankPosDelta.z);

        // Apply transformation to body
        Matrix4x4 bodyToTowerPivot1 = GetTranslateMatrixWithPivot(towerPos, Body.transform.position, -1);
        Matrix4x4 bodyToTowerPivot2 = GetTranslateMatrixWithPivot(towerPos, Body.transform.position, 1);

        Matrix4x4 bodyT = tankMove * bodyToTowerPivot2 * tankRotY * bodyToTowerPivot1;

        MeshUtils.ApplyTransformations(Body, bodyT, originals[(int) TankParts.Body]);

        // Apply transformations to tower and canon
        Matrix4x4 canonRotationX = Transformations.RotateM(canonRotAX, Transformations.AXIS.AX_X);
        Matrix4x4 towerRotationY = Transformations.RotateM(towerRotAY, Transformations.AXIS.AX_Y);

        Matrix4x4 canonToTowerPivot1 = GetTranslateMatrixWithPivot(towerPos, Canon.transform.position, -1);
        Matrix4x4 canonToTowerPivot2 = GetTranslateMatrixWithPivot(towerPos, Canon.transform.position, 1);

        Matrix4x4 towerT = tankMove * towerRotationY * tankRotY;
        Matrix4x4 canonT = tankMove * canonToTowerPivot2 * towerRotationY * canonToTowerPivot1 * canonToTowerPivot2 * tankRotY * canonToTowerPivot1 * canonRotationX;

        MeshUtils.ApplyTransformations(Tower, towerT, originals[(int) TankParts.Tower]);
        MeshUtils.ApplyTransformations(Canon, canonT, originals[(int) TankParts.Canon]);

        // Apply transformations to wheels
        Matrix4x4 lWheelRotX = Transformations.RotateM(lWheelRotAX, Transformations.AXIS.AX_X);
        Matrix4x4 rWheelRotX = Transformations.RotateM(rWheelRotAX, Transformations.AXIS.AX_X);

        Matrix4x4 lBackWheelToTowerPivot1 = GetTranslateMatrixWithPivot(towerPos, LBackWheel.transform.position, -1);
        Matrix4x4 lBackWheelToTowerPivot2 = GetTranslateMatrixWithPivot(towerPos, LBackWheel.transform.position, 1);

        Matrix4x4 lFrontWheelToTowerPivot1 = GetTranslateMatrixWithPivot(towerPos, LFrontWheel.transform.position, -1);
        Matrix4x4 lFrontWheelToTowerPivot2 = GetTranslateMatrixWithPivot(towerPos, LFrontWheel.transform.position, 1);

        Matrix4x4 rBackWheelToTowerPivot1 = GetTranslateMatrixWithPivot(towerPos, RBackWheel.transform.position, -1);
        Matrix4x4 rBackWheelToTowerPivot2 = GetTranslateMatrixWithPivot(towerPos, RBackWheel.transform.position, 1);

        Matrix4x4 rFrontWheelToTowerPivot1 = GetTranslateMatrixWithPivot(towerPos, RFrontWheel.transform.position, -1);
        Matrix4x4 rFrontWheelToTowerPivot2 = GetTranslateMatrixWithPivot(towerPos, RFrontWheel.transform.position, 1);

        Matrix4x4 lBackWheelT = tankMove * lBackWheelToTowerPivot2 * tankRotY * lBackWheelToTowerPivot1 * lWheelRotX;
        Matrix4x4 lFrontWheelT = tankMove * lFrontWheelToTowerPivot2 * tankRotY * lFrontWheelToTowerPivot1 * lWheelRotX;
        
        Matrix4x4 rBackWheelT = tankMove * rBackWheelToTowerPivot2 * tankRotY * rBackWheelToTowerPivot1 * rWheelRotX;
        Matrix4x4 rFrontWheelT = tankMove * rFrontWheelToTowerPivot2 * tankRotY * rFrontWheelToTowerPivot1 * rWheelRotX;

        MeshUtils.ApplyTransformations(LBackWheel, lBackWheelT, originals[(int) TankParts.LBackWheel]);
        MeshUtils.ApplyTransformations(LFrontWheel, lFrontWheelT, originals[(int) TankParts.LFrontWheel]);

        MeshUtils.ApplyTransformations(RBackWheel, rBackWheelT, originals[(int) TankParts.RBackWheel]);
        MeshUtils.ApplyTransformations(RFrontWheel, rFrontWheelT, originals[(int) TankParts.RFrontWheel]);
    }

    void RotateGOWithPivot(
        GameObject gameObjectToRotate,
        Vector3 pivot,
        float deltaGORotA,
        Vector3 axis)
    {
        gameObjectToRotate.transform.RotateAround(
            new Vector3(
                pivot.x + tankPosDelta.x,
                pivot.y,
                pivot.z + tankPosDelta.z
            ), 
            axis, 
            deltaGORotA
        );
    }

    Matrix4x4 GetTranslateMatrixWithPivot(Vector3 pivotPoint, Vector3 currentPos, int multiplier)
    {
        return Transformations.TranslateM(
            (pivotPoint.x - currentPos.x) * multiplier, 
            0, 
            (pivotPoint.z - currentPos.z) * multiplier
        );
    }
}
