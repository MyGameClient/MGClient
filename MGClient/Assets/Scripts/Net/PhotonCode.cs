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
	RoomActorBorning, // 玩家進入聊天室
	RoomActorActionUpdate, // 玩家更新行為資訊
	RoomSpeak,        // 玩家發言聊天
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
	Sex,
	MemberUniqueID,

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
