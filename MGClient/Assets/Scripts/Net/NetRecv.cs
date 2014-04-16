using UnityEngine;
using System.Collections;

public class NetRecv : MonoBehaviour {

	void OnEnable ()
	{
		PhotonClient.ProcessResult += ProcessResult;
	}

	void OnDisable ()
	{
		PhotonClient.ProcessResult -= ProcessResult;
	}

	void ProcessResult (Bundle bundle)
	{
		if (bundle.cmd == OperationCode.Login)
		{
			Main.Instance.account = bundle.account;
		}
	}
}
