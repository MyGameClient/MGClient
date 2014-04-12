using UnityEngine;
using System.Collections;

public enum OperationCode : byte
{
	Login = 5,
	Register,
	GetRoomInfo,
	GetAllRoomInfo,
	JoinRoom,
	QuitRoom,
	RoomActorBorning,
	RoomActorActionUpdate,
	RoomSpeak,
}

public enum EventCode: byte
{
	LobbyBroadcast = 1,
	RoomBroadcastActorAction,
	RoomBroadcastActorQuit,
	RoomBroadcastActorSpeak,
}

public enum ErrorCode: byte
{
	Ok = 0,
	InvalidOperation,
	InvalidParameter,
	CustomError
}

public class MemberData
{
	public string MemberID;
	public string MemberPW;
}

public enum LoginParameterCode: byte
{
	MemberID = 1,
	MemberPW,
}

public enum LoginResponseCode: byte
{
	MemberID = 1,
	MemberPW,
	Nickname,
	MemberUniqueID,
	Sex,

	PosX,
	PosY,
	PosZ,
	Direct,
	ActionNum,

	Ret = 80,
}

public enum GetRoomInfoParameterCode: byte
{
	RoomIndex = 1,
}

public enum GetRoomInfoResponseCode: byte
{
	RoomIndex = 1,
	RoomName,
	Limit,
	ActorCount,
}

public enum JoinRoomParameterCode: byte
{
	RoomIndex = 1,
}

public enum JoinRoomResponseCode: byte
{
	RoomIndex = 1,
	RoomName,
	Limit,
	ActorCount,
}

public enum GetRoomInfoEventCode: byte
{
	RoomIndex = 1,
	RoomName,
	Limit,
	ActorCount,
}

public enum RoomActorActionInfo: byte
{
	MemberUniqueID = 1,
	NickName,
	PosX,
	PosY,
	PosZ,
	Direct,
	ActionNum,
}

public enum RoomActorQuit: byte
{
	MemberUniqueID = 1,
	MemberUniqueName
}

public enum RoomActorSpeak: byte
{
	MemberUniqueID = 1,
	MemberUniqueName,
	TalkString,
}
