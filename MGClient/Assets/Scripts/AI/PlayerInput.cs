using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour {

	public delegate void InputDirDelegate (Vector2 dir);
	public delegate void InputAttDelegate ();

	public static InputDirDelegate inputDirDelegate;
	public static InputAttDelegate inputAttDelegate;

	private Vector2 dirIndex;

	void Update () {
#if UNITY_EDITOR
		dirIndex = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		if (Input.GetKeyDown(KeyCode.J))
		{
			if (inputAttDelegate != null)
			{
				inputAttDelegate ();
			}
		}
#elif UNITY_ANDROID
		dirIndex = ;
#endif
		if (inputDirDelegate != null)
		{
			inputDirDelegate (dirIndex);
		}
	}
}
