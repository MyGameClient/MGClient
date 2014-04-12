using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DC = DebugConsole;

public class NetTest : MonoBehaviour {

	private const string repose = "<<<<<<<repose<<<<<<<";

	public bool isOpenDenug = true;

	void Start () 
	{
		DC.IsOpen = isOpenDenug;
		DC.LogWarning ("A,S,D,W controll player dir;\n right mouse click attack \n button spell1,spell2,spell3 is use spell");
		DC.RegisterCommand (OperationCode.Register.ToString (), RegisterTest);
		DC.RegisterCommand (OperationCode.Login.ToString (), LoginTest);
		DC.RegisterCommand (OperationCode.GetRoomInfo.ToString (), GetRoomInfo);
		DC.RegisterCommand (OperationCode.JoinRoom.ToString (), JoinRoom);
		DC.RegisterCommand (OperationCode.GetAllRoomInfo.ToString (), GetAllRoomInfo);
		DC.RegisterCommand (OperationCode.QuitRoom.ToString (), QuitRoom);
		DC.RegisterCommand (OperationCode.RoomSpeak.ToString (), RoomSpeak);
	}

	private string RegisterTest (params string[] p)
	{
		if (p.Length >= 5)
		{
			Account a = new Account ();
			a.id = p[1];
			a.pw = p[2];
			a.nickName = p[3];
			a.sex = int.Parse (p[4]);
			NetSend.SendRegister (a);
			//NotificationCenter.PostNotification (this, "SendRegister", a);
			return repose;
		}
		return p[0] + " userId,userPw,userName,userSex usage";
	}

	private string LoginTest (params string[] p)
	{
		if (p.Length >= 3)
		{
			Account user = new Account ();
			user.id = p[1];
			user.pw = p[2];
			NetSend.SendLogin (user);
			return repose;
		}
		return p[0] + " ID" + " PW usage";
	}

	private string GetRoomInfo (params string[] p)
	{
		if (p.Length >= 2)
		{
			Room room = new Room ();
			try
			{
				room.RoomIndex = int.Parse (p[1]);
				NetSend.SendGetRoomInfo(room);
				return repose;
			}
			catch (System.Exception e)
			{

			}
		}
		return p[0] + " roomId usage";
	}

	private string JoinRoom (params string[] p)
	{
		if (p.Length >= 2)
		{
			Room room = new Room ();
			try
			{
				room.RoomIndex = int.Parse (p[1]);
				NetSend.SendJoinRoom (room);
				return repose;
			}
			catch (System.Exception e)
			{
				
			}
		}
		return p[0] + "RoomId usage";
	}

	private string GetAllRoomInfo (params string[] p)
	{
		if (p.Length >= 1)
		{
			NetSend.SendGetAllRoomInfo ();
			//NotificationCenter.PostNotification (this, "SendGetAllRoomInfo");
			return repose;
		}
		return p[0] +" usage";
	}

	private string QuitRoom (params string[] p)
	{
		if (p.Length >= 1)
		{
			NetSend.SendQuitRoom ();
			//NotificationCenter.PostNotification (this, "SendQuitRoom");
			return repose;
		}
		return p[0] +" usage";
	}

	private string RoomSpeak (params string[] p)
	{
		if (p.Length >= 2)
		{
			Message message = new Message ();
			message.content = p[1];
			NetSend.SendRoomSpeak (message);
			//NotificationCenter.PostNotification (this, "SendRoomSpeak", message);
			return repose;
		}
		return p[0] + " message usage";
	}
}
