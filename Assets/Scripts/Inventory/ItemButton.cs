﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ItemButton : MonoBehaviour {

	[SerializeField]
	private LootUI lootUI;

	[Header("UI elements")]
	[SerializeField] private Button button;
	[SerializeField] private Text nameText;
	[SerializeField] private Text paramText;
	[SerializeField] private Text priceText;
	[SerializeField] private Text weightText;

	[SerializeField] private GameObject paramObj;
	[SerializeField] private GameObject priceObj;
	[SerializeField] private GameObject weightObj;

	private int index = 0;

	private string name;
	private string description;
	private int param;
	private int price;
	private int weight;

	bool enabled = false;

	public void Select () {
		lootUI.UpdateActionButton (index);
	}

	public bool Enabled {
		get {
			return enabled;
		}
		set {
			enabled = value;
			GetComponent<Button> ().interactable = value;
		}
	}

	#region params
	public string Name {
		get {
			return name;
		}
		set {
			name = value;
			nameText.text = name;
		}
	}

	public int Param {
		get {
			return param;
		}
		set {
			param = value;
			paramText.text = param.ToString ();
			paramObj.SetActive ( value > 0 );
		}
	}

	public int Price {
		get {
			return price;
		}
		set {
			price = value;
			priceText.text = price.ToString ();
			priceObj.SetActive (value > 0);
		}
	}

	public int Weight {
		get {
			return weight;
		}
		set {
			weight = value;
			weightText.text = weight.ToString ();
			weightObj.SetActive (value > 0);
		}
	}

	public Button Button {
		get {
			return button;
		}
	}

	public int Index {
		get {
			return index;
		}
		set {
			index = value;
		}
	}
	#endregion
}
