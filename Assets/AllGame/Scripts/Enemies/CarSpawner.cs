using System.Collections;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    [SerializeField] private GameObject carPrefab;
    [SerializeField] private Transform spawnPos;
    [SerializeField] private float minSeparationTime;
    [SerializeField] private float maxSeparationTime;
    [SerializeField] private bool isRightSide;
    [SerializeField] private float destroyTime = 13f;

    private bool isPaused = false;

    void Start()
    {
        // Generar un enemigo al inicio
        SpawnCar();
        // Iniciar la rutina de generación continua
        StartCoroutine(SpawnCarRoutine());
    }

    private IEnumerator SpawnCarRoutine()
    {
        while (true)
        {
            if (!isPaused)
            {
                SpawnCar();
            }
            yield return new WaitForSeconds(Random.Range(minSeparationTime, maxSeparationTime));
        }
    }

    private void SpawnCar()
    {
        GameObject newCar = Instantiate(carPrefab, spawnPos.position, Quaternion.identity);
        if (!isRightSide)
        {
            newCar.transform.Rotate(new Vector3(0, 180, 0));
        }
        StartCoroutine(DestroyCar(newCar));
    }

    private IEnumerator DestroyCar(GameObject carObject)
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(carObject);
    }

    public void Pause(bool pause)
    {
        isPaused = pause;
    }
}
