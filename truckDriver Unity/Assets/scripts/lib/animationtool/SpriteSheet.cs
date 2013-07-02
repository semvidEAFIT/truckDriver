using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class SpriteSheet : MonoBehaviour {
	
	public int materialIndex;
	public List<int> sequenceFrameCount = new List<int>();
	
	private Dictionary<string, int> mnemonics = new Dictionary<string, int>();
	public List<string> keys;
	public List<int> values;
	
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
	public int colCount;
	public int rowCount;
	public int currentRow;
	public int currentCol;
	
	private List<ISpriteSheetListener> listeners;
	private Dictionary<int, List<ISpriteSheetListener>> frameListeners;
	
	void Awake() {
		listeners = new List<ISpriteSheetListener>();
		frameListeners = new Dictionary<int, List<ISpriteSheetListener>>();
		sequences = new SpriteSequence[sequenceFrameCount.Count];
		
		mnemonics = new Dictionary<string, int>(keys.Count);
		for(int i = 0; i < keys.Count; i++){
			mnemonics.Add(keys[i], values[i]);
		}
		/*keys.Clear();
		values.Clear();*/
		
		int counter = 0;
		for (int i =0 ; i < sequences.Length ; i++){
			sequences[i] = new SpriteSequence(counter, counter + sequenceFrameCount[i] - 1);
			counter += sequenceFrameCount[i];
		}
	}
	
	// Use this for initialization
	void Start () {
		/*if (frameWidth != 0){
			colCount = renderer.materials[materialIndex].mainTexture.width / frameWidth;
		}
		if (frameHeight != 0){
			rowCount = renderer.materials[materialIndex].mainTexture.height / frameHeight;
		}*/
		
		//renderer.materials[materialIndex].SetTextureScale("_MainTex", new Vector2(1f/colCount,1f/rowCount));
		StartCoroutine(UpdateSprite());
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
		    	
				if(frameListeners.ContainsKey(currentFrame)){
					NotifyDisplayedFrame();
				}
				
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
	
	public void SetSequence(Enum secuence) {
		int requested = Convert.ToInt32(secuence);
		SetSequence(requested);
	}
	
	public void SetSequence(String secuence){
		if(!mnemonics.ContainsKey(secuence)){
			throw new InvalidSecuenceException(secuence);
		}else{
			SetSequence(mnemonics[secuence]);
		}	
	}
	
	public void SetSequence(int secuence){
		if(secuence < 0 || secuence > SecuenceCount){
			throw new InvalidSecuenceException(secuence);
		}
		CurrentSequence = secuence;
	}
	
	public void AddSpriteSheetListener(ISpriteSheetListener listener){
		if (!listeners.Contains(listener)){
			listeners.Add(listener);
		}
	}
	
	public void AddSpriteSheetListener(ISpriteSheetListener listener, int secuence, int frame){
		if(secuence >= 0 && secuence < SecuenceCount){
			if(frame >= 0 && frame < sequenceFrameCount[secuence]){
				int absoluteFrame = sequences[secuence].InitFrame + frame;
				List<ISpriteSheetListener> list;
				if(frameListeners.ContainsKey(absoluteFrame)){
					list = frameListeners[absoluteFrame];
				}else{
					list = new List<ISpriteSheetListener>();
					frameListeners.Add(absoluteFrame, list);
				}
				if(!list.Contains(listener)){
					list.Add(listener);
				}
			}else{
				throw new InvalidFrameException(frame);
			}
		}else{
			throw new InvalidSecuenceException(secuence);
		}
	}

	public void AddSpriteSheetListener(ISpriteSheetListener listener, Enum secuence, int frame){
		int requested = Convert.ToInt32(secuence);
		if(requested >= 0 && requested < SecuenceCount){
			if(frame >= 0 && frame < sequenceFrameCount[requested]){
				int absoluteFrame = sequences[requested].InitFrame + frame;
				List<ISpriteSheetListener> list;
				if(frameListeners.ContainsKey(absoluteFrame)){
					list = frameListeners[absoluteFrame];
				}else{
					list = new List<ISpriteSheetListener>();
					frameListeners.Add(absoluteFrame, list);
				}
				if(!list.Contains(listener)){
					list.Add(listener);
				}
			}else{
				throw new InvalidFrameException(frame);
			}
		}else{
			throw new InvalidSecuenceException(secuence);
		}
	}
	
	public void AddSpriteSheetListener(ISpriteSheetListener listener, string secuence, int frame){
		if(mnemonics.ContainsKey(secuence)){
			int requested = mnemonics[secuence];
			if(frame >= 0 && frame < sequenceFrameCount[requested]){
				int absoluteFrame = sequences[requested].InitFrame + frame;
				List<ISpriteSheetListener> list;
				if(frameListeners.ContainsKey(absoluteFrame)){
					list = frameListeners[absoluteFrame];
				}else{
					list = new List<ISpriteSheetListener>();
					frameListeners.Add(absoluteFrame, list);
				}
				if(!list.Contains(listener)){
					list.Add(listener);
				}
			}else{
				throw new InvalidFrameException(frame);
			}
		}else{
			throw new InvalidSecuenceException(secuence);
		}
	}
	
	public void RemoveSpriteSheetListener(ISpriteSheetListener listener){
		listeners.Remove(listener);
		foreach(List<ISpriteSheetListener> list in frameListeners.Values){
			if(list.Contains(listener)){
				list.Remove(listener);
			}
		}
	}
	
	public void NotifySequenceEnded(){
		foreach (ISpriteSheetListener iSpriteSheet in listeners){
			iSpriteSheet.SequenceEnded(this);
		}
	}

	void NotifyDisplayedFrame ()
	{
		foreach(List<ISpriteSheetListener> list in frameListeners.Values){
			foreach(ISpriteSheetListener listener in list){
				listener.DisplayedFrame(currentSequence, currentFrame - sequences[currentSequence].InitFrame, this);
			}
		}	
	}
	
	public int SecuenceCount{
		get{
			return sequences.Length;
		}
	}
}
