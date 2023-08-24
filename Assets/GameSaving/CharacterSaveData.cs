using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG
{
    [System.Serializable]
    //since we want to reference this data for every save file  this script is not monobehabvior
    public class CharacterSaveData {
        
        // Start is called before the first frame update
        [Header("Character Name")]
        public string characterName;
   
    }
}