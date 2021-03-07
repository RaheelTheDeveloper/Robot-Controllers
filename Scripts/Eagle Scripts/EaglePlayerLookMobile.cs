using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;


public class EaglePlayerLookMobile : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    PointerEventData myData;
    [HideInInspector]
    public Vector2 deltaOfPointer = Vector2.zero;
    [HideInInspector]
	public EaglePlayerMovement playermov;
    [HideInInspector]
    public bool canMouseLook = false;
	[HideInInspector]
	public bool isDragging = false;
	public static EaglePlayerLookMobile instance;
	public bool sideMove = false;

    void Awake()
    {
		playermov = GameObject.FindObjectOfType<EaglePlayerMovement>();
    }

	void Start()
	{
		instance = this;
	}


    public void OnEndDrag(PointerEventData Data)
    {
        myData = Data;
        canMouseLook = false;
		isDragging = false;
		sideMove = false;
    }

    void Update()
    {
		if (playermov)
        {
			playermov.AllowAiming(canMouseLook);
			if (myData != null) {
				deltaOfPointer = myData.delta;
				playermov.SetAim (deltaOfPointer);
			} else {
				deltaOfPointer = Vector2.zero;
				playermov.SetAim (deltaOfPointer);
			}
        }
    }

    public void OnBeginDrag(PointerEventData Data)
    {
		if (!EaglePlayerMovement.instance.eagleIsDead) 
		{
			canMouseLook = true;
			myData = Data;
		}
    }

    public void OnDrag(PointerEventData Data)
	{
		isDragging = true;
		myData = Data;

		if (!EaglePlayerMovement.instance.eagleIsDead)
		{
			if (deltaOfPointer.x < -15) {
				sideMove = true;
				if (playermov.isOnGround) {
					playermov.WalkForward ();
				} 
			}

			if (deltaOfPointer.x > 15)
			{
				sideMove = true;
				if (playermov.isOnGround) {
				
					playermov.WalkForward ();
				}
			}
		}
	}
}

