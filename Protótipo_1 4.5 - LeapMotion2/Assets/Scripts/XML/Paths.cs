using UnityEngine;
using System;

public static class Paths
{
	public static string TASK_TYPE
	{
		get { return Application.dataPath + "/Data/TXTs/tasks.txt"; }
		private set { }
	}

	public static string DEFAULT_TASK_TYPES_TXT
	{
		get { return Application.dataPath + "/Data/TXTs/PadraoTipoTentativas.txt"; }
		private set { }
	}

	public static string DEFAULT_TASK_TXT
	{
		get { return Application.dataPath + "/Data/TXTs/PadraoTentativas.txt"; }
		private set { }
	}

	public static string DEFAULT_WORD_TXT
	{
		get { return Application.dataPath + "/Data/TXTs/PadraoPalavras.txt"; }
		private set { }
	}

	public static string WORDS_XML_PATH
	{
		get { return Application.dataPath + "/Data/XMLs/GeneratedWords.xml"; }
		private set { }
	}

	public static string MINIGAMES_XML_PATH
	{
		get { return Application.dataPath + "/Data/XMLs/GeneratedMiniGames.xml"; }
		private set { }
	}

	public static string USER_PATH
	{
		get
		{
			string usersPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Amaru\\Data\\Users\\";

			if (!System.IO.Directory.Exists(usersPath))
				System.IO.Directory.CreateDirectory(usersPath);

			return usersPath;
		}
		private set { }
	}

	public static string DATA_XMLs_FOLDER_PATH
	{
		get
		{
			string xmlPath = Application.dataPath + "/Data/XMLs/";

			if (!System.IO.Directory.Exists(xmlPath))
				System.IO.Directory.CreateDirectory(xmlPath);

			return xmlPath;
		}
		private set { }
	}

	public static string XML_CONF_FILE_PATH
	{
		get { return Application.dataPath + "/Data/TXTs/config.txt"; }
		private set { }
	}
}