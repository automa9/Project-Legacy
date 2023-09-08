using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DA
{
    public class WeaponHolderSlot : MonoBehaviour
    {
        public Transform parentOverride;
        public bool isLeftHandSlot;
        public bool isRightHandSlot;

        public int megazineSize;

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
        
        //LOAD DATA
        public void LoadData(WeaponItem weaponItem)
        {
            Debug.Log(weaponItem.firearm.megazine);
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