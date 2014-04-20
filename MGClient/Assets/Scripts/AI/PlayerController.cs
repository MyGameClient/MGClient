using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : Unit {

	public enum AttMode
	{
		INF,
		BOW
	}
	public AttMode attMode = AttMode.INF;

	public int maxAtt = 2;
	//TODO: need data
	public float distanceAtt = 400;
	public float attMoveDis = 15;


	public static List<PlayerController> players = new List<PlayerController> ();
	
	void Awake () {
		players.Add (this);
	}
	
	void Start () 
	{
		isPlayer = true;
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
		players.Remove (this);
		PlayerInput.inputAttDelegate -= inputAttDelegate;
		PlayerInput.inputDirDelegate -= inputDirDelegate;
	}
	
	void inputDirDelegate (Vector2 dir)
	{
		if (isAttack == true || isFall == true)
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
		}
		if (isHitted == false && currentClip != Clip.spell2)
		{
			Play((x != 0 || y != 0) ? Clip.Walk : Clip.Stand);
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
		if (attMode == AttMode.INF)
		{
			atInx++;
			atInx = Mathf.Min (atInx, maxAtt);

			if (isIng == false)
			{
				isIng = true;
				StartCoroutine (attInterver ());
			}
		}
		else if (attMode == AttMode.BOW)
		{
			Play (Clip.Hit, null, AnimationEventTriggeredAtt);
		}
	}

	IEnumerator attInterver ()
	{
		for (int i = (int) Clip.Hit; i < (int) Clip.Hit + atInx; i++)
		{
			if (isHitted == false && isFall == false)
			{
				Play ((Clip) i, null, AnimationEventTriggeredAtt);
				yield return new WaitForSeconds (currentClipTime);
			}
		}
		isIng = false;
		atInx = 0;
	}

	void AnimationEventTriggeredAtt (tk2dSpriteAnimator a, tk2dSpriteAnimationClip b, int c)
	{
		if (attMode == AttMode.INF)
		{
			HitTarget ();
			MoveForwrd ();
		}
		else if (attMode == AttMode.BOW)
		{
			GameObject go = ObjectPool.Instance.LoadObject ("BF001");
			Vector3 POS = Vector3.zero;
			POS.x = transform.position.x;
			POS.y = transform.position.y + height + height / 2;
			POS.z = POS.y;
			go.transform.position = POS;

			go.GetComponent<MissileObject>().Refresh (transform.position, tkSp.scale.x, 400, hitTargetEvent);
		}
	}

	void hitTargetEvent (Vector3 pos, MissileObject mOjc)
	{
		for(int i = 0; i < EnemyController.enemys.Count; i++)
		{
			EnemyController ec = EnemyController.enemys[i];
			if (Unit.bowAttDistance (pos, ec, distanceAtt))
			{
				if (ec.isFall == false)
				{
					//AddEF ("EF001", ec);//TODO:"EF001" need data
					bool isBig = Random.Range (0, 2) == 1;
					float dmg = troop.attDmg * GameData.Instance.getSpByIdPro (MGConstant.PRO.ZS, currentClip.ToString()).dmg * (isBig ? 2 : 1);
					AddDMG ("NU001", ec, dmg, isBig);
					ec.Hitted (currentClip == Clip.AttackLast || currentClip == Clip.spell1 ? Clip.Fall : Clip.Hitted, dmg);
					ec.HittedMove (attMoveDis * MGMath.getDirNumber (this), this);

					mOjc.ShowMissile ();
				}
			}
		}
	}

	#region Hit Method
	private void MoveForwrd ()
	{
		tweenPosition = TweenPosition.Begin (gameObject, 0.1f, MGMath.getClampPos(new Vector3 (attMoveDis * MGMath.getDirNumber (this), 0, 0) + transform.position));
	}

	private void HitTarget ()
	{
		if (currentClip == Clip.spell1)
		{
			NotificationCenter.PostNotification (this, "PlayShake");
			AddSP ("SP1", 150);
		}
	

		for(int i = 0; i < EnemyController.enemys.Count; i++)
		{
			EnemyController ec = EnemyController.enemys[i];
			if (Unit.isFront (this, ec) && Unit.attDistance (this, ec, distanceAtt))
			{
				if (ec.isFall == false)
				{
					AddEF ("EF001", ec);//TODO:"EF001" need data
					bool isBig = Random.Range (0, 2) == 1;
					float dmg = troop.attDmg * GameData.Instance.getSpByIdPro (MGConstant.PRO.ZS, currentClip.ToString()).dmg * (isBig ? 2 : 1);
					AddDMG ("NU001", ec, dmg, isBig);
					ec.Hitted (currentClip == Clip.AttackLast || currentClip == Clip.spell1 ? Clip.Fall : Clip.Hitted, dmg);
					ec.HittedMove (attMoveDis * MGMath.getDirNumber (this), this);
				}
			}
		}
		/*foreach(EnemyController ec in EnemyController.enemys)
		{
			if (Unit.isFront (this, ec) && Unit.attDistance (this, ec, distanceTest))
			{
				if (ec.isFall == false)
				{
					AddEF ("EF001", ec);//TODO:"EF001" need data
					bool isBig = Random.Range (0, 2) == 1;
					float dmg = _dmg * (isBig ? 2 : 1);
					AddDMG ("NU001", ec, dmg, isBig);
					ec.Hitted (currentClip == Clip.AttackLast || currentClip == Clip.spell1 ? Clip.Fall : Clip.Hitted, dmg);
					ec.HittedMove (attMoveDis * MGMath.getDirNumber (this), this);
				}
			}
		}*/
	}
	#endregion

	#region Hitted Method
	#endregion
	public override void ExtraInfo ()
	{
//		cancel ();
//		hiddenSpells ();
	}

	public override void ApplyDmg (float dmg, bool isSlider)
	{}

	#region spell
	void hiddenSpells ()
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			transform.GetChild (i).gameObject.SetActive (false);
		}
	}
	public void assault ()
	{
		if (isFall == true && isHitted ==true)
		{
			return;
		}
		if (isSpell == true)
		{
			return;
		}
		AddSP ("SP0", -100);
		float dir = MGMath.getDirNumber (this);
		MoveToTarget (transform.position + new Vector3 (dir * 400, 0, 0), 1500);
		Play (Clip.spell0);
		tweenPosition.onUpdate = onUpdateAssault;
		tweenPosition.onFinished = onFinishAssault;
	}
	void onUpdateAssault()
	{
		for(int i = 0; i < EnemyController.enemys.Count; i++)
		{
			EnemyController ec = EnemyController.enemys[i];
			if (Unit.isFront (this, ec) && Unit.attDistance (this, ec, distanceAtt))
			{
				if (ec.isHitted == true)
				{
					continue;
				}
				if (ec.isFall == false)
				{
					bool isBig = Random.Range (0, 2) == 1;
					float dmg = troop.attDmg * GameData.Instance.getSpByIdPro (MGConstant.PRO.ZS, currentClip.ToString()).dmg * (isBig ? 2 : 1);
					AddEF ("EF001", ec);//TODO:"EF001" need data
					AddDMG ("NU001", ec, dmg, isBig);
					ec.Hitted (Clip.Fall, dmg);
					ec.stop ();

					ec.HittedMove (Mathf.Abs(transform.position.x - tweenPosition.to.x) * MGMath.getDirNumber (this), this);
				}
			}
		}
	}
	void onFinishAssault (UITweener u)
	{
		tweenPosition.onUpdate = null;
	}

	public void JumpAtt ()
	{
		if (isFall == true && isHitted ==true)
		{
			return;
		}
		if (isSpell == true)
		{
			return;
		}
		MoveForwrd ();
		Play (Clip.spell1, null, AnimationEventTriggeredAtt);

	}

	void cyclone ()
	{
		if (isFall == true && isHitted ==true)
		{
			return;
		}
		if (isSpell == true)
		{
			return;
		}
		cancel ();
		Play (Clip.spell2, null, AnimationEventTriggeredAtt);
		InvokeRepeating ("UpdateDmg", 0.1f, 0.8f);
		AddSP ("SP2");
		Invoke ("cancel", currentClipTime);
	}
	void cancel ()
	{
		CancelInvoke ("cancel");
		CancelInvoke ("UpdateDmg");
	}
	void UpdateDmg ()
	{
		if (isFall == true || isHitted == true || isSpell == false)
		{
			return;
		}
		for(int i = 0; i < EnemyController.enemys.Count; i++)
		{
			EnemyController ec = EnemyController.enemys[i];
			if (Unit.attDistance (this, ec, distanceAtt))
			{
				if (ec.isFall == false)
				{
					AddEF ("EF001", ec);//TODO:"EF001" need data
					bool isBig = Random.Range (0, 2) == 1;
					float dmg = troop.attDmg * GameData.Instance.getSpByIdPro (MGConstant.PRO.ZS, currentClip.ToString()).dmg * (isBig ? 2 : 1);
					AddDMG ("NU001", ec, dmg, isBig);
					ec.Hitted (Clip.Hitted, dmg);
					ec.HittedMove (0/*attMoveDis * MGMath.getDirNumber (this)*/, this);
				}
			}
		}
	}
	#endregion


	#region DEBUG
	public const int max = 3;
	void OnGUI ()
	{
		for (int i = 0; i < max; i++)
		{
			if (GUI.Button (new Rect (100 * i,0, 100, 100), "spell" + i.ToString()))
			{
				test (i);
			}
		}
	}
	void test(int i)
	{
		if (i == 0)
		{
			assault ();
		}
		else if (i == 1)
		{
			JumpAtt ();
		}
		else if (i == 2)
		{
			cyclone ();
		}
	}
	#endregion
}
