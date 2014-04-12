﻿using UnityEngine;
using System.Collections;

public class MultiPlayerController : Unit {

	public UILabel label;

	public override void ExtraInfo(){}

	void Awake()
	{
		Init ();
	}

	public void Refresh (RoomMeneber rm)
	{
		transform.position = new Vector3 (rm.posX, rm.posY, rm.posY);
		this.name = rm.userId;
		label.text = rm.userName;
		xDir = (rm.direct == 1) ? Dir.Right : Dir.Left;
		Play ((Clip)rm.act);
	}
}