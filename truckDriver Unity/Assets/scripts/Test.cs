using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {
	
	private SpriteSheet spriteSheet;
	
	void Awake(){
		spriteSheet = GetComponent<SpriteSheet>();
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Keypad0)){
			spriteSheet.SetSequence(0);
		}
		if(Input.GetKeyDown(KeyCode.Keypad1)){
			spriteSheet.SetSequence("Test");
		}
		if(Input.GetKeyDown(KeyCode.Keypad2)){
			spriteSheet.SetSequence(Animations.ArrowFront);
		}
	}
	
	public enum Animations{
		WalkFront, FallFront, ArrowFront 
	}
}


