using System.Collections;
using System.Globalization;
using UnityEngine;
using UnityEngine.Networking;

public class DataSender : MonoBehaviour
{
    private const string PhpURL = "https://citmalumnes.upc.es/~andreums/DataReceiver.php";

    private void OnEnable()
    {
        // Subscripció a l'esdeveniment de posició del jugador
        EventTracker.OnPlayerPosition += SendPlayerPositionData;
    }

    private void OnDisable()
    {
        // Desubscripció de l'esdeveniment
        EventTracker.OnPlayerPosition -= SendPlayerPositionData;
    }

    private void SendPlayerPositionData(Vector3 position)
    {
        // Crida a la corrutina per enviar les dades
        StartCoroutine(SendDataToPHP(position));
    }

    private IEnumerator SendDataToPHP(Vector3 position)
    {
        // Prepara les dades per enviar
        WWWForm form = new WWWForm();
        form.AddField("data_type", "player_position");
        form.AddField("value0", "PlayerMove"); // Acció identificadora
        form.AddField("value1", position.x.ToString((CultureInfo.InvariantCulture)));
        form.AddField("value2", position.y.ToString((CultureInfo.InvariantCulture)));
        form.AddField("value3", position.z.ToString((CultureInfo.InvariantCulture)));

        // Envia les dades al servidor
        using (UnityWebRequest www = UnityWebRequest.Post(PhpURL, form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error enviant dades: {www.error}");
            }
            else
            {
                Debug.Log("Dades de posició enviades correctament");
            }
        }
    }
}
