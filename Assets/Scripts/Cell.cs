using UnityEngine;
using System.Collections;

public class Cell : MonoBehaviour {

	public bool CanIMoveHere(){
		if(IsEmpty()){
			return true;
		} else {
			Piece otherPiece = transform.GetChild(0).GetComponent<Piece>();
			if(otherPiece != null && otherPiece.isDropped)
				return false;
			else 
				return true;
		}
	}

	public bool IsEmpty(){
		return transform.childCount == 0;
	}

	public void CopyToCell(Cell newCell){
		if(!IsEmpty())
			transform.GetChild(0).transform.SetParent(newCell.transform, false);
	}

	public void ClearCell() {
		if(!IsEmpty())
			Destroy(transform.GetChild(0).gameObject);
	}
}
