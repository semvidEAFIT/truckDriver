using UnityEngine;
using UnityEngine;

public class Vehicle : MonoBehaviour
{
	public bool shift;
	public Vector3 nextPosition;
	public float widthBlock;
	public float heigthBlock;
	private float xStart;
	private float yStart;
	private int fase;
	private float streetSize;
	public float speed=0.5f;
	private Vector3 target;
	void Awake(){
		
		fase=0;
		nextPosition=transform.position;
		xStart=transform.position.x;
		yStart=transform.position.y;
		target = nextPosition - transform.position;
	}

	void Update(){

			if(fase == 1){
				Debug.Log("entro");
				if ( target.x >= 0){
				  if ( transform.position.x >= xStart + widthBlock/2 + streetSize/2){
					 	if(target.y!=0)fase=2;
						else fase=3;
				  
					}else{
					    transform.Translate(speed,0,0,Space.World);
					}
			  }else if(target.x <0){
					if ( transform.position.x <= xStart - widthBlock/2 - streetSize/2){
					 	if(target.y!=0)fase=2;
					 	else fase=3;
					}else{
					 	transform.Translate(-speed,0,0,Space.World);
					}
			  }
		}else if (fase == 2){
			if (target.x >0){
				if(transform.position.x >= xStart+target.x-widthBlock+widthBlock/2){
						fase=3;
				}else{
					    transform.Translate(speed,0,0,Space.World);
				}
			}else if(target.x <0){
				
					if(transform.position.x <= xStart-(-target.x-widthBlock+widthBlock/2) ){
						fase=3;
					}else{
						transform.Translate(-speed,0,0,Space.World);
					}
					}else {
						fase=3;
				}
		}else if ( fase == 3){
					if(transform.position.y <= nextPosition.y + 0.5f && transform.position.y >= nextPosition.y - 0.5f ){
						fase=4;
					}else{
						 transform.localScale = new Vector3(0.5f,1f,1f);
						if(target.y>0 && nextPosition.y > yStart){
							transform.Translate(0,speed,0,Space.World);
						}else if (target.y<0 && nextPosition.y > yStart){
							transform.Translate(0,-speed,0,Space.World);
						}else if (target.y<0 && nextPosition.y < yStart){
							transform.Translate(0,-speed,0,Space.World);
						}else if (target.y>0 && nextPosition.y < yStart){
							transform.Translate(0,speed,0,Space.World);
						}
				}
				
			}else if (fase ==4 ){
			if (transform.position.x != nextPosition.x ){
				 transform.localScale = new Vector3(1f,1f,0.5f);
				if (target.x >0){
					transform.Translate(speed,0,0,Space.World);
				}else if (target.x <=0){
					transform.Translate(-speed,0,0,Space.World);
				}
				
			}else{
				xStart=transform.position.x;
				yStart=transform.position.y;
				fase=0;
			}
				
		}

	}

	public void setNextPosition( Vector3  position){
		nextPosition=position;	
		nextPosition.y=nextPosition.y + heigthBlock;
		target=nextPosition - transform.position;
		fase=1;
	}
	
	public void setStretSize( float size){
		streetSize=size;
	}
    
}