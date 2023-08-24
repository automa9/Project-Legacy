using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    [CreateAssetMenu(menuName = "Items/Weapon Item")]
    public class WeaponItem : Item
    {
        public GameObject modelPrefab;
        public bool isUnarmed;

        [Header("Attack Animations")]

        public string OH_Light_Attack_1;

        [Header("Idle Animations")]
        public string Idle;

        [Header("Weapon Type")]
        public bool isShotgun;
        public bool isRifle;
        public bool isPistol;
        public bool isMeleeWeapon;
    }
}
