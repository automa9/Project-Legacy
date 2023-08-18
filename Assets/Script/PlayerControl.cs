using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PlayerControl : MonoBehaviourPunCallbacks, IPunObservable
{
    PhotonView view;
    Vector2 moveDirection;

    public string targetEnemy = "Enemy";
    public float playerHealth = 100;
    private float _playerSpeed = 5f;
    public const float maxDashTime = 2.0f;
    public float dashDistance = 20;
    public float dashStoppingSpeed = 0.1f;
    public float dashSpeed = 5f;
    float currentDashTime = maxDashTime;
    public float detectionDistance = 10.0f;
    public float projectileSpeed = 10.0f; // Speed of the projectile

    public GameObject bloodSplatterPrefab;
    public GameObject muzzlePrefab;
    
    
    public GameObject projectilePrefab;
    private Transform enemyTransform;
    private bool isDead = false; 

    private float _basePlayerSpeed; //to Store the base player speed

    [SerializeField]
    private float _rotationSpeed = 10f;

    private Vector3 _playerVelocity;
    private bool _groundedPlayer;
    private Animator animator;

    [SerializeField]
    private float _gravityValue = -9.81f;

    private Photon.Realtime.Player attackingPlayer;

    Rigidbody _rigidbody;

    public bool isPoweredUp = false;
    private float powerUpDuration = 10f;
    private float powerUpTimer = 0f;
    public float shootingInterval = 2.0f;
    public int powerUpCount = 3;
    public float meleeDistance = 2.0f;

    public Vector3 muzzleOffset= Vector3.zero;
    public Vector3 bloodoffset = Vector3.zero;
    public Vector3 bulletoffset = Vector3.zero;
    private float lastShotTime; // Time of the last shot

    void OnDrawGizmosSelected(){
        Gizmos.color= Color.red;
        Gizmos.DrawWireSphere(transform.position,meleeDistance);
    }

    private void Start()
    {
        view = GetComponent<PhotonView>();
        animator = GetComponent<Animator>();
       // _controller = GetComponent<CharacterController>();
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.freezeRotation = true; // Freeze rotation to ensure proper movement
        _rigidbody.useGravity = false;

        _basePlayerSpeed = _playerSpeed;
        enemyTransform = GameObject.FindGameObjectWithTag(targetEnemy).transform;
        lastShotTime = Time.time;
       

       //camera work
        CamWork _cameraWork = gameObject.GetComponent<CamWork>();

                if (_cameraWork != null)
                {
                    if (photonView.IsMine)
                    {
                        _cameraWork.OnStartFollowing();
                    }
                }
                else
                {
                    Debug.LogError("<Color=Red><b>Missing</b></Color> CameraWork Component on player Prefab.", this);
                }
    }
    
    void FixedUpdate()
    {
       // Debug.Log("Player not poses controller");
        //StartCoroutine(DelayBlood());
        ////MOVE THE PLAYER-----------------
                //float horizontalInput = Input.GetAxis("Horizontal");
               // float verticalInput = Input.GetAxis("Vertical");

                //Vector3 movementInput = new Vector3(verticalInput, 0, -horizontalInput);
                //Vector3 movementDirection = movementInput.normalized;

                //_controller.Move(movementDirection * _playerSpeed * Time.deltaTime);
                
                //Debug.Log(_rigidbody.velocity.magnitude);
                //animator.SetBool("Running", true);    

        if ( view == null || view.IsMine)
        {
           // Debug.Log("Player Successfully Poses controller");
            if(!isDead){
                if (_groundedPlayer && _playerVelocity.y < 0)
                {
                    _playerVelocity.y = 0f;
                }

                if(enemyTransform != null){
                    // Calculate the distance to the player
                    float distanceToEnemy = Vector3.Distance(transform.position, enemyTransform.position);
                    animator.SetBool("isShoot",false);
                    //Debug.Log(distanceToEnemy);
                    if (distanceToEnemy <= detectionDistance)
                    {
                        // Calculate the direction to the player
                        Vector3 directionToPlayer = enemyTransform.position - transform.position;
                        directionToPlayer.y = 0; // Ensure the zombie doesn't move up/down
                        //Face the player
                            //might be optimized on how the rotation and movement handles
                            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(directionToPlayer), Time.deltaTime * 5.0f);
                            
                        //if Distance is greater than meele distance, shoot
                        if (distanceToEnemy > meleeDistance)
                        {
                            
                            
                            //SHOOT
                            // Check if enough time has passed since the last shot
                            if (Time.time - lastShotTime >= shootingInterval && !isDead)
                            {
                                Shoot();
                                lastShotTime = Time.time;
                            }
                            // Move the zombie towards the player using physics
                            // Vector3 movement = directionToPlayer.normalized * movementSpeed * Time.deltaTime;
                            //_rigidbody.MovePosition(_rigidbody.position + movement);
                        }
                        else
                        {
                            // melee when it's too close to the enemy
                            animator.SetBool("isShoot",false);
                            _rigidbody.velocity = Vector3.zero;
                            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(directionToPlayer), Time.deltaTime * 5.0f);
                        }
                    }
                }  
    /*          
                ////ROTATE THE PLAYER-----------------
                if (movementDirection != Vector3.zero){
                    
                    Quaternion desiredRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
                    transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, _rotationSpeed * Time.deltaTime);
                }else {animator.SetBool("Running", false);}
 
                //Dash
                if (Input.GetButtonDown("Fire2") && powerUpCount > 0) //Right mouse button
                {
                    currentDashTime = 0;
                    powerUpCount=-1;
                }
                    
                if(currentDashTime < maxDashTime)
                {
                    animator.SetBool("Running", true);
                    moveDirection = transform.forward * dashDistance;
                    currentDashTime += dashStoppingSpeed;}
                else
                {
                    moveDirection = Vector3.zero;
                    //animator.SetBool("Running", false);
                }     */
            
            
            }else{
                // PLAYER DEAD
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Write data to the stream (e.g., synchronize position, rotation, or custom properties)

            // Serialize the position
            stream.SendNext(transform.position);

            // Serialize the rotation
            stream.SendNext(transform.rotation);

            // Serialize the grounded state
            stream.SendNext(_groundedPlayer);

            // Serialize the player velocity
            stream.SendNext(_playerVelocity);

            // Serialize the kicking player
            stream.SendNext(attackingPlayer);

        }else{
            // Read data from the stream (e.g., update position, rotation, or custom properties)

            // Deserialize the position
            transform.position = (Vector3)stream.ReceiveNext();

            // Deserialize the rotation
            transform.rotation = (Quaternion)stream.ReceiveNext();

            // Deserialize the grounded state
            _groundedPlayer = (bool)stream.ReceiveNext();

            // Deserialize the player velocity
            _playerVelocity = (Vector3)stream.ReceiveNext();

            // Deserialize the kicking player
            attackingPlayer = (Photon.Realtime.Player)stream.ReceiveNext();
        }
    }

    private void Shoot()
    {
        /*
        if (projectilePrefab != null)
        {
            GameObject newProjectile = Instantiate(projectilePrefab, transform.position + bulletoffset, transform.rotation);
            GameObject newMuzzle = Instantiate(muzzlePrefab, transform.position + muzzleOffset, transform.rotation);
            Destroy(newMuzzle,.5f);
            Rigidbody projectileRigidbody = newProjectile.GetComponent<Rigidbody>();
            animator.SetBool("isShoot",true);
            if (projectileRigidbody != null)
            {
                projectileRigidbody.velocity = transform.forward * projectileSpeed;
                animator.SetTrigger("Shot");
            }
        }*/
    }

    private float GetPlayerSpeed()
    {
        if (isPoweredUp)
        {
            return _playerSpeed * 2f;

        }
        else
        {
            return _playerSpeed;
        }
    }

    public void ActivatePowerUp()
    {
        isPoweredUp = true;
        powerUpTimer = powerUpDuration;
        _playerSpeed = _basePlayerSpeed * 2f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")) // Check if the collider belongs to the player
        {
            StartCoroutine(DelayBlood());
            if(playerHealth <100){
                playerHealth -= 10;
            }else if(playerHealth<=0){
                playerHealth = 0;
                isDead = true;
                animator.SetBool("isDead", true);
            }

            //Can also add other actions here, like decreasing enemy health, playing hit sounds, etc.
        }
    }

    IEnumerator DelayBlood()
    {
        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(0.1f);
        // Instantiate the blood splatter prefab at the hit position and rotation
        Quaternion randomRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
        Instantiate(bloodSplatterPrefab, transform.position + bloodoffset, randomRotation);
    }
}
