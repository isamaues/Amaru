using System;
using System.Xml;
using UnityEngine;

public class LoginTest : MonoBehaviour
{
	private void Awake()
	{
		var profilesPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Amaru\\Data\\perfis.xml";
		var userProfilesDocument = new XmlDocument();
		userProfilesDocument.Load(profilesPath);
		var userList = userProfilesDocument.DocumentElement.SelectNodes("User");

		var player = userList[0].SelectSingleNode("NomePerfil").InnerText;

		TaskManagerInstance.Instance = new XMLTaskManager(player);
	}

	private void Start()
	{
		if (TaskManagerInstance.Instance.GetCurrentTaskId() == 0)
			AutoFade.LoadLevel("Openning", 0, 0, Color.white);
		else
			AutoFade.LoadLevel("new_level_001", 0, 0, Color.white);
	}
}