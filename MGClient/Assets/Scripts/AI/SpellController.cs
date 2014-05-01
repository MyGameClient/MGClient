using UnityEngine;
using System.Collections;

public class SpellController : MonoBehaviour {

	private Vector3 dir;
	private float startTime;

	private CharacterMotor motor;
	public void shootSpell1 (CharacterMotor motor)
	{
		this.motor = motor;
		startTime = Time.time;
		dir = transform.forward.normalized * 5 + transform.position;
		InvokeRepeating ("UpdateZSSpell1", 0, MGMath.UPDATE_RATE);
		Invoke ("ResetUpdateZSSpell1", 0.1f);
	}

	void UpdateZSSpell1 ()
	{
		//MGMath.UpdateMove (startTime, transform, dir);
		//motor.movement.maxForwardSpeed  = 100;
		//motor.inputMoveDirection = transform.forward * transform.rotation;
		motor.characterController.SimpleMove (transform.forward * 10);
	}

	void ResetUpdateZSSpell1 ()
	{
		CancelInvoke ("UpdateZSSpell1");
	}
}
