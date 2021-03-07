using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlSwitchManager : MonoBehaviour {

	public GameObject[] Robots;
	public GameObject[] crackEffects;
	public GameObject[] robotCharacters;
	bool isSharkActive = false;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Robot(string s){

		switch (s) {
		case"MonkeyController":

			if (isSharkActive) {
				StopWaterEffect ();
			}

			Robots [0].SetActive (true);
			Robots [1].SetActive (false);
			Robots [2].SetActive (false);
			Robots [3].SetActive (false);
			for (int i = 0; i < crackEffects.Length; i++) {
				crackEffects [i].gameObject.SetActive (false);
			}
	
		

			break;

		case"LionController":

			if (isSharkActive) {
				StopWaterEffect ();
			}

			Robots [0].SetActive (false);
			Robots [1].SetActive (true);
			Robots [2].SetActive (false);
			Robots [3].SetActive (false);
			for (int i = 0; i < crackEffects.Length; i++) {
				crackEffects [i].gameObject.SetActive (false);
			}

			break;

		case"EagleController":

			if (isSharkActive) {
				StopWaterEffect ();
			}

			Robots [0].SetActive (false);
			Robots [1].SetActive (false);
			Robots [2].SetActive (true);
			Robots [3].SetActive (false);

			break;

		case"SharkController":

			isSharkActive = true;
			Robots [0].SetActive (false);
			Robots [1].SetActive (false);
			Robots [2].SetActive (false);
			Robots [3].SetActive (true);
			for (int i = 0; i < crackEffects.Length; i++) {
				crackEffects [i].gameObject.SetActive (false);
			}

			break;
		default:
			break;
		}

	}

	void StopWaterEffect()
	{
		WaterZone.instance.StopUnderwaterEffects();
	}
}
