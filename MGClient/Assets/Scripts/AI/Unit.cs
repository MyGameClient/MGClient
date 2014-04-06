using UnityEngine;
using System.Collections;
using System;

public class Unit : MonoBehaviour {

	[HideInInspector]
	public tk2dSprite tkSp;
	[HideInInspector]
	public tk2dSpriteAnimator tkAnt;

	protected void Init ()
	{
		tkSp = GetComponent <tk2dSprite>();
		tkAnt = GetComponent<tk2dSpriteAnimator>();
	}

	public enum Dir
	{
		Left,
		Right,
	}

	public Dir xDir
	{
		get{
			return tkSp.scale.x > 0 ? Dir.Right : Dir.Left;
		}
		set
		{
			float x = value == Dir.Right ? 1 : -1;
			Vector3 v = tkSp.scale;
			v.x = x;
			tkSp.scale = v;
		}
	}

	public enum Clip
	{
		Stand = 0,
		Walk,
		Hit,
		AttackLast,
		Attack,
		AOE,
		Hitted,
		Fall,
		Die

	}

	public void Play (Clip c)
	{
		Play (c, null, null);
	}
	
	public void Play (Clip c, Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip> AnimationCompleted, Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int> AnimationEventTriggered)
	{
		if (tkAnt.CurrentClip.name != c.ToString ())
		{
			tkAnt.Play (c.ToString ());
			if (AnimationCompleted != null)
			{
				tkAnt.AnimationCompleted = AnimationCompleted;
			}
			if (AnimationEventTriggered != null)
			{
				tkAnt.AnimationEventTriggered = AnimationEventTriggered;
			}
		}
	}

	public void CompletedPalyStand (tk2dSpriteAnimator a, tk2dSpriteAnimationClip b)
	{
		Play (Clip.Stand);
	}
}
