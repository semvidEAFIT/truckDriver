using UnityEngine;
using System.Collections;

public class TraceSolver : MonoBehaviour {
	
	private Transform[] path;
	private int currNode;
	private bool isMoving;
	public bool IsMoving {
		get {
			return this.isMoving;
		}
		set {
			isMoving = value;
		}
	}
	private bool done;
	
	void Awake() {
		isMoving=false;
		done=false;
		currNode=0;
		path= new Transform[LevelSettings.Instance.NodeCount];
		for (int i=0; i<path.Length; i++){
			Vector2 nodeGridPos = Level.Instance.CurrentDay.TspCase.Nodes[Level.Instance.CurrentDay.Solution[i]];
			path[i] = Level.Instance.builder.Buildings[(int) nodeGridPos.x, (int) nodeGridPos.y].transform;
		}
	}
	
	void Update () {
		Debug.Log(currNode);
		
		if(!isMoving && currNode<LevelSettings.Instance.NodeCount){
			moveTruck(path[currNode].position);
			currNode++;
		}
		
		if(!isMoving && currNode==LevelSettings.Instance.NodeCount){
			moveTruck(path[0].position);
		}
	}
	
	public void moveTruck(Vector3 position){
		 this.GetComponent<Vehicle>().setNextPosition(position);
	}
}
