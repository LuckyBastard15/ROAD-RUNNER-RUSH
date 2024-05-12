using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    [SerializeField] private GameObject car;
    [SerializeField] private Transform spawnPos;
    [SerializeField] private float minSeparationTime;
    [SerializeField] private float maxSeparationTime;
    [SerializeField] private bool isRightSide;
    [SerializeField] private float destroyTime = 13f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnCar());
    }

    private IEnumerator SpawnCar()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minSeparationTime, maxSeparationTime));
            GameObject go = Instantiate(car, spawnPos.position, Quaternion.identity);
            
            if (!isRightSide)
            {
                go.transform.Rotate(new Vector3(0, 180, 0));
            }
            //StartCoroutine(DestroyCar(go));
           
        }

    }

    /*private IEnumerator DestroyCar(GameObject carObject)
    {
          yield return new WaitForSeconds(20f);
          Destroy(carObject);
    }*/

    // Update is called once per frame
    void Update()
    {
        
    }
}
