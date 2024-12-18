// Updated HeatmapLoader.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HeatmapLoader : MonoBehaviour
{
    private const string PhpURL = "https://citmalumnes.upc.es/~andreums/GetHeatmapData.php";

    [SerializeField]
    private GameObject heatPointPrefab; // Prefab per representar cada punt de la heatmap

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
        Vector3 position = new Vector3(data.x, data.y, data.z);
        GameObject heatPoint = Instantiate(heatPointPrefab, position, Quaternion.identity);

        // Ajusta la mida segons la intensitat
        float intensity = Mathf.Clamp(data.frequency / 10f, 0.1f, 1f);
        heatPoint.transform.localScale = Vector3.one * intensity;

        // Ajusta el color segons la intensitat
        Renderer renderer = heatPoint.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = Color.Lerp(Color.blue, Color.red, intensity);
        }
    }

    [System.Serializable]
    private class HeatmapData
    {
        public float x, y, z;
        public int frequency;
    }

    [System.Serializable]
    private class HeatmapDataList
    {
        public List<HeatmapData> data;
    }
}
