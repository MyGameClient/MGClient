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
	spell0,
	spell1,
	spell2,
	Die
	
}

public abstract class Unit : MonoBehaviour {


	public Troop troop;

	protected TweenPosition tweenPosition;
	protected Spell spell;
	public bool isSpell
	{
		get{
			return _currentClip.ToString().Contains("spell");
		}
	}

	[HideInInspector]
	public tk2dSprite tkSp;
	[HideInInspector]
	public tk2dSpriteAnimator tkAnt;

	protected bool isPlayer = false;

	protected void Init ()
	{
		tkSp = GetComponent <tk2dSprite>();
		tkAnt = GetComponent<tk2dSpriteAnimator>();
		tkAnt.AnimationCompleted = CompletedPalyStand;
		Vector3 pos =  transform.position;
		pos.z = pos.y;
		transform.position = pos;
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
				|| _currentClip == Clip.Hit || _currentClip == Clip.spell0 || _currentClip == Clip.spell1;
					//|| _currentClip == Clip.spell2;
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

	public bool isMove
	{
		get{
			return tweenPosition != null && tweenPosition.enabled;
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
			return tkAnt.CurrentClip.frames.Length / tkAnt.CurrentClip.fps;
		}
	}

	public void Play (Clip c)
	{
		Play (c, null, null);
	}
	
	public void Play (Clip c, Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip> AnimationCompleted, Action<tk2dSpriteAnimator, tk2dSpriteAnimationClip, int> AnimationEventTriggered)
	{
		if (tkAnt == null || tkAnt.CurrentClip == null)
		{
			return;
		}
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
	public abstract void ApplyDmg (float dmg, bool isSlider);
	public void Hitted(Clip c, float dmg = 0, bool isSlider = false)
	{
		if (isPlayer == true && _currentClip == Clip.spell2)
		{
			return;
		}
		else
		{
			Play (c);
		}
		ExtraInfo ();

		colorRed ();
		ApplyDmg (dmg, isSlider);
	}
	public void colorRed ()
	{
		TweenTk2dColor.Begin (gameObject, 0.25f, Color.red).onFinished = resetColor;
	}
	public void resetColor (UITweener u)
	{
		TweenTk2dColor.Begin (gameObject, 0.25f, Color.white).onFinished = null;
	}
	public void MoveToTarget (Vector3 target, float speed)
	{
		xDir = (target.x > transform.position.x) ? Dir.Right : Dir.Left;
		
		tweenPosition = TweenPosition.Begin (gameObject, MGMath.getDist2D (transform.position, MGMath.getClampPos (target)) / speed, MGMath.getClampPos (target));
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
	public void stop ()
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
		GameObject go = ObjectPool.Instance.LoadObject (path);
		go.transform.position = u.transform.position + new Vector3 (0, u.height, -10);//new Vector3 (ec.transform.position.x, height, ec.transform.position.z);
	}

	public void AddDMG (string path, Unit u, float dmg, bool isBig)
	{
		GameObject go = ObjectPool.Instance.LoadObject (path);
		go.transform.position = u.transform.position + new Vector3 (UnityEngine.Random.Range (-50, 50), UnityEngine.Random.Range (u.height, u.height * 2) , -1000);
		go.GetComponent <NumEF>().showMessage ((int)dmg, isBig);
	}

	public void AddSP (string path, float offset = 0)
	{
		GameObject go = ObjectPool.Instance.LoadObject (path);
		go.transform.position = transform.position + new Vector3 (MGMath.getDirNumber(this) * offset, height, -10);//new Vector3 (ec.transform.position.x, height, ec.transform.position.z);
		go.transform.parent = transform;
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
