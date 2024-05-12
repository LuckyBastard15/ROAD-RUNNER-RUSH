using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    private int consecutiveGrassCount = 0; // Variable para Generar terreno de Hierba al empezar el juego


    [SerializeField] private int minDistanceFromPlayer; // Variabke para medir la distancia min del terreno con el jugador para generar otra
    [SerializeField] private int maxTerrainCount; // Variable para medir la distancia maxima del terreno para no generar mas 
    [SerializeField] private List<TerrainData> terrainDatas = new List<TerrainData>(); //lista sobre datos del terreno existentes
    [SerializeField] private Transform terrainHolder; // Gameobject in game para guardar los terrenos generados

    private List<GameObject> currentTerrain = new List<GameObject>(); //
    public Vector3 currentPos = new Vector3(0, -1, 0);// posicion para generar los terrenos


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
    }


    /*private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            SpawnTerrain(false, new Vector3(0,0,0));
        }
    }*/


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
                    whichTerrain = Random.Range(0, terrainDatas.Count);
                    while (whichTerrain == 0) // Se asegura de que no se elija hierba nuevamente
                    {
                        whichTerrain = Random.Range(0, terrainDatas.Count);
                    }
                    consecutiveGrassCount = 0; // Reinicia el contador de terrenos de hierba consecutivos
                }
                else
                {
                    // Se elige aleatoriamente entre todos los terrenos disponibles
                    whichTerrain = Random.Range(0, terrainDatas.Count);
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
                        Destroy(currentTerrain[0]);
                        currentTerrain.RemoveAt(0);
                    }
                }

                currentPos.x++;
            }
        }
    }


}


