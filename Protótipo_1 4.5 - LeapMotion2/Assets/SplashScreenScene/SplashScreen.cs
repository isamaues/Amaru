using System;
using System.Xml;
using UnityEngine;
using System.Collections.Generic;

public class SplashScreen : MonoBehaviour
{
	public static string player;
	public UnityEngine.UI.Text text;
	public GameObject menuAudioSource;

	private void Awake()
	{
		//if (menuAudioSource == null)
		//{
			menuAudioSource = GameObject.Find("MenuAudioSource");
//			Debug.Log("T");
		//}

		if (TaskManagerInstance.Instance != null)
		{
            Debug.Log(TaskManagerInstance.Instance.Name);
			player = TaskManagerInstance.Instance.Name;
			text.text = player;
		}
		else 
		{
            Debug.Log("OI EU SOU GOKU");
			/*var profilesPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Amaru\\Dados\\perfis.xml";
			var userProfilesDocument = new XmlDocument();
			try {
				userProfilesDocument.Load(profilesPath);
			}
			catch (Exception e)
			{
				Debug.Log("!@#");
			}

			var userList = userProfilesDocument.DocumentElement.SelectNodes("Usuario");

			player = userList[0].SelectSingleNode("NomePerfil").InnerText;
			text.text = player;*/

            List<Profile> nameList = new List<Profile>();
            string profilePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Amaru\\Dados\\Usuarios";
            string a = System.IO.Directory.GetDirectories(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Amaru\\Dados\\Usuarios")[0];
            Debug.Log(a);

            player = XMLManager.LoadXML<Usuario>(a).participante;

			TaskManagerInstance.Instance = new XMLTaskManager(player);
		}
	}


	public void ChangePlayer()
	{
		AutoFade.LoadLevel("Menu", 0.5f, 0.5f, Color.white);
	}

	public void ChangeLevel()
	{
		//DestroyObject(menuAudioSource);
        if (menuAudioSource == null)
        {
            menuAudioSource = GameObject.Find("MenuAudioSource");
        }
		menuAudioSource.SetActive(false);
		//menuAudioSource.SetActive(fa

		if (TaskManagerInstance.Instance.GetCurrentTaskId() == 0)
			AutoFade.LoadLevel("Openning", 0.1f, 0.1f, Color.white);
		else
			AutoFade.LoadLevel("new_level_001", 0.1f, 0.1f, Color.white);
	}

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Jogo Fechado");
    }
}