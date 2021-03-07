using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class FireProjectile : MonoBehaviour {

    RaycastHit hit;
    public GameObject[] projectiles;
    public Transform spawnPosition1;
	public Transform spawnPosition2;
    [HideInInspector]
    public int currentProjectile = 0;
	public GameObject crossHair;
	bool robotFire = false;
	public float Fire1Delay = 0.1f;
	public float Fire2Delay = 0.25f;
	public float Firerate = 0.2f;
	public float FireSpeed = 10000f;
	public List <GameObject> bulletsList;
	public GameObject bullet;
	public GameObject bulletsParent;
	[Range (0.1f, 1f)]
	public float fireRate = 0.2f;
	public Vector3 offset;
	private int currentBulletIndex = 0;
	private bool firstBullet = true, projectileSpawned = false, hasHit = false;

	void Start ()
	{
		SpawnProjectile (10, false);
	}

	void FixedUpdate()
	{
		if (Physics.Raycast (Camera.main.ScreenPointToRay (crossHair.transform.position), out hit, 10000f))
		{
			if (hit.collider.tag == "Enemy")
			{
				crossHair.GetComponent<Image> ().color = new Color32 (255, 0, 0, 255);
			} 
			else 
			{
				crossHair.GetComponent<Image> ().color = new Color32 (255, 255, 255, 255);
			}
			hasHit = true;
			Debug.DrawRay (Camera.main.ScreenPointToRay (Input.mousePosition).origin, Camera.main.ScreenPointToRay (Input.mousePosition).direction * 1000, Color.yellow);
		}
		else 
		{
			hasHit = false;
		}
	}

	public void Fire(bool isFiring)
	{
		if (isFiring) 
		{
			InvokeRepeating ("ShootProjectile", Fire1Delay, fireRate);
		}
		else 
		{
			CancelInvoke ("ShootProjectile");
		}
	}

	void ProjectileSetting(GameObject projectileInUse)
	{
		Transform spawnPoint = firstBullet ? spawnPosition1 : spawnPosition2;
		projectileInUse.SetActive (true);
		projectileInUse.transform.position = spawnPoint.transform.position;

		if (hasHit)
			projectileInUse.transform.LookAt (hit.point);
		else
			projectileInUse.transform.LookAt(Camera.main.transform.forward * 1000000);

		projectileInUse.GetComponent<Rigidbody> ().isKinematic = false;
		projectileInUse.GetComponent<Rigidbody> ().velocity = Vector3.zero;
		projectileInUse.GetComponent<Rigidbody> ().angularVelocity = Vector3.zero;
		projectileInUse.GetComponent<Rigidbody> ().AddForce (projectileInUse.transform.forward * 10000);
		firstBullet = !firstBullet;
	}

	void ShootProjectile()
	{
		foreach (GameObject projectile in bulletsList) 
		{
			for (int i = 0; i < projectile.transform.childCount; i++)
			{
				if(projectile.transform.GetChild(i).GetComponent<ProjectileScript>())
				{
					if (!projectile.transform.GetChild (i).gameObject.activeInHierarchy && !projectileSpawned) 
					{
						ProjectileSetting (projectile.transform.GetChild (i).gameObject);
						projectileSpawned = true;
					}
				}
			}
		}

		if(!projectileSpawned)
		{
			SpawnProjectile (1, true);
		}

		projectileSpawned = false;
	}

	void SpawnProjectile(int count, bool shouldShoot)
	{
		GameObject newProjectile = null;

		for (int i = 0; i < count; i++) 
		{
			newProjectile = Instantiate (bullet);
			newProjectile.SetActive (true);
			newProjectile.transform.SetParent (bulletsParent.transform);
			bulletsList.Add (newProjectile);
		}
		
		if (shouldShoot) 
		{
			ProjectileSetting (newProjectile.transform.GetChild(1).gameObject);
		}
	}
}