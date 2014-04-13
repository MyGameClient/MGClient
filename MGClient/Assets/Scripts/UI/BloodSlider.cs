using UnityEngine;
using System.Collections;

public class BloodSlider : MonoBehaviour {


	public static BloodSlider instance;

	public UISprite middleBlood;

	private UISlider slider;

	private float sliderValue
	{
		set{
			if (value < slider.sliderValue)
			{
				middleBlood.transform.localScale = slider.foreground.transform.localScale;
			}

			slider.sliderValue = value;
		}
	}

	private UITweener tweener;

	void Start()
	{
		instance = this;
		slider = GetComponent<UISlider>();
		slider.onValueChange = onValueChange;
	}
	
	void onValueChange (float val)
	{
		CancelInvoke ("UpdateUIVal");
		Invoke("UpdateUIVal", 0.1f);

	}

	void UpdateUIVal()
	{
		tweener = TweenScale.Begin (middleBlood.gameObject, 0.3f, slider.foreground.transform.localScale);
	}

	public void Refresh (float val)
	{
		//sliderValue = 1.0f;
		if (tweener != null)
		{
			tweener.enabled = false;
		}
		sliderValue = val;
	}

}
