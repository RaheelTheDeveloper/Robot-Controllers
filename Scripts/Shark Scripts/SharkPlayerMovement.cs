using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SharkPlayerMovement : MonoBehaviour {

	Animator anim;
	float amtToMove;
	float speed;
	private bool canAim = false;
	public UnityStandardAssets.Utility.SmoothFollow mainCamera;
//	[HideInInspector]
	public float Touchsensitivity = 0.02f;
	[HideInInspector]
	public bool isRunning = false;
	[HideInInspector]
	public Rigidbody rb;
	[HideInInspector]
	public bool fly = false;
	[HideInInspector]
	public bool swimUp = false;
	[HideInInspector]
	public bool swimDown = false;
	[HideInInspector]
	public bool isOnGround = false;
	float x, y;
	ETCJoystick playerJoystick;
	public GameObject playerControls;
	ETCButton easyTouchButton;
	ETCJoystick easyTouchJoystick;
	public static SharkPlayerMovement instance;
	[HideInInspector]
	public bool cantFly = false;
	[HideInInspector]
	public bool cantWalk = false;
//	public GameObject rayCastPoint;
	public GameObject eagleBody;
	Vector3 tempPosition;
	RaycastHit hit;
	Vector3 pos;
	bool sharkAttacking = false;
	bool sharkIsMovingBackward = false;
	bool sharkIsDead = false;

	// Use this for initialization
	void Start () 
	{
		instance = this;
		anim = GetComponent<Animator> ();
		easyTouchJoystick = GameObject.FindObjectOfType<ETCJoystick> ();
		easyTouchButton = GameObject.FindObjectOfType<ETCButton> ();
		rb = gameObject.GetComponent<Rigidbody> ();
	}

	// Update is called once per frame
	void Update () 
	{
		x = easyTouchJoystick.axisX.axisValue;
		y = easyTouchJoystick.axisY.axisValue;

		anim.speed = 1;

		if (!sharkIsDead) {
			if (y == 0) 
			{
				rb.velocity = Vector3.zero;
				rb.angularVelocity = Vector3.zero;
				if (sharkAttacking) {
					AttackAnimation ();
				} else {
					Idle ();
				}
			}
			else if (y > 0.01f && y < 0.4) 
			{
				rb.velocity = transform.forward.normalized * 0.1f;
				if (sharkAttacking) {
					AttackAnimation ();
				} else {
					WalkForward ();
				}
				anim.speed = 0.5f;
			}
			else if (y >= 0.4f && y < 0.8f) 
			{
				rb.velocity = transform.forward.normalized * 0.2f;
				if (sharkAttacking) {
					AttackAnimation ();
				} else {
					WalkForward ();
				}
			}
			else if(y >= 0.8f)
			{
				rb.velocity = transform.forward.normalized * 0.3f;
				if (sharkAttacking) {
					AttackAnimation ();
				} else {
					Run();
				}
			}
			else if (y < -0.1f && y > -0.4)
			{
				rb.velocity = transform.forward.normalized * -0.1f;
				if (sharkAttacking) {
					AttackAnimation ();
				} else {
					WalkForward ();
				}
				anim.speed = 0.5f;
			}
			else if(y < -0.4f && y > -0.8f)
			{
				rb.velocity = transform.forward.normalized * -0.2f;
				if (sharkAttacking) {
					AttackAnimation ();
				} else {
					WalkForward ();
				}
			}
			else if (y < -0.8f)
			{
				rb.velocity = transform.forward.normalized * -0.3f;
				sharkIsMovingBackward = true; 

			}

			if (sharkAttacking && !sharkIsMovingBackward) {
				rb.velocity = transform.forward.normalized * 20f;
			}

			if (swimUp) {
				gameObject.transform.Translate(Vector3.up * Time.deltaTime * 1f);

			}
			else if (swimDown) {
				gameObject.transform.Translate(Vector3.down * Time.deltaTime * 1f);
			}
		}
	}

	public void Idle()
	{
		anim.SetBool ("isIdle", true);
		anim.SetBool ("isWalking", false);
		anim.SetBool ("isRunning", false);
		anim.SetBool ("isFlying", false);
	}

	public void WalkForward()
	{
		anim.SetBool ("isWalking", true);
		anim.SetBool ("isRunning", false);
		anim.SetBool ("isIdle", false);
	}
		
	public void Run()
	{
		anim.SetBool ("isRunning", true);
		anim.SetBool ("isWalking", false);
		anim.SetBool ("isIdle", false);
		isRunning = true;
	}

	public void RunBackward()
	{
		anim.SetBool ("isRunning", true);
		anim.SetBool ("isWalking", false);
		anim.SetBool ("isIdle", false);
	}

	public void SwimUpPressed()
	{
		swimUp = true;
		swimDown = false;
	}

	public void SwimUpDownReleased()
	{
		swimUp = false;
		swimDown = false;
	}
		
	public void SwimDownPressed()
	{
		swimUp = false;
		swimDown = true;
	}

	void AttackAnimation(){
		anim.SetBool ("isIdle", false);
		anim.SetBool ("isRunning", false);
		anim.SetBool ("isWalking", false);
		anim.SetBool ("isAttacking", true);
	}


	public void Attack()
	{
		sharkAttacking = true;
		Invoke ("SharkAttackOff", 0.5f);
	}

	void SharkAttackOff()
	{
		sharkAttacking = false;
		sharkIsMovingBackward = false; 
	}


	public void Dead(){
		sharkIsDead = true;
		anim.SetBool ("isIdle", false);
		anim.SetBool ("isRunning", false);
		anim.SetBool ("isWalking", false);
		anim.SetBool ("isAttacking", false);
		anim.SetBool ("isDead", true);
		GameObject.FindObjectOfType<ETCJoystick> ().activated = false;
		GameObject.FindObjectOfType<ETCButton> ().activated = false;
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

}
