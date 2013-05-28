using Boomlagoon.JSON;
using UnityEngine;
using System;
using System.IO;

public class Options : MonoBehaviour
{
    public const string nodeCount = "nodeCount", time = "time", errorMargin = "errorMargin", dimensions = "dimensions";
    private string[,] current; 
    public Difficulty[] difficulties = {Difficulty.Easy, Difficulty.Medium, Difficulty.Hard, Difficulty.Extreme};
    public Data[] data = {Data.NodeCount, Data.Time, Data.ErrorMargin, Data.DimensionX, Data.DimensionY };

    private const double easyNodeCount = 5.0d, easyTime = 10 * 60 * 1000.0d, easyDimensionsX = 12d, easyDimensionsY = 13.0d, easyErrorMargin = 70.0d,
        mediumNodeCount = 10.0d, mediumTime = 10 * 60 * 1000.0d, mediumDimensionsX = 12.0d, mediumDimensionsY = 13.0d, mediumErrorMargin = 50.5d,
        hardNodeCount = 20.0d, hardTime = 10 * 60 * 1000.0d, hardDimensionsX = 12.0d, hardDimensionsY = 13.0d, hardErrorMargin = 20.0d,
        extremeNodeCount = 30.0d, extremeTime = 10 * 60 * 1000.0d, extremeDimensionsX = 12.0d, extremeDimensionsY = 13.0d, extremeErrorMargin = 10.0d;
    private JSONObject difficultySettings;//time, dimensions, nodeCount, errorMargin

    private void Awake()
    {
        current = new string[difficulties.Length, data.Length];
        TextAsset config = Resources.Load("configuration") as TextAsset;
        difficultySettings = JSONObject.Parse(config.text);
        
        for (int i = 0; i < difficulties.Length; i++)
        {
            JSONObject difficulty = difficultySettings.GetObject(difficulties[i].ToString());
            current[(int)difficulties[i], (int)Data.NodeCount] = ((int)difficulty.GetNumber(nodeCount)).ToString();
            current[(int)difficulties[i], (int)Data.Time] = ((float)difficulty.GetNumber(time)).ToString();
            current[(int)difficulties[i], (int)Data.ErrorMargin] = ((float)difficulty.GetNumber(errorMargin)).ToString();
            JSONObject vector = difficulty.GetObject(dimensions);
            current[(int)difficulties[i], (int)Data.DimensionX] = ((int)vector.GetNumber("x")).ToString();
            current[(int)difficulties[i], (int)Data.DimensionY] = ((int)vector.GetNumber("y")).ToString();
        }
    }

    //public static void createNonExistentSettings()
    //{
    //    JSONObject difficultySettings = new JSONObject();
    //    JSONObject easy = new JSONObject();
    //    easy.Add(time, new JSONValue(easyTime));
    //    easy.Add(nodeCount, new JSONValue(easyNodeCount));
    //    easy.Add(errorMargin, new JSONValue(easyErrorMargin));
    //    JSONObject vector = new JSONObject();
    //    vector.Add("x", new JSONValue(easyDimensionsX));
    //    vector.Add("y", new JSONValue(easyDimensionsY));
    //    easy.Add(dimensions, vector);
    //    difficultySettings.Add(Difficulty.Easy.ToString(), easy);

    //    JSONObject medium = new JSONObject();
    //    medium.Add(time, new JSONValue(mediumTime));
    //    medium.Add(nodeCount, new JSONValue(mediumNodeCount));
    //    medium.Add(errorMargin, new JSONValue(mediumErrorMargin));
    //    vector = new JSONObject();
    //    vector.Add("x", new JSONValue(mediumDimensionsX));
    //    vector.Add("y", new JSONValue(mediumDimensionsY));
    //    medium.Add(dimensions, vector);
    //    difficultySettings.Add(Difficulty.Medium.ToString(), medium);

