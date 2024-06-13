using System.Collections;
using UnityEngine;

public class CarScript : MonoBehaviour
{
    [SerializeField] private float speed;
    public bool isLog;

    private bool isDestroyed = false; 

    
    void Start()
    {
        StartCoroutine(DestroyCar());
    }

    private void Update()
    {
        if (!isDestroyed) 
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }

    private IEnumerator DestroyCar()
    {
        yield return new WaitForSeconds(20f);

        isDestroyed = true; 
        
        speed = 0f;

       
        yield return new WaitForSeconds(2f);

        Destroy(this.gameObject);
    }
}
    