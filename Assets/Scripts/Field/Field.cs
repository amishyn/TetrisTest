using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class Field : MonoBehaviour {

	public int fieldWidth;
	public int fieldHeight;
	public int upperInvisibleRows;

	public GameObject emptyCell;

	protected Cell[,] fieldMatrix;
	GridLayoutGroup myGrid;
	RectTransform myRT;

	public virtual void OnEnable() {
		ResolutionTracker.OnResolutionChange += RecalculateCellSize;
	}

	public virtual void OnDisable() {
		ClearAllCells();
		ResolutionTracker.OnResolutionChange -= RecalculateCellSize;
	}

	public virtual void Start () {
		fieldMatrix = new Cell[fieldWidth, fieldHeight + upperInvisibleRows];

		myRT = GetComponent<RectTransform>();

		myGrid = GetComponent<GridLayoutGroup>();
		RecalculateCellSize();

		CreatingFieldMatrix();
	}

	public void Update () {
	}

	// TODO: rename to createFieldMatrix()
	void CreatingFieldMatrix(){
		for(int y=0; y< fieldHeight + upperInvisibleRows; y++){
			for(int x=0; x < fieldWidth; x++){
				fieldMatrix[x,y] = Instantiate(emptyCell).GetComponent<Cell>();
				fieldMatrix[x,y].transform.SetParent(myRT, false);
			}
		}
	}

	void RecalculateCellSize(){
		myGrid.cellSize = new Vector2(myRT.rect.width/fieldWidth , myRT.rect.height/fieldHeight);
	}


	void ClearAllCells(){
		for(int y=0; y< fieldHeight + upperInvisibleRows; y++){
			for(int x=0; x < fieldWidth; x++){
				fieldMatrix[x,y].ClearCell();
			}
		}
	}
}
