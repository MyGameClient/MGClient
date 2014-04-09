//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2013 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;

/// <summary>
/// Tween the object's color.
/// </summary>

[AddComponentMenu("NGUI/Tween/TKColor")]
public class TweenTk2dColor : UITweener
{
	public Color from = Color.white;
	public Color to = Color.white;

	Transform mTrans;
	tk2dSprite mWidget;

	/// <summary>
	/// Current color.
	/// </summary>

	public Color color
	{
		get
		{
			if (mWidget != null) return mWidget.color;
			return Color.black;
		}
		set
		{
			if (mWidget != null) mWidget.color = value;
		}
	}

	/// <summary>
	/// Find all needed components.
	/// </summary>

	void Awake ()
	{
		mWidget = GetComponentInChildren<tk2dSprite>();
		Renderer ren = renderer;
	}

	/// <summary>
	/// Interpolate and update the color.
	/// </summary>

	protected override void OnUpdate(float factor, bool isFinished) { color = Color.Lerp(from, to, factor); }

	/// <summary>
	/// Start the tweening operation.
	/// </summary>

	static public TweenTk2dColor Begin (GameObject go, float duration, Color color)
	{
		TweenTk2dColor comp = UITweener.Begin<TweenTk2dColor>(go, duration);
		comp.from = comp.color;
		comp.to = color;

		if (duration <= 0f)
		{
			comp.Sample(1f, true);
			comp.enabled = false;
		}
		return comp;
	}
}