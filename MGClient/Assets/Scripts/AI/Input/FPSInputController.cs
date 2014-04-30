using UnityEngine;
using System.Collections;


[RequireComponent(typeof(CharacterMotor))]
[AddComponentMenu("Character/FPS Input Controller")]
public class FPSInputController : MonoBehaviour {
	private Vector3 m_directionVector;
	public Vector3 directionVector
	{
		get
		{
			return m_directionVector;
		}
	}
	private CharacterMotor m_motor ;
	public CharacterMotor motor
	{
		get
		{
			return m_motor;
		}
	}
	public bool canControl;
	
	// Use this for initialization
	void Awake () {
		m_motor = GetComponent<CharacterMotor>();
	}
	
	// Update is called once per frame
	void Update () {
		// Get the input vector from kayboard or analog stick
		float x = Input.GetKey (KeyCode.A) ? -1 : Input.GetKey (KeyCode.D) ? 1 : 0 ;
		float z = Input.GetKey (KeyCode.S) ? -1 : Input.GetKey (KeyCode.W) ? 1 : 0 ;
		//Debug.Log (x + "___" + y);
		m_directionVector = new Vector3(x, 0, z);
		
		if (m_directionVector != Vector3.zero) {
			if (canControl == true)
				transform.forward = m_directionVector;
			// Get the length of the directon vector and then normalize it
			// Dividing by the length is cheaper than normalizing when we already have the length anyway
			var directionLength = m_directionVector.magnitude;
			m_directionVector = m_directionVector / directionLength;
			
			// Make sure the length is no bigger than 1
			directionLength = Mathf.Min(1, directionLength);
			
			// Make the input vector more sensitive towards the extremes and less sensitive in the middle
			// This makes it easier to control slow speeds when using analog sticks
			directionLength = directionLength * directionLength;
			
			// Multiply the normalized direction vector by the modified length
			m_directionVector = m_directionVector * directionLength;
		}
		
		// Apply the direction to the CharacterMotor
		
		m_motor.inputMoveDirection = m_directionVector;
		m_motor.inputJump = Input.GetButtonDown("Jump");
	}
	
}