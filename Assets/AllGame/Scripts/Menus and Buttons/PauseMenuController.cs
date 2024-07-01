using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuController : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject deathMenu;
    public GameObject tutorialPanel;
    public GameObject tutorialFirstButton;
    public GameObject pauseButton;
    public TMPro.TextMeshProUGUI tutorialText;

    public GameObject mainMenuButton;
    public GameObject startGameButton;

    private bool isPaused = false;
    private bool isDead = false;
    private bool isTutorialActive = false;

    private PlayerInput playerInput;

    [SerializeField] private Player player;
    [SerializeField] private GameObject pauseMenuFirstButton;
    [SerializeField] private GameObject deathMenuFirstButton;

    private int tutorialStep = 0;
    private string[] tutorialMessages = {
        "Bienvenido al juego!",
        "Usa el joystick izquierdo para moverte.",
        "Cuidado con los Carros y No te caigas al rio",
        "¡Buena suerte y diviértete!"
    };

    private Vector3 originalScale;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        if (playerInput != null)
        {
            playerInput.actions.FindAction("Pause").performed += TogglePauseMenu;
        }

        if (!PlayerPrefs.HasKey("HasSeenTutorial"))
        {
            ShowTutorial();
        }
        originalScale = tutorialFirstButton.transform.localScale;

        mainMenuButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(BackToMainMenu);
        startGameButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(StartGame);
    }

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        if (playerInput == null)
        {
            Debug.LogWarning("PlayerInput component not found.");
        }
        //ShowTutorial();
    }

    public void TogglePauseMenu(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            TogglePauseMenuLogic();
        }
    }

    public void TogglePauseMenuLogic()
    {
        if (isDead || isTutorialActive) return;

        isPaused = !isPaused;
        pauseMenu.SetActive(isPaused);

        if (isPaused)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }

        player.enabled = !isPaused;
        EventSystem.current.SetSelectedGameObject(isPaused ? pauseMenuFirstButton : null);
    }

    void OnSubmit(InputAction.CallbackContext context)
    {
        if (isTutorialActive)
        {
            return;
        }

        ShowNextTutorialMessage();
    }

    void ShowTutorial()
    {
        isTutorialActive = true;
        tutorialPanel.SetActive(true);
        ShowNextTutorialMessage();
        player.enabled = false;

        
        pauseButton.SetActive(false);
    }

    public void ShowNextTutorialMessage()
    {
        if (tutorialStep < tutorialMessages.Length)
        {
            StartCoroutine(ScaleButtonEffect(tutorialFirstButton, 0.1f, 1.2f));
            tutorialText.text = tutorialMessages[tutorialStep];
            tutorialStep++;
            EventSystem.current.SetSelectedGameObject(tutorialFirstButton);
        }
        else
        {
            EndTutorial();
        }
    }

    void EndTutorial()
    {
        tutorialPanel.SetActive(false);
        isTutorialActive = false;
        PlayerPrefs.SetInt("HasSeenTutorial", 1);
        PlayerPrefs.Save();
        Time.timeScale = 1f;
        player.enabled = true;

        pauseButton.SetActive(true);
    }

    public void ShowDeathMenu()
    {
        isDead = true;
        deathMenu.SetActive(true);
        Time.timeScale = 0f;
        player.enabled = false;
        EventSystem.current.SetSelectedGameObject(deathMenuFirstButton);
    }

    void OnDisable()
    {
        if (playerInput != null)
        {
            playerInput.actions.FindAction("Pause").performed -= TogglePauseMenu;
            playerInput.actions.FindAction("Submit").performed -= OnSubmit;
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    public void ResetTutorial()
    {
        PlayerPrefs.DeleteKey("HasSeenTutorial");
        tutorialStep = 0;
        ShowTutorial();
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    private IEnumerator ScaleButtonEffect(GameObject button, float duration, float targetScale)
    {
        float timer = 0f;
        Vector3 startScale = button.transform.localScale;
        Vector3 target = new Vector3(targetScale, targetScale, 1f);

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;
            button.transform.localScale = Vector3.Lerp(startScale, target, t);
            yield return null;
        }

        button.transform.localScale = target;
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(ScaleButtonEffectBack(button, 0.1f, originalScale));
    }

    private IEnumerator ScaleButtonEffectBack(GameObject button, float duration, Vector3 targetScale)
    {
        float timer = 0f;
        Vector3 startScale = button.transform.localScale;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;
            button.transform.localScale = Vector3.Lerp(startScale, targetScale, t);
            yield return null;
        }

        button.transform.localScale = targetScale;
    }
}
