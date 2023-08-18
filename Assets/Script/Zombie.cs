using UnityEngine;
using System.Collections;

public class Zombie : MonoBehaviour
{
    public string playerTag = "Player";
    public float movementSpeed = 2.0f;
    public float detectionDistance = 10.0f;
    public float stoppingDistance = 2.0f;
    public GameObject bloodSplatterPrefab;
    public Vector3 bloodofset = Vector3.zero;
    Animator enemy_Animator;
    private Transform playerTransform;
    private Rigidbody rb;
    private UnityEngine.AI.NavMeshAgent navAgent;

    private void Start()
    {
        enemy_Animator = GetComponent<Animator>();
        playerTransform = GameObject.FindGameObjectWithTag(playerTag).transform;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Freeze rotation to ensure proper movement
        rb.useGravity = false;
        navAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
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
                    //Vector3 movement = directionToPlayer.normalized * movementSpeed * Time.deltaTime;
                    //rb.MovePosition(rb.position + movement);
                    navAgent.SetDestination(playerTransform.position);
                }
                else if(distanceToPlayer < 3.5)
                {
                    Attack();
                }
                else
                {
                     // Stop the zombie when it's too close to the player
                    rb.velocity = Vector3.zero;
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(directionToPlayer), Time.deltaTime * 5.0f);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet")) // Check if the collider belongs to the player
        {
             StartCoroutine(DelayBlood());

            //Can also add other actions here, like decreasing enemy health, playing hit sounds, etc.
            
        }
    }

    IEnumerator DelayBlood()
    {
        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(0.0f);
        // Instantiate the blood splatter prefab at the hit position and rotation
        Quaternion randomRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
        Instantiate(bloodSplatterPrefab, transform.position + bloodofset, randomRotation);
    }

    private void Attack(){
        enemy_Animator.SetBool("attacking", true);
    }

  
}
