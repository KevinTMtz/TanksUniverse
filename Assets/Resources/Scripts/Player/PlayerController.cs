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

    private GameObject[] tankPartsArr;
    private List<Vector3[]> originals;

    private Vector3 tankPosDelta;
    private Vector3 towerPos;

    private float tankRotAY;
    private float towerRotAY;
    private float canonRotAX;
    private float lWheelRotAX;
    private float rWheelRotAX;

    private float deltaTankMoveMulti;
    private float deltaWheelRotAY;
    private float deltaTankRotAY;
    private float deltaShotForce;
    private float minCanonRotAngle;
    private float maxCanonRotAngle;

    private PlayerShootController playerShootController;

    private PlayerShotForce playerShotForce;
    private PlayerCurrentWeapon playerCurrentWeapon;

    private PlayerTankInfoUI playerTankInfoUI;

    public bool ableToShoot;

    public float colliderRadius;

    public float TankRotAY
    {
        set { tankRotAY = value; }
    }

    public Vector3 TankPosDelta
    {
        get { return tankPosDelta; }
    }

    void Start()
    {
        tankPartsArr = new GameObject[] { 
            Body,
            Tower,
            Canon,
            RFrontWheel,
            LFrontWheel,
            RBackWheel,
            LBackWheel
        }; 
        
        // Extract Vertices
        originals = new List<Vector3[]>();

        for (int i=0; i<tankPartsArr.Length; i++)
        {
            MeshUtils.ExtractVertices(tankPartsArr[i], originals);
        }

        tankPosDelta = new Vector3(0, 0, 0);
        towerPos = Tower.transform.position;

        towerRotAY = 0;
        canonRotAX = 0;
        lWheelRotAX = 0;
        rWheelRotAX = 0;

        ApplyRotationToGO(tankRotAY);

        playerShootController = GetComponent<PlayerShootController>();

        playerShotForce = GetComponent<PlayerShotForce>();
        playerCurrentWeapon = GetComponent<PlayerCurrentWeapon>();
        playerTankInfoUI = GetComponent<PlayerTankInfoUI>();

        playerTankInfoUI.SetTankPositionText(new Vector3(
            transform.position.x + tankPosDelta.x,
            transform.position.y,
            transform.position.z + tankPosDelta.z
        ));

        ableToShoot = false;

        // Constants
        deltaTankMoveMulti = 0.04f;
        deltaTankRotAY = 1;
        deltaWheelRotAY = 3;
        deltaShotForce = 0.25f;
        minCanonRotAngle = 30;
        maxCanonRotAngle = -45;
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
                lWheelRotAX += deltaWheelRotAY;
                rWheelRotAX += deltaWheelRotAY;

                float deltaMoveX = Mathf.Sin(Mathf.Deg2Rad * tankRotAY) * deltaTankMoveMulti;
                float deltaMoveZ = Mathf.Cos(Mathf.Deg2Rad * tankRotAY) * deltaTankMoveMulti;

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
                lWheelRotAX -= deltaWheelRotAY;
                rWheelRotAX += deltaWheelRotAY;

                tankRotAY -= deltaTankRotAY;

                ApplyRotationToGO(-deltaTankRotAY);
            }
            // Rotate tank right
            else if (Input.GetKey("d"))
            {
                lWheelRotAX += deltaWheelRotAY;
                rWheelRotAX -= deltaWheelRotAY;

                tankRotAY += deltaTankRotAY;

                ApplyRotationToGO(deltaTankRotAY);
            }

            playerTankInfoUI.SetTankRotationText(tankRotAY);
            playerTankInfoUI.SetTowerRotationText(towerRotAY, tankRotAY);
        }

        // Rotate tower
        if (Input.GetKey("q") || Input.GetKey("e"))
        {
            float deltaTowerRotAY = (Input.GetKey("q") ? -1 : 1);

            towerRotAY += deltaTowerRotAY;

            playerTankInfoUI.SetTowerRotationText(towerRotAY, tankRotAY);

            ApplyRotationToGO(deltaTowerRotAY);
        }
        
        // Rotate canon
        float deltaCanonRotAX = -Input.GetAxis("Vertical");
        if (Input.GetAxis("Vertical") != 0)
        {
            if (canonRotAX + deltaCanonRotAX > minCanonRotAngle) 
                deltaCanonRotAX = minCanonRotAngle - canonRotAX;
            else if (canonRotAX + deltaCanonRotAX < maxCanonRotAngle) 
                deltaCanonRotAX = maxCanonRotAngle - canonRotAX;

            canonRotAX += deltaCanonRotAX;

            playerTankInfoUI.SetCanonRotAXText(canonRotAX);

            RotateGOWithPivot(
                shootPoint,
                canonPoint.transform.position,
                deltaCanonRotAX,
                shootPoint.transform.right,
                true
            );
        }

        // Change projectile force
        if (Input.GetKey("f") || Input.GetKey("r"))
        {
            if(Input.GetKey("f"))
                playerShotForce.DecreaseForce(deltaShotForce);
            else
                playerShotForce.IncreaseForce(deltaShotForce);
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
        if (Input.GetKey("space") && ableToShoot)
        {
            ableToShoot = false;

            playerShootController.ShootProjectile(
                shootPoint.transform,
                playerShotForce.Force,
                playerCurrentWeapon.CurrentWeapon
            );
        }
    }

    void ApplyTankTransformations()
    {
        Matrix4x4 tankRotY = Transformations.RotateM(tankRotAY, Transformations.AXIS.AX_Y);
        Matrix4x4 tankMove = Transformations.TranslateM(tankPosDelta.x, 0, tankPosDelta.z);

        Matrix4x4 canonRotationX = Transformations.RotateM(canonRotAX, Transformations.AXIS.AX_X);
        Matrix4x4 towerRotationY = Transformations.RotateM(towerRotAY, Transformations.AXIS.AX_Y);

        Matrix4x4 lWheelRotX = Transformations.RotateM(lWheelRotAX, Transformations.AXIS.AX_X);
        Matrix4x4 rWheelRotX = Transformations.RotateM(rWheelRotAX, Transformations.AXIS.AX_X);

        for (int i=0; i < tankPartsArr.Length; i++)
        {
            Matrix4x4 partToTowerPivot1 = GetTranslateMatrixWithPivot(
                towerPos, 
                tankPartsArr[i].transform.position, 
                -1
            );
            Matrix4x4 partToTowerPivot2 = GetTranslateMatrixWithPivot(
                towerPos, 
                tankPartsArr[i].transform.position, 
                1
            );

            Matrix4x4[] transformations = new Matrix4x4[] {
                tankMove * partToTowerPivot2 * tankRotY * partToTowerPivot1,
                tankMove * towerRotationY * tankRotY,
                tankMove * partToTowerPivot2 * towerRotationY * partToTowerPivot1 * partToTowerPivot2 * tankRotY * partToTowerPivot1 * canonRotationX,
                tankMove * partToTowerPivot2 * tankRotY * partToTowerPivot1 * rWheelRotX,
                tankMove * partToTowerPivot2 * tankRotY * partToTowerPivot1 * lWheelRotX,
                tankMove * partToTowerPivot2 * tankRotY * partToTowerPivot1 * rWheelRotX,
                tankMove * partToTowerPivot2 * tankRotY * partToTowerPivot1 * lWheelRotX,
            };

            MeshUtils.ApplyTransformations(tankPartsArr[i], transformations[i], originals[i]);
        }
    }

    void ApplyRotationToGO(float deltaRotAY)
    {
        RotateGOWithPivot(playerCamera.gameObject, towerPos, deltaRotAY, Vector3.up, true);
        RotateGOWithPivot(shootPoint, towerPos, deltaRotAY, Vector3.up, true);
        RotateGOWithPivot(canonPoint, Tower.transform.position, deltaRotAY, Vector3.up, false);
    }

    void RotateGOWithPivot(
        GameObject gameObjectToRotate,
        Vector3 pivot,
        float deltaGORotA,
        Vector3 axis,
        bool useTankPosDelta
    )
    {
        gameObjectToRotate.transform.RotateAround(
            new Vector3(
                pivot.x + (useTankPosDelta ? tankPosDelta.x : 0),
                pivot.y,
                pivot.z + (useTankPosDelta ? tankPosDelta.z : 0)
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
