using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public int damage;
    public float mass;
    public float r;

    public Vector3 currentPos;
    public Vector3 prevPos;
    public Vector3 forces;
    public Vector3 accel;

    public float restitution;

    float dt;

    private float gravityAcc;
    private float gravityForce;
    public Vector3 shootForce;

    public int maxBounces;
    private int bounces;

    void Start()
    {
        gravityAcc = 9.81f;
        gravityForce = -mass * gravityAcc;

        forces.x = shootForce.x + WindController.Wind.x;
        forces.y = gravityForce + shootForce.y;
        forces.z = shootForce.z + WindController.Wind.z;

        transform.position = currentPos;

        bounces = 0;
    }

    void Update() 
    {
        if (Time.timeScale != 0)
        {
            UpdatePhysics();
        }
    }
    void UpdatePhysics()
    {
        dt = Time.deltaTime;

        if (
            Mathf.Abs(currentPos.y - prevPos.y) < 0.001f && 
            Mathf.Abs(currentPos.y - r) < 0.001f ||
            bounces > maxBounces
        )
        {
            StopBall();
            Destroy(gameObject, 5);
        }
        else
        {
            shootForce.y -= gravityAcc * dt;
            forces.y = gravityForce + Mathf.Max(shootForce.y, 0);

            // Atmoshperic Air Resistance
            Vector3 v = (currentPos - prevPos) / (dt == 0 ? 1 : dt);

            // Going up
            if (currentPos.y > prevPos.y)
            {
                forces.y -= r * 0.00001f * v.magnitude;
            }
            // Going down
            else if (currentPos.y < prevPos.y)
            {
                forces.y += r * 0.00001f * v.magnitude;
            }

            Vector3 tempcurrentPos = currentPos;
            accel = forces / mass;

            // Varlet's integration method
            currentPos = 2 * currentPos - prevPos + accel * dt * dt;

            prevPos = tempcurrentPos;
            transform.position = currentPos;

            CollisionFloor();
        }
    }

    void CollisionFloor()
    {
        if (currentPos.y <= r)
        {
            bounces++;

            prevPos.y = currentPos.y;
            currentPos.y = r;
            
            forces.y = -forces.y * restitution;
        }
    }

    void StopBall()
    {
        currentPos.y = r;
        prevPos.y = r;
        forces = new Vector3(0, 0, 0);
    }

    public bool CheckCollision(ProjectileController other)
    {
        float sumR = r + other.r;

        float dx = currentPos.x - other.currentPos.x;
        float dy = currentPos.y - other.currentPos.y;
        float dz = currentPos.z - other.currentPos.z;

        float distance2 = dx * dx + dy * dy + dz * dz;

        return sumR >= distance2;
    }
}
