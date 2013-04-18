using Boomlagoon.JSON;
using UnityEngine;

public class Options : MonoBehaviour
{
    public const string nodeCount = "nodeCount", time = "time", errorMargin = "errorMargin", dimensions = "dimensions";

    private JSONObject difficultySettings;//time, dimensions, nodeCount, errorMargin

    private void Awake()
    {
        if (!PlayerPrefs.HasKey("difficultySettings"))
        {
            createNonExistentSettings();
        }

        difficultySettings = JSONObject.Parse(PlayerPrefs.GetString("difficultySettings"));
    }

    public static void createNonExistentSettings()
    {
        JSONObject difficultySettings = new JSONObject();
        JSONObject easy = new JSONObject();
        easy.Add(time, new JSONValue(10 * 60 * 1000.0d));
        easy.Add(nodeCount, new JSONValue(5.0d));
        easy.Add(errorMargin, new JSONValue(0.5d));
        JSONObject vector = new JSONObject();
        vector.Add("x", new JSONValue(10.0d));
        vector.Add("y", new JSONValue(10.0d));
        easy.Add(dimensions, vector);
        difficultySettings.Add(Difficulty.Easy.ToString(), easy);

        JSONObject medium = new JSONObject();
        medium.Add(time, new JSONValue(10 * 60 * 1000.0d));
        medium.Add(nodeCount, new JSONValue(10.0d));
        medium.Add(errorMargin, new JSONValue(0.5d));
        vector = new JSONObject();
        vector.Add("x", new JSONValue(10.0d));
        vector.Add("y", new JSONValue(10.0d));
        medium.Add(dimensions, vector);
        difficultySettings.Add(Difficulty.Medium.ToString(), medium);

        JSONObject hard = new JSONObject();
        hard.Add(time, new JSONValue(10 * 60 * 1000.0d));
        hard.Add(nodeCount, new JSONValue(20.0d));
        hard.Add(errorMargin, new JSONValue(0.5d));
        vector = new JSONObject();
        vector.Add("x", new JSONValue(20.0d));
        vector.Add("y", new JSONValue(20.0d));
        hard.Add(dimensions, vector);
        difficultySettings.Add(Difficulty.Hard.ToString(), hard);
        PlayerPrefs.SetString("difficultySettings", difficultySettings.ToString());
        Debug.Log(difficultySettings.ToString());
        PlayerPrefs.Save();
    }

    private void OnGUI()
    {
        //edit difficulty settings
    }
}