using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mathematics
{
    public static float Dot(Vector3 a, Vector3 b)
    {
        return a.x * b.x + a.y * b.y + a.z * b.z;
    }

    public static Vector3 Cross(Vector3 a, Vector3 b)
    {
        return new Vector3(a.y * b.z - a.z * b.y, a.z * b.x - a.x * b.z, a.x * b.y - a.y * b.x);
    }

    public static Vector3 Add(Vector3 a, Vector3 b)
    {
        return new Vector3(a.x + b.x, a.y + b.y, a.z + b.z);
    }

    public static Vector3 Subtract(Vector3 a, Vector3 b)
    {
        return new Vector3(a.x - b.x, a.y - b.y, a.z - b.z);
    }

    public static Vector3 Scalar(Vector3 a, float s)
    {
        return new Vector3 (a.x * s, a.y * s, a.z * s);
    }

    public static float Magnitude(Vector3 v)
    {
        return Mathf.Sqrt(v.x*v.x + v.y*v.y + v.z*v.z);
    }

    public static Vector3 Normalized(Vector3 v)
    {
        float mag = Magnitude(v);
        return new Vector3(v.x/mag, v.y/mag, v.z/mag);
    }

    public static float AngleBetween(Vector3 a, Vector3 b)
    {
        // dot(a, b) = la|*|b|*cos (angle)
        // angle = arccos(dot (au, bu))
        return Mathf.Acos(Dot(Normalized(a), Normalized(b))) ;
    }

    public static Vector3 Reflect(Vector3 v, Vector3 n)
    {
        Vector3 nu = Normalized(n);
        Vector3 vp = Dot(nu, v) * nu;
        Vector3 vo = v - vp;
        Vector3 r = vp - vo;
        
        return r;
    }

    public static Vector3 SphericalToCartesian(float i, float a, float r)
    {
        float inclination = i * Mathf.Deg2Rad;
        float azimuth = a * Mathf.Deg2Rad;
        Vector3 xyz = Vector3.zero; 
        xyz.x = r * Mathf.Sin(inclination) * Mathf.Sin(azimuth);
        xyz.y = r * Mathf.Cos(inclination);
        xyz.z = r * Mathf.Sin(inclination) * Mathf.Cos(azimuth);

        return xyz;
    }

    public static Vector2 SphericalMapping(Vector3 nu)
    {
        Vector2 uv = new Vector2(0, 0);
        uv.x = 0.5f + Mathf.Asin(nu.x) / Mathf.PI;
        uv.y = 0.5f + Mathf.Asin(nu.y) / Mathf.PI;

        return uv;
    }
}
