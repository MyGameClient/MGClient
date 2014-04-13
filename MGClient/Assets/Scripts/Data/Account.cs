using UnityEngine;
using System;
using System.Collections.Generic;
using System.ComponentModel;

public class Account
{
	[DefaultValue (null)]
	public string uniqueId;
	[DefaultValue (null)]
	public string id;
	[DefaultValue (null)]
	public string pw;
	[DefaultValue (null)]
	public long creatTime;
}
