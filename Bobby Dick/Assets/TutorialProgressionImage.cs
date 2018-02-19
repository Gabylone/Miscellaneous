﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Holoville.HOTween;

public class TutorialProgressionImage : MonoBehaviour {

	public int progression = 0;
	int max = 0;

	[Range(0,1)]
	public float progressionTest = 0f;

	public Image progression_FillImage;
	public Image progression_BackgroundImage;

	void Start () {
		max = System.Enum.GetValues (typeof(TutorialStep)).Length;

		Tutorial.onDisplayTutorial += HandleOnDisplayTutorial;
	}

	void HandleOnDisplayTutorial (TutoStep tutoStep)
	{
		progression++;

		UpdateProgressionBar ();
	}

	void UpdateProgressionBar () {

		float w = progression_BackgroundImage.rectTransform.rect.width;
		float l1 = (float)(progression-1) / (float)max;
		float l2 = (float)progression / (float)max;

		progression_FillImage.rectTransform.sizeDelta = new Vector2 ( -(w) + ( l1 *w) , 0 );

		Vector2 targetScale = new Vector2 (-(w) + (l2 * w), 0);
		HOTween.To ( progression_FillImage.rectTransform , 1f , "sizeDelta" , targetScale );

	}
}
