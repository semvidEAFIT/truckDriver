using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject truck;
	public GameObject Truck {
		get {
			return this.truck;
		}
	}
	
	public GameObject trace;
	
	private bool doneMoving;
	public bool DoneMoving {
		get {
			return this.doneMoving;
		}
		set {
			doneMoving = value;
		}
	}
	
	private bool doneSelecting;
	
    private static float travelCost = 1.0f;

    private static Player instance;
    public static Player Instance
    {
        get { return instance; }
    }
    
    private Day currentDay;

    public Day CurrentDay
    {
      get { return currentDay; }
      set {
          currentDay = value; 
          playerSolution = new int[currentDay.Solution.Length];
          playerSolution[0] = 0;
          index = 1;
      }
    }

    private double spentMoney;

    public double SpentMoney
    {
        get { return spentMoney; }
    }

    private int[] playerSolution;
    private int index;

    public bool CanVisitOrigin
    {
        get {
            return index == playerSolution.Length; 
        }
    }
	
	public GameObject traceSolverPrefab;
	
	private GameObject traceSolverInstance;
	
	private GameObject actualTrace;
	
	private bool traceSolverSpawned;

    void Awake() {
		actualTrace = Instantiate(trace, this.transform.position, Quaternion.identity) as GameObject;
		actualTrace.transform.parent=this.transform;
		doneMoving=true;
		doneSelecting=false;
		traceSolverSpawned=false;
        if (instance == null)
        {
            instance = this;
        }
        else 
        {
            Debug.Log("Solo puede haber un Player");
            Destroy(this.gameObject);
        }
		
    }
	
	void Update(){
		if(doneMoving && doneSelecting && !traceSolverSpawned){
			traceSolverInstance = Instantiate(traceSolverPrefab, this.transform.position + new Vector3(0,0,-0.5f), Quaternion.identity) as GameObject;
			traceSolverSpawned=true;
		}
		
		if(doneMoving && doneSelecting){
			Level.Instance.showReportScreen();
		}
	}
	
    public void addNodeToSelection(int b) {
        //Debug.Log("Position" + b + " Index"+ index + " Lenght" + playerSolution.Length);
        if(index < playerSolution.Length){
            if (b == 0) return;
            playerSolution[index] = b;
            spentMoney += travelCost* currentDay.TspCase.DistanceMatrix[playerSolution[index - 1], playerSolution[index]];
            index++;
        }else{
            spentMoney += travelCost* currentDay.TspCase.DistanceMatrix[playerSolution[index - 1], playerSolution[0]];
            doneSelecting=true;
        }
		
		
    }
	
	public void NextLevel(){
		Destroy(traceSolverInstance);
		traceSolverInstance=null;
		traceSolverSpawned=false;
		
		Destroy(actualTrace);
		actualTrace= Instantiate(trace, this.transform.position, Quaternion.identity) as GameObject;
		actualTrace.transform.parent=this.transform;
		
		doneSelecting=false;
		doneMoving=true;
		
		Level.Instance.nextDay(playerSolution, spentMoney);
		spentMoney = 0;
	}
	
	public void moveTruck(Vector3 position){
		 truck.GetComponent<Vehicle>().setNextPosition(position);
	}
}