using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Bundle
{
	public bool isRegister;
	public string error = null;
	public string notice = "";
	public OperationCode cmd;

	public Account account = new Account ();
	public User user = new User ();
	public Room room = new Room ();
	public List<Room> rooms = new List<Room> ();
	public Message mesaage = new Message ();
	public RoomMeneber roomMember = new RoomMeneber ();
}

public class Account
{
	public string id;
	public string pw;
	public string nickName;
	public int sex;
}

public class User
{
	public Account account = new Account ();
	public Room room = new Room ();
}

public class Room
{
	public int RoomIndex = -1;
	public string RoomName;
	public int Limit;
	public int ActorCount;
}

public class Message
{
	public string from = "";
	public string to = DefineString.TO_WORLD_CHAT;
	public string content;
	public DateTime time;
}

public class Item
{
	public int id;
	public string name;
	public string type;
}

public class Role
{
	public float speed;
}

public class RoomMeneber
{
	public string userId;
	public string userName;
	public float posX;
	public float posY;
	public float posZ;
	public float direct;
	public float act;//0,stand, 1, walk, 2,attack....
}
