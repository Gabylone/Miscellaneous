﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Karma : MonoBehaviour {

	public static Karma Instance;

	private bool visible = false;

	private int currentKarma = 0;
	private int previousKarma = 0;

	private int bounty = 0;

	[Header("Params")]
	[SerializeField]
	private int bountyStep = 10;

	[SerializeField]
	private int maxKarma = 10;

	[Header("UI")]
	[SerializeField]
	private GameObject group;
	[SerializeField]
	private Sprite[] sprites;
	[SerializeField]
	private Image feedbackImage;
	[SerializeField]
	private Image progressionImage;

	float timer = 0f;
	[SerializeField]
	private float lerpDuration = 0.5f;
	[SerializeField]
	private float appearDuration = 1.5f;
	private Color initColor;

	[Header("Sound")]
	[SerializeField] private AudioClip karmaGoodSound;
	[SerializeField] private AudioClip karmaBadSound;

	bool lerping = false;

	void Awake () {
		Instance = this;

	}

	void Start () {

		UpdateUI ();
		Visible = false;
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.H))
			CurrentKarma--;

		if (Input.GetKeyDown (KeyCode.J))
			CurrentKarma++;

		UpdateLerp ();
	}

	public void CheckKarma () {

		int decal = 0;

		if ( CurrentKarma > (float)(maxKarma / 2) ) {
			decal = 0;
			// un exemple de moralité
		} else if ( CurrentKarma > 0 ) {
			decal = 1;
			// rien à signaler
		} else if ( CurrentKarma > -(float)(maxKarma/2) ) {
			decal = 2;
			// un mec louche
		} else {
			decal = 3;
			// une sous merde
		}

		StoryReader.Instance.NextCell ();
		StoryReader.Instance.SetDecal (decal);

		StoryReader.Instance.UpdateStory ();

	}

	public void AddKarma () {
		++CurrentKarma;
		StoryReader.Instance.NextCell ();
		StoryReader.Instance.UpdateStory ();
	}

	public void RemoveKarma () {
		--CurrentKarma;

		bounty += bountyStep;

		StoryReader.Instance.NextCell ();
		StoryReader.Instance.UpdateStory ();
	}

	public void PayBounty () {

		StoryReader.Instance.NextCell ();

		if ( GoldManager.Instance.CheckGold (Bounty) ) {

			CurrentKarma = -2;

			GoldManager.Instance.GoldAmount -= Bounty;

		} else {

			StoryReader.Instance.SetDecal (1);

			--CurrentKarma;

		}

		StoryReader.Instance.UpdateStory ();

	}

	public int CurrentKarma {
		get {
			return currentKarma;
		}
		set {

			previousKarma = CurrentKarma;

			currentKarma = Mathf.Clamp ( value , -maxKarma , maxKarma);

			UpdateUI ();
		}
	}

	public void UpdateUI () {

		float fill = ((float)currentKarma / (float)maxKarma);

		if ( fill < 0.5 && fill > -0.5f ) {
			feedbackImage.sprite = sprites [2];
		} else if (fill < 0) {
			feedbackImage.sprite = sprites [1];
		} else {
			feedbackImage.sprite = sprites [0];
		}

		timer = 0f;

		initColor = progressionImage.color;

		Visible = true;

		lerping = true;

		SoundManager.Instance.PlaySound ( currentKarma < previousKarma ? karmaBadSound : karmaGoodSound );

	}


	void UpdateLerp ()
	{
		if (Visible && lerping) {

			if (timer <= lerpDuration) {

				float targetFillAmount = ((float)currentKarma / (float)maxKarma);

				float lerp = timer / lerpDuration;

				float bef = (float)previousKarma / (float)maxKarma;
				float currentFillAmount = Mathf.Lerp (bef, targetFillAmount, lerp);

				progressionImage.fillClockwise = currentFillAmount > 0f;

				Color endColor = currentFillAmount < 0f ? Color.red : Color.green;

				if (currentFillAmount < 0)
					currentFillAmount = -currentFillAmount;

				Color targetColor = Color.Lerp (Color.white, endColor, currentFillAmount);
				progressionImage.color = Color.Lerp (initColor, targetColor, lerp);

				progressionImage.fillAmount = currentFillAmount;

			}

			if (timer >= appearDuration) {
				Visible = false;
				lerping = false;
			}
			timer += Time.deltaTime;

		}
			
	}

	public int Bounty {
		get {
			return bounty;
		}
	}

	public bool Visible {
		get {
			return visible;
		}
		set {
			visible = value;

			group.SetActive (value);
		}
	}
}