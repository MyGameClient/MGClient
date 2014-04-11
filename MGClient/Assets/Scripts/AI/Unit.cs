using UnityEngine;
using System.Collections;
using System;

public enum Dir
{
	Left,
	Right,
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

public abstract class Unit : MonoBehaviour {


	protected TweenPosition tweenPosition;

	[HideInInspector]
	public tk2dSprite tkSp;
	[HideInInspector]
	public tk2dSpriteAnimator tkAnt;

	protected void Init ()
	{
		tkSp = GetComponent <tk2dSprite>();
		tkAnt = GetComponent<tk2dSpriteAnimator>();
		tkAnt.AnimationCompleted = CompletedPalyStand;
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

	//hight
	public float height
	{
		get{
			return tkSp.GetBounds().size.y / 2;
		}
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

	public void CompletedPalyStand (tk2dSpriteAnimator a, tk2dSpriteAnimationClip b)
	{
		Play (Clip.Stand);
	}

	#region Hit or hitted method

	public abstract void ExtraInfo ();

	public void Hitted(Clip c)
	{
		Play (c);
		colorRed ();
	}
	public void colorRed ()
	{
		TweenTk2dColor.Begin (gameObject, 0.25f, Color.red).onFinished = resetColor;
	}
	public void resetColor (UITweener u)
	{
		TweenTk2dColor.Begin (gameObject, 0.25f, Color.white).onFinished = null;
	}
	public void HittedMove (float target, Unit att)
	{
		if (!Unit.isFront (this, att))
		{
			xDir = (xDir == Dir.Left) ? Dir.Right : Dir.Left;
		}
		stop ();
		tweenPosition = TweenPosition.Begin (gameObject, 0.1f, MGMath.getClampPos (transform.position + new Vector3 (target, 0, 0)));
	}
	protected void stop ()
	{
		ExtraInfo ();
		if (tweenPosition != null)
		{
			tweenPosition.onUpdate = null;
			tweenPosition.enabled = false;
		}
	}

	public void AddEF (string path, Unit u)
	{
		GameObject go = ObjectPool.Instance.LoadObject (MGConstant.EF + path);
		go.transform.position = u.transform.position + new Vector3 (0, u.height, -10);//new Vector3 (ec.transform.position.x, height, ec.transform.position.z);
	}

	#endregion

	public static bool attDistance (Unit att, Unit def, float disVal)
	{
		float x = Mathf.Abs(def.transform.position.x - att.transform.position.x);
		float y = Mathf.Abs(def.transform.position.y - att.transform.position.y);
		return x <= disVal / 2 && y <= disVal / 10;
	}
	
	public static bool isFront (Unit att, Unit def)
	{
		if (att.xDir == Dir.Right)
		{
			return def.transform.position.x >= att.transform.position.x;
		}
		else if (att.xDir == Dir.Left)
		{
			return def.transform.position.x < att.transform.position.x;
		}
		return false;
	}
}
