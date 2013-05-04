using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject truck;
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

    void Awake() {
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
    public void addNodeToSelection(int b) {
        //Debug.Log("Position" + b + " Index"+ index + " Lenght" + playerSolution.Length);
        if(index < playerSolution.Length){
            if (b == 0) return;
            playerSolution[index] = b;
            spentMoney += travelCost* currentDay.TspCase.DistanceMatrix[playerSolution[index - 1], playerSolution[index]];
            index++;
        }else{
            spentMoney += travelCost* currentDay.TspCase.DistanceMatrix[playerSolution[index - 1], playerSolution[0]];
            Level.Instance.nextDay(playerSolution, spentMoney);
        }
    }
	
	public void moveTruck(Vector3 position){
		 truck.GetComponent<Vehicle>().setNextPosition(position);
	}
}