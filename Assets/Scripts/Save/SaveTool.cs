﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System;
using System.Text;


//tu peux changer le chemin de sauvegarde il y a troi ligne a changer :
//string path = Application.dataPath + dataPathSave;
public class SaveTool : MonoBehaviour
{
	public static SaveTool Instance;
	private const string dataPathSave = "/GameSaveData";

	void Awake () {
		Instance = this;
	}

	#region Save
	public void Save(int index)
    {
		string path = Application.dataPath + dataPathSave + index.ToString () + ".xml";

		byte[] bytes = Encoding.Unicode.GetBytes(path);
		path = Encoding.Unicode.GetString(bytes);

		FileStream file = File.Open(path, FileMode.Create);
		XmlSerializer serializer = new XmlSerializer(typeof(GameData));
		serializer.Serialize(file, SaveManager.Instance.CurrentData);

		file.Close();

    }
	#endregion

	#region Load
	public GameData Load(int index)
    {
		GameData gameSaveData = new GameData();

		string path = Application.dataPath + dataPathSave + index.ToString () + ".xml";

		byte[] bytes = Encoding.Unicode.GetBytes(path);
		path = Encoding.Unicode.GetString(bytes);


		FileStream file = File.Open(path, FileMode.OpenOrCreate);
		XmlSerializer serializer = new XmlSerializer(typeof(GameData));
		gameSaveData = (GameData)serializer.Deserialize(file);
		file.Close();

		return gameSaveData;
	}
	#endregion

	public bool FileExists(int index)
    {
		string path = Application.dataPath + dataPathSave + index.ToString () + ".xml";

        byte[] bytes = Encoding.Unicode.GetBytes(path);
        path = Encoding.Unicode.GetString(bytes);

		bool exists = (File.Exists(path));

		return exists;
    }



}