using UnityEngine;
using System.IO;
using NUnit.Framework;
using System.Collections.Generic;

public class DatasetLoader : MonoBehaviour
{
    public List<PlayerData> DataSet { get; private set; }

    private void Awake()
    {
        LoadData();
    }
    public void LoadData()
    {
        DataSet = new List<PlayerData>();

        string path = Path.Combine(Application.streamingAssetsPath, "player_dataset.csv");

        if (!File.Exists(path))
        {
            Debug.Log($"Error,dataset no encontrado");
            return;
        }

        string[] lines = File.ReadAllLines(path);

        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = lines[i].Split(',');

            PlayerData player = new PlayerData(int.Parse(values[0]),
                float.Parse(values[1]),
                float.Parse(values[2]),
                float.Parse(values[3]),
                values[4]);
            DataSet.Add(player);
        }
        Debug.Log($"Dataset cargado: {DataSet.Count} registros");
    }
}
