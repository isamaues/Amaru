using System;
using System.Collections;
using UnityEngine;

//REVISION 126
public class UserManager
{
	private static UserManager userManager = null;

//	public static ConfiguracaoAI CurrentUserSetings { get; private set; }

	public static UserGameProgress CurrentUserProgress { get; private set; }

	public static string CurrentPath { get; private set; }

	public static WordList CurrentWordList { get; private set; }

	public static TasksTypesList CurrentTaskTypesList { get; private set; }

	public static TasksList CurrentTasksList { get; set; }

    public static MinigameList CurrentMinigames { get; private set; }

	public static RespostaList CurrentRespostaList { get; set; }

	public static ResultadoObstaculoList CurrentResultadoObstaculoList { get; set; }

	public static Progresso CurrentProgress { get; private set; }

	public static Usuario CurrentUser { get; private set;}

	public static UserManager GetInstance()
	{
		if (userManager == null)
		{
			userManager = new UserManager();
		}

		return userManager;
	}

/*	/// <summary>
	/// Sets the current user. Deve ser usada quando se criar um novo player.
	/// </summary>
	/// <param name='currentUser'>
	/// Current user.
	/// </param>
	/// <param name='currentPath'>
	/// Current path.
	/// </param>
	/// <param name='currentWordList'>
	/// Current word list.
	/// </param>
	/// <param name='currentTasksTypesList'>
	/// Current tasks types list.
	/// </param>
	/// <param name='currentTasksList'>
	/// Current tasks list.
	/// </param>
	public void SetCurrentUser(ConfiguracaoAI currentUser, string currentPath, WordList currentWordList,
								TasksTypesList currentTasksTypesList, TasksList currentTasksList)
	{
		CurrentUserSetings = currentUser;
		CurrentPath = currentPath;
		CurrentWordList = currentWordList;
		CurrentTaskTypesList = currentTasksTypesList;
		CurrentTasksList = currentTasksList;
		CurrentMinigames = XMLManager.LoadXML<MinigameList>(Paths.DATA_XMLs_FOLDER_PATH, "GeneratedMiniGames.xml");
		CurrentRespostaList = null;
		CurrentResultadoObstaculoList = null;
		CurrentUserProgress = XMLManager.LoadXML<UserGameProgress>(Paths.DATA_XMLs_FOLDER_PATH, "GeneratedUserGameProgress.xml");
		XMLManager.CreateXML<RespostaList>(CurrentPath, "Respostas.xml", null);
		XMLManager.CreateXML<ResultadoObstaculoList>(CurrentPath, "ResultadoObstaculos.xml", null);
	}

	public void SetCurrentUser(ConfiguracaoAI currentUser, string currentPath, WordList currentWordList,
								TasksTypesList currentTasksTypesList, TasksList currentTasksList, RespostaList currentRespostaList)
	{
		CurrentUserSetings = currentUser;
		CurrentPath = currentPath;
		CurrentWordList = currentWordList;
		CurrentTaskTypesList = currentTasksTypesList;
		CurrentTasksList = currentTasksList;
		CurrentRespostaList = currentRespostaList;
		CurrentResultadoObstaculoList = null;
		XMLManager.CreateXML<ResultadoObstaculoList>(CurrentPath, "ResultadoObstaculos.xml", null);
		CurrentMinigames = XMLManager.LoadXML<MinigameList>(Paths.DATA_XMLs_FOLDER_PATH, "GeneratedMiniGames.xml");
	}

	public void SetCurrentUser(ConfiguracaoAI currentUser, string currentPath, WordList currentWordList,
								TasksTypesList currentTasksTypesList, TasksList currentTasksList, RespostaList currentRespostaList, ResultadoObstaculoList currentObstaculoList)
	{
		CurrentUserSetings = currentUser;
		CurrentPath = currentPath;
		CurrentWordList = currentWordList;
		CurrentTaskTypesList = currentTasksTypesList;
		CurrentTasksList = currentTasksList;
		CurrentRespostaList = currentRespostaList;
		CurrentResultadoObstaculoList = currentObstaculoList;
		CurrentMinigames = XMLManager.LoadXML<MinigameList>(Paths.DATA_XMLs_FOLDER_PATH, "GeneratedMiniGames.xml");
	}
*/

