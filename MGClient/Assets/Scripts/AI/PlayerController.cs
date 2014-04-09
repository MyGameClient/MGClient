using UnityEngine;
using System.Collections;

public class PlayerController : Unit {

	public int maxAtt = 2;
	//TODO:
	public float distanceTest = 400;
	public float attMoveDis = 15;

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
		if (isAttack == true)
		{
			return;
		}
		float x = dir.x;
		float y = dir.y;	
		if (x != 0 || y != 0)
		{
			if (x != 0)
			{
				xDir = x > 0 ? Dir.Right : Dir.Left;
			}
			Play (Clip.Walk);
		}
		else
		{
			Play(Clip.Stand);
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
			Play ((Clip) i, null, AnimationEventTriggeredAtt);
			yield return new WaitForSeconds (currentClipTime);
		}
		isIng = false;
		atInx = 0;
	}

	void AnimationEventTriggeredAtt (tk2dSpriteAnimator a, tk2dSpriteAnimationClip b, int c)
	{
		//TODO:
		HitTarget ();
		MoveForwrd ();
	}

	void MoveForwrd ()
	{
		Vector3 v = ((xDir == Dir.Right) ? 1 : -1) * new Vector3 (attMoveDis, 0, 0);
		TweenPosition.Begin (gameObject, 0.1f, MGMath.getClampPos(v + transform.position));
	}

	void HitTarget ()
	{
		foreach(EnemyController ec in EnemyController.enemys)
		{
			if (MGMath.isFront (this, ec) && MGMath.attDistance (this, ec, distanceTest))
			{
				GameObject go = ObjectPool.Instance.LoadObject (MGConstant.EF + "EF001");//TODO:
				go.transform.position = ec.transform.position + new Vector3 (0, height, -10);//new Vector3 (ec.transform.position.x, height, ec.transform.position.z);
				ec.Hitted ();
			}
		}
	}
}