    //    JSONObject hard = new JSONObject();
    //    hard.Add(time, new JSONValue(hardTime));
    //    hard.Add(nodeCount, new JSONValue(hardNodeCount));
    //    hard.Add(errorMargin, new JSONValue(hardErrorMargin));
    //    vector = new JSONObject();
    //    vector.Add("x", new JSONValue(hardDimensionsX));
    //    vector.Add("y", new JSONValue(hardDimensionsY));
    //    hard.Add(dimensions, vector);
    //    difficultySettings.Add(Difficulty.Hard.ToString(), hard);

    //    JSONObject extreme = new JSONObject();
    //    extreme.Add(time, new JSONValue(extremeTime));
    //    extreme.Add(nodeCount, new JSONValue(extremeNodeCount));
    //    extreme.Add(errorMargin, new JSONValue(extremeErrorMargin));
    //    vector = new JSONObject();
    //    vector.Add("x", new JSONValue(extremeDimensionsX));
    //    vector.Add("y", new JSONValue(extremeDimensionsY));
    //    extreme.Add(dimensions, vector);
    //    difficultySettings.Add(Difficulty.Extreme.ToString(), extreme);

    //    StreamWriter writer = new StreamWriter("assets/resources/configuration.txt");
    //    writer.Write(difficultySettings.ToString());
    //    writer.Flush();
    //    writer.Close();
    //}

    private void OnGUI()
    {
        Rect[] rects = new Rect[4];
        rects[0] = new Rect(Screen.width / 8, Screen.height / 6, 3 * Screen.width / 8 - 5, Screen.height / 3 - 5);
        rects[1] = new Rect(Screen.width / 2, Screen.height / 6, 3 * Screen.width / 8 - 5, Screen.height / 3 - 5);
        rects[2] = new Rect(Screen.width / 8, Screen.height / 2, 3 * Screen.width / 8 - 5, Screen.height / 3 - 5);
        rects[3] = new Rect(Screen.width / 2, Screen.height / 2, 3 * Screen.width / 8 - 5, Screen.height / 3 - 5);
        for (int i = 0; i < rects.Length; i++)
        {
            GUI.Box(rects[i], difficulties[i].ToString());
            GUI.BeginGroup(rects[i]);
            for (int j = 0; j < data.Length; j++)
            {
                GUI.Label(new Rect(10, (rects[i].height / (data.Length+1)) * (j+1), rects[i].width / 3 - 10, rects[i].height / (data.Length+1)),data[j].ToString());
                GUI.TextField(new Rect(rects[i].width / 3, (rects[i].height / (data.Length + 1)) * (j + 1), 2 * rects[i].width / 3 - 5, rects[i].height / (data.Length + 1) - 5), current[i, j]); 
            }
            GUI.EndGroup();
        }
        //Boton para guardar
        /*if(GUI.Button(new Rect(Screen.width/8, 5*Screen.height/6, 3*Screen.width/4, Screen.height/16), "Save")){
            difficultySettings = new JSONObject();
            try
            {
                for (int i = 0; i < difficulties.Length; i++)
                {
                    JSONObject difficulty = new JSONObject();
                    difficulty.Add(nodeCount, new JSONValue(int.Parse(current[(int)difficulties[i], (int)Data.NodeCount])));
                    difficulty.Add(time, new JSONValue(float.Parse(current[(int)difficulties[i], (int)Data.Time])));
                    difficulty.Add(errorMargin, new JSONValue(float.Parse(current[(int)difficulties[i], (int)Data.ErrorMargin])));
                    JSONObject vector = new JSONObject();
                    vector.Add("x", new JSONValue(int.Parse(current[(int)difficulties[i], (int)Data.DimensionX])));
                    vector.Add("y", new JSONValue(int.Parse(current[(int)difficulties[i], (int)Data.DimensionY])));
                    difficulty.Add(dimensions, vector);
                    difficultySettings.Add(difficulties[i].ToString(), difficulty);
                }
            }
            catch (Exception ex) {
                Debug.Log(ex);
                return;
            }
            PlayerPrefs.SetString("difficultySettings", difficultySettings.ToString());
            PlayerPrefs.Save();
        }*/
    }
}

public enum Data { 
    NodeCount = 0, Time, ErrorMargin, DimensionX, DimensionY 
}