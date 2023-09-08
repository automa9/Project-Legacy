using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DA
{
    [CreateAssetMenu(menuName = "Items/Weapon Item")]
    public class WeaponItem : Item
    {
        public GameObject modelPrefab;
        public bool isUnarmed;

        public Firearm firearm;

        [Header("Attack Animations")]

        public string attackAnim;

        [Header("Idle Animations")]
        public string Idle;

        [Header("Weapon Type")]
        public bool isShotgun;
        public bool isRifle;
        public bool isPistol;
        public bool isMeleeWeapon;
    }
}
