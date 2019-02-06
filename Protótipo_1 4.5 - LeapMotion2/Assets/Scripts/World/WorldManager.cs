using System.Collections.Generic;
using UnityEngine;

public class WorldManager
{
	//Instancia global;
	private static WorldManager _worldManager;

	//Mundo atual;
	private World _currentWorld;

	private int _currentWorldId;
	private List<World> _worldList;

	private static ITaskManager taskManager = TaskManagerInstance.Instance;

	public static int TotalWorld
	{
		get { return 5; }
	}



	//Pegar instancia global.
	public static WorldManager GetInstance()
	{
		if (_worldManager == null)
		{
			_worldManager = new WorldManager();
		}
		return _worldManager;
	}

	//Get set
	public World CurrentWorld
	{
		get
		{
			return _currentWorld;
		}
		set
		{
			_currentWorld = value;
			_currentWorld.LoadMaterials();
		}
	}

	//Class constructor
	public WorldManager()
	{
		_worldList = new List<World>(6);

        _worldList.Add(World.treino);
        _worldList.Add(World.floresta);
		_worldList.Add(World.fazenda);
		_worldList.Add(World.industria);
		_worldList.Add(World.cidade);
		_worldList.Add(World.praia);


		_currentWorldId = taskManager.GetCurrentWorld();
        Debug.Log(_currentWorldId);
		_currentWorld = _worldList[_currentWorldId];
	}

	public int CurrentWorldId
	{
		get
		{
			return _currentWorldId;
		}
		set
		{
			_currentWorldId = value;
			CurrentWorld = _worldList[_currentWorldId];
		}
	}

	public List<World> WorldList
	{
		get
		{
			return _worldList;
		}
		set
		{
			_worldList = value;
		}
	}

	public bool checkCurrentWorld()
	{
		int current = taskManager.GetCurrentWorld();
		if (current != _currentWorldId)
		{
			if (current >= _worldList.Count)
			{
				AutoFade.LoadLevel("Final", 1f, 1f, Color.white);
				CurrentWorldId = 0;
				return true;
			}

			AutoFade.LoadLevel("WorldScene", 1f, 1f, Color.white);
			CurrentWorldId = current;
			return true;
		}
		return false;
	}
}