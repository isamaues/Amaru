using UnityEngine;

public class World
{
	//Variaveis dos mundos
	private string _directory;

	private int _nBGs;
	private System.Collections.Generic.Dictionary<string, Material> _tileMaterials;
	private System.Collections.Generic.List<Texture2D> _bgLayer1List;
	private System.Collections.Generic.List<Texture2D> _bgLayer2List;
	private System.Collections.Generic.List<Texture2D> _bgLayer3List;
	private Texture2D _skyBot;
	private Texture2D _skyTop;
	private AudioClip _backgroundMusic;
	private bool _isMaterialsLoaded = false;
	private bool _isBGLoaded = false;

	//Class constructor
	public World(string directory, int nBGs)
	{
		_directory = directory;
		_nBGs = nBGs;
		_bgLayer1List = new System.Collections.Generic.List<Texture2D>();
		_bgLayer2List = new System.Collections.Generic.List<Texture2D>();
		_bgLayer3List = new System.Collections.Generic.List<Texture2D>();
		_tileMaterials = new System.Collections.Generic.Dictionary<string, Material>();
	}

    //Lista de mundos
    public static World treino = new World("treino", 3);
	public static World floresta = new World("Floresta", 3);
	public static World fazenda = new World("Fazenda", 3);
	public static World cidade = new World("Cidade", 3);
	public static World industria = new World("Industria", 3);
	public static World praia = new World("Praia", 3);

	//Gets Sets
	public System.Collections.Generic.Dictionary<string, Material> TileMaterials
	{
		get
		{
			return _tileMaterials;
		}
	}

	public System.Collections.Generic.List<Texture2D> BGLayer1
	{
		get
		{
			return _bgLayer1List;
		}
	}

	public System.Collections.Generic.List<Texture2D> BGLayer2
	{
		get
		{
			return _bgLayer2List;
		}
	}

	public System.Collections.Generic.List<Texture2D> BGLayer3
	{
		get
		{
			return _bgLayer3List;
		}
	}

	public Texture2D SkyBot
	{
		get
		{
			return _skyBot;
		}
	}

	public Texture2D SkyTop
	{
		get
		{
			return _skyTop;
		}
	}

	public AudioClip BackgroundSong
	{
		get
		{
			return _backgroundMusic;
		}
	}

	//Carregar tileset atual ao dicionario.
	public void LoadMaterials()
	{
		if (!_isMaterialsLoaded)
		{
			_tileMaterials = new System.Collections.Generic.Dictionary<string, Material>();
			_tileMaterials.Add("PTL", (Material)Resources.Load("Textures/Tiles/" + _directory + "/Materials/pt00"));
			_tileMaterials.Add("PTM", (Material)Resources.Load("Textures/Tiles/" + _directory + "/Materials/pt01"));
			_tileMaterials.Add("PTR", (Material)Resources.Load("Textures/Tiles/" + _directory + "/Materials/pt02"));
			_tileMaterials.Add("PTL1", (Material)Resources.Load("Textures/Tiles/" + _directory + "/Materials/pt03"));
			_tileMaterials.Add("PTL2", (Material)Resources.Load("Textures/Tiles/" + _directory + "/Materials/pt05"));
			_tileMaterials.Add("PTR1", (Material)Resources.Load("Textures/Tiles/" + _directory + "/Materials/pt04"));
			_tileMaterials.Add("PTR2", (Material)Resources.Load("Textures/Tiles/" + _directory + "/Materials/pt06"));
			_tileMaterials.Add("PTS", (Material)Resources.Load("Textures/Tiles/" + _directory + "/Materials/pt07"));
			_tileMaterials.Add("PBL", (Material)Resources.Load("Textures/Tiles/" + _directory + "/Materials/pb00"));
			_tileMaterials.Add("PBM", (Material)Resources.Load("Textures/Tiles/" + _directory + "/Materials/pb01"));
			_tileMaterials.Add("PBR", (Material)Resources.Load("Textures/Tiles/" + _directory + "/Materials/pb02"));
			_tileMaterials.Add("PBS", (Material)Resources.Load("Textures/Tiles/" + _directory + "/Materials/pb03"));

			_tileMaterials.Add("NTL", (Material)Resources.Load("Textures/Tiles/NP/Materials/np00"));
			_tileMaterials.Add("NTM", (Material)Resources.Load("Textures/Tiles/NP/Materials/np01"));
			_tileMaterials.Add("NTR", (Material)Resources.Load("Textures/Tiles/NP/Materials/np02"));
			_tileMaterials.Add("NBL", (Material)Resources.Load("Textures/Tiles/NP/Materials/np03"));
			_tileMaterials.Add("NBM", (Material)Resources.Load("Textures/Tiles/NP/Materials/np04"));
			_tileMaterials.Add("NBR", (Material)Resources.Load("Textures/Tiles/NP/Materials/np05"));
			_tileMaterials.Add("NTS", (Material)Resources.Load("Textures/Tiles/NP/Materials/np06"));
			_tileMaterials.Add("NBS", (Material)Resources.Load("Textures/Tiles/NP/Materials/np07"));
			_isMaterialsLoaded = true;
		}
	}

	public void LoadBGS()
	{
		if (!_isBGLoaded)
		{
			for (int i = 1; i <= _nBGs; i++)
			{
				_bgLayer1List.Add((Texture2D)Resources.Load("Textures/BG/" + _directory + "/bg" + i + "_layer1"));
			}

			for (int i = 1; i <= _nBGs; i++)
			{
				_bgLayer2List.Add((Texture2D)Resources.Load("Textures/BG/" + _directory + "/bg" + i + "_layer2"));
			}

			for (int i = 1; i <= _nBGs; i++)
			{
				_bgLayer3List.Add((Texture2D)Resources.Load("Textures/BG/" + _directory + "/bg" + i + "_layer3"));
			}
			/*_skyBot = (Texture2D)Resources.Load("Textures/Skys/" + _directory + "/skyBot");
			_skyTop = (Texture2D)Resources.Load("Textures/Skys/" + _directory + "/skyTop");*/

			_skyBot = (Texture2D)Resources.Load("Textures/Skys/skyBot");
			_skyTop = (Texture2D)Resources.Load("Textures/Skys/skyTop");

			_isBGLoaded = true;
		}
	}

	public void LoadAudioCLIP()
	{
		_backgroundMusic = (AudioClip)Resources.Load("SoundFX/BG_Sound/" + _directory);
	}
}