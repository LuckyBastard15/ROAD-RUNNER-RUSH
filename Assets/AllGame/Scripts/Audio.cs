using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{

    private float raycastDistance = 5f;
    public AudioClip detectionSound;
    private float volume = 4f;

    // Referencia al AudioSource
    private AudioSource audioSource;

    private void Start()
    {
        // Obtener o agregar AudioSource al objeto
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, raycastDistance))
        {
            if (hit.collider.CompareTag("Player"))
            {
                PlayDetectionSound();
            }
        }
        Debug.DrawRay(transform.position, transform.forward * raycastDistance, Color.red);
    }

    void PlayDetectionSound()
    {
        // Reproducir el sonido
        if (detectionSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(detectionSound, volume);
        }
    }

}
