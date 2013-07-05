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
		set{
			budget = value;
		}
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
	public double optimalBudget;
	
	public Texture2D[] botones;
	public GUISkin skin, reportSkin;
	
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
		optimalBudget = budget;
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

    public void nextDay(int[] playerSolution) {
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
				}
				Application.LoadLevel("Game");
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
				timeSpent= Time.timeSinceLevelLoad - totalTimeSpent;
				totalTimeSpent += timeSpent;
				totalBudgetSpent+= Player.Instance.SpentMoney;
				calculatedTimeSpent=true;
			}
		}
	}
    void OnGUI() {
		GUI.skin = skin;
		
		float countdownTimer = timePerDay - Time.timeSinceLevelLoad;
		if(countdownTimer <= 0){
			lostLevel=true;
		}

		GUI.Label(new Rect(Screen.width/20, 0, Screen.width/4, Screen.height/10), botones[0]);
		int hour = ((int)countdownTimer) / 60 ;
		int minute = ((int)countdownTimer) % 60;
		string hourStr = ((hour<10)? "0":"") + hour + ":"+((minute<10)? "0":"")+ minute;
		GUI.Label(new Rect(Screen.width/10, Screen.height/80 ,Screen.width/4,Screen.height/10),hourStr);
		
		GUI.Label(new Rect(Screen.width/20+Screen.width/3, 0, Screen.width/4, Screen.height/10), botones[1]);
		GUI.Label(new Rect(Screen.width/10+Screen.width/3,Screen.height/80,Screen.width/4,Screen.height/10),(int)budget + "");
		
		GUI.Label(new Rect(Screen.width/20+2*Screen.width/3, 0, Screen.width/4, Screen.height/10), botones[2]);
		GUI.Label(new Rect(Screen.width/10+2*Screen.width/3,Screen.height/80,Screen.width/4,Screen.height/10), (currentDay+1) + "");
		
		if(lostLevel){
            if(GUI.Button(new Rect(Screen.width/3, Screen.height/2 - 5 - Screen.height/4, Screen.width/3, Screen.height/4), "Reintentar")){
                Application.LoadLevel("Game");
            }
            if (GUI.Button(new Rect(Screen.width / 3, Screen.height / 2, Screen.width / 3, Screen.height / 4), "Salir"))
            {
				LevelSettings.Instance.Difficulty = Difficulty.Easy;
                Application.LoadLevel("Main Menu");
            }
        } else {
			GUI.skin = reportSkin;
			
			if(onLevelReportScreen){
				Rect reportSpace = new Rect(Screen.width/6 , Screen.height/ 4, 2*Screen.width/3, Screen.height/2);
				GUI.Box(reportSpace, "");
				GUI.BeginGroup(reportSpace);
				GUI.Label(new Rect(0, 0, reportSpace.width, reportSpace.height/7), "Nivel Completado!");
				GUI.Label(new Rect(0, reportSpace.height/7, reportSpace.width, reportSpace.height/7), "Tiempo Gastado: " +  (int)totalTimeSpent); 
				GUI.Label(new Rect(0, 2*reportSpace.height/7, reportSpace.width, reportSpace.height/7), "Presupuesto Inicial: " + (int)totalLevelBudget); 
				GUI.Label(new Rect(0, 3*reportSpace.height/7, reportSpace.width, reportSpace.height/7), "Presupuesto Gastado: " + (int)totalBudgetSpent); 
				GUI.Label(new Rect(0, 4*reportSpace.height/7, reportSpace.width, reportSpace.height/7), "Presupuesto Final: " + (int)budget);
				GUI.Label(new Rect(0, 5*reportSpace.height/7, reportSpace.width, reportSpace.height/7), "Mejor Solucion: " + (int)optimalBudget);
				if(GUI.Button(new Rect(reportSpace.width/4 - botones[3].width/2, 6*reportSpace.height/7 + reportSpace.height/14 - botones[3].height/2, botones[3].width, botones[3].height), "Menu")){
					Application.LoadLevel("Main Menu");
					LevelSettings.Instance.Difficulty = Difficulty.Easy;
				}
				if(GUI.Button(new Rect(3 * reportSpace.width/4 - botones[3].width/2, 6*reportSpace.height/7 + reportSpace.height/14 - botones[3].height/2, botones[3].width, botones[3].height), "Siguiente Nivel")){
					onLevelReportScreen=false;
					Player.Instance.NextLevel();
				}
				GUI.EndGroup();
			}
			if(onDayReportScreen){
				Rect reportSpace = new Rect(Screen.width/6 , Screen.height/ 3, 2*Screen.width/3, Screen.height/3);
				GUI.Box(reportSpace, "");
				GUI.BeginGroup(reportSpace);
				GUI.Label(new Rect(0, 0, reportSpace.width, reportSpace.height/6), "Dia " + (currentDay+1));
				GUI.Label(new Rect(0, reportSpace.height/5, reportSpace.width, reportSpace.height/5), "Tiempo Gastado: " +  (int)timeSpent);
				GUI.Label(new Rect(0, 2*reportSpace.height/5, reportSpace.width, reportSpace.height/5), "Presupuesto Gastado: " + (int)Player.Instance.SpentMoney);
				GUI.Label(new Rect(0, 3*reportSpace.height/5, reportSpace.width, reportSpace.height/5), "Mejor Solucion: " + TSPSolver.calculateCost(CurrentDay.Solution, CurrentDay.TspCase));
				if (currentDay+1 >= daysPerLevel){
		            if(GUI.Button(new Rect(reportSpace.width/2 - botones[3].width/2, 4*reportSpace.height/5 + reportSpace.height/10 - botones[3].height/2,  botones[3].width,  botones[3].height), "Continuar")){
						//show level report screen
						onDayReportScreen=false;
						onLevelReportScreen=true;
					}
		        } else {
					if(GUI.Button(new Rect(reportSpace.width/2 - botones[3].width/2, 4*reportSpace.height/5 + reportSpace.height/10 - botones[3].height/2,  botones[3].width,  botones[3].height), "Dia Siguiente")){
						Player.Instance.NextLevel();
						onDayReportScreen=false;
					}
				}
				
				GUI.EndGroup();
			}
		}
    }
}
