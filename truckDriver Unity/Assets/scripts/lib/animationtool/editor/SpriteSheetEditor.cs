using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(SpriteSheet))]
public class SpriteSheetEditor : Editor {

	public override void OnInspectorGUI(){
		SpriteSheet spriteSheet = (SpriteSheet)target;
		
		EditorGUILayout.BeginVertical();
		
			EditorGUILayout.LabelField("Animation Parameters", EditorStyles.boldLabel);
			spriteSheet.running = EditorGUILayout.Toggle("Active",spriteSheet.running);
			spriteSheet.loop = EditorGUILayout.Toggle("Loop",spriteSheet.loop);
			spriteSheet.reverse = EditorGUILayout.Toggle("Reverse",spriteSheet.reverse);
			spriteSheet.smoothTransition = EditorGUILayout.Toggle("Smooth Transition",spriteSheet.smoothTransition);
			

			spriteSheet.fps = EditorGUILayout.FloatField("FPS",spriteSheet.fps);
			int currentSequence = EditorGUILayout.IntField("Current Sequence",spriteSheet.currentSequence);
			if (currentSequence < spriteSheet.sequenceFrameCount.Count && 
				currentSequence >= 0){
				spriteSheet.currentSequence = currentSequence;
			}
			EditorGUILayout.BeginHorizontal();
			
				EditorGUILayout.LabelField(
				"Current Frame: " + spriteSheet.currentFrame 
				+ " (" + spriteSheet.currentCol + "," + spriteSheet.currentRow + ")");
			
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.LabelField("Animation Settings", EditorStyles.boldLabel);
			spriteSheet.materialIndex = EditorGUILayout.IntSlider("Material",spriteSheet.materialIndex,0,spriteSheet.gameObject.renderer.sharedMaterials.Length-1);
			spriteSheet.frameWidth = EditorGUILayout.IntField("Sprite Width",spriteSheet.frameWidth);
			spriteSheet.frameHeight = EditorGUILayout.IntField("Sprite Height",spriteSheet.frameHeight);
			
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
			for (int i =0  ; i<spriteSheet.sequenceFrameCount.Count ; i++){
				spriteSheet.sequenceFrameCount[i] = EditorGUILayout.IntField("Seq " + i + " : Frame Count",spriteSheet.sequenceFrameCount[i]);
			}
			
		EditorGUILayout.EndVertical();
		
		int rowCount = spriteSheet.renderer.sharedMaterials[spriteSheet.materialIndex].mainTexture.height / spriteSheet.frameHeight;
		if (spriteSheet.frameWidth != 0 && spriteSheet.frameHeight != 0){
			int colCount = spriteSheet.renderer.sharedMaterials[spriteSheet.materialIndex].mainTexture.width / spriteSheet.frameWidth;
			
			spriteSheet.renderer.sharedMaterials[spriteSheet.materialIndex].SetTextureScale("_MainTex", new Vector2(1f/colCount,1f/rowCount));
		}
		Vector2 offset = new Vector2( 0,(rowCount - 1)/((float)rowCount));
	    spriteSheet.renderer.sharedMaterials[spriteSheet.materialIndex].SetTextureOffset("_MainTex", offset);
		if (GUI.changed){
			EditorUtility.SetDirty(target);
		}
	}

}
