using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DA
{
    public class ItemStatWindowUI : MonoBehaviour
    {
        public Text itemNameText;
        public Image itemIconImage;

        public void UpdateWeaponItemState(WeaponItem weapon)
        {
            //REFERENCING FOROM WEAPONITEM SCRIPT
            itemNameText.text = weapon.itemName;
            itemIconImage.sprite = weapon.itemIcon;
        }

        //UPDATE WEAPON ITEM STATS

        //UPDATE ARMOR ITEM STATS

        //UPDATE CONSUMABLE ITEM STATS

        //UPDATE RING ITEM STATS
    }  
}

