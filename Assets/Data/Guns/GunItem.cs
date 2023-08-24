using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG{

    public class GunItem : MonoBehaviour
    {
       public GameObject muzzleFlashFx;
       public string recoilAnimation;

       [Header("Gun Type")]
       public bool isShotgun;
       public bool isPistol;
       public bool isRifle;

       [Header("Gun Description")]
       [TextArea]
       public string gunDescription;

       public virtual void MuzzleFlashFx()
       {
            Debug.Log("Firing");
       }
    }
}