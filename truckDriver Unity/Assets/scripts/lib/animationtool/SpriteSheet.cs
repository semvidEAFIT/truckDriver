using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class SpriteSheet : MonoBehaviour {
	
	public int materialIndex;
	public List<int> sequenceFrameCount = new List<int>();
	
	public bool loop;
	public bool reverse;
	public bool running = true;
	public bool smoothTransition; // This is when the sequence changes based on the get and sets
	private bool smooth = false;
	private int nextSequence;
	private bool notifyEnd;
	public int frameWidth;
	public int frameHeight;
	public int currentSequence;
	public int currentFrame;
	public int lastFrame;
	
	public float fps;
	
	private SpriteSequence[] sequences;
	private int colCount;
	private int rowCount;
	public int currentRow;
	public int currentCol;
	
	private List<ISpriteSheet> listeners;
	
	void Awake() {
		listeners = new List<ISpriteSheet>();
		sequences = new SpriteSequence[sequenceFrameCount.Count];
		int counter = 0;
		for (int i =0 ; i < sequences.Length ; i++){
			sequences[i] = new SpriteSequence(counter, counter + sequenceFrameCount[i] - 1);
			counter += sequenceFrameCount[i];
		}
	}
	
	// Use this for initialization
	void Start () {
		if (frameWidth != 0){
			colCount = renderer.materials[materialIndex].mainTexture.width / frameWidth;
		}
		if (frameHeight != 0){
			rowCount = renderer.materials[materialIndex].mainTexture.height / frameHeight;
		}
		
		//renderer.materials[materialIndex].SetTextureScale("_MainTex", new Vector2(1f/colCount,1f/rowCount));
		StartCoroutine(UpdateSprite());
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	private IEnumerator UpdateSprite()
    {
    	while (true){
	    	if (running){
	    		SpriteSequence sequence = sequences[currentSequence];
	    		// Checks current frame is within the current sequence
	    		if (currentFrame < sequence.InitFrame || currentFrame > sequence.EndFrame){
	    			currentFrame = sequence.InitFrame;
	    		}
	    		if (smoothTransition && smooth){
	    			if (!reverse){
	    				if (sequence.InitFrame < currentFrame){
		    				addFrame(-1);
		    			}
	    			}else{
		    			if (sequence.EndFrame > currentFrame){
		    				addFrame(1);
		    			}
	    			}
	    		} else if (loop){
	    			if (reverse){
	    				if (currentFrame <= sequence.InitFrame){
	    					currentFrame = sequence.EndFrame;
	    				}else{
	    					addFrame(-1);
	    				}
	    			}else{
	    				if (currentFrame >= sequence.EndFrame){
	    					currentFrame = sequence.InitFrame;
	    				}else{
	    					addFrame(1);
	    				}
	    			}
	    		}else{
	    			if (reverse){
	    				if (sequence.InitFrame < currentFrame){
		    				addFrame(-1);
		    			}
	    			}else{
		    			if (sequence.EndFrame > currentFrame){
		    				addFrame(1);
		    			}
	    			}
	    		}
	    		
	    		if ( smooth && (sequence.EndFrame == currentFrame || sequence.InitFrame == currentFrame)){
	    			currentSequence = nextSequence;
	    				smooth = false;
	    		}
	    		
	    		if ( ((sequence.EndFrame == currentFrame && !reverse) 
	    				||  (sequence.InitFrame == currentFrame && reverse))
	    				&& (loop || (reverse && lastFrame != currentFrame))
	    				){
	    				NotifySequenceEnded();
	    		}
		    	UpdateFramePosition();
		    	
		    	Vector2 offset = new Vector2( (currentCol)/((float)colCount),(rowCount - 1 - currentRow)/((float)rowCount));
	            renderer.materials[materialIndex].SetTextureOffset("_MainTex", offset);
		    	
	    	}
	    	yield return new WaitForSeconds(1f / fps);
    	}
    }
    
    private void addFrame(int frameNumber){
    	lastFrame = currentFrame;
    	currentFrame += frameNumber;
    	
    }
    
    private void UpdateFramePosition(){
    	currentCol = currentFrame % colCount;
    	currentRow = currentFrame / colCount;
    }

	public int CurrentSequence {
		get {
			return this.currentSequence;
		}
		set {
			if (value != currentSequence){
				if (smoothTransition){
					smooth = true;
					nextSequence = value;
				}else{
					currentSequence = value;
				}
			}
		}
	}
	
	public void setSecuence(Enum secuence) {
		int requested = Convert.ToInt32(secuence);
		if(requested < 0 || requested > SecuenceCount){
			throw new InvalidSecuenceException(requested);
		}
		CurrentSequence = requested;
	}
	
	public void AddSpriteSheetListener(ISpriteSheet listener){
		if (!listeners.Contains(listener)){
			listeners.Add(listener);
		}
	}
	
	public void RemoveSpriteSheetListener(ISpriteSheet listener){
		listeners.Remove(listener);
	}
	
	public void NotifySequenceEnded(){
		foreach (ISpriteSheet iSpriteSheet in listeners){
			iSpriteSheet.SequenceEnded(this);
		}
	}
	
	public int SecuenceCount{
		get{
			return sequences.Length;
		}
	}
}
