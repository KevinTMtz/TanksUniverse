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

    float towerRotY;
    float canonRotX;

    float lWheelsRotX;
    float rWheelsRotX;

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

        ExtractVertices(Body, TankParts.Body.ToString(), originals);
        ExtractVertices(Tower, TankParts.Tower.ToString(), originals);
        ExtractVertices(Canon, TankParts.Canon.ToString(), originals);
        ExtractVertices(RFrontWheel, TankParts.RFrontWheel.ToString(), originals);
        ExtractVertices(LFrontWheel, TankParts.LFrontWheel.ToString(), originals);
        ExtractVertices(RBackWheel, TankParts.RBackWheel.ToString(), originals);
        ExtractVertices(LBackWheel, TankParts.LBackWheel.ToString(), originals);

        towerRotY = 0;
        canonRotX = 0;
        lWheelsRotX = 0;
        rWheelsRotX = 0;
    }

    void Update()
    {
        if (Input.GetAxis("Vertical") != 0) {
            if (canonRotX >= 30) canonRotX = 30;
            if (canonRotX <= -35) canonRotX = -35;

            canonRotX -= Input.GetAxis("Vertical");
            Matrix4x4 rotationX = Transformations.RotateM(canonRotX, Transformations.AXIS.AX_X);

            ApplyTransformations(Canon, rotationX, originals[(int) TankParts.Canon]);
        }

        if (Input.GetKey("q") || Input.GetKey("e")) {
            towerRotY += Input.GetKey("q") ? -1 : 1;
            
            Matrix4x4 rotationY = Transformations.RotateM(towerRotY, Transformations.AXIS.AX_Y);

            Matrix4x4 tm1 = Transformations.TranslateM(-Tower.transform.position.x, -Tower.transform.position.y, -Tower.transform.position.z);

            Matrix4x4 tm2 = Transformations.TranslateM(Tower.transform.position.x, Tower.transform.position.y, Tower.transform.position.z);

            ApplyTransformations(Tower, rotationY, originals[(int) TankParts.Tower]);
            ApplyTransformations(Canon, tm1 * rotationY * tm2, originals[(int) TankParts.Canon]);
        }

        if (Input.GetKey("a") || Input.GetKey("d")) {
            if (Input.GetKey("a") && Input.GetKey("d"))
            {
                lWheelsRotX += 1;
                rWheelsRotX += 1;
            } 
            else if (Input.GetKey("a"))
            {
                lWheelsRotX -= 1;
                rWheelsRotX += 1;
            }
            else if (Input.GetKey("d"))
            {
                lWheelsRotX += 1;
                rWheelsRotX -= 1;
            }

            Matrix4x4 transformationL = Transformations.RotateM(lWheelsRotX, Transformations.AXIS.AX_X);
            Matrix4x4 transformationR = Transformations.RotateM(rWheelsRotX, Transformations.AXIS.AX_X);

            ApplyTransformations(LBackWheel, transformationL, originals[(int) TankParts.LBackWheel]);
            ApplyTransformations(LFrontWheel, transformationL, originals[(int) TankParts.LFrontWheel]);

            ApplyTransformations(RBackWheel, transformationR, originals[(int) TankParts.RBackWheel]);
            ApplyTransformations(RFrontWheel, transformationR, originals[(int) TankParts.RFrontWheel]);
        }

        // 1, 2, 3, 4, 5
        if (Input.GetKey("1") || Input.GetKey("2") || Input.GetKey("3") || 
                Input.GetKey("4") || Input.GetKey("5"))
        {
            Debug.Log("Change Weapon");
        }
        
        // Space
        if (Input.GetKey("space"))
        {
            Debug.Log("Shoot");
        }
    }

    public static void ApplyTransformations(GameObject go, Matrix4x4 t, Vector3[] orig)
    {
        Mesh m = go.GetComponent<MeshFilter>().mesh;
        Vector3[] transformed = new Vector3[orig.Length];

        for (int i = 0; i < transformed.Length; i++)
        {
            Vector4 temp = new Vector4(orig[i].x, orig[i].y, orig[i].z, 1);
            transformed[i] = t * temp;
        }

        orig = transformed;
        m.vertices = transformed;
        m.RecalculateNormals();
    }

    public static void ExtractVertices(GameObject go, string name, List<Vector3[]> orig)
    {
        go.name = name;
        Mesh m = go.GetComponent<MeshFilter>().mesh;
        Vector3[] o = new Vector3[m.vertices.Length];

        for (int i = 0; i < m.vertices.Length; i++)
        {
            o[i] = new Vector3(m.vertices[i].x, m.vertices[i].y, m.vertices[i].z);
        }

        orig.Add(o);
    }
}
