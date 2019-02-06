using System.Collections;
using UnityEngine;

public class TaskManagerInstance
{
	private static ITaskManager instance;

	public static ITaskManager Instance
	{
		get
		{
			if (instance == null)
			{
                string profilePath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "\\Amaru\\Dados\\Usuarios";
                string a = System.IO.Directory.GetDirectories(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "\\Amaru\\Dados\\Usuarios")[0];
                var player = XMLManager.LoadXML<Usuario>(a + "\\Usuario.xml").participante;
                instance = new XMLTaskManager(player);

			}
			return instance;
		}
		set
		{
			instance = value;
		}
	}
}

public interface ITaskManager
{
	string Name{ get; set; }

	/// <summary>
	/// Gets the audio by identifier.
	/// </summary>
	/// <param name="id">The identifier.</param>
	/// <returns></returns>
	AudioClip GetAudioById(int id);

	/// <summary>
	/// Gets the texture by identifier.
	/// </summary>
	/// <param name="id">The identifier.</param>
	/// <returns></returns>
	Texture2D GetTextureById(int id);

	/// <summary>
	/// Gets the word by identifier.
	/// </summary>
	/// <param name="id">The identifier.</param>
	/// <returns></returns>
	string GetWordById(int id);

	/// <summary>
	/// Gets the syllabes by word identifier.
	/// </summary>
	/// <param name="id">The identifier.</param>
	/// <returns></returns>
	ArrayList GetSyllabesByWordId(int id);

	/// <summary>
	/// Gets the current task.
	/// </summary>
	/// <returns></returns>
	Task GetCurrentTask();

	/// <summary>
	/// Gets the type of the current task.
	/// </summary>
	/// <returns></returns>
	string GetCurrentTaskType();

	/// <summary>
	/// Gets the current task identifier.
	/// </summary>
	/// <returns></returns>
	int GetCurrentTaskId();

	/// <summary>
	/// Gets the current world.
	/// </summary>
	/// <returns></returns>
	int GetCurrentWorld();

	/// <summary>
	/// Função que retorna o numero minimo de tarefas possiveis para completar o jogo. Util para saber a distribuição dos parafusos.
	/// </summary>
	/// <returns></returns>
	int GetMinimunTaskCont();

	/// <summary>
	/// Função que retorna a pontuação maxima que pode ser feita no jogo.
	/// </summary>
	/// <returns></returns>
	int MaximumPontuation { get; }

	/// <summary>
	/// Função que retorna se a tarefa atual será jogada mais uma vez ou não.
	/// </summary>
	/// <returns></returns>
	bool ShouldRepeatTask();

	/// <summary>
	/// Passa para a proxima tarefa
	/// </summary>
	void DoNextTask();

	/// <summary>
	/// Registra a tarefa realizada e chama as funções necessarias para preparar a proxima tarefa
	/// </summary>
	/// <param name="task">The task.</param>
	void RegisterTask(TaskResponseModel task);

	int CurrentPontuation {get;}
}