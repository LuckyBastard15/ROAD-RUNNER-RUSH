using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MainUIManager : MonoBehaviour
{
    public GameManager gameManager;
    public Button loadButton; // Referencia al bot�n de cargar en el canvas
    private PlayerInput playerInput;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        if (playerInput == null)
        {
            Debug.LogError("PlayerInput component not found on the GameObject or its children.");
        }

        // Asignar el m�todo OnLoadButtonClicked al bot�n de cargar
        loadButton.onClick.AddListener(OnLoadButtonClicked);
    }

    private void OnEnable()
    {
        if (playerInput != null)
        {
            playerInput.actions.FindAction("LoadGame").performed += OnLoadGameInputAction;
        }
    }

    private void OnDisable()
    {
        if (playerInput != null)
        {
            playerInput.actions.FindAction("LoadGame").performed -= OnLoadGameInputAction;
        }
    }

    public void OnLoadGameInputAction(InputAction.CallbackContext context)
    {
        // Simular clic en el bot�n de cargar
        loadButton.onClick.Invoke();
    }

    public void OnLoadButtonClicked()
    {
        gameManager.LoadGame();
    }
}
