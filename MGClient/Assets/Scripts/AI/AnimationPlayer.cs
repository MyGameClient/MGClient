using UnityEngine;
using System.Collections;

public enum Clip
{
	Idle,
	Run,
	Attack1,
	Attack2,
	Hit,
	Fall,
	Die,
	Spell1,
}

public class AnimationPlayer : MonoBehaviour {

	public delegate void AnimationEventDelegate (Clip clip);
	public AnimationEventDelegate animationEventDelegate;

	private Clip m_Clip = Clip.Idle;
	public Clip clip
	{
		get
		{
			return m_Clip;
		}
	}

	/**********************Unit State*********************************/
	public bool isAttacking
	{
		get
		{
			return m_Clip.ToString ().Contains ("Attack");
		}
	}
	public bool isSpell
	{
		get
		{
			return m_Clip.ToString ().Contains ("Spell");
		}
	}
	public bool isHitted
	{
		get
		{
			return m_Clip == Clip.Hit;
		}
	}
	public bool isDead
	{
		get
		{
			return m_Clip == Clip.Die;
		}
	}
	public bool isFall
	{
		get
		{
			return m_Clip == Clip.Fall;
		}
	}
	
	public float length
	{
		get{
			//TODO:
			string currentClip  = m_Clip.ToString ();
			return animation.GetClip (currentClip).length / animation[currentClip].speed;
		}
	}
	public bool isPlaying (Clip clip)
	{
		return animation.IsPlaying (clip.ToString ());
	}
	public bool dontMove
	{
		get
		{
			return isAttacking || isFall || isHitted || isDead || isSpell;
		}
	}
	/**********************Unit State*********************************/

	void Start ()
	{
		animation["Hit"].speed = 2;
		animation["Attack1"].speed = 2;
		animation["Attack2"].speed = 2;
	}

	void Update ()
	{
		if (animation.isPlaying == false)
		{
			Play (Clip.Idle);
		}
	}

	public void Play (Clip clip)
	{
		if (m_Clip != clip)
		{
			m_Clip = clip;
			animation.CrossFade (clip.ToString ());
		}
	}

	void AntMessage ()
	{
		if (animationEventDelegate != null)
		{
			animationEventDelegate (m_Clip);
		}
	}
}
