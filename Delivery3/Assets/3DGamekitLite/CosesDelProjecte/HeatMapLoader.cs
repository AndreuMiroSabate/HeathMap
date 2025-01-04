using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HeatmapLoader : MonoBehaviour
{
    [SerializeField]
    private GameObject heatPointPrefab; // Prefab para los puntos del mapa de calor

    private const string PhpURL = "https://citmalumnes.upc.es/~andreums/GetHeatmapData.php";

    // Diccionario para almacenar los puntos de calor por acci�n
    private Dictionary<string, List<GameObject>> heatmapPointsByAction = new Dictionary<string, List<GameObject>>();

    private void Start()
    {
        StartCoroutine(LoadHeatmapData());
    }

    private IEnumerator LoadHeatmapData()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(PhpURL))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error cargando datos: {www.error}");
            }
            else
            {
                string jsonData = www.downloadHandler.text;
                List<HeatmapData> heatmapData = JsonUtility.FromJson<HeatmapDataList>($"{{\"data\":{jsonData}}}").data;

                foreach (var data in heatmapData)
                {
                    GenerateHeatPoint(data);
                }
            }
        }
    }

    private void GenerateHeatPoint(HeatmapData data)
    {
        if (string.IsNullOrEmpty(data.action))
        {
            Debug.LogError("HeatmapData.Action es nulo o vac�o. Revisa los datos del servidor.");
            return;
        }
        Vector3 position = new Vector3(data.x, 10, data.z); // Ajustar la altura (y = 10)
        GameObject heatPoint = Instantiate(heatPointPrefab, position, Quaternion.identity);

        // Escalar el punto seg�n la intensidad
        float intensity = Mathf.Clamp(data.Heath / 10f, 0.1f, 1f);

        // Ajustar el color seg�n la acci�n
        Renderer renderer = heatPoint.GetComponent<Renderer>();
        if (renderer != null)
        {
            if (intensity < 0.5f)
            {
                renderer.material.color = Color.Lerp(Color.green, Color.yellow, intensity);
            }
            else
            {
                renderer.material.color = Color.Lerp(Color.green, Color.yellow, intensity);
            }

        }

        // A�adir el punto al diccionario
        if (!heatmapPointsByAction.ContainsKey(data.action))
        {
            heatmapPointsByAction[data.action] = new List<GameObject>();
        }
        heatmapPointsByAction[data.action].Add(heatPoint);
    }

    // M�todos para activar o desactivar los puntos por acci�n
    public void SetHeatmapVisibility(string action, bool isVisible)
    {
        if (string.IsNullOrEmpty(action))
        {
            Debug.LogError("La clave 'action' es nula o vac�a.");
            return;
        }

        if (heatmapPointsByAction.ContainsKey(action))
        {
            foreach (var point in heatmapPointsByAction[action])
            {
                point.SetActive(isVisible);
            }
        }
        else
        {
            Debug.LogWarning($"No se encontraron puntos para la acci�n '{action}'.");
        }
    }

    [System.Serializable]
    private class HeatmapData
    {
        public float x, z;
        public int Heath;
        public string action; // Nueva propiedad para la acci�n
    }

    [System.Serializable]
    private class HeatmapDataList
    {
        public List<HeatmapData> data;
    }
}


