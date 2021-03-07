using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackHandler : MonoBehaviour {

	public float timeToDestroy = 3.0f;
	public void OnEnable(){

		Destroy (this.gameObject, timeToDestroy);
	}
}
