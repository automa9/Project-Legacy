using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SG{
    public class PlayerAttacker : MonoBehaviour
    {
        AnimationHandler animationHandler;
        WeaponSlotManager weaponSlotManager;
        PlayerInventory playerInventory;
        PlayerControl playerControl;


        public void Awake()
        {
            animationHandler = GetComponent<AnimationHandler>();
            playerControl = GetComponentInParent<PlayerControl>();
            playerInventory =GetComponentInParent<PlayerInventory>();
            weaponSlotManager =GetComponent<WeaponSlotManager>();

        }

        public void HandleLightAttack(WeaponItem weapon)
        {
            animationHandler.PlayTargetAnimation(weapon.OH_Light_Attack_1, true);
            //Debug.Log(weapon.OH_Light_Attack_1);
        }

        public void HandleHeavyAttack(WeaponItem weapon)
        {
            //animationHandler.PlayTargetAnimation(weapon.OH_Heavy_Attack_1,true);
            //Debug.Log(weapon.OH_Heavy_Attack_1);
        }

        public void HandleRIghtHandAttackAction(){
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
