using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HeatmapLoader : MonoBehaviour
{
    [SerializeField]
    private GameObject heatPointPrefab; // Assignar el prefab per als punts del mapa

    private const string PhpURL = "https://citmalumnes.upc.es/~andreums/GetHeatmapData.php";

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
                Debug.LogError($"Error carregant dades: {www.error}");
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
        Vector3 position = new Vector3(data.x, 0, data.z); // Ajustar l'alçada (y = 0)
        GameObject heatPoint = Instantiate(heatPointPrefab, position, Quaternion.identity);

        // Escalar el punt segons la intensitat
        float intensity = Mathf.Clamp(data.Heath / 5f, 0.1f, 1f);
        

        // Ajustar el color segons la intensitat
        Renderer renderer = heatPoint.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = Color.Lerp(Color.blue, Color.red, intensity);
        }
    }

    [System.Serializable]
    private class HeatmapData
    {
        public float x, z;
        public int Heath;
    }

    [System.Serializable]
    private class HeatmapDataList
    {
        public List<HeatmapData> data;
    }
}

