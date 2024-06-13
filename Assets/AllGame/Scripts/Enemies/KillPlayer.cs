using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class KillPlayer : MonoBehaviour
{
    public GameObject restartMenu;

    private void OnCollisionEnter(Collision collision)
    {
        /*if (collision.collider.GetComponent<Player>() != null)
        {
            Destroy(collision.gameObject);
            Time.timeScale = 0f;

        }*/
        if (collision.collider.CompareTag("Player"))
        {
            Destroy(collision.gameObject);
            Time.timeScale = 0f;

        }


    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(other.gameObject);
            Time.timeScale = 0f;

            restartMenu.SetActive(true);

        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(other.gameObject);
            Time.timeScale = 0f;

            restartMenu.SetActive(true);

        }

    }
}
