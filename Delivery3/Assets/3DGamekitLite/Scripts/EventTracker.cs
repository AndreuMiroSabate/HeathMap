using System;
using System.Collections;
using UnityEngine;

public class EventTracker : MonoBehaviour
{
    public static Action<Vector3> OnPlayerPosition; // Esdeveniment per a la posició del jugador

    [SerializeField]
    private float trackingInterval = 1.0f; // Interval en segons per registrar la posició

    private void Start()
    {
        // Inicia la corrutina per fer seguiment de la posició
        StartCoroutine(TrackPlayerPosition());
    }

    private IEnumerator TrackPlayerPosition()
    {
        while (true) // La corrutina funciona indefinidament
        {
            // Obté la posició actual del jugador
            Vector3 currentPosition = transform.position;

            // Crida l'esdeveniment per enviar la posició
            OnPlayerPosition?.Invoke(currentPosition);

            // Espera l'interval abans de la següent execució
            yield return new WaitForSeconds(trackingInterval);
        }
    }
}

