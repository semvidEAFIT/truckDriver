using Boomlagoon.JSON;
using UnityEngine;

public class Options : MonoBehaviour
{
    public const string nodeCount = "nodeCount", time = "time", errorMargin = "errorMargin", dimensions = "dimensions";
    private string currentNodeCount, currentTime, currentErrorMargin, currentDimension, x, y;
    private JSONObject difficultySettings;//time, dimensions, nodeCount, errorMargin

    private void Awake()
    {
        if (!PlayerPrefs.HasKey("difficultySettings"))
        {
            createNonExistentSettings();
        }

        difficultySettings = JSONObject.Parse(PlayerPrefs.GetString("difficultySettings"));
        JSONObject medium = difficultySettings.GetObject(Difficulty.Medium.ToString());
        currentNodeCount = ((int)medium.GetNumber(nodeCount)).ToString();
        currentTime = ((float)medium.GetNumber(time)).ToString();
        currentErrorMargin = ((float)medium.GetNumber(errorMargin)).ToString();
        JSONObject vector = medium.GetObject(dimensions);
        x = ((int)vector.GetNumber("x")).ToString();
        y = ((int)vector.GetNumber("y")).ToString();
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
        vector.Add("x", new JSONValue(7.0d));
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
        int groupWidth = Screen.width / 3, groupHeight = 3*Screen.height /5;
        GUI.BeginGroup(new Rect(Screen.width/3, Screen.height/5, groupWidth, groupHeight));
        GUI.Label(new Rect(0, (groupHeight / 5) * 0, groupWidth/2, groupHeight/5), nodeCount);
        GUI.Label(new Rect(0, (groupHeight / 5) * 1, groupWidth / 2, groupHeight / 5), time);
        GUI.Label(new Rect(0, (groupHeight / 5) * 2, groupWidth/2, groupHeight/5), errorMargin);
        GUI.Label(new Rect(0, (groupHeight / 5) * 3, groupWidth / 2, groupHeight / 5), dimensions);
        currentNodeCount = GUI.TextField(new Rect(groupWidth/2, (groupHeight / 5) * 0, groupWidth / 2, groupHeight / 5), currentNodeCount);
        currentTime = GUI.TextField(new Rect(groupWidth / 2, (groupHeight / 5) * 1, groupWidth / 2, groupHeight / 5), currentTime);
        currentErrorMargin = GUI.TextField(new Rect(groupWidth / 2, (groupHeight / 5) * 2, groupWidth / 2, groupHeight / 5), currentErrorMargin);
        x = GUI.TextField(new Rect(groupWidth / 2, (groupHeight / 5) * 3, groupWidth / 4, groupHeight / 5), x);
        y = GUI.TextField(new Rect(groupWidth / 2 + groupWidth / 4, (groupHeight / 5) * 3, groupWidth / 4, groupHeight / 5), y);
        if(GUI.Button(new Rect(0, (groupHeight / 5) * 4,groupWidth,groupHeight/5),"Save")){
            JSONObject medium = new JSONObject();
            difficultySettings.Remove(Difficulty.Medium.ToString());
            medium.Add(nodeCount, new JSONValue(int.Parse(currentNodeCount)));
            medium.Add(time, new JSONValue(float.Parse(currentTime)));
            medium.Add(errorMargin, new JSONValue(float.Parse(currentErrorMargin)));
            JSONObject vector = new JSONObject();
            vector.Add("x", new JSONValue(int.Parse(x)));
            vector.Add("y", new JSONValue(int.Parse(y)));
            medium.Add("dimensions", vector);
            difficultySettings.Add(Difficulty.Medium.ToString(), medium);
            PlayerPrefs.SetString("difficultySettings", difficultySettings.ToString());
            PlayerPrefs.Save();
        }
        GUI.EndGroup();
    }
}