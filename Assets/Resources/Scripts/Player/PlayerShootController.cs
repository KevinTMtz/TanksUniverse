using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootController : MonoBehaviour
{
    public void ShootProjectile(Vector3 posCannon, Vector3 shootPointDirection, float shotForce)
    {
        GameObject newProjectile = new GameObject();
        newProjectile.name = "Projectile";

        ProjectileController projectileController = newProjectile.AddComponent<ProjectileController>();
        
        projectileController.cpos = posCannon;
        projectileController.prev = projectileController.cpos;

        projectileController.r = 0.15f;
        projectileController.mass = 5;
        projectileController.restitution = 0.05f;

        Vector3 forceVector = shootPointDirection.normalized * (200 + 50 * (shotForce / 100));
        projectileController.shootForce = forceVector;

        Destroy(newProjectile, 2);
    }
}
