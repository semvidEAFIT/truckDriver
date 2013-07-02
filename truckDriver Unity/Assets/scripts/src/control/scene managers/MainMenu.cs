	using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	public Texture2D[] images;
	public GUISkin skin;
	
	void OnGUI() {
		if(skin != null){
			GUI.skin = skin;
		}
		if (GUI.Button(new Rect(3*Screen.width/8, 5*Screen.height/16, Screen.width/4, Screen.height/8), "Jugar")){
			Application.LoadLevel("Game");
		}
		if (GUI.Button(new Rect(3*Screen.width/8, 9*Screen.height/16, Screen.width/4, Screen.height/8), "Tutorial")){
			Application.LoadLevel("Game");
		}
	}
}