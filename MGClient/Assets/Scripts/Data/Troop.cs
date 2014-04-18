using UnityEngine;
using System.Collections;
using System.ComponentModel;

public class Troop
{
	[DefaultValue (null)]
	public string id;
	[DefaultValue (1)]
	public int level;
	[DefaultValue (null)]
	public string type;
	[DefaultValue (null)]
	public string assetbundle;
	[DefaultValue (null)]
	public string icon;
	[DefaultValue (null)]
	public string desc;
	[DefaultValue (null)]
	public string name;
	[DefaultValue (0F)]
	public float dmg;
	[DefaultValue (0F)]
	public float def;
	[DefaultValue (0F)]
	public float hpMax;
	[DefaultValue (0F)]
	public float hp;
}
