using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security;

//Sned to server, follow;
//auto receive function "AutoProcessResult" parameter is Bunle
//http receive function "ProcessResult" parameter is Bundle

public class NetSend {

	public static void SendRegister (Account a)
	{
		PhotonClient.Instance.SendServer<Account> (OperationCode.Register, a);
	}

	public static void SendLogin (Account a)
	{
		PhotonClient.Instance.SendServer<Account> (OperationCode.Login, a);
	}

	public static void SendGetRoomInfo (Room room)
	{
		PhotonClient.Instance.SendServer<Room> (OperationCode.GetRoomInfo, room);
	}

	public static void SendJoinRoom (Room room)
	{
		PhotonClient.Instance.SendServer<Room> (OperationCode.JoinRoom, room);
	}

	public static void SendGetAllRoomInfo ()
	{
		PhotonClient.Instance.SendServer (OperationCode.GetAllRoomInfo);
	}

	public static void SendQuitRoom ()
	{
		PhotonClient.Instance.SendServer (OperationCode.QuitRoom);
	}

	public static void SendRoomSpeak (Message message)
	{
		PhotonClient.Instance.SendServer<Message> (OperationCode.RoomSpeak, message);
	}
}
