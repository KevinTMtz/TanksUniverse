using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootController : MonoBehaviour
{
    public GameObject[] projectiles;
    public void ShootProjectile(Transform shootPointTransform, float shotForce, int projectileType)
    {

        GameObject newProjectile = GameObject.Instantiate(
            projectiles[projectileType-1],
            shootPointTransform.position,
            shootPointTransform.rotation
        );

        ProjectileController projectileController = newProjectile.GetComponent<ProjectileController>();
        
        projectileController.currentPos = shootPointTransform.position;
        projectileController.prevPos = projectileController.currentPos;

        Vector3 forceVector = shootPointTransform.forward.normalized * (50 + 35 * (shotForce / 100));
        projectileController.shootForce = forceVector;

        Destroy(newProjectile, 10);
    }
}
