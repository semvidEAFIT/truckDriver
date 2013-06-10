
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
	private Vector3 nextPosition;
	private Vector2 blockDistance;
	private int fase =0;
	private float streetSize;
	public float speed=5;
	private Vector3 target;
	private bool isTurning;
	private float targetTurnDir;
	
	private int defaultDir;
	
	public bool isPlayerTruck;
	public Component truckController;
	
	void Awake(){
		blockDistance=LevelSettings.Instance.DistanceManhatan;
		fase=0;
		nextPosition=transform.position;
		defaultDir=1;
		isTurning=false;
		targetTurnDir=0;
	}

	void Update(){
		
		//TODO -- truck speed
		if(isTurning){
			if(Mathf.Abs(targetTurnDir - transform.eulerAngles.z)< 5){
				transform.eulerAngles = new Vector3(0,0,targetTurnDir);
				isTurning=false;
				targetTurnDir=0;
			} else {
				if(targetTurnDir-transform.eulerAngles.z<=-180 || (targetTurnDir-transform.eulerAngles.z <= 180 && targetTurnDir-transform.eulerAngles.z>=0)){
					transform.Rotate(new Vector3(0,0,Time.deltaTime*200));
				} else {
					transform.Rotate(new Vector3(0,0,-Time.deltaTime*200));
				}
			}
		} else {
			switch (fase){
			default:
				target=transform.position;
				break;
			case 1:
				if(Mathf.Abs(target.x-transform.position.x)>1){
					//not there yet
					if(transform.position.x<target.x){
						//right
						transform.Translate(speed*Time.deltaTime*10,0,0,Space.World);
					} else {
						//left
						transform.Translate(-speed*Time.deltaTime*10,0,0,Space.World);
					}
				} else {
					//set on place
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
						transform.Translate(speed*Time.deltaTime*10,0,0,Space.World);
					} else {
						transform.Translate(-speed*Time.deltaTime*10,0,0,Space.World);
					}
				} else {
					transform.position = new Vector3(target.x, transform.position.y, transform.position.z);
					target= new Vector3(transform.position.x, nextPosition.y, transform.position.z);
					if(Mathf.Abs(transform.position.y-target.y)<1){
						transform.position= target;
						target= nextPosition;
						fase=4;
					} else {
						if(nextPosition.y>transform.position.y){
							turnVehicle(90);
						} else {
							turnVehicle(270);
						}
						fase=3;
					}
				}
				break;
			case 3:
				if(Mathf.Abs(transform.position.y-target.y)>1){
					if(transform.position.y<target.y){
						transform.Translate(0,speed*Time.deltaTime*10,0,Space.World);
					} else {
						transform.Translate(0,-speed*Time.deltaTime*10,0,Space.World);
					}
				} else {
					if(transform.position.x < nextPosition.x){
						turnVehicle(0);
					} else {
						turnVehicle(180);
					}
					
					transform.position= target;
					target= nextPosition;
					fase=4;
				}
				break;
			case 4:
				if(Mathf.Abs(transform.position.x-target.x)>1){
					if(transform.position.x<target.x){
						transform.Translate(speed*Time.deltaTime*10,0,0,Space.World);
					} else {
						transform.Translate(-speed*Time.deltaTime*10,0,0,Space.World);
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
	}
	
	//rotate vehicle to match given z angle
	private void turnVehicle(float zAngle){
		isTurning=true;
		targetTurnDir=zAngle;
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
			//target to the rigth
			turnVehicle(0);
			target= new Vector3(transform.position.x+blockDistance.x/2,transform.position.y, transform.position.z);
			defaultDir=1;
		} else {
			if(transform.position.x > position.x){
				//target to the left
				turnVehicle(180);
				target= new Vector3(transform.position.x-blockDistance.x/2,transform.position.y, transform.position.z);
				defaultDir=-1;
			} else {
				//target on same column
				if(defaultDir==1){
					turnVehicle(0);
				}else {
					turnVehicle(180);
				}
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