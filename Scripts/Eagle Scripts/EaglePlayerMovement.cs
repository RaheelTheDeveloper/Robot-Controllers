using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EaglePlayerMovement : MonoBehaviour {

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
	public bool isOnGround = false;
	float x, y;
	ETCJoystick playerJoystick;
	ETCButton easyTouchButton;
	ETCJoystick easyTouchJoystick;
	public static EaglePlayerMovement instance;
	[HideInInspector]
	public bool cantFly = false;
	[HideInInspector]
	public bool cantWalk = false;
	public GameObject rayCastPoint;
	public GameObject eagleBody;
	Vector3 tempPosition;
	RaycastHit hit;
	Vector3 pos;
	public GameObject flyButton;
	[HideInInspector]
	public bool eagleIsDead = false;

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



		if (!eagleIsDead) 
		{
			anim.speed = 1;

			if (!fly && !cantWalk)
			{
				if (y == 0) 
				{
					rb.velocity = Vector3.zero;
					rb.angularVelocity = Vector3.zero;
					if (isOnGround) {
						if (!EaglePlayerLookMobile.instance.sideMove) 
						{
							anim.SetBool ("isWalking", false);
							anim.SetBool ("isIdle", true);
						}

					} 
					else 
					{
						anim.SetBool ("isFlying", true);
					}
				}
				else if (y > 0.01f && y < 0.4) 
				{
					rb.velocity = transform.forward.normalized * 0.2f;
					if (isOnGround) {
						WalkForward ();
						anim.speed = 0.5f;
					} else {
						anim.SetBool ("isWalking", false);
						anim.SetBool ("isFlying", true);
					}
				}
				else if (y >= 0.4f && y < 0.8f) 
				{
					rb.velocity = transform.forward.normalized * 0.5f;
					if (isOnGround) {
						WalkForward ();
					} else {
						anim.SetBool ("isWalking", false);
						anim.SetBool ("isFlying", true);
					}
				}
				else if(y >= 0.8f)
				{
					rb.velocity = transform.forward.normalized * 1f;
					if (isOnGround) {
						Run ();
					} else {
						anim.SetBool ("isRunning", false);
						anim.SetBool ("isFlying", true);
					}
				}
				else if (y < -0.1f && y > -0.4)
				{
					rb.velocity = transform.forward.normalized * -0.2f;

					if (isOnGround) {
						WalkForward ();
						anim.speed = 0.5f;
					} else {
						anim.SetBool ("isWalking", false);
					}
				}
				else if(y < -0.4f && y > -0.8f)
				{
					rb.velocity = transform.forward.normalized * -0.5f;
					if (isOnGround) {
						WalkForward ();
					} else {
						anim.SetBool ("isWalking", false);
					}
				}
				else if (y < -0.8f)
				{
					rb.velocity = transform.forward.normalized * -1f;

					if (isOnGround) {
						RunBackward ();
					} else {
						anim.SetBool ("isRunning", false);
					}
				}
			} 

			if (fly && !cantFly) 
			{
				gameObject.transform.Translate(Vector3.up * Time.deltaTime * 2f);
			}

			if (isOnGround && !fly && cantFly)
			{
				rb.useGravity = true;
			}
			else if (isOnGround && fly && !cantFly)
			{
				rb.useGravity = false;
			}

			else if (!isOnGround && !fly)
			{
				anim.SetBool ("isIdle", false);
				anim.SetBool ("isWalking", false);
				anim.SetBool ("isRunning", false);
				gameObject.transform.Translate(Vector3.down * Time.deltaTime * 1f);
				rb.velocity = Vector3.zero;
				rb.angularVelocity = Vector3.zero;
			}
		}
	}

	void FixedUpdate()
	{
		if (Physics.Raycast (rayCastPoint.transform.position, Vector3.down,  out hit, 25f))
		{

			if (hit.collider.tag == "Surface")
			{
				float a = hit.distance;
				if (a <= 0.1f) {
					isOnGround = true;
					fly = false;
					cantFly = false;
					if (!EaglePlayerLookMobile.instance.sideMove)
					{
						Idle ();
					}

					cantWalk = false;
					flyingFirstTimeFromGround = false;

				} 
				else if (a >= 24f) 
				{
					cantFly = true;
					cantWalk = true;
				}
				else 
				{
					if (y == 0) {
						anim.SetBool ("isFlying", true);
						anim.SetBool ("isFlyingBackward", false);
					}
					else if (y < -0.1f && y > -0.4)
					{
						anim.SetBool ("isFlyingBackward", true);
						anim.SetBool ("isFlying", false);
					}
					else if(y < -0.4f && y > -0.8f)
					{
						anim.SetBool ("isFlyingBackward", true);
						anim.SetBool ("isFlying", false);
					}
					else if (y < -0.8f)
					{
						anim.SetBool ("isFlyingBackward", true);
						anim.SetBool ("isFlying", false);
					}
					else 
					{
						anim.SetBool ("isFlying", true);
						anim.SetBool ("isFlyingBackward", false);
					}

					isOnGround = false;
					cantFly = false;
					cantWalk = true;
				}
			}
			Debug.DrawRay(rayCastPoint.transform.position, Vector3.down *1000, Color.yellow);
		}
	}

	public void Idle()
	{
		anim.SetBool ("isIdle", true);
		anim.SetBool ("isWalking", false);
		anim.SetBool ("isRunning", false);
		anim.SetBool ("isFlying", false);
		isRunning = false;
	}

	public void WalkForward()
	{
		anim.SetBool ("isWalking", true);
		anim.SetBool ("isRunning", false);
		anim.SetBool ("isIdle", false);
		anim.SetBool ("isFlying", false);
		isRunning = false;
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


	public void FlyBackward()
	{
		anim.SetBool ("isRunning", false);
		anim.SetBool ("isWalking", false);
		anim.SetBool ("isIdle", false);
		anim.SetBool ("isFlying", false);
		anim.SetBool ("isFlyingBackward", true);
	}

	public void FlyingOn()
	{
		flyButton.GetComponent<Button> ().interactable = false;
		fly = true;
		anim.SetBool ("isFlying", true);
		anim.SetBool ("isIdle", false);
	}

	bool flyingFirstTimeFromGround = false;

	public void FlyingOff()
	{
		fly = false;
		flyButton.GetComponent<Button> ().interactable = true;
	}

	public void Dead()
	{
		anim.SetBool ("isWalking", false);
		anim.SetBool ("isRunning", false);
		anim.SetBool ("isIdle", false);
		anim.SetBool ("isFlying", false);
		anim.SetBool ("isDead", true);
		eagleIsDead = true;
		flyButton.GetComponent<Button> ().interactable = false;
		easyTouchJoystick.GetComponent<ETCJoystick> ().activated = false;
		rb.useGravity = true;
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
