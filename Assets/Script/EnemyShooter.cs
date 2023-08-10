using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    public string targetTag = "Enemy"; // Tag of the enemies you want to target
    public GameObject projectilePrefab; // Prefab of the projectile to be shot
    public float shootingInterval = 2.0f; // Time interval between shots
    public float shootingRange = 10.0f; // Maximum range for shooting
    public float projectileSpeed = 10.0f; // Speed of the projectile

    private Transform target; // Reference to the nearest enemy
    private float lastShotTime; // Time of the last shot

    void OnDrawGizmosSelected(){
        Gizmos.color= Color.red;
        Gizmos.DrawWireSphere(transform.position,shootingRange);
    }

    private void Start()
    {
        lastShotTime = Time.time;
    }

    private void Update()
    {
        FindNearestTarget();

        if (target != null)
        {
            // Calculate the time it takes for the projectile to reach the enemy
            float timeToReach = Vector3.Distance(transform.position, target.position) / projectileSpeed;

            // Predict the enemy's future position
            Vector3 predictedPosition = target.position + target.GetComponent<Rigidbody>().velocity * timeToReach;

            // Calculate the direction to the intersection point based on projectile speed
            Vector3 projectileDirection = (predictedPosition - transform.position).normalized;

            // Rotate to face the calculated direction
            Quaternion rotation = Quaternion.LookRotation(projectileDirection);
            transform.rotation = rotation;

            // Check if enough time has passed since the last shot
            if (Time.time - lastShotTime >= shootingInterval)
            {
                Shoot();
                lastShotTime = Time.time;
            }
        }
    }

    private void FindNearestTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(targetTag);
        float nearestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);

            if (distance < nearestDistance && distance <= shootingRange)
            {
                nearestDistance = distance;
                target = enemy.transform;
            }
        }
    }

    private void Shoot()
    {
        if (projectilePrefab != null)
        {
            GameObject newProjectile = Instantiate(projectilePrefab, transform.position, transform.rotation);
            Rigidbody projectileRigidbody = newProjectile.GetComponent<Rigidbody>();

            if (projectileRigidbody != null)
            {
                projectileRigidbody.velocity = transform.forward * projectileSpeed;
            }
        }
    }
}
