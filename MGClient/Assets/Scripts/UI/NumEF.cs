using UnityEngine;
using System.Collections;

public class NumEF : MonoBehaviour {

	public UIPanel panel;
	public UILabel label;
	public Transform pool;
	public float offset = 50;

	public void showMessage (int text, bool isBig)
	{
		label.text = text.ToString ();
		panel.alpha = 1.0f;
		pool.localScale = Vector3.one;
		if (!isBig)
		{
			Vector3 target = transform.position + new Vector3(0, offset, 0);
			TweenPosition.Begin (gameObject, 2.0f, target);
			Invoke ("AlphaSetting", 1.0f);
		}
		else
		{
			pool.localScale = Vector3.one * 3;
			TweenScale.Begin (pool.gameObject, 0.1f, Vector3.one * 1.5f);
			Invoke ("AlphaSetting", 1.0f);
		}
	}

	void AlphaSetting ()
	{
		TweenAlpha.Begin (gameObject, 1.0f, 0).onFinished = onFinishedPos;
	}

	void onFinishedPos (UITweener u)
	{
		gameObject.SetActive (false);
	}

//	void OnGUI ()
//	{
//		if (GUI.Button (new Rect (0, 0, 100, 100), "1"))
//		{
//			showMessage (100, false);
//		}
//		if (GUI.Button (new Rect (0, 100, 100, 100), "2"))
//		{
//			showMessage (100, true);
//		}
//	}
}
