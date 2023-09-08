using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DA
{
    [CreateAssetMenu(menuName = "Items/Firearm")]
    public class Firearm : WeaponItem
    {
        public float baseDamage;
        public float projectileVelocity;
        Rigidbody rigidbody;
        public GameObject weaponFx;
        public string weaponAnimation;
        public AudioClip clip;
        public AudioSource audioSource;
        public int megazine;

        public Transform muzzlePosition;

        WeaponItem weaponItem;

        public void Start()
        {
            muzzlePosition = modelPrefab.GetComponent<MuzzlePos>().transform;
        }
        
        public void SpawnBullet(AnimationHandler animationHandler, PlayerStats playerStats, WeaponSlotManager weaponSlotManager)
        {
            // GameObject instantiatefX = Instantiate(weaponFx,weaponSlotManager.rightHandSlot.transform);
            //instantiatefX.gameObject.transform.localScale = new Vector3(1,1,1);
            GameObject newProjectile = Instantiate(weaponFx, muzzlePosition);
            animationHandler.PlayTargetAnimation(weaponAnimation,true);
            audioSource.PlayOneShot(clip, 0.5f);
            //Destroy(instantiatefX,.5f);
        }
    }
}