	/// <summary>
	/// Sets the current user. Deve ser usado na hora de se carregar um player.
	/// </summary>
	/// <param name='PlayerLogin'>
	/// Player login.
	/// </param>
	public void SetCurrentUser(string PlayerLogin)
	{
		CurrentPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Amaru\\Dados\\Usuarios\\" + PlayerLogin;
		CurrentProgress = XMLManager.LoadXML<Progresso>(CurrentPath, "Progresso.xml");
		CurrentUser = XMLManager.LoadXML<Usuario> (CurrentPath, "Usuario.xml");
//		CurrentUserSetings = XMLManager.LoadXML<ConfiguracaoAI>(CurrentPath, "ConfiguracaoAI.xml");
		CurrentWordList = XMLManager.LoadXML<WordList>(CurrentPath, "Palavras.xml");
		//CurrentTaskTypesList = XMLManager.LoadXML<TasksTypesList>(CurrentPath, "TipoTarefas.xml");
		CurrentTasksList = XMLManager.LoadXML<TasksList>(CurrentPath, "Tarefas.xml");
        CurrentRespostaList = XMLManager.LoadXML<RespostaList>(CurrentPath, "Respostas.xml");
		CurrentResultadoObstaculoList = XMLManager.LoadXML<ResultadoObstaculoList>(CurrentPath, "ResultadoObstaculos.xml");
		CurrentMinigames = XMLManager.LoadXML<MinigameList>(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Amaru\\Dados\\PadroesXML\\PadraoMiniGames.xml");

        foreach(Word w in CurrentWordList.arrayList)
		{
			//w.Resource = w.Resource.ToUpper ();
			w.Name = w.Name.ToUpper ();

			for (int i = 0; i < w.Syllables.Count; i++)
			{
				w.Syllables[i] = w.Syllables[i].ToString().ToUpper();
			}

            Debug.Log(w.Resource);
        }
	}

	#region XML Meths

	public void UpdateTaskListXML()
	{
		XMLManager.CreateXML<TasksList>(CurrentPath, "Tarefas.xml", CurrentTasksList);
	}

	public void UpdateWordListXML()
	{
		XMLManager.CreateXML<WordList>(CurrentPath, "Palavras.xml", CurrentWordList);
	}

	//public void UpdateCurrentUserXML()
	//{
	//	XMLManager.CreateXML<ConfiguracaoAI>(CurrentPath, "Configuracoes.xml", CurrentUserSetings);
	//}

	public void UpdateCurrentProgressXML()
	{
		XMLManager.CreateXML<Progresso>(CurrentPath, "Progresso.xml", CurrentProgress);
	}

	public void UpdateRespostaListXML()
	{
		XMLManager.CreateXML<RespostaList>(CurrentPath, "Respostas.xml", CurrentRespostaList);
	}

	public void UpdateResultadoObstaculoListXML()
	{
		XMLManager.CreateXML<ResultadoObstaculoList>(CurrentPath, "ResultadoObstaculos.xml", CurrentResultadoObstaculoList);
	}

	#endregion XML Meths

	#region Tasks Meths

	public static Task GetTaskById(int id)
	{
		foreach (Task t in CurrentTasksList.arrayList)
		{
			if (t.Id == id)
				return t;
		}

		return null;
	}

	public static ArrayList GetTasksByModel(string model)
	{
		ArrayList selectedTasks = new ArrayList();

		foreach (Task task in CurrentTasksList.arrayList)
		{
			Debug.Log("Modelo do Task: " + CurrentWordList.arrayList[task.Model]);
			if (CurrentWordList.arrayList[task.Model].Equals(model))
			{
				selectedTasks.Add(task);
			}
		}

		return selectedTasks;
	}

	#endregion Tasks Meths

	#region Minigames Meths

	public static string GetCurrentMiniGameName()
	{
		foreach (MiniGame mg in CurrentMinigames.arrayList)
		{
			if (mg.Id == CurrentTasksList.arrayList[CurrentProgress.id_Da_Ultima_Tarefa].MiniGame)
				return mg.Name;
		}
		//caso não ache, tenta um último recurso
		return CurrentMinigames.arrayList[CurrentTasksList.arrayList[CurrentProgress.id_Da_Ultima_Tarefa].MiniGame - 1].Name;
	}

	public static bool IsPreTestFinished()
	{
		return CurrentProgress.id_Da_Ultima_Tarefa > CurrentTasksList.arrayList.Count;
	}

	#endregion Minigames Meths

	#region Words Meths

	/// <summary>
	/// Gets the learned words (Writing and Reading words)
	/// </summary>
	public static ArrayList GetLearnedWords()
	{
		ArrayList wordList = new ArrayList();

		foreach (Word w in CurrentWordList.arrayList)
		{
			if (w.LearnedWrite.Equals(1) && w.LearnedRead.Equals(1))
				wordList.Add(w);
		}

		return wordList;
	}

	public static ArrayList GetLearnedWritingWords()
	{
		ArrayList wordList = new ArrayList();

		foreach (Word w in CurrentWordList.arrayList)
		{
			if (w.LearnedWrite.Equals(1))
				wordList.Add(w);
		}

		return wordList;
	}

	public static ArrayList GetLearnedReadingWords()
	{
		ArrayList wordList = new ArrayList();

		foreach (Word w in CurrentWordList.arrayList)
		{
			if (w.LearnedRead.Equals(1))
				wordList.Add(w);
		}

		return wordList;
	}

	public static ArrayList GetNonLearnedWords()
	{
		ArrayList wordList = new ArrayList();

		foreach (Word w in CurrentWordList.arrayList)
		{
			if (w.LearnedWrite.Equals(0) || w.LearnedRead.Equals(0))
				wordList.Add(w);
		}

		return wordList;
	}

	#endregion Words Meths
}