using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class LobbyManager : MonoBehaviourPunCallbacks, IInRoomCallbacks, IMatchmakingCallbacks
{
    public TextMeshProUGUI[] playerName;
    public Button[] selectButton;

    public GameObject roomSettingPanel;
    public GameObject waitingPanel;

    public Button startButton;

    RoomOptions options;
    public int maxNumberOfPlayers = 6;
    public int minNumberOfPlayers = 2;

    private string level1RoomID = "Level1Room";

    private string roomName = string.Empty;
    private string namePlayer;

    void Start()
    {
        options = new RoomOptions
        {
            MaxPlayers = (byte)maxNumberOfPlayers,
            IsOpen = true,
            IsVisible = true
        };

        PhotonNetwork.GameVersion = "1";
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.AutomaticallySyncScene = true;
        JoinRoom("Level1Room");

    }

    void Update()
    {
        if (PhotonNetwork.IsMasterClient && CheckPlayersTeamSelected())
        {
            startButton.interactable = true;
        }
        else
        {
            startButton.interactable = false;
        }
    }
    void IMatchmakingCallbacks.OnJoinRoomFailed(short returnCode, string message)
    {
       Debug.Log("fail laaa connect");
    }


    public void JoinRoom(string room)
    {
        //must add check if room is > max player then create new room
        roomSettingPanel.SetActive(false);
        waitingPanel.SetActive(true);
        roomName = room;

        PhotonNetwork.JoinOrCreateRoom(roomName, options, TypedLobby.Default);
    }

    public bool CheckPlayersTeamSelected()
    {
        int count = 0;
        // Use to check if all players have selected the team
        for(int i = 0; i < playerName.Length; i++)
        {
            if(playerName[i].text != "Waiting for player...")
            {
                count++;
            }
        }

        if(count >= minNumberOfPlayers)
        {
            return true;
        }

        return false;
    }

    public void onClickToSelectTeam(int buttonIndex)
    {
        namePlayer = PlayerPrefs.GetString("username");
        PhotonNetwork.NickName = namePlayer;

        if (PhotonNetwork.IsMasterClient)
        {
            UpdatePlayerName(buttonIndex, namePlayer + " [Master]");
            photonView.RPC("SendPlayerName", RpcTarget.OthersBuffered, buttonIndex, namePlayer + " [Master]");
        }
        else
        {
            UpdatePlayerName(buttonIndex, namePlayer);
            photonView.RPC("SendPlayerName", RpcTarget.OthersBuffered, buttonIndex, namePlayer);
        }

        // Previous Selected Place
        if (PlayerPrefs.HasKey("previousIndex"))
        {
            int previousIndex = PlayerPrefs.GetInt("previousIndex");

            UpdatePlayerName(previousIndex, "Waiting for player...");
            photonView.RPC("SendPlayerName", RpcTarget.OthersBuffered, previousIndex, "Waiting for player...");
        }

        // Team Select Updated
        if(buttonIndex == 0 || buttonIndex == 1 || buttonIndex == 2)
        {
            PlayerPrefs.SetString("teamSelected", "Lightning Strikes");
        }
        else
        {
            PlayerPrefs.SetString("teamSelected", "Mighty Warriors");
        }

        PlayerPrefs.SetInt("previousIndex", buttonIndex);
    }

    public void LoadScene()
    {
        // Load the scene based on the room ID
        if (roomName == level1RoomID)
        {
            PhotonNetwork.LoadLevel("MultiLevel");
            Debug.Log(roomName);
        }
    }

    public override void OnConnectedToMaster() 
    {
        // callback function for when first connection is made
        PhotonNetwork.JoinLobby(TypedLobby.Default);
        Debug.Log("Connected");
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
       
    }

    public void ClickToLeffRoom()
    {
        PhotonNetwork.LeaveRoom();
        PlayerPrefs.DeleteKey("previousIndex");

        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < playerName.Length; i++)
            {
                if (playerName[i].text.Contains("[Master]"))
                {
                    UpdatePlayerName(i, "Waiting for player...");
                    photonView.RPC("SendPlayerName", RpcTarget.OthersBuffered, i, "Waiting for player...");
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < playerName.Length; i++)
            {
                if (playerName[i].text == namePlayer)
                {
                    UpdatePlayerName(i, "Waiting for player...");
                    photonView.RPC("SendPlayerName", RpcTarget.OthersBuffered, i, "Waiting for player...");
                    break;
                }
            }
        }

        roomSettingPanel.SetActive(true);
        waitingPanel.SetActive(false);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        for(int i = 0; i < playerName.Length; i++)
        {
            if(playerName[i].text == otherPlayer.NickName)
            {
                SendPlayerName(i, "Waiting for player..."); 
                break;
            }
        }  
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        // Handle player property updates
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        // Handle master client switch
        for (int i = 0; i < playerName.Length; i++)
        {
            if (playerName[i].text != newMasterClient.NickName && playerName[i].text.Contains("[Master]"))
            {
                SendPlayerName(i, "Waiting for player...");              
            }
            else if(newMasterClient.NickName == playerName[i].text)
            {
                string name = playerName[i].text + " [Master]";
                SendPlayerName(i, name);
               
            }
        }
    }

    void UpdatePlayerName(int index, string name)
    {
        playerName[index].text = name;
        GUIUtility.ExitGUI();
        if (name == "Waiting for player...")
        {
            selectButton[index].interactable = true;
        }
        else
        {
            selectButton[index].interactable = false;
            name = "Waiting for player...";
        }

    }

    [PunRPC]
    void SendPlayerName(int index, string name)
    {
        playerName[index].text = name;
        GUIUtility.ExitGUI();
        if (name == "Waiting for player...")
        {
            selectButton[index].interactable = true;
        }
        else
        {
            selectButton[index].interactable = false;
            name = "Waiting for player...";
        }    
    }

}
