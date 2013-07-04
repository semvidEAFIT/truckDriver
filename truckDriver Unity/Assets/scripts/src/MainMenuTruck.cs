using UnityEngine;
using System.Collections;

public class MainMenuTruck : MonoBehaviour {
	
	public float speed, error, turnSpeed;
	public Vector3[] positions;
	private Vector3 direction;
	private float degrees = 0.0f;
	private int i = 1, j = 0;
	private bool turning = false;
	
	void Start(){
		direction = (positions[1] - positions[0]).normalized;
	}
	
	void Update () {
		if(!turning){
			transform.Translate(direction * speed * Time.deltaTime, Space.World);
			if(((positions[i] - transform.position).sqrMagnitude <= error * error) || ((positions[i] - positions[j]).sqrMagnitude < (positions[i] - transform.position).sqrMagnitude)){
				NextPosition();
			}
		}else{
			degrees += turnSpeed * Time.deltaTime;
			transform.Rotate(turnSpeed * Vector3.back * Time.deltaTime, Space.World); 
			
			if(degrees >= 90.0f){
				transform.Rotate((90.0f - degrees) * Vector3.back, Space.World);
				degrees = 0.0f;
				turning = false;
			}
		}
	}
	
	private void NextPosition () {
		transform.position = positions[i];
		j = i;
		i = (i+1) % positions.Length;
		//Debug.Log(j + " " + i);
		direction = (positions[i] - positions[j]).normalized;
		turning = true;
	}
}
