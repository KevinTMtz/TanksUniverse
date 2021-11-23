using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindController
{
    private static float force;
    private static float angle;
    private static Vector3 wind;

    void Start()
    {
        force = 0;
        angle = 0;
        wind = new Vector3(0, 0, 0);
    }

    public static void SetRandomWind()
    {
        force = Random.Range(0, 5f);
        angle = Random.Range(0, 360);

        float windX = Mathf.Sin(Mathf.Deg2Rad * angle) * force;
        float windZ = Mathf.Cos(Mathf.Deg2Rad * angle) * force;

        wind = new Vector3(windX, 0, windZ);
    }

    public static Vector3 Wind
    {
        get { return wind; }
    }

    public static float Force
    {
        get { return force; }
    }

    public static float Angle
    {
        get{ return angle; }
    }
}
