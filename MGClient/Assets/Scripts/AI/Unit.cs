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


	//dir
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

	//isAttack
	public bool isAttack
	{
		get{
			return _currentClip == Clip.AOE || _currentClip == Clip.Attack || _currentClip == Clip.AttackLast
				|| _currentClip == Clip.Hit;
		}
	}

	//isHitted
	public bool isHitted
	{
		get{
			return _currentClip == Clip.Hitted;
		}
	}

	//isFall
	public bool isFall
	{
		get{
			return _currentClip == Clip.Fall;
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

	private Clip _currentClip;
	public Clip currentClip
	{
		get{
			return _currentClip;
		}
	}

	public float currentClipTime
	{
		get
		{
			return tkAnt.CurrentClip.fps / tkAnt.CurrentClip.frames.Length;
		}
	}

	public void Play (Clip c)
	{
		Play (c, null, null);
	}
	
	public void Play (Clip c, Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip> AnimationCompleted, Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int> AnimationEventTriggered)
	{
		if (tkAnt.CurrentClip.name != c.ToString ())
		{
			_currentClip = c;
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
}
