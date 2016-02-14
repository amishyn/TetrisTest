using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class PlayField : Field {

	bool previewFieldCreated = false;
	bool playFieldCreated = false;

	public static PlayField instance;
	
	public static event Action FieldCreated;
	public static event Action RowClearingComplete;
	public static event Action GameOver;

	void Awake(){
		instance = this;
	} 

	public override void OnEnable() {
		base.OnEnable();

		Tetramin.TetraminDropped += ClearFullRows;
		PreviewField.OnCreate += RegisterPreviewField;

		if(playFieldCreated){
			StartCoroutine(RestartGame());
		}
	}

	public override void OnDisable() {
		base.OnDisable();

		Tetramin.TetraminDropped -= ClearFullRows;
		PreviewField.OnCreate -= RegisterPreviewField;
	}

	public override void Start () {
		base.Start();

		playFieldCreated = true;
		if(FieldCreated != null && previewFieldCreated)
			FieldCreated();
	}

	IEnumerator RestartGame() {
		yield return null;

		if(FieldCreated != null)
			FieldCreated();
	}

	/// <summary>
	/// Event handlers section
	/// </summary>
	void RegisterPreviewField() {
		previewFieldCreated = true;
		if(playFieldCreated && FieldCreated != null) FieldCreated();
	}

	void ClearFullRows() {
		int rowDropAmount = 0;
		bool isRowHasEmptyCells;
		for(int y=0; y< fieldHeight + upperInvisibleRows; y++){
			isRowHasEmptyCells = false;
			for(int x=0; x< fieldWidth; x++){
				if(fieldMatrix[x, y].IsEmpty()){
					isRowHasEmptyCells = true;
					break;
				}
			}
			
			if(isRowHasEmptyCells){
				DropRowDownByAmount(rowDropAmount, y);
			} else {
				RemoveRow(y);
				rowDropAmount++;
			}
		}
		
		if(IsLastRowHavePieces()){
			if(GameOver != null) GameOver();
		} else {
			if(RowClearingComplete != null) RowClearingComplete();
		}
	}	

	/// <summary>
	/// Event handler helpers section
	/// </summary>
	void DropRowDownByAmount(int rowDropAmount, int y){
		for(int x=0; x< fieldWidth; x++){
			fieldMatrix[x, y].CopyToCell(fieldMatrix[x, y-rowDropAmount]);
		}
	}
	
	void RemoveRow(int y){
		for(int x=0; x< fieldWidth; x++){
			fieldMatrix[x, y].ClearCell();
		}
	}

	bool IsLastRowHavePieces(){
		for(int x=0; x< fieldWidth; x++){
			if(!fieldMatrix[x, fieldHeight].IsEmpty()){
				return true;
			}
		}
		return false;
	}

	/// <summary>
	/// Third party static helpers
	/// </summary>
	public static bool IsCellFree(int x, int y){
		if(!IsInBounds(x, y))
			return false;

		return instance.fieldMatrix[x, y].CanIMoveHere();
	}

	public static bool IsInBounds(int x, int y){
		return (x >= 0 && x< instance.fieldWidth && y >=0 && y < instance.fieldHeight + instance.upperInvisibleRows);
	}

	public static void SetPieceToPosition(Piece piece){
		piece.transform.SetParent(instance.fieldMatrix[piece.x,piece.y].transform, false);
	}

}
