using System;
using System.Collections;
using UnityEngine;

public class EventTracker : MonoBehaviour
{
    public static Action<Vector3> OnPlayerPosition; // Esdeveniment per a la posici� del jugador

    [SerializeField]
    private float trackingInterval = 1.0f; // Interval en segons per registrar la posici�

    private void Start()
    {
        // Inicia la corrutina per fer seguiment de la posici�
        StartCoroutine(TrackPlayerPosition());
    }

    private IEnumerator TrackPlayerPosition()
    {
        while (true) // La corrutina funciona indefinidament
        {
            // Obt� la posici� actual del jugador
            Vector3 currentPosition = transform.position;

            // Crida l'esdeveniment per enviar la posici�
            OnPlayerPosition?.Invoke(currentPosition);

            // Espera l'interval abans de la seg�ent execuci�
            yield return new WaitForSeconds(trackingInterval);
        }
    }
}

