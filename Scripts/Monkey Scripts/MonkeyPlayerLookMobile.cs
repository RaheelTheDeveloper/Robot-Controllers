using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;


public class MonkeyPlayerLookMobile : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    PointerEventData myData;
    [HideInInspector]
    public Vector2 deltaOfPointer = Vector2.zero;
    [HideInInspector]
	public MonkeyPlayerMovement playermov;
    [HideInInspector]
    public bool canMouseLook = false;
	[HideInInspector]
	public bool isDragging = false;
	public static MonkeyPlayerLookMobile instance;

    void Awake()
    {
		playermov = GameObject.FindObjectOfType<MonkeyPlayerMovement>();
    }

	void Start()
	{
		instance = this;
	}


    public void OnEndDrag(PointerEventData Data)
    {
        myData = Data;
        canMouseLook = false;
//      print("OnEndDrag");
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
		if (!MonkeyPlayerMovement.instance.isJumpedClicked && !MonkeyPlayerMovement.instance.monkeyIsDead)
		{
			canMouseLook = true;
			myData = Data;
//			print ("OnBeginDrag");
		}

    }
    public void OnDrag(PointerEventData Data)
    {
		if (!MonkeyPlayerMovement.instance.isJumpedClicked && !MonkeyPlayerMovement.instance.monkeyIsDead)
		{
			isDragging = true;
        	myData = Data;

			if (deltaOfPointer.x < -15)
			{
				playermov.WalkForward ();
			}

			if (deltaOfPointer.x > 15) {
				playermov.WalkForward ();
			}
//			Debug.Log ("Dragging");
		}
    }
}
