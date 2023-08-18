using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;

public class LegacyGame : MonoBehaviour
{
    PhotonView view;
    public TextMeshProUGUI timerText;
    public float countdownDuration = 5f;
    public float gameTimeDuration = 10f;
    public string goToScene;

    private bool isGameStarted = false;
    private string gold;
    public string sceneMP;

    private bool isPaused = false;
    public GameObject pauseMenu;


    private void Start()
    {
        //currentSceneName = SceneManager.GetActiveScene().name;
        view = GetComponent<PhotonView>();
        StartCoroutine(CountdownCoroutine());

        if (pauseMenu.active == true)
        {
            pauseMenu.SetActive(false);
        }
        
    }

    private IEnumerator CountdownCoroutine()
    {
        // Countdown
        float countdownTimer = countdownDuration;
        while (countdownTimer >= 1)
        {
            timerText.text = countdownTimer.ToString("0");
            yield return new WaitForSeconds(1f);
            countdownTimer--;
        }

        // Display "Start"
        timerText.text = "Start";
        isGameStarted = true;
        yield return new WaitForSeconds(1f);

        // Game Time
        float gameTimer = 0f;
        while (gameTimer <= gameTimeDuration)
        {
            if (isGameStarted)
            {
                timerText.text = FormatTime(gameTimer);
                gameTimer++;
            }
            yield return new WaitForSeconds(1f);
        }

        // Game Over
        timerText.text = "Game Over";
        isGameStarted = false;

        //Debug.Log(gold);
        SceneManager.LoadScene(goToScene);
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0; // Pause the game
            pauseMenu.SetActive(true); // Show pause menu
        }
        else
        {
            Time.timeScale = 1; // Unpause the game
            pauseMenu.SetActive(false); // Hide pause menu
        }
    }

    private void Update()
    {
        
        if (view.IsMine)
        {
            // Check if the game has started
            if (SceneManager.GetActiveScene().name == sceneMP)
            {
                if (!isGameStarted)
                {
                    managePlayerMP(false);
                }
                else
                {
                    managePlayerMP(true);
                }
            }
        }
}

    [PunRPC]
    private void managePlayerMP(bool status)
    {
        FindObjectOfType<PlayerControl>().enabled = status;
    }
}

