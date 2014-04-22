using UnityEngine;
using System.Collections;

public class ObjectController : MonoBehaviour {

	public bool isHiddenParent = false;

	private tk2dSpriteAnimator tkAnt;

	public delegate void HitTargetAoe (Vector3 pos);
	public HitTargetAoe hitTargetAoe;

	void OnEnable ()
	{
		if (tkAnt == null)
		{
			tkAnt = GetComponent<tk2dSpriteAnimator>();
		}
		tkAnt.Play ();
		tkAnt.AnimationCompleted = AnimationCompletedHidden;
		tkAnt.AnimationEventTriggered = AnimationEventTriggeredDelegate;
	}

	void OnDisable ()
	{
		hitTargetAoe = null;
	}

	void AnimationCompletedHidden (tk2dSpriteAnimator a, tk2dSpriteAnimationClip b)
	{
		if (transform.parent != null && isHiddenParent == true)
		{
			transform.parent.gameObject.SetActive (false);
		}
		gameObject.SetActive (false);
	}

	void AnimationEventTriggeredDelegate (tk2dSpriteAnimator a, tk2dSpriteAnimationClip b, int c)
	{
		Debug.Log ("P____" + hitTargetAoe);
		if (hitTargetAoe != null)
		{
			Vector3 pos = transform.position;
			pos.y = pos.z;
			hitTargetAoe (pos);
			//hitTargetAoe = null;
		}
	}
}
