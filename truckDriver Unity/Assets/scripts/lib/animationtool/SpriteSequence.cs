using UnityEngine;
using System.Collections;

public class SpriteSequence {

	private int initFrame;
	private int endFrame;
	
	public SpriteSequence (int initFrame, int endFrame)
	{
		this.initFrame = initFrame;
		this.endFrame = endFrame;
	}
	
	public int InitFrame {
		get {
			return this.initFrame;
		}
	}

	public int EndFrame {
		get {
			return this.endFrame;
		}
	}
}
