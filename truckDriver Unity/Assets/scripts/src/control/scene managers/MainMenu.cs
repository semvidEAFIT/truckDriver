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
		skin.button.normal.background = images[0];
		if (GUI.Button(new Rect(Screen.width/4, 2*Screen.height/3, images[0].width, images[0].height), "JUGAR")){
			Application.LoadLevel("Game");
		}
		skin.button.normal.background = images[1];
		if (GUI.Button(new Rect(Screen.width/2, 2*Screen.height/3,  images[1].width, images[1].height), "TUTORIAL")){
			Application.LoadLevel("Game");
		}
		//GUI.EndGroup();
	}
}