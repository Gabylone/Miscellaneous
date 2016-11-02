﻿using UnityEngine;
using System.Collections;

[System.Serializable]
public class Item {

	public int ID = 0;

	public string 	name = "";
	public string 	description = "";
	public int 		value = 0;
	public int 		price = 0;
	public int 		weight = 0;

	public ItemCategory category;
		
	public Item (

		int _id,

		string _name,
		string _description,
		int _value,
		int _price,
		int _weight,

		ItemCategory _cat
		)
	{
		ID = _id;

		name = _name;
		description = _description;
		value = _value;
		price = _price;
		weight = _weight;

		category = _cat;
	}
}
