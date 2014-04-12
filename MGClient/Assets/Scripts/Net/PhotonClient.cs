using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using DC = DebugConsole;
using PC = PhotonClient;

using System.Security;
using ExitGames.Client.Photon;


public class Events
{
	public delegate void ProcessResult (Bundle bundle);
	public delegate void ProcessResultSync (Bundle bundle);
}

public class PhotonClient : MonoBehaviour, IPhotonPeerListener {


	/*
	 *Events
	 */
	public static  Events.ProcessResult ProcessResult;
	public static  Events.ProcessResultSync ProcessResultSync;


	public static PhotonClient Instance;


	//LOAD SERVER ADDRESS
	public string LOAD_SERVER_ADDRESS = "localhost:5055";


	protected string ServerApplication = "EZServer";
	protected PhotonPeer peer;
	public bool ServerConnected {get; private set;}
	
	// Use this for initialization
	void Start () {

		Instance = this;
		Application.runInBackground = true;

		this.ServerConnected = false;

		PC.LogError ("Disconnected");

		this.peer = new PhotonPeer(this, ConnectionProtocol.Udp);
		this.Connect();
	}

	public virtual void Connect()
	{
		try
		{
			this.peer.Connect(this.LOAD_SERVER_ADDRESS, this.ServerApplication);
		}
		catch (SecurityException se)
		{
			this.DebugReturn(0, "Connection Failed. " + se.ToString());
		}
	}

	private bool isVal = false;
	// Update is called once per frame
	void Update () {
		this.peer.Service ();
		if (this.ServerConnected != isVal)
		{
			isVal = this.ServerConnected;
			if (isVal)
			{
				PC.Log ("Connected");
			}
			else
			{
				PC.LogError ("Disconnected");
			}
		}
	}

	public void DebugReturn (DebugLevel level, string message)
	{
		PC.Log (message);
		Debug.Log (message);
	}
	public void OnOperationResponse (OperationResponse operationResponse)
	{
		Bundle bundle = new Bundle ();
		bundle.cmd = (OperationCode) operationResponse.OperationCode;//cmd
		this.DebugReturn(0, string.Format("CMD : " + bundle.cmd));//Debug;
		bool success = (operationResponse.ReturnCode == (short)ErrorCode.Ok);
		switch (operationResponse.OperationCode)
		{
		case (byte)OperationCode.Register:
		{
			bundle.isRegister = (operationResponse.DebugMessage == DefineString.REGISTER_ID_SUCCESS);
			PC.Log (operationResponse.DebugMessage + "___bundle.isRegister = " + bundle.isRegister);
			break;
		}
		case (byte)OperationCode.Login:
		{
			if( success ) // if success
			{
				//int getRet = (int) operationResponse.Parameters[(byte)LoginResponseCode.Ret];/*Convert.ToInt32(operationResponse.Parameters[80])*/;
				//bundle.account = operationResponse.Parameters[(byte)LoginResponseCode.].ToString ()/*Convert.ToString(operationResponse.Parameters[1])*/;
				bundle.account.uniqueId = operationResponse.Parameters[(byte)LoginResponseCode.MemberUniqueID].ToString ()/*Convert.ToString(operationResponse.Parameters[1])*/;
				bundle.account.id = operationResponse.Parameters[(byte)LoginResponseCode.MemberID].ToString ()/*Convert.ToString(operationResponse.Parameters[1])*/;
				bundle.account.pw = operationResponse.Parameters[(byte)LoginResponseCode.MemberPW].ToString ();
				bundle.account.nickName = operationResponse.Parameters[(byte)LoginResponseCode.Nickname].ToString ();/*Convert.ToString(operationResponse.Parameters[3])*/;

				bundle.roomMember.userName = bundle.account.nickName.ToString();
				bundle.roomMember.userId = operationResponse.Parameters[(byte)LoginResponseCode.MemberUniqueID].ToString ()/*Convert.ToString(operationResponse.Parameters[1])*/;
				bundle.roomMember.posX = float.Parse (operationResponse.Parameters[(byte)LoginResponseCode.PosX].ToString());
				bundle.roomMember.posY = float.Parse (operationResponse.Parameters[(byte)LoginResponseCode.PosY].ToString());
				bundle.roomMember.act = int.Parse (operationResponse.Parameters[(byte)LoginResponseCode.ActionNum].ToString());
				bundle.roomMember.direct = int.Parse (operationResponse.Parameters[(byte)LoginResponseCode.Direct].ToString());
				PC.Log ("{ user: " + bundle.account.id + '\n' + "uniqueId: " + bundle.account.uniqueId + '\n' + " name : " + bundle.account.nickName + "}");
				PC.Log (string.Format("userName:{0}, userId:{1}, POSX:{2}, POSY:{3}, ACTOR:{4}:",bundle.roomMember.userName, bundle.roomMember.userId, bundle.roomMember.posX, bundle.roomMember.posY, bundle.roomMember.act));

			}
			else
			{
				bundle.error = operationResponse.DebugMessage;
				PC.LogError (operationResponse.DebugMessage);
			}
			break;
		}
		case (byte)OperationCode.GetRoomInfo:
		{
			if( success) // if success
			{
				SetRoomInfo (bundle, operationResponse);
			}
			else
			{
				bundle.error = operationResponse.DebugMessage;
				PC.LogError (operationResponse.DebugMessage);
			}
			break;
		}
		case (byte)OperationCode.JoinRoom:
		{
			if( success) // if success
			{
				SetRoomInfo (bundle, operationResponse);
			}
			else
			{
				bundle.error = operationResponse.DebugMessage;
				PC.LogError (operationResponse.DebugMessage);
			}
			break;
		}
		case (byte)OperationCode.GetAllRoomInfo:
		{
			if( success) // if success
			{
				Dictionary<byte, object> dict = new Dictionary<byte, object> ();
				dict = (Dictionary<byte, object>) operationResponse.Parameters;
				foreach (KeyValuePair<byte, object> kvp in dict)
				{
					Room room = new Room ();
					Dictionary<byte, object> v = new Dictionary<byte, object> ();
					v = (Dictionary<byte, object>) kvp.Value;
					room.RoomIndex = (int) v[(byte)GetRoomInfoResponseCode.RoomIndex];
					room.RoomName = v[(byte)GetRoomInfoResponseCode.RoomName].ToString ();
					room.ActorCount = (int)v[(byte)GetRoomInfoResponseCode.ActorCount];
					room.Limit = (int)v[(byte)GetRoomInfoResponseCode.Limit];
					PC.Log (kvp.Key + "<<<<<<<<<<");
					PC.Log (room.RoomIndex.ToString () + '\n' + room.RoomName.ToString () + '\n' + 
					        room.ActorCount.ToString () + '\n' + room.Limit.ToString ());
				}
				//SetRoomInfo (bundle, operationResponse);
			}
			else
			{
				bundle.error = operationResponse.DebugMessage;
				PC.LogError (operationResponse.DebugMessage);
			}
			break;
		}
		case (byte)OperationCode.QuitRoom:
		{
			if( success) // if success
			{
				SetRoomInfo (bundle, operationResponse);
			}
			else
			{
				bundle.error = operationResponse.DebugMessage;
				PC.LogError (operationResponse.DebugMessage);
			}
			break;
		}
		}
		if (ProcessResult != null)
		{
			ProcessResult (bundle);
		}
		//NotificationCenter.PostNotification (this, "ProcessResult", bundle);
	}
	public void OnStatusChanged (StatusCode statusCode)
	{
		this.DebugReturn(0, string.Format("PeerStatusCallback: {0}", statusCode));
		switch (statusCode)
		{
		case StatusCode.Connect:
			this.ServerConnected = true;
			break;
		case StatusCode.Disconnect:
			this.ServerConnected = false;
			break;
		}
	}

