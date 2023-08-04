using UnityEngine;

public class ZombieController : MonoBehaviour
{
    public string playerTag = "Player";
    public float movementSpeed = 2.0f;

    private Transform playerTransform;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag(playerTag).transform;
    }

    private void Update()
    {
        if (playerTransform != null)
        {
            // Calculate the direction to the player
            Vector3 directionToPlayer = playerTransform.position - transform.position;
            directionToPlayer.y = 0; // Ensure the zombie doesn't move up/down

            // Rotate the zombie to face the player
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(directionToPlayer), Time.deltaTime * 5.0f);

            // Move the zombie towards the player
            transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);
        }
    }
}
