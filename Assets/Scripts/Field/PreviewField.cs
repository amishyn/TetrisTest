using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class PreviewField : Field {

	public static PreviewField instance;
	public static event Action OnCreate;
	
	void Awake(){
		instance = this;
	} 

	public override void Start () {
		base.Start();

		if(OnCreate != null) OnCreate();
	}
	
	public static void SetPieceToPosition(Piece piece){
		piece.transform.SetParent(instance.fieldMatrix[piece.x,piece.y].transform, false);
	}

}
