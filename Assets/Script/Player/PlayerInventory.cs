using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DA
{
    public class PlayerInventory : MonoBehaviour
    {
        WeaponSlotManager weaponSlotManager;

        public WeaponItem rightWeapon;
        public WeaponItem leftWeapon;

        public WeaponItem unarmedWeapon;

        public WeaponItem[] weaponsInRightHandSlots = new WeaponItem[1];
        public WeaponItem[] weaponsInLeftHandSlots = new WeaponItem[1];

        public int currentRightWeaponIndex = -1;
        public int currentLeftWeaponIndex = -1;

        private void Awake()
        {
            weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
        }

        private void Start()
        {  
            rightWeapon = unarmedWeapon;
            leftWeapon = unarmedWeapon;
        }

        public void ChangeWeapon(){
            currentRightWeaponIndex = currentRightWeaponIndex +1;

            if(currentRightWeaponIndex == 0 && weaponsInRightHandSlots[0] != null)
            {
                rightWeapon = weaponsInRightHandSlots[currentRightWeaponIndex];
                weaponSlotManager.LoadWeaponSlot(weaponsInRightHandSlots[currentRightWeaponIndex],false);
            }
            else if (currentRightWeaponIndex == 0 && weaponsInRightHandSlots[0]==null)
            {
                currentRightWeaponIndex = currentRightWeaponIndex +1;
            }

            else if(currentRightWeaponIndex == 1 && weaponsInRightHandSlots[1] != null)
            {
                rightWeapon = weaponsInRightHandSlots[currentRightWeaponIndex];
                weaponSlotManager.LoadWeaponSlot(weaponsInRightHandSlots[currentRightWeaponIndex],false);
            }
            else 
            {
                currentRightWeaponIndex = currentRightWeaponIndex +1;
            }

            if(currentRightWeaponIndex > weaponsInRightHandSlots.Length -1)
            {
                currentRightWeaponIndex =-1;
                rightWeapon = unarmedWeapon;
                weaponSlotManager.LoadWeaponSlot(unarmedWeapon,false);
            }
        }
    }
}
