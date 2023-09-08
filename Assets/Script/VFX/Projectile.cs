using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

namespace DA{
    public class Projectile : MonoBehaviour
    {
        public float timer=0.01f;
        public float muzzletimer = 0.10f;
        private float t;
        
        //public GameObject muzzle;
        private MeshRenderer projectile;
        public GameObject muzzle;
        private GameObject player;
        private Rigidbody rigidbody;
        private float speed;

        // Start is called before the first frame update

        void Awake()
        {
            if(this != null){
                projectile = GetComponent<MeshRenderer>();
                rigidbody = GetComponent<Rigidbody>();
                player = GameObject.FindGameObjectWithTag("Player");
                speed = player.GetComponent<PlayerControl>().projectileSpeed;
                //muzzle = this.GetComponentsInChildren<GameObject>()
            }
        }

        async void Start(){ 
            if(this !=null){
                projectile.enabled = false;
                muzzle.SetActive(true);
                
                await Task.Delay(100);
                muzzle.SetActive(false);

                Debug.Log("Printed Shoot");
                projectile.enabled = true;
                rigidbody.velocity = transform.forward * speed;

                //await Task.Delay(1000);
                //Destroy(this); 
            }
        }
    }
}
