using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public float mass;
    public float r;
    public Vector3 cpos; // Current position
    public Vector3 prev; // Previous position
    public Vector3 forces;
    public Vector3 accel; // Acceleration
    public float restitution;

    float dt; // Delta time

    public GameObject sphere;

    public Color color;

    public bool colliding;

    void Start()
    {
        forces.y = -mass * 9.81f;

        forces.x = Random.Range(-5.0f, 5.0f);
        forces.z = Random.Range(-5.0f, 5.0f);

        sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.SetParent(transform);

        sphere.transform.position = cpos;
        sphere.transform.localScale = new Vector3(r*2, r*2, r*2);

        Renderer rend = sphere.GetComponent<Renderer>();

        color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        rend.material.SetColor("_Color", color);
    }

    void Update()
    {
        dt = Time.deltaTime;

        if (Mathf.Abs(cpos.y - prev.y) < 0.00001f && Mathf.Abs(cpos.y - r) < 0.00001f)
        {
            cpos.y = r;
            prev.y = r;
            forces.y = 0;
        }
        else
        {
            forces.y = -mass * 9.81f;

            // Atmoshperic Air Resistance
            Vector3 v = (cpos - prev) / Time.deltaTime;
            if (cpos.y > prev.y) // Going up
            {
                forces.y -= r * 0.00001f * v.magnitude;
            }
            else if (cpos.y < prev.y) // Going down
            {
                forces.y += r * 0.00001f * v.magnitude;
            }

            Vector3 tempCPos = cpos;
            accel = forces / mass;
            cpos = 2 * cpos - prev + accel * dt * dt; // Varlet's integration method
            prev = tempCPos;
            sphere.transform.localPosition = cpos;
            CollisionFloor();
        }
    }

    void CollisionFloor()
    {
        if (cpos.y <= r)
        {
            prev.y = cpos.y;
            cpos.y = r;
            forces.y = -forces.y * restitution;
        }
    }

    public bool CheckCollision(ProjectileController other)
    {
        float sumR = r + other.r;

        float dx = cpos.x - other.cpos.x;
        float dy = cpos.y - other.cpos.y;
        float dz = cpos.z - other.cpos.z;

        float distance2 = dx * dx + dy * dy + dz * dz;

        return sumR >= distance2;
    }
}
