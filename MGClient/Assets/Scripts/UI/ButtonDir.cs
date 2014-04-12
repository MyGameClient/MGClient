using UnityEngine;
using System.Collections;

public class ButtonDir : MonoBehaviour {

	public static ButtonDir instance;

	public Camera uiCamera;
	public Vector2 position;

	void Awake ()
	{
#if UNITY_EDITOR
		gameObject.SetActive (false);
#elif UNITY_ANDROID || UNITY_IPHONE
		instance = this;
		gameObject.SetActive (true);
#endif
	}

	public void OnPress (bool isPressed)
	{
		if (isPressed)
		{
			InvokeRepeating ("HitRotation", 0, (float)1 / (float)30);
		}
		else
		{
			position = Vector2.zero;
			CancelInvoke ("HitRotation");
		}
	}


	private Vector2 orinal = Vector3.zero;
	public bool isDC = true;
	void HitRotation ()
	{
		orinal =  uiCamera.WorldToScreenPoint (transform.position);
		transform.up = UICamera.lastTouchPosition - orinal;
		position = transform.up;
	}
}
