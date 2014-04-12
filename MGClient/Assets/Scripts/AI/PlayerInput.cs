using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour {

	public delegate void InputDirDelegate (Vector2 dir);
	public delegate void InputAttDelegate ();

	public static InputDirDelegate inputDirDelegate;
	public static InputAttDelegate inputAttDelegate;

	private Vector2 dirIndex;

	void Update () {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
		//dirIndex = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
		/*dirIndex.x = Mathf.CeilToInt(dirIndex.x);
		dirIndex.y = Mathf.CeilToInt(dirIndex.y);*/
		float x = Input.GetKey (KeyCode.A) ? -1 : Input.GetKey (KeyCode.D) ? 1 : 0;
		float y = Input.GetKey (KeyCode.S) ? -1 : Input.GetKey (KeyCode.W) ? 1 : 0;
		dirIndex = new Vector2 (x, y);
		if (Input.GetMouseButtonDown (1))
		{
			if (inputAttDelegate != null)
			{
				inputAttDelegate ();
			}

		}
#elif UNITY_ANDROID || UNITY_IPHONE
		dirIndex = new Vector2(ButtonDir.instance.position.x, ButtonDir.instance.position.y);
#endif
		if (inputDirDelegate != null)
		{
			inputDirDelegate (dirIndex);
		}
	}
}
