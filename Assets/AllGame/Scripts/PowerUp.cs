using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public float initialActivationProbability = 0.5f;

    void Start()
    {
        float randomValue = Random.value;

        if (randomValue < initialActivationProbability)
        {
            ActivatePowerUp();
        }
        else
        {
            DeactivatePowerUp();
        }
    }

    void ActivatePowerUp()
    {
        Debug.Log("Power Up activado");
    }

    void DeactivatePowerUp()
    {
        Debug.Log("Power Up desactivado");
    }
}
