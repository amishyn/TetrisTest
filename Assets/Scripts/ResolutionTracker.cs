using UnityEngine;
using System.Collections;
using System;

public class ResolutionTracker : MonoBehaviour {

	bool isAlive = true;
	Vector2 resolution;  
	float CheckDelay = 0.5f;  

	public static event Action OnResolutionChange;

	IEnumerator Start () {
		WaitForSeconds wait = new WaitForSeconds(CheckDelay);
		yield return wait;
		while(isAlive){
			if (resolution.x != Screen.width || resolution.y != Screen.height ) {
				resolution = new Vector2(Screen.width, Screen.height);
				if (OnResolutionChange != null) OnResolutionChange();
			}
			yield return wait;
		}
	}

	void OnDestroy(){
		isAlive = false;
	}

}
