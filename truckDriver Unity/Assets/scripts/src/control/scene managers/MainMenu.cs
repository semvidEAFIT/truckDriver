	using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnGUI() {
		
		if (GUI.Button(new Rect(3*Screen.width/8, 4*Screen.height/16, Screen.width/4, Screen.height/8), "Jugar")){
			Application.LoadLevel("Game");
		}
		if (GUI.Button(new Rect(3*Screen.width/8, 7*Screen.height/16, Screen.width/4, Screen.height/8), "Tutorial")){
			Application.LoadLevel("Game");
		}
		if (GUI.Button(new Rect(3*Screen.width/8, 10*Screen.height/16, Screen.width/4, Screen.height/8), "Opciones")){
			Application.LoadLevel("Options");
		}
	}
}
