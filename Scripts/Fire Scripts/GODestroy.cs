using UnityEngine;
using System.Collections;

public class GODestroy : MonoBehaviour {
	
	public float Time;

	void OnEnable()
	{
		Invoke ("Disabler", Time);
	}

	void Disabler ()
	{
		gameObject.SetActive (false);
	}

}
