using UnityEngine;
using System;
public class Level : MonoBehaviour
{
    public Builder builder;	public Builder Builder {
		get {
			return this.builder;
		}
	}
    private const int daysPerLevel = 3;

    private Day[] days;
    public Day[] Days
    {
        get { return days; }
    }
    private int currentDay;
	public int CurrentDayNumber{
		get{return currentDay;}
	}
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
    private bool lostLevel;

    public static Level Instance
    {
        get { return instance; }
    }
	
	private bool onDayReportScreen;
	private bool onLevelReportScreen;
	private float totalTimeSpent;
	private float timeSpent;
	private double totalBudgetSpent;
	private double totalLevelBudget;
	private double totalSolutionBudget;
	private bool calculatedTimeSpent;

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
		Debug.Log(LevelSettings.Instance.Difficulty);
        budget = 0.0d;
		timeSpent = 0;
		totalSolutionBudget=0;
		totalBudgetSpent = 0.0d;
		calculatedTimeSpent=false;
		onLevelReportScreen=false;
		onDayReportScreen=false;
        days = new Day[daysPerLevel];
        days[0] = new Day(TSPSolver.generateCase(LevelSettings.Instance.NodeCount, LevelSettings.Instance.CityDimensions));
        double cost = TSPSolver.calculateCost(days[0].Solution, days[0].TspCase);
		budget += cost;
		Debug.Log("Costo "+ 0 + ": "+cost);
        for (int i = 1; i < daysPerLevel; i++)
        {
            days[i] = new Day(TSPSolver.generateCase(LevelSettings.Instance.NodeCount, days[0].TspCase.Nodes[0], LevelSettings.Instance.CityDimensions));
            cost = TSPSolver.calculateCost(days[i].Solution, days[i].TspCase);
			Debug.Log("Costo "+ i + ": "+cost);
			budget += cost;
        }
		
		Debug.Log("Optimo:"+budget);
        budget += budget * (LevelSettings.Instance.ErrorMargin / 100.0f);
        Debug.Log("Maximo:"+budget);
		timePerDay = LevelSettings.Instance.Time;
        currentDay = 0;
		totalLevelBudget=budget;
        loadDay();
		Player.Instance.truck = builder.truck;
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
		Debug.Log("SpentMoney = "+spentMoney+", Budget = "+budget);
        currentDay++;
		calculatedTimeSpent=false;
        if (currentDay < daysPerLevel)
        {
            loadDay();
            Player.Instance.CurrentDay = CurrentDay;
        }
        else 
        {
            Debug.Log(budget);
            if (budget > 0.0d)
            {
               if(LevelSettings.Instance.Difficulty != Difficulty.Extreme){
                   LevelSettings.Instance.Difficulty = (Difficulty)(((int)LevelSettings.Instance.Difficulty)+1);
                   Application.LoadLevel("Game");
               }
            }
            else 
            {
                lostLevel = true;
            }
        }
    }
	
	public void showDayReportScreen(){
		if(budget<0.0d){
			lostLevel=true;
		} else {
			if(!calculatedTimeSpent){
				onDayReportScreen=true;
				totalTimeSpent+= Time.timeSinceLevelLoad;
				timeSpent= Time.timeSinceLevelLoad;
				totalBudgetSpent+= Player.Instance.SpentMoney;
				totalSolutionBudget+= TSPSolver.calculateCost(CurrentDay.Solution, CurrentDay.TspCase);
				calculatedTimeSpent=true;
			}
		}
	}
    void OnGUI() {
		if(lostLevel){
            if(GUI.Button(new Rect(Screen.width/3, Screen.height/2 - 5 - Screen.height/4, Screen.width/3, Screen.height/4), "Retry")){
                Application.LoadLevel("Game");
            }
            if (GUI.Button(new Rect(Screen.width / 3, Screen.height / 2, Screen.width / 3, Screen.height / 4), "Quit"))
            {
                Application.LoadLevel("Main Menu");
            }
        } else {
			if(onLevelReportScreen){
				GUI.BeginGroup(new Rect(Screen.width/4 + Screen.width/8, Screen.height/4, 2*Screen.width/8, Screen.height/3));
				GUI.Box(new Rect(0, 0, 2 * Screen.width / 8, Screen.height / 3), "");
				GUI.Label(new Rect(80, 10, 200, 20), "Level Complete");
				GUI.Label(new Rect(30, 50, 200, 20), "Total budget remaining: " + budget);
				GUI.Label(new Rect(30, 75, 200, 20), "Total time spent: " + (int)totalTimeSpent);
				GUI.Label(new Rect(30, 100, 200, 20), "Total budget spent: " + totalBudgetSpent);
				GUI.Label(new Rect(30, 124, 200, 20), "Best solution's budget: " + totalSolutionBudget);
				if(GUI.Button(new Rect(30, 180, 100, 50), "Main Menu")){
					Application.LoadLevel("Main Menu");
				}
				if(GUI.Button(new Rect(140, 180, 100, 50), "Next Level")){
					onLevelReportScreen=false;
					Player.Instance.NextLevel();
				}
				GUI.EndGroup();
			}
			if(onDayReportScreen){
				GUI.BeginGroup(new Rect(Screen.width/4 + Screen.width/8, Screen.height/4, 2*Screen.width/8, Screen.height/3));
				GUI.Box(new Rect(0, 0, 2 * Screen.width / 8, Screen.height / 3), "");
				GUI.Label(new Rect(110, 10, 200, 20), "Day " + (currentDay+1));
				GUI.Label(new Rect(30, 50, 200, 20), "Time Spent: " +  (int)timeSpent);
				GUI.Label(new Rect(30, 75, 200, 20), "Budget Remaining: " + budget);
				GUI.Label(new Rect(30, 100, 200, 20), "Budget Spent this day: " + Player.Instance.SpentMoney);
				GUI.Label(new Rect(30, 125, 200, 20), "Best solution's budget: " + TSPSolver.calculateCost(CurrentDay.Solution, CurrentDay.TspCase));
				if (currentDay+1 >= daysPerLevel){
		            if(GUI.Button(new Rect(75, 180, 100, 50), "Continue")){
						//show level report screen
						onDayReportScreen=false;
						onLevelReportScreen=true;
					}
		        } else {
					if(GUI.Button(new Rect(75, 180, 100, 50), "Next Day")){
						Player.Instance.NextLevel();
						onDayReportScreen=false;
					}
				}
				
				GUI.EndGroup();
			}
			
			float countdownTimer = timePerDay - Time.timeSinceLevelLoad;
			if(countdownTimer <= 0){
				lostLevel=true;
			}
			double money = budget - Player.Instance.SpentMoney;
			GUI.Label(new Rect(Screen.width/20,Screen.height/200,Screen.width/4,Screen.height/16), "Time Remaining: " + (int)countdownTimer);
			GUI.Label(new Rect(Screen.width/4,Screen.height/200,Screen.width/4,Screen.height/16), "Budget Remaining: " + (int)money);
			GUI.Label(new Rect(Screen.width/2,Screen.height/200,Screen.width/4,Screen.height/16), "Current Day: " + (currentDay+1));
		}
    }
}
    