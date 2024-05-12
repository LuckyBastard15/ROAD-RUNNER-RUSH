using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarScript : MonoBehaviour
{
    [SerializeField] private float speed;
    public bool isLog;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyCar());

    }

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
    private IEnumerator DestroyCar()
    {
        yield return new WaitForSeconds(20f);
        Destroy(this.gameObject);
    }

}
