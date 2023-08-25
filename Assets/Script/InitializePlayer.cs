using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InitializePlayer : MonoBehaviour
{
    public GameData gameData;
    public TextMeshProUGUI uiCoins;
    public TextMeshProUGUI uiDistance;

    private void Awake() 
    {
        gameData = SaveSystemBinary.Load();
        RefreshUi();
    }

    public void RefreshUi(){
        uiCoins.text = gameData.totalcoin.ToString() + " x";
        uiDistance.text = gameData.totalDistance.ToString() + " m";

    }
}
