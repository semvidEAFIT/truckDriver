using UnityEngine;
using System.Collections;
using Boomlagoon.JSON;

public class LevelSettings
{
    private static LevelSettings instance;
    public static LevelSettings Instance
    {
        get
        {
            if (instance == null){
                instance = new LevelSettings();
            }
            return instance;
        }
    }

    private int nodeCount;
    public int NodeCount
    {
        get { return nodeCount; }
    } 

    private float time;
    public float Time
    {
        get { return time; }
    }

    private float errorMargin;
    public float ErrorMargin
    {
        get { return errorMargin; }
    }

    private Vector2 cityDimensions;
    public Vector2 CityDimensions
    {
        get { return cityDimensions; }
    }

    private Difficulty difficulty = Difficulty.Medium;
    public Difficulty Difficulty
    {
        get { return difficulty; }
        set
        {
            difficulty = value;
            setSettings();
        }
    }

    private LevelSettings()
    {
        if (!PlayerPrefs.HasKey("difficultySettings") && PlayerPrefs.GetString("difficultySettings") != null && PlayerPrefs.GetString("difficultySettings") != "")
        {
            Options.createNonExistentSettings();
        }
        setSettings();
    }

    private void setSettings()
    {
        JSONObject settings = JSONObject.Parse(PlayerPrefs.GetString("difficultySettings")).GetObject(difficulty.ToString());
        nodeCount = (int)settings.GetNumber(Options.nodeCount);
        time = (float)settings.GetNumber(Options.time);
        errorMargin = (float)settings.GetNumber(Options.errorMargin);
        JSONObject dimensions = settings.GetObject(Options.dimensions);
        cityDimensions = new Vector2((float)dimensions.GetNumber("x"),(float)dimensions.GetNumber("y"));
    }
}

public enum Difficulty
{
    Easy = 0, Medium, Hard, Extreme
}