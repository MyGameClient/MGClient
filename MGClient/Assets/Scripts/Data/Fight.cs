using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

public class Fight
{
	[DefaultValue(null)]
	public List<Troop> enemys;
	[DefaultValue(null)]
	public List<Troop> players;
	[DefaultValue(null)]
	public string map;


	[DefaultValue (0L)]
	private float _hpMax;

	public float hpMax
	{
		get
		{
			if (_hpMax == 0)
			{
				foreach (Troop troop in enemys)
				{
					_hpMax += troop.hpMax;
				}
			}
			return _hpMax;
		}
	}
}
