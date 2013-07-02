	using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	public Texture2D[] images;
	public GUISkin skin;
	
	void OnGUI() {
		if(skin != null){
			GUI.skin = skin;
		}
		
		//GUI.BeginGroup(new Rect(Screen.width/10, Screen.height/3, Screen.width - Screen.width/5, Screen.height/3 - Screen.height/12));
		if (GUI.Button(new Rect(Screen.width/6, 2*Screen.height/3, Screen.width/4.5f, (Screen.height/3 - Screen.height/12)/3), images[0])){
			Application.LoadLevel("Game");
		}
		if (GUI.Button(new Rect(Screen.width/2 + Screen.width/12, 2*Screen.height/3,  Screen.width/4.5f, (Screen.height/3 - Screen.height/12)/3), images[1])){
			Application.LoadLevel("Game");
		}
		//GUI.EndGroup();
	}
}