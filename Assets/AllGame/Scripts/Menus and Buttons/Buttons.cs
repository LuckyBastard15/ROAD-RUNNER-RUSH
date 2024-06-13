using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Buttons : MonoBehaviour
{
    //public Button startButton;
    //public Button pauseButton;
    //public Button continueButton;
    public GameObject pauseMenu;
    [SerializeField] private Player player;


    private void Start()
    {
        //pauseMenu.SetActive(false);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
        Time.timeScale = 1;
        
    }

    /*public void PauseGame()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
    }*/

    public void ContinueGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        if (Time.timeScale == 1f)
        {
            player.enabled = true;
        }
    }

    
}