	void SetRoomInfo (Bundle bundle, OperationResponse operationResponse)
	{
		bundle.user.room.RoomIndex = (int) operationResponse.Parameters[(byte)GetRoomInfoResponseCode.RoomIndex];
		bundle.user.room.RoomName = operationResponse.Parameters[(byte)GetRoomInfoResponseCode.RoomName].ToString ();
		bundle.user.room.ActorCount = (int)operationResponse.Parameters[(byte)GetRoomInfoResponseCode.ActorCount];
		bundle.user.room.Limit = (int)operationResponse.Parameters[(byte)GetRoomInfoResponseCode.Limit];

		PC.Log (bundle.user.room.RoomIndex.ToString () + '\n' + bundle.user.room.RoomName.ToString () + '\n' + 
		        bundle.user.room.ActorCount.ToString () + '\n' + bundle.user.room.Limit.ToString ());
	}

	public void OnEvent (EventData eventData)
	{
		if (eventData.Code == 1)
		{
			return;
		}
		if (eventData.Code == (byte)EventCode.RoomBroadcastActorQuit)
		{
			Bundle bundle = new Bundle ();
			bundle.eventCmd = EventCode.RoomBroadcastActorQuit;
			string memeberuniqueid = eventData.Parameters[(byte) RoomActorQuit.MemberUniqueID].ToString ();
			string memeberName = eventData.Parameters[(byte) RoomActorQuit.MemberUniqueName].ToString ();
			PC.Log ("Player : " + memeberName + " id " + memeberuniqueid + " has quit!!!");
			if (ProcessResultSync != null)
			{
				ProcessResultSync (bundle);
			}
		}
		else if (eventData.Code == (byte)EventCode.RoomBroadcastActorSpeak)
		{
			Bundle bundle = new Bundle ();
			bundle.eventCmd = EventCode.RoomBroadcastActorSpeak;
			bundle.mesaage.from = eventData.Parameters[(byte) RoomActorSpeak.MemberUniqueName].ToString ();
			bundle.mesaage.content = eventData.Parameters[(byte) RoomActorSpeak.TalkString].ToString ();
			PC.Log (bundle.mesaage.from + " Talk : " + bundle.mesaage.content);
			if (ProcessResultSync != null)
			{
				ProcessResultSync (bundle);
			}
		}
		else if (eventData.Code == (byte)EventCode.JoinRoomNotify)
		{
			Bundle bundle = new Bundle ();
			bundle.eventCmd = EventCode.JoinRoomNotify;
			bundle.roomMember.posX = float.Parse (eventData.Parameters[(byte) RoomActorActionInfo.PosX].ToString ());
			bundle.roomMember.posY = float.Parse (eventData.Parameters[(byte) RoomActorActionInfo.PosY].ToString ());
			bundle.roomMember.userId = eventData.Parameters[(byte) RoomActorActionInfo.MemberUniqueID].ToString ();
			bundle.roomMember.userName = eventData.Parameters[(byte) RoomActorActionInfo.NickName].ToString ();
			PC.Log (bundle.roomMember.userName + " id : " + bundle.roomMember.userId + " Join room");
			if (ProcessResultSync != null)
			{
				ProcessResultSync (bundle);
			}
		}
	}

	public void SendServer (OperationCode operationCode, Dictionary<byte, object> parameter)
	{
		this.peer.OpCustom((byte)operationCode, parameter, true); // operationCode is 5
	}

	#region DEBUG
	public static void Log (object message)
	{
		DC.Log (message);
	}
	public static void LogError (object message)
	{
		DC.LogError (message);
	}
	#endregion
}
