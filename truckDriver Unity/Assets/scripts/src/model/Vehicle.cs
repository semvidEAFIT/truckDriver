
using UnityEngine;

public class Vehicle : MonoBehaviour
{
	private Vector3 startPosition;
	public Vector3 StartPosition {
		get {
			return this.startPosition;
		}
		set {
			startPosition = value;
		}
	}
	public bool shift;
	public Vector3 nextPosition;
	private Vector2 blockDistance;
	private int fase =0;
	private float streetSize;
	public float speed=0.5f;
	private Vector3 target;
	
	private int defaultDir;
	
	public bool isPlayerTruck;
	public Component truckController;
	
	void Awake(){
		blockDistance=LevelSettings.Instance.DistanceManhatan;
		fase=0;
		nextPosition=transform.position;
		defaultDir=1;
	}

	void Update(){
		switch (fase){
		default:
			target=transform.position;
			break;
		case 1:
			if(Mathf.Abs(target.x-transform.position.x)>1){
				if(transform.position.x<target.x){
					transform.Translate(speed,0,0,Space.World);
				} else {
					transform.Translate(-speed,0,0,Space.World);
				}
			} else {
				transform.position = new Vector3(target.x,transform.position.y, transform.position.z);
				if(transform.position.x<nextPosition.x){
					target= new Vector3(nextPosition.x-blockDistance.x/2,transform.position.y,transform.position.z);
				} else {
					target= new Vector3(nextPosition.x+blockDistance.x/2,transform.position.y,transform.position.z);
				}
				fase=2;
			}
			break;
		case 2:
			if(Mathf.Abs(target.x-transform.position.x)>1){
				if(transform.position.x<target.x){
					transform.Translate(speed,0,0,Space.World);
				} else {
					transform.Translate(-speed,0,0,Space.World);
				}
			} else {
				transform.position = new Vector3(target.x, transform.position.y, transform.position.z);
				target= new Vector3(transform.position.x, nextPosition.y, transform.position.z);
				fase=3;
			}
			break;
		case 3:
			if(Mathf.Abs(transform.position.y-target.y)>1){
				if(transform.position.y<target.y){
					transform.Translate(0,speed,0,Space.World);
				} else {
					transform.Translate(0,-speed,0,Space.World);
				}
			} else {
				transform.position= target;
				target= nextPosition;
				fase=4;
			}
			break;
		case 4:
			if(Mathf.Abs(transform.position.x-target.x)>1){
				if(transform.position.x<target.x){
					transform.Translate(speed,0,0,Space.World);
				} else {
					transform.Translate(-speed,0,0,Space.World);
				}
			} else {
				transform.position = nextPosition;
				target=transform.position;
				fase=0;
				if(isPlayerTruck){
					truckController.GetComponent<Player>().DoneMoving=true;
				} else {
					truckController.GetComponent<TraceSolver>().IsMoving=false;
				}
			}
			break;
		}
	}
	
	public void setOnStartPosition(){
		transform.position = new Vector3(startPosition.x, startPosition.y+blockDistance.y, startPosition.z);
	}

	public void setNextPosition(Vector3  position){
		if(isPlayerTruck){
			truckController.GetComponent<Player>().DoneMoving=false;
		} else {
			truckController.GetComponent<TraceSolver>().IsMoving=true;;
		}
			
		nextPosition=position;	
		nextPosition.y = nextPosition.y + blockDistance.y/2;
		if(transform.position.x < position.x){
			target= new Vector3(transform.position.x+blockDistance.x/2,transform.position.y, transform.position.z);
			defaultDir=1;
		} else {
			if(transform.position.x > position.x){
				target= new Vector3(transform.position.x-blockDistance.x/2,transform.position.y, transform.position.z);
				defaultDir=-1;
			} else {
				target= new Vector3(transform.position.x + defaultDir*blockDistance.x/2,transform.position.y, transform.position.z);
				defaultDir*=-1;
			}
		}
		fase=1;
	}
	
	public void setStretSize( float size){
		streetSize=size;
	}
}