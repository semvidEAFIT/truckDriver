using UnityEngine;

public class Vehicle : MonoBehaviour
{
	public bool shift;
	public Vector3 nextPsotion;
	private float widthBlock;
	private float heigthBlock;
	private float  finalmove;
	private bool move;
	void Awake(){
		move=false;
		nextPsotion=transform.position;
	}
	
	void Update(){
			
			
			 if(transform.position.x != nextPsotion.x-widthBlock ){
				Vector3 trans=nextPsotion-transform.position;
				if(trans.x>0){
						finalmove=1;
						transform.localScale = new Vector3(1f,1f,0.5f);
						transform.Translate(0.5f,0,0,Space.Self);
						
				}
			}
				//transform.Translate(trans * Time.deltaTime,Space.World);
			  if(transform.position.x != nextPsotion.x+widthBlock){
					Vector3 trans=nextPsotion-transform.position;
					if (trans.x <0){
						finalmove=-1;
						transform.localScale = new Vector3(1f,1f,0.5f);
						transform.Translate(-0.5f,0,0,Space.Self);
					}
			}
			
			 if(transform.position.y >= nextPsotion.y +0.5f || transform.position.y <= nextPsotion.y -0.5f   ){
					Vector3 trans=nextPsotion-transform.position;
					if(trans.y >=0 && (transform.position.x == nextPsotion.x+widthBlock || transform.position.x == nextPsotion.x - widthBlock)){
				
				     transform.localScale = new Vector3(0.5f,1f,1f);
					 transform.Translate(0,0.5f,0,Space.World);
				
					}else if ( trans.y <= 0 &&(transform.position.x == nextPsotion.x+widthBlock || transform.position.x == nextPsotion.x - widthBlock)){
					 transform.localScale = new Vector3(0.5f,1f,1f);
					 transform.Translate(0,-0.5f,0,Space.World);
					}else if (transform.position.x == nextPsotion.x) {
						transform.Translate(-widthBlock,0,0,Space.Self);
					}
				
			  }
					
			if(transform.position.y <= nextPsotion.y + 0.5f && transform.position.y >= nextPsotion.y -0.5f && transform.position.x != nextPsotion.x ){
					
					if(finalmove >0){
						transform.localScale = new Vector3(1f,1f,0.5f);
						transform.Translate(0.5f,0,0,Space.Self);
					}else{
						transform.localScale = new Vector3(1f,1f,0.5f);
						transform.Translate(-0.5f,0,0,Space.Self);
				}
			}		
	}
	
	public void setNextPosition( Vector3  position, float ancho, float alto){
		nextPsotion=position;	
		nextPsotion.y=nextPsotion.y+heigthBlock;		
		this.widthBlock=ancho;
		Debug.Log(widthBlock);
		this.heigthBlock=alto;
	}
    
}