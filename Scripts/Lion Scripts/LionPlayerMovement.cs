using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LionPlayerMovement : MonoBehaviour {

	Animator anim;
	float amtToMove;
	float speed;
	private bool canAim = false;
	public UnityStandardAssets.Utility.SmoothFollow mainCamera;
	public float Touchsensitivity = 0.02f;
	[HideInInspector]
	public bool isRunning = false;
	[HideInInspector]
	public bool isJumpedClicked = false;
	float x, y;
	public GameObject crackEffect;
	ETCJoystick playerJoystick;
	public GameObject playerControls;
	public GameObject jumpButton;
	ETCButton easyTouchButton;
	ETCJoystick easyTouchJoystick;
	public static LionPlayerMovement instance;
	Rigidbody rb;
	[HideInInspector]
	public bool lionisDead = false;
	ShakeCamera shakeCamera;

	// Use this for initialization
	void Start () 
	{
		instance = this;
		anim = GetComponent<Animator> ();
		easyTouchJoystick = GameObject.FindObjectOfType<ETCJoystick> ();
		easyTouchButton = GameObject.FindObjectOfType<ETCButton> ();
		shakeCamera = GameObject.FindObjectOfType<ShakeCamera> ();
		rb = GetComponent<Rigidbody> ();
	}

	// Update is called once per frame
	void Update () 
	{
		x = easyTouchJoystick.axisX.axisValue;
		y = easyTouchJoystick.axisY.axisValue;
		if (!lionisDead) {
			if (isRunning)
			{
				rb.velocity = transform.forward.normalized * 0.5f;
			}

			anim.speed = 1;

			if (!isJumpedClicked && !LionPlayerLookMobile.instance.isDragging)
			{
				if (y == 0) 
				{
					Idle();
					jumpButton.SetActive (true);
				}
				else if (y > 0.01f && y < 0.4) 
				{
					WalkForward ();
					anim.speed = 0.5f;
					jumpButton.SetActive (false);
				}
				else if(y >= 0.8f)
				{
					Run ();
					jumpButton.SetActive (false);
				}
				else if (y < -0.1f && y > -0.4)
				{
					WalkForward ();
					anim.speed = 0.5f;
					jumpButton.SetActive (false);
				}
				else if(y < -0.4f && y > -0.8f)
				{
					WalkForward ();
					jumpButton.SetActive (false);
				}
				else if (y < -0.8f)
				{
					RunBackward ();
					jumpButton.SetActive (false);
				}
			}
		}

	}

	public void Idle()
	{
		anim.SetBool ("isWalking", false);
		anim.SetBool ("hasJumped", false);
		anim.SetBool ("isRunning", false);
		anim.SetBool ("isIdle", true);
		isRunning = false;
	}

	public void WalkForward()
	{
		anim.SetBool ("isWalking", true);
		anim.SetBool ("isRunning", false);
		anim.SetBool ("isIdle", false);
		isRunning = false;
	}

	public void WalkBackward()
	{
		anim.SetBool ("isWalking", true);
		anim.SetBool ("isIdle", false);
	}

	public void TurnLeft()
	{
		anim.SetBool ("isWalking", true);
	}

	public void TurnRight()
	{
		anim.SetBool ("isWalking", true);
	}

	public void Jump()
	{
		isJumpedClicked = true;
		anim.SetBool ("hasJumped", true);
		easyTouchButton.GetComponent<ETCButton> ().activated = false;
		easyTouchJoystick.GetComponent<ETCJoystick> ().activated = false;
		Invoke ("isJumpedClickedOff", 3f);
		StartCoroutine (ShakeCameraEffect ());
		Invoke ("ActivateTheCrackButton", 3f);
	}
		
	public void Run()
	{
		anim.SetBool ("isRunning", true);
		isRunning = true;
	}

	public void RunBackward()
	{
		anim.SetBool ("isRunning", true);
	}

	public void Dead()
	{
		anim.SetBool ("isWalking", false);
		anim.SetBool ("hasJumped", false);
		anim.SetBool ("isRunning", false);
		anim.SetBool ("isIdle", false);
		anim.SetBool ("isDead", true);
		lionisDead = true;
		easyTouchButton.GetComponent<ETCButton> ().activated = false;
		easyTouchJoystick.GetComponent<ETCJoystick> ().activated = false;
	}

	internal void SetAim(Vector2 deltaOfPointer)
	{
		if (canAim) 
		{
			if (deltaOfPointer.x != 0)
			{
				gameObject.transform.localEulerAngles = new Vector3 (gameObject.transform.localEulerAngles.x, gameObject.transform.localEulerAngles.y + deltaOfPointer.x * Touchsensitivity,
				gameObject.transform.localEulerAngles.z);
			} 

			// For Vertical Touch Movement
			if (mainCamera && deltaOfPointer.y != 0) 
			{
				mainCamera.setRotation (deltaOfPointer, true);
			} 
		} 

		else 
		{
			mainCamera.setRotation (Vector2.zero, false);
		}
	}

	internal void AllowAiming(bool canMouseLook)
	{
		canAim = canMouseLook;
	}

	void isJumpedClickedOff()
	{
		isJumpedClicked = false;
	}

	IEnumerator ShakeCameraEffect()
	{
		yield return new WaitForSeconds (1f);
		shakeCamera.ShakeCameraMine (4f, 1f);
//		Instantiate (crackEffect, gameObject.transform.GetChild(0).transform.position, Quaternion.Euler(gameObject.transform.GetChild(0).transform.localRotation.eulerAngles.x, gameObject.transform.GetChild(0).transform.localRotation.eulerAngles.y, gameObject.transform.GetChild(0).transform.localRotation.eulerAngles.z) );
		gameObject.transform.GetChild(0).gameObject.SetActive(true);
		yield return new WaitForSeconds (2f);
		gameObject.transform.GetChild (0).gameObject.SetActive (false);
	}

	void ActivateTheCrackButton()
	{
		easyTouchButton.GetComponent<ETCButton> ().activated = true;
		easyTouchJoystick.GetComponent<ETCJoystick> ().activated = true;
	}

}
