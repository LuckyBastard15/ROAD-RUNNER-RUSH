using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameUIManager : MonoBehaviour
{
    public GameManager gameManager;
    public Button saveButton; // Referencia al bot�n de guardar en el canvas
    private PlayerInput playerInput;
    [SerializeField] private Player player; // Referencia al objeto del jugador
    private List<TerrainState> terrains; // Referencia a los terrenos

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        if (playerInput == null)
        {
            Debug.LogError("PlayerInput component not found on the GameObject or its children.");
        }

        // Asignar el m�todo OnSaveButtonClicked al bot�n de guardar
        saveButton.onClick.AddListener(OnSaveButtonClicked);
    }

    private void OnEnable()
    {
        if (playerInput != null)
        {
            playerInput.actions.FindAction("SaveGame").performed += OnSaveGameInputAction;
        }
    }

    private void OnDisable()
    {
        if (playerInput != null)
        {
            playerInput.actions.FindAction("SaveGame").performed -= OnSaveGameInputAction;
        }
    }

    public void OnSaveGameInputAction(InputAction.CallbackContext context)
    {
        // Simular clic en el bot�n de guardar
        saveButton.onClick.Invoke();
    }

    public void OnSaveButtonClicked()
    {
        // Recolectar la posici�n del jugador y los estados de los terrenos antes de guardar
        Vector3 playerPosition = player.transform.position;
        terrains = CollectTerrainStates();

        gameManager.SaveGame(playerPosition, terrains);
    }

    private List<TerrainState> CollectTerrainStates()
    {
        List<TerrainState> terrainStates = new List<TerrainState>();
        // Recolectar los datos de los terrenos aqu� (ejemplo)
        Terrain[] allTerrains = FindObjectsOfType<Terrain>();
        foreach (var terrain in allTerrains)
        {
            TerrainState terrainState = new TerrainState
            {
                position = terrain.transform.position,
                terrainType = terrain.name // O alg�n otro identificador �nico
            };
            terrainStates.Add(terrainState);
        }
        return terrainStates;
    }
}
