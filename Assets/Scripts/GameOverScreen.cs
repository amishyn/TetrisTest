using UnityEngine;
using System.Collections;

public class GameOverScreen : MonoBehaviour {

	public GameObject panel;

	void OnEnable(){
		PlayField.GameOver += ShowScreen;
	}

	void OnDisable() {
		PlayField.GameOver -= ShowScreen;
	}

	void ShowScreen() {
		panel.SetActive(true);
	}

}
