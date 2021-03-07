using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProjectileScript : MonoBehaviour {


    [HideInInspector]
    public Vector3 impactNormal; //Used to rotate impactparticle.
	public GameObject explosionParticles;
	private bool isReady;

	void Awake()
	{

	}
		
	void OnEnable () {

		isReady = true;
		Invoke ("SetOff", 3f);
	}
	
//	 Update is called once per frame
	void OnCollisionEnter (Collision hit) {
		if(isReady && !hit.gameObject.CompareTag("FireBullet")){

			explosionParticles.transform.position = transform.position;
			foreach (ParticleSystem particles in GetComponentsInChildren<ParticleSystem>()) 
			{
				particles.Stop(true);
				particles.Play(true);
			}
			explosionParticles.SetActive (true);

			if (hit.gameObject.tag == "Untagged")
			{

			}

			if (hit.gameObject.tag == "Surface")
			{

			}

			if (hit.gameObject.tag == "Enemy") 
			{
				Destroy (hit.gameObject);

			}

			GetComponent<Rigidbody> ().isKinematic = true;
		}
	}

	void SetOff()
	{
		gameObject.SetActive(false);
	}

}
