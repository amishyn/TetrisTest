using UnityEngine;
using System.Collections;
using System;

public class Tetramin : MonoBehaviour {

	public enum RotationModes {NONE, HALF, FULL};

	public RotationModes avalableRotations;

	public Vector2 centerCoordinate;
	public Vector2[] piecesCoordinates;
	public Piece[] pieces;

	private float stepCounter = 0;
	private Vector2[,] rotationMatrix = new Vector2[5,5];
	private Vector2[,] rotationMatrixClockwise = new Vector2[5,5];
	private int currentRotation = 0;
	public bool isOnPlayField = false;

	public static event Action TetraminDropped;

	void Start() {
		PrepareRotationMatrixes();

		//PutOnPlayField();
		PutOnPreviewField();
	}

	void Update() {
		if(!isOnPlayField)
			return;

		if(stepCounter > 1f){
			StepDownAttempt(Vector2.down);
			stepCounter = 0;
		}
		if(Input.GetKeyDown(KeyCode.LeftArrow)){
			SlideAttempt(Vector2.left);
		}
		if(Input.GetKeyDown(KeyCode.RightArrow)){
			SlideAttempt(Vector2.right);
		}
		if(Input.GetKeyDown(KeyCode.UpArrow)){
			RotateAttept();

		}
		if(Input.GetKeyDown(KeyCode.DownArrow)){
			while(StepDownAttempt(Vector2.down));
		}
		stepCounter += Time.deltaTime;
	}

	/// <summary>
	/// Instantiation section
	/// </summary>
	public void PutOnPreviewField() {
		for(int i=0; i<pieces.Length; i++){
			pieces[i].x = (int)piecesCoordinates[i].x;
			pieces[i].y = (int)piecesCoordinates[i].y;
			PreviewField.SetPieceToPosition(pieces[i]);
		}
	}

	public IEnumerator PutOnPlayField(){
		yield return null;
		isOnPlayField = true;
		for(int i=0; i<pieces.Length; i++){
			Vector2 globalPosition = TetraSpowner.startPosition - (centerCoordinate - piecesCoordinates[i]);
			pieces[i].x = (int)globalPosition.x;
			pieces[i].y = (int)globalPosition.y;
			PlayField.SetPieceToPosition(pieces[i]);
		}
		centerCoordinate = TetraSpowner.startPosition;
	}

	/// <summary>
	/// Rotation section
	/// </summary>

	void RotateAttept(){
		if(CanIRotate())
			Rotate();
	}

	bool CanIRotate(){
		if(avalableRotations == RotationModes.NONE)
			return false;
		
		Vector2 transition;
		for(int i=0; i < pieces.Length; i++){
			transition = GetRotationTransition(pieces[i]);
			if(!PlayField.IsCellFree(pieces[i].x+(int)transition.x,pieces[i].y+(int)transition.y)){
				return false;
			}
		}	
		return true;
	}

	Vector2 GetRotationTransition(Piece piece){
		int matrixCenter = 2;
		int matrixX = piece.x - (int)centerCoordinate.x + matrixCenter;
		int matrixY = piece.y - (int)centerCoordinate.y + matrixCenter;
		if(avalableRotations == RotationModes.HALF){
			if(currentRotation == 0)
				return rotationMatrix[matrixX, matrixY];
			else
				//return new Vector2(rotationMatrix[matrixY, matrixX].y, rotationMatrix[matrixY, matrixX].x);
				return rotationMatrixClockwise[matrixX, matrixY];
		} else 
			return rotationMatrix[matrixX, matrixY];
	}
	
	void Rotate(){
		Vector2 transition;
		for(int i=0; i < pieces.Length; i++){
			transition = GetRotationTransition(pieces[i]);
			pieces[i].x += (int)transition.x;
			pieces[i].y += (int)transition.y;
			PlayField.SetPieceToPosition(pieces[i]);
		}	
		switch(avalableRotations){
		case RotationModes.FULL:
			currentRotation = (currentRotation+90)%360;
			break;
		case RotationModes.HALF:
			if(currentRotation == 0)
				currentRotation +=90;
			else
				currentRotation = 0;
			break;
		}		
	}

	/// <summary>
	/// Moving section: move down, left & right function here
	/// </summary>

	bool StepDownAttempt(Vector2 direction) {
		bool canIMove = CanIGoDirection(direction);
		if(canIMove)
			GoToDirection(direction);
		else 
			DropPiecesAndDestroyMyself();
		
		return canIMove;
	}

	void SlideAttempt(Vector2 direction){
		if(CanIGoDirection(direction))
			GoToDirection(direction);
	}

	bool CanIGoDirection(Vector2 direction){
		for(int i=0; i < pieces.Length; i++){
			if(!PlayField.IsCellFree(pieces[i].x+(int)direction.x,pieces[i].y+(int)direction.y)){
				return false;
			}
		}		
		return true;
	}

	void GoToDirection(Vector2 direction) {
		for(int i=0; i < pieces.Length; i++){
			pieces[i].x += (int)direction.x;
			pieces[i].y += (int)direction.y;
			PlayField.SetPieceToPosition(pieces[i]);
		}
		centerCoordinate += direction;
	}

	void DropPiecesAndDestroyMyself() {
		for(int i=0; i < pieces.Length; i++){
			pieces[i].isDropped = true;
		}
		if(TetraminDropped != null)
			TetraminDropped();
		Destroy(gameObject);
	}

	void PrepareRotationMatrixes(){
		for(int y=0; y < rotationMatrix.GetLength(1); y++){
			for(int x=0; x < rotationMatrix.GetLength(0); x++){
				rotationMatrix[x,y] = new Vector2(y-x, rotationMatrix.GetLength(0)-1-x-y);
				rotationMatrixClockwise[y,x] = new Vector2(rotationMatrix.GetLength(0)-1-x-y, y-x);
			}
		}
	}
}
