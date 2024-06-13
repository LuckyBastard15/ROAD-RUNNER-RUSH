using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement; // Necesario para cargar escenas
using System.Collections;

public class MenuController : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject deathMenu;
    public GameObject tutorialPanel;
    public GameObject tutorialFirstButton;
    public TMPro.TextMeshProUGUI tutorialText;

    public GameObject mainMenuButton; // Referencia al botón de salir al menú principal
    public GameObject startGameButton; // Referencia al botón de empezar partida

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
       //    ResetTutorial();

        // Agregar listener para el botón de salir al menú principal
        mainMenuButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(BackToMainMenu);

        // Agregar listener para el botón de empezar partida
        startGameButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(StartGame);
    }

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        if (playerInput == null)
        {
            Debug.LogError("PlayerInput component not found on the GameObject or its children.");
        }
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

    public void TogglePauseMenu()
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
        if (!isTutorialActive) return;

        // Solo avanzar en el tutorial si el tutorial está activo y no estamos pausados
        if (!isPaused && isTutorialActive)
        {
            ShowNextTutorialMessage();
        }
    }

    void ShowTutorial()
    {
        isTutorialActive = true;
        tutorialPanel.SetActive(true);
        ShowNextTutorialMessage();
        player.enabled = false; // Deshabilitar movimiento del personaje
    }

    public void ShowNextTutorialMessage()
    {
        if (tutorialStep < tutorialMessages.Length)
        {
            // Escalar el botón visualmente
            StartCoroutine(ScaleButtonEffect(tutorialFirstButton, 0.1f, 1.2f));

            // Mostrar el siguiente mensaje del tutorial
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
        player.enabled = true; // Habilitar movimiento del personaje
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

    // Método para volver al menú principal
    public void BackToMainMenu()
    {
        SceneManager.LoadScene("Menu"); 
    }

    // Método para empezar la partida
    public void StartGame()
    {
        SceneManager.LoadScene("Game"); 
    }

    // Corutina para escalar el botón
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

        // Asegurarse de que el botón tenga el tamaño objetivo al finalizar la corutina
        button.transform.localScale = target;

        // Esperar un momento
        yield return new WaitForSeconds(0.1f);

        // Regresar el botón a su tamaño original
        StartCoroutine(ScaleButtonEffectBack(button, 0.1f, originalScale));
    }

    // Corutina para devolver el botón a su tamaño original
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

        // Asegurarse de que el botón tenga el tamaño original al finalizar la corutina
        button.transform.localScale = targetScale;
    }
}
