using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DA
{
    public class PlayerAttacker : MonoBehaviour
    {
        AnimationHandler animationHandler;
        WeaponSlotManager weaponSlotManager;
        PlayerInventory playerInventory;
        PlayerControl playerControl;
        Firearm firearm;


        public void Awake()
        {
            animationHandler = GetComponent<AnimationHandler>();
            playerControl = GetComponentInParent<PlayerControl>();
            playerInventory =GetComponentInParent<PlayerInventory>();
            weaponSlotManager =GetComponent<WeaponSlotManager>();
        }

        public void HandleLightAttack(WeaponItem weapon)
        {
            animationHandler.PlayTargetAnimation(weapon.attackAnim, true);
            //Debug.Log(weapon.OH_Light_Attack_1);
        }

        public void HandleGun(Firearm firearm)
        {
            animationHandler.PlayTargetAnimation(firearm.attackAnim, true);
            //Debug.Log(weapon.OH_Light_Attack_1);
        }

        public void HandleRightHandAttackAction(){
            if(playerInventory.rightWeapon.isMeleeWeapon)
            {
                //Handle Melee Action
                animationHandler.PlayTargetAnimation("isUsingRightHand", true);
            }
            else if (playerInventory.rightWeapon.isRifle || playerInventory.rightWeapon.isPistol || playerInventory.rightWeapon.isShotgun)
            {
                //Handle Rifle and give player inventory right handed weapon
                PerformGunAction(playerInventory.rightWeapon);
            }
        }

        private void PerformGunAction(WeaponItem weapon){
            if (weapon.isRifle){
                
            }
        }
    }
}
