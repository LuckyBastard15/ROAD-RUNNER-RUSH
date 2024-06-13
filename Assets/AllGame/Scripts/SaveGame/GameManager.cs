using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private string saveFilePath;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Asegura que el objeto persista entre escenas
            saveFilePath = Path.Combine(Application.persistentDataPath, "savegame.json");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveGame(Vector3 playerPosition, List<TerrainState> terrains)
    {
        GameState gameState = new GameState
        {
            playerPosition = playerPosition,
            terrains = terrains
        };

        string json = JsonUtility.ToJson(gameState);
        File.WriteAllText(saveFilePath, json);
        Debug.Log("Game Saved!");
    }

    public GameState LoadGame()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            GameState gameState = JsonUtility.FromJson<GameState>(json);
            Debug.Log("Game Loaded!");
            return gameState;
        }
        else
        {
            Debug.LogWarning("Save file not found!");
            return null;
        }
    }
}
