using UnityEngine;
using System.Collections;

public class PlayerController : Unit {

	public int maxAtt = 2;

	void Start () 
	{
		CameraController.instance.SetCameraTargetInfo (transform);
		Init ();
	}

	void OnEnable ()
	{
		PlayerInput.inputAttDelegate += inputAttDelegate;
		PlayerInput.inputDirDelegate += inputDirDelegate;
	}

	void OnDisable ()
	{
		PlayerInput.inputAttDelegate -= inputAttDelegate;
		PlayerInput.inputDirDelegate -= inputDirDelegate;
	}
	
	void inputDirDelegate (Vector2 dir)
	{
		float x = dir.x;
		float y = dir.y;	
		if (x != 0 || y != 0)
		{
			if (x != 0)
			{
				xDir = x > 0 ? Dir.Right : Dir.Left;
			}
			if (isAttack == false)
			{
				Play (Clip.Walk);
			}
		}
		else
		{
			if (isAttack == false)
			{
				Play(Clip.Stand);
			}
		}
		Vector2 op = new Vector2 (x, y);
		transform.Translate (op + op * Main.Instance.hero.speed * Time.deltaTime);
		Vector3 pos = transform.position;
		pos.x = Mathf.Clamp (pos.x, 0, CameraController.instance.width);
		pos.y = Mathf.Clamp (pos.y, 0, CameraController.instance.targetMaxHight);
		pos.z = pos.y;
		transform.position = pos;
	}

	private bool isIng = false;
	private int atInx = 0;
	void inputAttDelegate()
	{
		atInx++;
		atInx = Mathf.Min (atInx, maxAtt);

		if (isIng == false)
		{
			isIng = true;
			StartCoroutine (attInterver ());
		}
	}

	IEnumerator attInterver ()
	{
		for (int i = (int) Clip.Hit; i < (int) Clip.Hit + atInx; i++)
		{
			Play ((Clip) i, CompletedPalyStand, AnimationEventTriggeredAtt);
			yield return new WaitForSeconds (currentClipTime);
		}
		isIng = false;
		atInx = 0;
	}

	void CompletedPalyStand (tk2dSpriteAnimator a, tk2dSpriteAnimationClip b)
	{
		Play (Clip.Stand);
	}

	void AnimationEventTriggeredAtt (tk2dSpriteAnimator a, tk2dSpriteAnimationClip b, int c)
	{
		//TODO:

	}
}
