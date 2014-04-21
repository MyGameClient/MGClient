using UnityEngine;
using System.Collections;

public class ObjectController : MonoBehaviour {

	public bool isHiddenParent = false;
	private tk2dSpriteAnimator tkAnt;


	void OnEnable ()
	{
		if (tkAnt == null)
		{
			tkAnt = GetComponent<tk2dSpriteAnimator>();
		}
		tkAnt.Play ();
		tkAnt.AnimationCompleted = AnimationCompletedHidden;
	}

	void AnimationCompletedHidden (tk2dSpriteAnimator a, tk2dSpriteAnimationClip b)
	{
		if (transform.parent != null && isHiddenParent == true)
		{
			transform.parent.gameObject.SetActive (false);
		}
		gameObject.SetActive (false);
	}
}
