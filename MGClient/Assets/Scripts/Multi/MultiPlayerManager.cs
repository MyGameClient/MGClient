using UnityEngine;
using System.Collections;

public class MultiPlayerManager : MonoBehaviour {

	void OnEnable ()
	{
		PhotonClient.ProcessResult += ProcessResult;
		PhotonClient.ProcessResultSync += ProcessResultSync;
	}

	void OnDisable ()
	{
		PhotonClient.ProcessResult -= ProcessResult;
		PhotonClient.ProcessResultSync -= ProcessResultSync;
	}

	void ProcessResult (Bundle bundle)
	{
		if (bundle.cmd == OperationCode.Login)
		{
			AddMU (bundle.roomMember);
		}
	}

	void ProcessResultSync (Bundle bundle)
	{

	}

	void AddMU (RoomMeneber rm)
	{
		GameObject go = ObjectPool.Instance.LoadObject (MGConstant.MU + "MU001");
		//go.transform.position = new Vector3 (rm.posX, rm.posY, rm.posY);
		go.GetComponent<MultiPlayerController>().Refresh (rm);
	}

	void AddPY (RoomMeneber rm)
	{
		GameObject go = ObjectPool.Instance.LoadObject (MGConstant.PY + "PY001");
		go.transform.position = new Vector3 (rm.posX, rm.posY, rm.posY);
	}
}
