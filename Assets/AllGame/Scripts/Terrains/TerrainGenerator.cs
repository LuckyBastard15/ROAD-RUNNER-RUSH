using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    private int consecutiveGrassCount = 0; // Variable para Generar terreno de Hierba al empezar el juego

    [SerializeField] public int minDistanceFromPlayer; // Variable para medir la distancia mínima del terreno con el jugador para generar otra
    [SerializeField] private int maxTerrainCount; // Variable para medir la distancia máxima del terreno para no generar más 
    [SerializeField] private List<TerrainData> terrainDatas = new List<TerrainData>(); // Lista sobre datos del terreno existentes
    [SerializeField] private Transform terrainHolder; // GameObject en el juego para guardar los terrenos generados

    private List<GameObject> currentTerrain = new List<GameObject>(); // Lista de terrenos actualmente generados
    private int lastTerrainIndex = -1; // Índice del último tipo de terreno generado
    private int consecutiveTerrainCount = 0; // Contador de terrenos consecutivos del mismo tipo
    private const int maxConsecutiveTerrains = 2; // Máximo de repeticiones consecutivas permitidas
    public Vector3 currentPos = new Vector3(0, -1, 0); // Posición para generar los terrenos
    public Vector3 lastRemovedTerrainPos; // Posición del último terreno eliminado

    private Transform playerTransform; // Referencia al transform del jugador

    private void Start()
    {
        // Agregar los primeros cuatro terrenos de hierba
        for (int i = 0; i < 4; i++)
        {
            SpawnTerrain(true, new Vector3(0, 0, 0), 0); // 0 indica Grass en terrainDatas
        }

        // Luego, generar el resto de los terrenos aleatoriamente
        for (int i = 4; i < maxTerrainCount; i++)
        {
            SpawnTerrain(true, new Vector3(0, 0, 0)); // Se generan aleatoriamente
        }
        maxTerrainCount = currentTerrain.Count;

        // Asigna la referencia del jugador (asegúrate de que el objeto jugador tenga el tag "Player")
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (playerTransform != null)
        {
            // Llama a SpawnTerrain en cada frame con la posición actual del jugador
            SpawnTerrain(false, playerTransform.position);
        }
    }

    public void SpawnTerrain(bool isStart, Vector3 playerPos, int terrainType = -1)
    {
        if (currentPos.x - playerPos.x < minDistanceFromPlayer || isStart)
        {
            int whichTerrain;

            if (terrainType == -1)
            {
                if (consecutiveGrassCount >= 2) // Si se han generado más de 2 terrenos de hierba consecutivos
                {
                    // Se elige aleatoriamente un tipo de terreno diferente al Grass
                    whichTerrain = GetNonRepeatingRandomTerrain(1); // Comienza desde 1 para excluir Grass
                    consecutiveGrassCount = 0; // Reinicia el contador de terrenos de hierba consecutivos
                }
                else
                {
                    // Se elige aleatoriamente entre todos los terrenos disponibles
                    whichTerrain = GetNonRepeatingRandomTerrain(0); // Permite incluir Grass
                }
            }
            else
            {
                whichTerrain = terrainType;
                if (terrainType == 0) // Si el terreno generado es de hierba
                {
                    consecutiveGrassCount++; // Incrementa el contador de terrenos de hierba consecutivos
                }
                else
                {
                    consecutiveGrassCount = 0; // Reinicia el contador de terrenos de hierba consecutivos
                }
            }

            int terrainInSuccession = Random.Range(1, terrainDatas[whichTerrain].maxInSuccesion);
            for (int i = 0; i < terrainInSuccession; i++)
            {
                GameObject terrain = Instantiate(terrainDatas[whichTerrain].posibleTerrain[Random.Range(0, terrainDatas[whichTerrain].posibleTerrain.Count)], currentPos, Quaternion.identity, terrainHolder);
                currentTerrain.Add(terrain);
                if (!isStart)
                {
                    if (currentTerrain.Count > maxTerrainCount)
                    {
                        lastRemovedTerrainPos = currentTerrain[0].transform.position;
                        Debug.Log(lastRemovedTerrainPos);
                        Destroy(currentTerrain[0]);
                        currentTerrain.RemoveAt(0);
                    }
                }

                currentPos.x++;
            }

            // Actualiza el último terreno generado y el contador de consecutivos
            if (whichTerrain == lastTerrainIndex)
            {
                consecutiveTerrainCount++;
                if (consecutiveTerrainCount >= maxConsecutiveTerrains)
                {
                    consecutiveTerrainCount = 0; // Reinicia el contador de consecutivos
                    lastTerrainIndex = -1; // Fuerza la generación de un terreno diferente
                }
            }
            else
            {
                consecutiveTerrainCount = 1;
                lastTerrainIndex = whichTerrain;
            }
        }
    }

    private int GetNonRepeatingRandomTerrain(int startIndex)
    {
        int newTerrain;
        do
        {
            newTerrain = Random.Range(startIndex, terrainDatas.Count);
        } while (newTerrain == lastTerrainIndex);

        return newTerrain;
    }
}
