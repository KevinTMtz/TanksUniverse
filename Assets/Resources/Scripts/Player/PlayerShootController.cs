using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootController : MonoBehaviour
{
    void Start()
    {
        
    }

    public void ShootProjectile()
    {
        GameObject newProjectile = new GameObject();
        newProjectile.name = "Projectile";
        ProjectileController ProjectileController = newProjectile.AddComponent<ProjectileController>();
        ProjectileController.cpos = new Vector3(
                Random.Range(-3.0f, 3.0f), 
                Random.Range(5.0f, 7.0f), 
                Random.Range(-3.0f, 3.0f)
            );
        ProjectileController.prev = ProjectileController.cpos;
        ProjectileController.r = 0.15f;
        ProjectileController.mass = 5;
        ProjectileController.restitution = Random.Range(0.2f, 0.8f);

        Destroy(newProjectile, 5);
    }
}
