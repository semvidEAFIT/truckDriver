using UnityEngine;
using System.Collections;
using Boomlagoon.JSON;
using System;
using System.IO;

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
	
	private Vector3 distanceManhatan;
	public Vector3 DistanceManhatan {
		get {
			return this.distanceManhatan;
		}
		set {
			distanceManhatan = value;
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

    private Difficulty difficulty = Difficulty.Easy;
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
        setSettings();
    }

    private void setSettings()
    {
		string settingsString;
		try{
			using(StreamReader sr = new StreamReader(Application.dataPath+"/playerConfiguration.txt")){
				settingsString = sr.ReadToEnd ();
			}
		}catch(Exception e){
			Debug.Log("Problem");
			TextAsset settingsFile = Resources.Load("configuration") as TextAsset;
			settingsString = settingsFile.text;
		}
		JSONObject settings = JSONObject.Parse(settingsString).GetObject(difficulty.ToString());
        nodeCount = (int)settings.GetNumber(Options.nodeCount);
        time = (float)settings.GetNumber(Options.time);
        errorMargin = (float)settings.GetNumber(Options.errorMargin);
        JSONObject dimensions = settings.GetObject(Options.dimensions);
        cityDimensions = new Vector2((float)dimensions.GetNumber("x"),(float)dimensions.GetNumber("y"));
    }
}

public enum Difficulty
{
    Easy = 0, Medium = 1, Hard = 2, Extreme = 3
}