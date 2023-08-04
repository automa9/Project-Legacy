using UnityEngine;

public class Zombie : MonoBehaviour
{
    public string playerTag = "Player";
    public float movementSpeed = 2.0f;
    public float detectionDistance = 10.0f;
    public float stoppingDistance = 2.0f;
    Animator enemy_Animator;
    private Transform playerTransform;
    private Rigidbody rb;

    private void Start()
    {
        enemy_Animator = GetComponent<Animator>();
        playerTransform = GameObject.FindGameObjectWithTag(playerTag).transform;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Freeze rotation to ensure proper movement
        rb.useGravity = false;
    }

    private void FixedUpdate()
    {
        if (playerTransform != null)
        {
            // Calculate the distance to the player
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

            if (distanceToPlayer <= detectionDistance)
            {
                // Calculate the direction to the player
                Vector3 directionToPlayer = playerTransform.position - transform.position;
                directionToPlayer.y = 0; // Ensure the zombie doesn't move up/down

                if (distanceToPlayer > stoppingDistance)
                {
                    //Face the player
                    //might be optimized on how the rotation and movement handles
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(directionToPlayer), Time.deltaTime * 5.0f);
                    // animate to walk
                    enemy_Animator.SetBool("attacking", false);
                    // Move the zombie towards the player using physics
                    Vector3 movement = directionToPlayer.normalized * movementSpeed * Time.deltaTime;
                    rb.MovePosition(rb.position + movement);
                }
                else
                {
                    // Stop the zombie when it's too close to the player
                    rb.velocity = Vector3.zero;
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(directionToPlayer), Time.deltaTime * 5.0f);
                    Attack();
                }
            }
        }
    }

    private void Attack(){
        enemy_Animator.SetBool("attacking", true);
    }
}
