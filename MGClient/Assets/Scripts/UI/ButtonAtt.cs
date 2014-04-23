using UnityEngine;
using System.Collections;

public class ButtonAtt : MonoBehaviour {

	void Awake ()
	{
#if UNITY_EDITOR
		gameObject.SetActive (false);
#elif UNITY_ANDROID || UNITY_IPHONE
		gameObject.SetActive (true);
#endif
	}

	public void OnClick ()
	{
		PlayerInput.AttNormal ();
	}
}
