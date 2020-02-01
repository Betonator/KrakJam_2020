using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CanvasManager : MonoBehaviour
{
    [SerializeField]
    private House playerHouse1;
    [SerializeField]
    private House playerHouse2;

    public static CanvasManager canvasManager;

    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject gameFreezePanel;
    [SerializeField] private Text gameOverText;

    private void Awake()
    {
        if (canvasManager == null)
        {
            canvasManager = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        gameOverPanel.SetActive(false);
        gameFreezePanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EscapeToggle();
        }
    }



    public void CheckForGameFinish()
    {
        if(playerHouse1.houseLevel == playerHouse1.maxLevel)
        {
            ShowEndGame();
            gameOverText.text = "Player 1 has won!";
        }
        else if (playerHouse2.houseLevel == playerHouse2.maxLevel)
        {
            ShowEndGame();
            gameOverText.text = "Player 2 has won!";
        }
    }

    public void EscapeToggle()
    {
        gameFreezePanel.active = !gameFreezePanel.active;
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
            Cursor.visible = true;
        }
        else
        {
            Time.timeScale = 1;
            Cursor.visible = false;
        }
    }

    public void ShowEndGame()
    {
        Time.timeScale = 0;
        Cursor.visible = true;
        gameOverPanel.active = true;
    }

    public void Reload()
    {
        Destroy(this.gameObject);
        SceneManager.LoadScene(0);
    }
}
