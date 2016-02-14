using UnityEngine;
using System.Collections;
using System;
using Random = UnityEngine.Random;

// TODO: typofix, TetraSpawner

public class TetraSpowner : MonoBehaviour {

	public GameObject[] tetramines;
	public Vector2 userDefinedStartPosition;

	public static Vector2 startPosition;

	private GameObject nextTetramin;
	private GameObject currentTetramin;


	void Awake(){
		startPosition = userDefinedStartPosition;
	}

	void OnEnable() {
		PlayField.FieldCreated += StartSpowning;
		PlayField.RowClearingComplete += SpownNext;
	}

	void OnDisable() {
		PlayField.FieldCreated -= StartSpowning;
		PlayField.RowClearingComplete -= SpownNext;
	}

	void StartSpowning(){
		nextTetramin = Instantiate(tetramines[Random.Range(0,tetramines.Length)]);
		nextTetramin.transform.SetParent(transform, false);

		SpownNext();
	}


	void SpownNext() {
		currentTetramin = nextTetramin;
		currentTetramin.GetComponent<Tetramin>().StartCoroutine("PutOnPlayField");
		nextTetramin = Instantiate(tetramines[Random.Range(0,tetramines.Length)]);
		nextTetramin.transform.SetParent(transform, false);
	}

}
