using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]
public class CFX_AutoDestructShuriken : MonoBehaviour
{
	void OnEnable ()
	{
		transform.rotation = Quaternion.Euler (new Vector3 (0, Random.Range (0, 361), 0));
	}
	void Update ()
	{
		if(!particleSystem.IsAlive(true))
		{
			gameObject.SetActive (false);
			//GameObject.Destroy(this.gameObject);
		}
	}
}
