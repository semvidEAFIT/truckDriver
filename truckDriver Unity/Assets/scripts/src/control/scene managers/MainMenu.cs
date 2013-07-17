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
			Application.LoadLevel("Tutorial");
		}
		//GUI.EndGroup();
		float horizontalBorder = Screen.width/16, downBorder = Screen.height / 32;
		
		Rect logos = new Rect(horizontalBorder, Screen.height - Screen.height/9.8f/* - downBorder*/, Screen.width - 2*horizontalBorder, Screen.height/10);
		GUI.BeginGroup(logos);
			GUI.Label(new Rect(0, 0, images[2].width*1.5f, images[2].height*1.5f), images[2]);
			GUI.Label(new Rect(logos.width/2 - images[3].width/2, 0, images[3].width*1.5f, images[3].height*1.5f), images[3]);
			GUI.Label(new Rect(logos.width - images[4].width, 0, images[4].width*1.5f, images[4].height*1.5f), images[4]);
		GUI.EndGroup();
	}
	
	void Update(){
		if(Input.GetKeyDown(KeyCode.Escape)){
			Application.Quit();
		}
	}
}