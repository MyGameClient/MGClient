using UnityEngine;
using System.Collections;

public class ObjectController : MonoBehaviour {

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
		gameObject.SetActive (false);
	}
}
