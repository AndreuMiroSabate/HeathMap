using System.Collections;
using System.Globalization;
using UnityEngine;
using UnityEngine.Networking;

public class DataSender : MonoBehaviour
{
    private const string PhpURL = "https://citmalumnes.upc.es/~andreums/DataReciever.php";

    private void OnEnable()
    {
        // Subscripción a los eventos del EventTracker
        EventTracker.OnPlayerPosition += SendPlayerPositionData;
        EventTracker.OnPlayerDeath += SendPlayerDeath;
        EventTracker.OnPlayerDamaged += SendPlayerDamaged;
        EventTracker.OnPlayerBecomeVulnerable += SendPlayerBecomeVulnerable;
        EventTracker.OnPlayerHitInvulnerable += SendPlayerHitInvulnerable;
    }

    private void OnDisable()
    {
        // Desuscripción de los eventos
        EventTracker.OnPlayerPosition -= SendPlayerPositionData;
        EventTracker.OnPlayerDeath -= SendPlayerDeath;
        EventTracker.OnPlayerDamaged -= SendPlayerDamaged;
        EventTracker.OnPlayerBecomeVulnerable -= SendPlayerBecomeVulnerable;
        EventTracker.OnPlayerHitInvulnerable -= SendPlayerHitInvulnerable;
    }

    private void SendPlayerPositionData(Vector3 position)
    {
        StartCoroutine(SendDataToPHP(position, "PlayerMove"));
    }

    private void SendPlayerDeath(Vector3 position)
    {
        StartCoroutine(SendDataToPHP(position, "Death"));
    }

    private void SendPlayerDamaged(Vector3 position)
    {
        StartCoroutine(SendDataToPHP(position, "Damaged"));
    }

    private void SendPlayerBecomeVulnerable(Vector3 position)
    {
        StartCoroutine(SendDataToPHP(position, "Vulnerable"));
    }

    private void SendPlayerHitInvulnerable(Vector3 position)
    {
        StartCoroutine(SendDataToPHP(position, "HitInvulnerable"));
    }

    private IEnumerator SendDataToPHP(Vector3 position, string action)
    {
        // Prepara las datos para enviar
        WWWForm form = new WWWForm();
        form.AddField("data_type", "player_position");
        form.AddField("value0", action); // Acción identificadora
        form.AddField("value1", position.x.ToString(CultureInfo.InvariantCulture));
        form.AddField("value2", position.y.ToString(CultureInfo.InvariantCulture));
        form.AddField("value3", position.z.ToString(CultureInfo.InvariantCulture));

        // Envía los datos al servidor
        using (UnityWebRequest www = UnityWebRequest.Post(PhpURL, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error enviando datos: {www.error}");
            }
            else
            {
                Debug.Log("Datos enviados correctamente: " + action);
            }
        }
    }
}
