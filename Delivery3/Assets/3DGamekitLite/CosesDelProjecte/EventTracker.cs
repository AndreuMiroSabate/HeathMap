using Gamekit3D;
using System;
using System.Collections;
using UnityEngine;

public class EventTracker : MonoBehaviour
{
    public static Action<Vector3> OnPlayerPosition; // Esdeveniment per a la posici� del jugador
    public static Action<Vector3> OnPlayerDeath;
    public static Action<Vector3> OnPlayerDamaged;
    public static Action<Vector3> OnPlayerHitInvulnerable;
    public static Action<Vector3> OnPlayerBecomeVulnerable;

    public Damageable damageable;

    [SerializeField]
    private float trackingInterval = 1.0f; // Interval en segons per registrar la posici�

    private void Start()
    {
        // Inicia la corrutina per fer seguiment de la posici�
        StartCoroutine(TrackPlayerPosition());

        // Subscripci�n a los eventos de Damageable
        if (damageable != null)
        {
            damageable.OnDeath.AddListener(() => PlayerEventTriggered(OnPlayerDeath));
            damageable.OnReceiveDamage.AddListener(() => PlayerEventTriggered(OnPlayerDamaged));
            damageable.OnHitWhileInvulnerable.AddListener(() => PlayerEventTriggered(OnPlayerHitInvulnerable));
            damageable.OnBecomeVulnerable.AddListener(() => PlayerEventTriggered(OnPlayerBecomeVulnerable));
        }
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

    private void PlayerEventTriggered(Action<Vector3> playerEvent)
    {
        if (playerEvent != null)
        {
            Vector3 currentPosition = transform.position;
            playerEvent.Invoke(currentPosition);
        }
    }
}
