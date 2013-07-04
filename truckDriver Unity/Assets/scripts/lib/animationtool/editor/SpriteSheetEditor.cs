using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(SpriteSheet))]
public class SpriteSheetEditor : Editor {

	public override void OnInspectorGUI(){
		SpriteSheet spriteSheet = (SpriteSheet)target;

		EditorGUILayout.BeginVertical();
		
			EditorGUILayout.LabelField("Animation Controls", EditorStyles.boldLabel);
			spriteSheet.running = EditorGUILayout.Toggle("Active",spriteSheet.running);
			spriteSheet.loop = EditorGUILayout.Toggle("Loop",spriteSheet.loop);
			spriteSheet.reverse = EditorGUILayout.Toggle("Reverse",spriteSheet.reverse);
			spriteSheet.smoothTransition = EditorGUILayout.Toggle("Smooth Transition",spriteSheet.smoothTransition);
			

			spriteSheet.fps = EditorGUILayout.FloatField("FPS",spriteSheet.fps);
			int currentSequence = EditorGUILayout.IntField("Current Sequence",spriteSheet.currentSequence);
			if (currentSequence < spriteSheet.sequenceFrameCount.Count && currentSequence >= 0 && currentSequence != spriteSheet.currentSequence){
				spriteSheet.currentSequence = currentSequence;
			}
		
			EditorGUILayout.BeginHorizontal();
			
				EditorGUILayout.LabelField(
				"Current Frame: " + spriteSheet.currentFrame 
				+ " (" + spriteSheet.currentCol + "," + spriteSheet.currentRow + ")");
			
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.LabelField("Sprite Sheet Settings", EditorStyles.boldLabel);
			spriteSheet.materialIndex = EditorGUILayout.IntSlider("Material",spriteSheet.materialIndex,0,spriteSheet.gameObject.renderer.sharedMaterials.Length-1);
			
			//TODO Sprite size
			/*spriteSheet.frameWidth = EditorGUILayout.IntField("Sprite Width",spriteSheet.frameWidth);
			spriteSheet.frameHeight = EditorGUILayout.IntField("Sprite Height",spriteSheet.frameHeight);*/
			
			spriteSheet.colCount = EditorGUILayout.IntField("Columns", spriteSheet.colCount);
			spriteSheet.rowCount = EditorGUILayout.IntField("Rows", spriteSheet.rowCount);
			
			if(spriteSheet.colCount <= 0){
				spriteSheet.colCount = 1;
			}
			if(spriteSheet.rowCount <= 0){
				spriteSheet.rowCount = 1;
			}
			
			spriteSheet.frameWidth = spriteSheet.gameObject.renderer.sharedMaterial.GetTexture("_MainTex").width / spriteSheet.colCount;
			spriteSheet.frameHeight = spriteSheet.gameObject.renderer.sharedMaterial.GetTexture("_MainTex").height / spriteSheet.rowCount;
			
			if (spriteSheet.colCount != 0 && spriteSheet.rowCount != 0 && !Application.isPlaying){
				spriteSheet.renderer.sharedMaterials[spriteSheet.materialIndex].SetTextureScale("_MainTex", new Vector2(1f/spriteSheet.colCount,1f/spriteSheet.rowCount));
				Vector2 offset = new Vector2( 0,(spriteSheet.rowCount - 1)/((float)spriteSheet.rowCount));
				spriteSheet.renderer.sharedMaterials[spriteSheet.materialIndex].SetTextureOffset("_MainTex", offset);
			}	
		
			EditorGUILayout.LabelField("Sequence List", EditorStyles.boldLabel);
			int sequenceCount = EditorGUILayout.IntField("Number of Sequences",spriteSheet.sequenceFrameCount.Count);
			if (sequenceCount != spriteSheet.sequenceFrameCount.Count){
				while (sequenceCount > spriteSheet.sequenceFrameCount.Count){
					spriteSheet.sequenceFrameCount.Add(0);
				}
				while (sequenceCount < spriteSheet.sequenceFrameCount.Count){
					spriteSheet.sequenceFrameCount.RemoveAt(spriteSheet.sequenceFrameCount.Count-1);
				}
			}
			
			EditorGUILayout.BeginHorizontal();
				EditorGUILayout.BeginVertical();
					EditorGUILayout.LabelField("",  EditorStyles.boldLabel, GUILayout.ExpandWidth(false), GUILayout.MaxWidth(20));
					for (int i =0  ; i<spriteSheet.sequenceFrameCount.Count ; i++){
						EditorGUILayout.LabelField(i + "", EditorStyles.boldLabel, GUILayout.ExpandWidth(false), GUILayout.MaxWidth(20));
					}
				EditorGUILayout.EndVertical();
				
				EditorGUILayout.BeginVertical();
					EditorGUILayout.LabelField("Name", EditorStyles.wordWrappedLabel);
					for (int i =0  ; i<spriteSheet.sequenceFrameCount.Count ; i++){
						string s = "";
						for(int j = 0; j < spriteSheet.keys.Count; j++){
							if(spriteSheet.values[j] == i){
								s = spriteSheet.keys[j];
								spriteSheet.keys.RemoveAt(j);
								spriteSheet.values.RemoveAt(j);
							}
						}
						s = EditorGUILayout.TextField(s).Trim();
						if(s != null && s.Length > 0 && !s.Equals("")){
							spriteSheet.keys.Add(s);
							spriteSheet.values.Add(i);
						}
					}
				EditorGUILayout.EndVertical();
				
				EditorGUILayout.BeginVertical();
					EditorGUILayout.LabelField("# of frames", EditorStyles.wordWrappedLabel);
					for (int i =0  ; i<spriteSheet.sequenceFrameCount.Count ; i++){
						spriteSheet.sequenceFrameCount[i] = EditorGUILayout.IntField(spriteSheet.sequenceFrameCount[i]);
					}
				EditorGUILayout.EndVertical();
			EditorGUILayout.EndHorizontal();
		EditorGUILayout.EndVertical();
		
		if (GUI.changed){
			EditorUtility.SetDirty(target);
		}
	}

}
