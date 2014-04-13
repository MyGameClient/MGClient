using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MultiPlayerManager : MonoBehaviour {

	public Dictionary<int, User> userMemeber = new Dictionary<int, User> ();

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
			//AddMU (bundle.roomMember);
		}
		else if (bundle.cmd == OperationCode.GetRoomInfo)
		{

		}
	}

	void ProcessResultSync (Bundle bundle)
	{
		if (bundle.eventCmd == EventCode.JoinRoomNotify)
		{
			if (bundle.roomMember.userId == Main.Instance.account.uniqueId)
			{
				AddPY (bundle.roomMember);
			}
			else
			{
				AddMU (bundle.roomMember);
			}
		}
	}

	void AddMU (RoomMember rm)
	{
		GameObject go = ObjectPool.Instance.LoadObject (MGConstant.MU + "MU001");
		//go.transform.position = new Vector3 (rm.posX, rm.posY, rm.posY);
		go.GetComponent<MultiPlayerController>().Refresh (rm);
	}

	void AddPY (RoomMember rm)
	{
		GameObject go = ObjectPool.Instance.LoadObject (MGConstant.PY + "PY001");
		go.transform.position = new Vector3 (rm.posX, rm.posY, rm.posY);
	}

	void OnSelectionChange(string i)
	{
		if (i == "Login1")
		{
			Account a = new Account();
			a.id = "a1";
			a.pw = "123";
			NetSend.SendLogin (a);
		}
		if (i == "Login2")
		{
			Account a = new Account();
			a.id = "a2";
			a.pw = "123";
			NetSend.SendLogin (a);
		}
		else if (i == "JoinRoom")
		{
			Room room = new Room();
			room.RoomIndex = 1;
			NetSend.SendJoinRoom(room);
		}
	}
}
