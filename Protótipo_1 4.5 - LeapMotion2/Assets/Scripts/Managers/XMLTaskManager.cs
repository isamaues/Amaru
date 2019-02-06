using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XMLTaskManager : ITaskManager
{
	#region Fields

	private ArrayList audioList = new ArrayList();
	private ArrayList textureList = new ArrayList();

	#endregion Fields

    private int modTasks;
    private int tasksPerWorld;
    private List<int> mundos = new List<int>();
    private bool flag = false;

	#region Methods

	public XMLTaskManager(string currentUser)
	{
		Name = currentUser;
	}

	private string name;

	public string Name 
	{
		get
		{
			return name;
		}
		set
		{
			name = value;
			UserManager.GetInstance().SetCurrentUser(name);
			Debug.Log("Current User:" + name);
			LoadResources();
		}
	}

	public void BackupRespostas()
	{
		Debug.Log(UserManager.CurrentUser.participante);

		var Path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Amaru\\Dados\\Usuarios\\" + UserManager.CurrentUser.participante;

		int i = 1;
        while (System.IO.Directory.Exists(Path + "\\BackupsRespostas\\Backup " + i))
		{
			i++;
			Debug.Log("ASD : " + i);
		}
        System.IO.Directory.CreateDirectory(Path + "\\BackupsRespostas\\Backup " + i);
        
        System.IO.File.Move(Path + "\\Respostas.xml", Path + "\\BackupsRespostas\\Backup " + i +"\\Respostas.xml");
        System.IO.File.Copy(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Amaru\\Dados\\PadroesXML\\PadraoRespostas.xml", Path + "\\Respostas.xml");
        System.IO.File.Move(Path + "\\ResultadoObstaculos.xml", Path + "\\BackupsRespostas\\Backup " + i + "\\ResultadoObstaculos.xml");
        System.IO.File.Copy(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Amaru\\Dados\\PadroesXML\\PadraoResultadoObstaculos.xml", Path + "\\ResultadoObstaculos.xml");
    }

	public void ResetXML()
	{
		UserManager.CurrentProgress.quantidade_Parafusos = 0;
		UserManager.CurrentProgress.id_Da_Ultima_Tarefa = 1;
		UserManager.GetInstance().UpdateCurrentProgressXML();
    }

	public void DoNextTask()
	{
        if (GetCurrentTask().Id == 1)
            UserManager.CurrentProgress.id_Da_Ultima_Tarefa = 1;
        UserManager.CurrentProgress.id_Da_Ultima_Tarefa++;
		//Descomentar quando estiver valendo
		//salva o novo id no XML (configurações)
		UserManager.GetInstance().UpdateCurrentProgressXML();
	}

	public AudioClip GetAudioById(int id)
	{
		foreach (Word w in UserManager.CurrentWordList.arrayList)
		{
			if (w.WordId == id)
            {          
				foreach (AudioClip a in audioList)
				{
					if (a.name.Equals(w.Resource))
					{
						return a;
					}
				}
				break;
			}
		}

		return null;
	}

	public int CurrentPontuation
	{
		get { return UserManager.CurrentProgress.quantidade_Parafusos; }
	}

	public Task GetCurrentTask()
	{
        return UserManager.CurrentTasksList.arrayList[GetCurrentTaskId()];
	}

	public int GetCurrentTaskId()
	{
		return UserManager.CurrentProgress.id_Da_Ultima_Tarefa - 1;
	}

	public string GetCurrentTaskType()
	{
		return GetCurrentTask().TaskType;
	}

	/* GetCurrentWorld() retorna o mundo que esta sendo executado no momento.
	 * modTasks e usado para conhecer o resto da divisao entre a quantidade de tarefas e o total de mundos existentes no jogo, caso haja tarefas demais.
	 * tasksPerWorld e usado para calcular a quantidade de tarefas igualmente dividas entre os mundos.
	 * 
	 * 		Os mundos sao dividos em uma lista. Começando do 1, cada elemento da lista carrega a quantidade de tarefas que devem se executadas
	 * em cada mundo apos calculados essas quantidades individualmente. Com isso feito e utlizando o id da ultima tarefa e possivel achar a 
	 * faixa de valores ao qual o id da tarefa pertence.
	 */
	public int GetCurrentWorld()
	{
        #region Tarefas por mundo
        if (!flag)
        {
            int qtdTask = 0;
            mundos.Clear();

            foreach (Task t in UserManager.CurrentTasksList.arrayList) {
                if (t.Id < 100)
                    qtdTask++;
            }

            modTasks = qtdTask % WorldManager.TotalWorld;
            tasksPerWorld = (qtdTask / WorldManager.TotalWorld);
            mundos.Add(0);
            for (int i = 1; i <= WorldManager.TotalWorld; i++)
            {
                if (modTasks > 0)
                {
                    mundos.Add(mundos[i - 1] + tasksPerWorld + 1);
                    modTasks--;
                }
                else
                    mundos.Add(mundos[i - 1] + tasksPerWorld);
            }

            flag = true;
        }
        #endregion

        for (int i = 1; i <= WorldManager.TotalWorld; i++)
		{
			if ( (UserManager.CurrentProgress.id_Da_Ultima_Tarefa - 1) >= mundos[i-1] && (UserManager.CurrentProgress.id_Da_Ultima_Tarefa - 1) < mundos[i] )
			{
				return i;
			}
		}
        return WorldManager.TotalWorld + 1;
	}

	public int GetMinimunTaskCont()
	{
		return UserManager.CurrentTasksList.arrayList.Count;
	}

	private int maxPontuation = -1;

	public int MaximumPontuation
	{
		get
		{
			if (maxPontuation == -1)
			{
				var i = 0;
				foreach (Task a in UserManager.CurrentTasksList.arrayList)
				{
					if (a.TaskRole == 0)
						i += GearsManager.PontuacaoPorcaBronze;
					else
						i += GearsManager.PontuacaoEngrenagemOuro;
				}
				maxPontuation = i;
				return i;
			}
			else
			{
				return maxPontuation;
			}
		}
	}

	public ArrayList GetSyllabesByWordId(int id)
	{
		var list = new List<string>();
		foreach (Word w in UserManager.CurrentWordList.arrayList)
		{
			if (w.WordId == id)
			{
				return w.Syllables;
			}
		}
		return null;
	}

	public Texture2D GetTextureById(int id)
	{
		foreach (Word w in UserManager.CurrentWordList.arrayList)
		{
			if (w.WordId == id)
			{
				foreach (Texture2D t in textureList)
				{
//					Debug.Log(t.name + " " + id);
					//Debug.Log("Id procurado " + id + " Textura compara " + t.name);
					if (t.name.Equals(w.Resource))
					{
						//Debug.Log(t.name + " " + id);
						return t;
					}
				}
				break;
			}
		}
		return null;
	}

	public string GetWordById(int id)
	{
		foreach (Word w in UserManager.CurrentWordList.arrayList)
		{
			if (w.WordId == id)
			{
				return w.Name;
			}
		}

		return null;
	}

	public void RegisterTask(TaskResponseModel task)
	{
		UserManager.CurrentRespostaList.arrayList.Add(
			new Resposta(" ", task.TaskType, task.Escolhas, task.RespostaCerta, task.RespostaDada, task.NumeroTentativa,
				task.Latencia, task.Resultado, task.MiniGame, task.FaseProcedimento, task.DateTime));

		UserManager.GetInstance().UpdateRespostaListXML();
	}

	public bool ShouldRepeatTask()
	{
		return GetCurrentTask().TaskRole == 1;
	}

	private void LoadImages() { 
		textureList = new ArrayList();

		var Path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Amaru\\Dados\\Recursos\\";
		var imagesFiles = System.IO.Directory.GetFiles(Path + "Images");
		
		foreach (string file in imagesFiles)
		{
			byte[] fileData;
			fileData = System.IO.File.ReadAllBytes(file);
			Texture2D texture = new Texture2D(1,1);
			texture.LoadImage(fileData);
			var name = System.IO.Path.GetFileName(file);
			texture.name = name.Remove(name.Length-4);
			textureList.Add(texture);
		}
	}

	private void LoadSounds ()
	{
        audioList = GameObject.Find("AudioLoader").GetComponent<AudioLoader>().LoadAudios(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Amaru\\Dados\\Recursos\\");
	}

	private void LoadResources()
    {
		LoadSounds ();
        LoadImages();
	}

	#endregion Methods
}