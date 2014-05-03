using UnityEngine;
using System.Collections;

public class SpellController : MonoBehaviour {
	public delegate void CalculateDmgDelegate (Spell.SpellType spell);
	public CalculateDmgDelegate calculateDmgDelegate;

	private Vector3 dir;
	private float startTime;
	//GameObject ef = null;
	private CharacterMotor motor;
	public void shootSpell1 (CharacterMotor motor)
	{
		//Transform child = ObjectPool.Instance.LoadObject ("EF/EF002", transform.position).transform;
		//ef = child.gameObject; 
		//child.parent = transform;
		this.motor = motor;
		startTime = Time.time;
		dir = transform.forward.normalized * 5 + transform.position;
		InvokeRepeating ("UpdateZSSpell1", 0, MGMath.UPDATE_RATE);
		Invoke ("ResetUpdateZSSpell1", 0.1f);
	}

	void UpdateZSSpell1 ()
	{
		if (calculateDmgDelegate != null)
		{
			calculateDmgDelegate (Spell.SpellType.Spell1);
		}
		motor.characterController.Move (transform.forward * .1f);
	}

	void ResetUpdateZSSpell1 ()
	{
		//ef.SetActive (false);
		CancelInvoke ("UpdateZSSpell1");
	}
}
