using UnityEngine;
using System.Collections;
using System;
public class InvalidSecuenceException : Exception{
	private string secuence;
	
	public string Secuence {
		get {
			return this.secuence;
		}
	}
	
	public InvalidSecuenceException(int secuence): base("The secuence \"" + secuence + "\" is not defined" ){
		this.secuence = secuence + "";
	}
	
	public InvalidSecuenceException(string secuence): base("The secuence \"" + secuence + "\" is not defined" ){
		this.secuence = secuence;
	}
	
	public InvalidSecuenceException(Enum secuence): base("The secuence \"" + secuence.ToString() + "\" is not defined" ){
		this.secuence = secuence.ToString();
	}
}

