using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public GameObject startButton;
    public GameObject quitButton;
    public GameObject settingsButton;
    public GameObject mainMenuCanvas;
    public GameObject settingsCanvas;
    public GameObject firstSelectedButtonMainMenu;
    public GameObject firstSelectedButtonSettings;

    private PlayerInput playerInput;
    private LoadingScreenController loadingScreenController;
    private PersistentObject persistentObject;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        if (playerInput != null)
        {
            playerInput.actions["Submit"].performed += OnSubmit;
        }
        else
        {
        }

        if (startButton == null || quitButton == null || settingsButton == null || firstSelectedButtonMainMenu == null || firstSelectedButtonSettings == null)
        {
        }

        EventSystem.current.SetSelectedGameObject(firstSelectedButtonMainMenu);

        persistentObject = FindObjectOfType<PersistentObject>();
        if (persistentObject != null)
        {
            loadingScreenController = persistentObject.loadingScreenController;
        }
        else
        {
        }
    }

    private void OnSubmit(InputAction.CallbackContext context)
    {
        GameObject selectedObject = EventSystem.current.currentSelectedGameObject;
        if (selectedObject == startButton)
        {
            StartGame();
        }
        else if (selectedObject == quitButton)
        {
            QuitGame();
        }
        else if (selectedObject == settingsButton)
        {
            OpenSettings();
        }
    }

    public void StartGame()
    {
        Time.timeScale = 1f;

        if (loadingScreenController != null)
        {
            loadingScreenController.LoadScene("Game");
        }
        else
        {
            SceneManager.LoadScene("Game");
        }
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void OpenSettings()
    {
        mainMenuCanvas.SetActive(false);
        settingsCanvas.SetActive(true);
        EventSystem.current.SetSelectedGameObject(firstSelectedButtonSettings);
    }

    public void BackToMainMenu()
    {
        settingsCanvas.SetActive(false);
        mainMenuCanvas.SetActive(true);
        EventSystem.current.SetSelectedGameObject(firstSelectedButtonMainMenu);
    }

    void OnDisable()
    {
        if (playerInput != null)
        {
            playerInput.actions["Submit"].performed -= OnSubmit;
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}
