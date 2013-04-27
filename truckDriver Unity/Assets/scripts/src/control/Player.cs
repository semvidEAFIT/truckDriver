using UnityEngine;

public class Player : MonoBehaviour
{
    public Vehicle truck;

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
          index = 0;
      }
    }

    private double spentMoney;

    public double SpentMoney
    {
        get { return spentMoney; }
    }

    private int[] playerSolution;
    private int index;

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
	
	public float travelCost=1;

    public void addNodeToSelection(int b) {
        index++;
        if(index < playerSolution.Length){
            if (b == 0) return;
            playerSolution[index] = b;
            spentMoney += travelCost* currentDay.TspCase.DistanceMatrix[playerSolution[index - 1], playerSolution[index]];
        }else{
            spentMoney += travelCost* currentDay.TspCase.DistanceMatrix[playerSolution[index - 1], playerSolution[0]];
            Level.Instance.nextDay(playerSolution, spentMoney);
        }
    }
}