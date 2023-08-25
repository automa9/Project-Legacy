using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
   public int totalcoin;
   public int totalDistance;

   public bool[] levelUnlocked;
   public int qualitySettings;

   public GameData()
   {
    totalcoin = 0;
    totalDistance =0;
    levelUnlocked = new bool[5];
    levelUnlocked[0] = true;
    qualitySettings =1;
   }
}
