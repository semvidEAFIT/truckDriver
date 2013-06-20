using UnityEngine;
using System.Collections;
using System;
public class InvalidSecuenceException : Exception{
	private int secuence;
	
	public int Secuence {
		get {
			return this.secuence;
		}
	}
	
	public InvalidSecuenceException(int secuence): base("The secuence " + secuence + " is not defined" ){
		this.secuence = secuence;
	}
}

