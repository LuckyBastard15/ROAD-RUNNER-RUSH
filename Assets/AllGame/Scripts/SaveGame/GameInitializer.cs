using UnityEngine;
using System.Collections.Generic;

public class GameInitializer : MonoBehaviour
{
    private void Start()
    {
        GameState gameState = GameManager.Instance.LoadGame();
        if (gameState != null)
        {
            ApplyGameState(gameState);
        }
    }

    private void ApplyGameState(GameState gameState)
    {
        // Encuentra el jugador y actualiza su posición
        Player player = FindObjectOfType<Player>();
        if (player != null)
        {
            player.transform.position = gameState.playerPosition;
        }
        else
        {
            Debug.LogWarning("Player object not found!");
        }

        // Elimina todos los terrenos actuales
        foreach (var existingTerrain in FindObjectsOfType<Terrain>())
        {
            Destroy(existingTerrain.gameObject);
        }

        // Crea o reposiciona los terrenos según el estado cargado
        foreach (var terrainState in gameState.terrains)
        {
            // Suponiendo que tienes una manera de instanciar terrenos por tipo
            GameObject terrainPrefab = Resources.Load<GameObject>(terrainState.terrainType);
            if (terrainPrefab != null)
            {
                GameObject terrainObject = Instantiate(terrainPrefab, terrainState.position, Quaternion.identity);
            }
            else
            {
                Debug.LogWarning("Terrain prefab not found for type: " + terrainState.terrainType);
            }
        }
    }
}
