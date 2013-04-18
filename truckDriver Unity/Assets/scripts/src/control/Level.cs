using UnityEngine;
using System;
public class Level : MonoBehaviour
{
    public Builder builder;

    private const int daysPerLevel = 3;

    private Day[] days;
    public Day[] Days
    {
        get { return days; }
    }
    private int currentDay;
    public Day CurrentDay {
        get {
            if(currentDay < daysPerLevel){
                return days[currentDay];
            }else{
                return null;
            }
        }
    }
    private double budget;
    public double Budget
    {
        get { return budget; }
    }
    private float timePerDay;
    public float TimePerDay
    {
        get { return timePerDay; }
    }

    private static Level instance;

    public static Level Instance
    {
        get { return instance; }
    }

    void Awake() {
        if (instance == null)
        {
            instance = this;
        }
        else 
        {
            Debug.Log("Solo puede haber un level activo a la vez");
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        budget = 0.0d;
        days = new Day[daysPerLevel];
        days[0] = new Day(TSPSolver.generateCase(LevelSettings.Instance.NodeCount, LevelSettings.Instance.CityDimensions));
        budget += TSPSolver.calculateCost(days[0].Solution, days[0].TspCase);
        for (int i = 1; i < daysPerLevel; i++)
        {
            days[i] = new Day(TSPSolver.generateCase(LevelSettings.Instance.NodeCount, days[0].TspCase.Nodes[0], LevelSettings.Instance.CityDimensions));
            budget += TSPSolver.calculateCost(days[i].Solution, days[i].TspCase);
        }

        budget += budget * LevelSettings.Instance.ErrorMargin;
        timePerDay = LevelSettings.Instance.Time;
        currentDay = 0;
        loadDay();
        Player.Instance.CurrentDay = CurrentDay;
    }

    private void loadDay()
    {
        try
        {
            builder.buildCity(CurrentDay);
        }
        catch (Exception e) 
        {
            Debug.Log(e);
        }
    }

    public void nextDay(int[] playerSolution, double spentMoney) {
        budget -= spentMoney;
        currentDay++;
        if (currentDay < daysPerLevel)
        {
            Player.Instance.CurrentDay = CurrentDay;
            loadDay();
        }
        else 
        {
            //EndLevel
        }
        Debug.Log(budget);
    }
}