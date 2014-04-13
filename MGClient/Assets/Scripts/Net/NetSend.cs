using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using ExitGames.Client.Photon;


//Sned to server, follow;
//auto receive function "AutoProcessResult" parameter is Bunle
//http receive function "ProcessResult" parameter is Bundle

public class NetSend {

	public static void SendRegister (Account a)
	{
		Dictionary<byte, object> parameter = new Dictionary<byte, object> ();
		parameter.Add ((byte) LoginResponseCode.MemberID, a.id);
		parameter.Add ((byte) LoginResponseCode.MemberPW, a.pw);
		PhotonClient.Instance.SendServer (OperationCode.Register, parameter);
	}

	public static void SendLogin (Account a)
	{
		Dictionary<byte, object> parameter = new Dictionary<byte, object> ();
		parameter.Add ((byte) LoginResponseCode.MemberID, a.id);
		parameter.Add ((byte) LoginResponseCode.MemberPW, a.pw);
		PhotonClient.Instance.SendServer (OperationCode.Login, parameter);
	}

	public static void SendGetRoomInfo (Room room)
	{
		Dictionary<byte, object> parameter = new Dictionary<byte, object> ();
		parameter.Add ((byte) GetRoomInfoResponseCode.RoomIndex, room.RoomIndex);
		PhotonClient.Instance.SendServer (OperationCode.GetRoomInfo, parameter);
	}

	public static void SendJoinRoom (Room room)
	{
		Dictionary<byte, object> parameter = new Dictionary<byte, object> ();
		parameter.Add ((byte) GetRoomInfoResponseCode.RoomIndex, room.RoomIndex);
		PhotonClient.Instance.SendServer (OperationCode.JoinRoom, parameter);
	}

	public static void SendGetAllRoomInfo ()
	{
		PhotonClient.Instance.SendServer (OperationCode.GetAllRoomInfo, null);
	}

	public static void SendQuitRoom ()
	{
		PhotonClient.Instance.SendServer (OperationCode.QuitRoom, null);
	}

	public static void SendRoomSpeak (Message message)
	{
		Dictionary<byte, object> parameter = new Dictionary<byte, object> ();
		parameter.Add ((byte)RoomActorSpeak.TalkString, message.content);
		PhotonClient.Instance.SendServer (OperationCode.RoomSpeak, parameter);
	}
}
