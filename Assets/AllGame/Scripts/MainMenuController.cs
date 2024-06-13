using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public GameObject startButton;
    public GameObject quitButton;
    public GameObject firstSelectedButton;

    private PlayerInput playerInput;
    private LoadingScreenController loadingScreenController;
    private GameObject eventSystem; // Referencia al EventSystem de la escena principal
    private PersistentObject persistentObject; // Referencia al objeto persistente

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        if (playerInput != null)
        {
            playerInput.actions["Submit"].performed += OnSubmit;
        }
        else
        {
            Debug.LogError("PlayerInput component not found on the GameObject or its children.");
            return;
        }

        // Verificar que los botones están asignados
        if (startButton == null || quitButton == null || firstSelectedButton == null)
        {
            Debug.LogError("One or more buttons are not assigned in the inspector.");
            return;
        }

        // Establecer el primer botón seleccionado
        EventSystem.current.SetSelectedGameObject(firstSelectedButton);

        // Encontrar el objeto persistente y obtener la referencia al LoadingScreenController
        persistentObject = FindObjectOfType<PersistentObject>();
        if (persistentObject != null)
        {
            loadingScreenController = persistentObject.loadingScreenController;
        }
        else
        {
            Debug.LogError("PersistentObject not found in the scene.");
        }

        eventSystem = GameObject.Find("EventSystem");
    }

    void OnSubmit(InputAction.CallbackContext context)
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
    }

    public void StartGame()
    {
        Time.timeScale = 1f;

        // Desactivar el EventSystem de la escena principal
        if (eventSystem != null)
        {
            eventSystem.SetActive(false);
        }

        if (loadingScreenController != null)
        {
            loadingScreenController.LoadScene("Game");
        }
        else
        {
            Debug.LogError("LoadingScreenController not found in the scene.");
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

    void OnDisable()
    {
        if (playerInput != null)
        {
            playerInput.actions["Submit"].performed -= OnSubmit;
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}
