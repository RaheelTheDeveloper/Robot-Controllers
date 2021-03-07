using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;


public class LionPlayerLookMobile : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    PointerEventData myData;
    [HideInInspector]
    public Vector2 deltaOfPointer = Vector2.zero;
    [HideInInspector]
	public LionPlayerMovement playermov;
    [HideInInspector]
    public bool canMouseLook = false;
	[HideInInspector]
	public bool isDragging = false;
	public static LionPlayerLookMobile instance;

    void Awake()
    {
		playermov = GameObject.FindObjectOfType<LionPlayerMovement>();
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
		if (!LionPlayerMovement.instance.isJumpedClicked && !LionPlayerMovement.instance.lionisDead)
		{
			canMouseLook = true;
			myData = Data;
		}

    }
    public void OnDrag(PointerEventData Data)
    {
		if (!LionPlayerMovement.instance.isJumpedClicked && !LionPlayerMovement.instance.lionisDead)
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
		}
    }
}
