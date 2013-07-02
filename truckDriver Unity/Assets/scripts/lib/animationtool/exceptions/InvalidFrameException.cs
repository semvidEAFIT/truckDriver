using UnityEngine;
using System.Collections;
using System;

public class InvalidFrameException : Exception {

	private int frame;
	
	public int Frame{
		get{
			return frame;
		}
	}
	
	public InvalidFrameException(int frame):base("The frame "+ frame + " is out of bounds"){
		this.frame = frame;
	}
}
