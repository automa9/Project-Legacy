using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    public class WeaponHolderSlot : MonoBehaviour
    {
        public Transform parentOverride;
        public bool isLeftHandSlot;
        public bool isRightHandSlot;

        public GameObject currentWeaponModel;

        public void UnloadWeapon(){
            if (currentWeaponModel!=null)
            {
                currentWeaponModel.SetActive(false);
            }
        } 

        public void UnloadWeaponAndDestroy(){
           if (currentWeaponModel!=null)
            {
                Destroy(currentWeaponModel);
            }
        }

        public void LoadWeaponModel(WeaponItem weaponItem)
        {
            //UNLOAD THE WEAPON AND DESTROY IT
            UnloadWeaponAndDestroy();

            if(weaponItem == null)
            {
                //UNLOAD THE WEAPON
                UnloadWeapon();
                return;
            }

            GameObject model = Instantiate(weaponItem.modelPrefab) as GameObject;

            if(model != null)
            {
                //PUT THE TRANSFORM AS THE TRANSFORM OF THE SCRIPT
                if(parentOverride !=null)
                {
                    model.transform.parent = parentOverride;
                }else
                {
                    model.transform.parent = transform;
                }

                model.transform.localPosition = Vector3.zero;
                model.transform.localRotation = Quaternion.identity;
                model.transform.localScale = Vector3.one;
            }
            currentWeaponModel = model;
            
        } 
    }
}