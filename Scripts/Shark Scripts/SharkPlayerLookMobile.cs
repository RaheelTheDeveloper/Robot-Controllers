using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;


public class SharkPlayerLookMobile : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    PointerEventData myData;
    [HideInInspector]
    public Vector2 deltaOfPointer = Vector2.zero;
    [HideInInspector]
	public SharkPlayerMovement playermov;
    [HideInInspector]
    public bool canMouseLook = false;
	[HideInInspector]
	public bool isDragging = false;
	public static SharkPlayerLookMobile instance;
	public bool sideMove = false;

    void Awake()
    {
		playermov = GameObject.FindObjectOfType<SharkPlayerMovement>();
    }

	void Start()
	{
		instance = this;
	}


    public void OnEndDrag(PointerEventData Data)
    {
        myData = Data;
        canMouseLook = false;
		playermov.Idle ();
		isDragging = false;
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

			canMouseLook = true;
			myData = Data;
    }
    public void OnDrag(PointerEventData Data)
	{

		isDragging = true;
		myData = Data;

			if (deltaOfPointer.x < -15) {

				if (playermov.isOnGround) {
				
					playermov.WalkForward ();
				} 
				
			}

			if (deltaOfPointer.x > 15) {

				sideMove = true;
				if (playermov.isOnGround) {
				
					playermov.WalkForward ();
				}

			}
		}
}
