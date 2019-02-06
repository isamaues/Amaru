using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
	public float zPos = 20f;
	public float scaleFactor = 25.6f;
	public Vector2 bgInitialPosition = new Vector2(15f, 10f);

	private Texture2D[] _textureLayer1;
	private Texture2D[] _textureLayer2;
	private Texture2D[] _textureLayer3;
	private WorldManager _wM;
	private Texture2D _skyBot;
	private Texture2D _skyTop;

	private ArrayList _bgList = new ArrayList();
	private ArrayList _objsList = new ArrayList();
	private static GameObject _backGroundHolder;


	private List<GameObject> objectPool; 

	private void Awake()
	{
		_wM = WorldManager.GetInstance();
		LoadResources();
	}

	private void Start()
	{
		_backGroundHolder = new GameObject();
		_backGroundHolder.name = "Back Ground Holder";

		GetComponent<AudioSource>().volume = GameSetup.Volume;
		//Screen.SetResolution((int)GameSetup.GameScreenSize.x, (int)GameSetup.GameScreenSize.y, Screen.fullScreen);
	}

	private void LoadResources()
	{
		_wM.CurrentWorld.LoadBGS();
		_textureLayer1 = new Texture2D[_wM.CurrentWorld.BGLayer1.Count];
		for (int i = 0; i < _wM.CurrentWorld.BGLayer1.Count; i++)
		{
			_textureLayer1[i] = _wM.CurrentWorld.BGLayer1[i];
		}
		_textureLayer2 = new Texture2D[_wM.CurrentWorld.BGLayer2.Count];
		for (int i = 0; i < _wM.CurrentWorld.BGLayer2.Count; i++)
		{
			_textureLayer2[i] = _wM.CurrentWorld.BGLayer2[i];
		}
		_textureLayer3 = new Texture2D[_wM.CurrentWorld.BGLayer3.Count];
		for (int i = 0; i < _wM.CurrentWorld.BGLayer3.Count; i++)
		{
			_textureLayer3[i] = _wM.CurrentWorld.BGLayer3[i];
		}

		_skyBot = _wM.CurrentWorld.SkyBot;
		_skyTop = _wM.CurrentWorld.SkyTop;
	}

	private void Update()
	{
		UpdateBGList(bgInitialPosition);
	}

	// Gera novos backgrounds de acordo com: posição da câmera, campo de visão da câmera e tamanho da textura de background
	private void UpdateBGList(Vector3 initPos)
	{
		if (_bgList.Count == 0)
		{
			GameObject bgTemp = CreateBgGameObject(_textureLayer1[0], _textureLayer2[0], _textureLayer3[0]);

			_bgList.Add(bgTemp);
			bgTemp.transform.position = new Vector3(Camera.main.transform.position.x, bgInitialPosition.y, zPos);

			PutNewBG(bgTemp, false);
			PutNewBG(bgTemp, true);
		}
		else
		{
			var edgeBG = (_bgList[0] as GameObject);
			if (Camera2DTracker.IsOnCamera(edgeBG, true))
				PutNewBG(edgeBG, false);
			else if (_bgList.Count > 1)
			{
				if (!Camera2DTracker.IsOnCamera((_bgList[1] as GameObject), true))
				{
					if ((_bgList[0] as GameObject) == edgeBG)
					{
						_bgList.Remove(edgeBG);
						Destroy(edgeBG);
					}
				}
			}

			edgeBG = (_bgList[_bgList.Count - 1] as GameObject);
			if (Camera2DTracker.IsOnCamera(edgeBG, true))
				PutNewBG(edgeBG, true);
			else if (_bgList.Count > 1)
			{
				if (!Camera2DTracker.IsOnCamera((_bgList[_bgList.Count - 2] as GameObject), true))
				{
					if ((_bgList[_bgList.Count - 1] as GameObject) == edgeBG)
					{
						_bgList.Remove(edgeBG);
						Destroy(edgeBG);
					}
				}
			}
		}
	}

	private GameObject CreateBgGameObject(Texture2D layer1, Texture2D layer2, Texture2D layer3)
	{
		var scale = new Vector2(layer1.width / scaleFactor, layer1.height / scaleFactor);

		GameObject bgCubeLayer1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
		bgCubeLayer1.transform.localScale = new Vector3(scale.x, scale.y, 0.01f);
		bgCubeLayer1.GetComponent<Renderer>().material = new Material(Shader.Find("Transparent/Diffuse"));
		bgCubeLayer1.GetComponent<Renderer>().material.mainTexture = layer1;
		bgCubeLayer1.transform.Rotate(0f, 0f, 180f);
		bgCubeLayer1.transform.parent = _backGroundHolder.transform;

		GameObject bgCubeLayer2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
		bgCubeLayer2.transform.localScale = new Vector3(scale.x, scale.y, 0.01f);
		bgCubeLayer2.GetComponent<Renderer>().material = new Material(Shader.Find("Transparent/Diffuse"));
		bgCubeLayer2.GetComponent<Renderer>().material.mainTexture = layer2;
		bgCubeLayer2.transform.Rotate(0f, 0f, 180f);
		bgCubeLayer2.transform.parent = bgCubeLayer1.transform;
		bgCubeLayer2.transform.Translate(0, 0, 0.2f);

		GameObject bgCubeLayer3 = GameObject.CreatePrimitive(PrimitiveType.Cube);
		bgCubeLayer3.transform.localScale = new Vector3(scale.x, scale.y, 0.01f);
		bgCubeLayer3.GetComponent<Renderer>().material = new Material(Shader.Find("Transparent/Diffuse"));
		bgCubeLayer3.GetComponent<Renderer>().material.mainTexture = layer3;
		bgCubeLayer3.transform.Rotate(0f, 0f, 180f);
		bgCubeLayer3.transform.parent = bgCubeLayer2.transform;
		bgCubeLayer3.transform.Translate(0, 0, 0.4f);

		GameObject skyCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
		skyCube.transform.localScale = new Vector3(scale.x, scale.y, 0.01f);
		skyCube.GetComponent<Renderer>().material = new Material(Shader.Find("Transparent/Diffuse"));
		skyCube.GetComponent<Renderer>().material.mainTexture = _skyBot;
		skyCube.transform.Rotate(0f, 0f, 180f);
		skyCube.transform.parent = bgCubeLayer1.transform;
		skyCube.transform.Translate(new Vector3(0, 0, 2));
		skyCube.AddComponent<CreateBG>();

		return bgCubeLayer1;
	}

	private void PutNewBG(GameObject bgTemp, bool toRight)
	{
		float missingCount;

		if (toRight)
			missingCount = ((Camera.main.ViewportToWorldPoint(Vector3.one).x - bgTemp.GetComponent<Renderer>().bounds.max.x) / bgTemp.GetComponent<Renderer>().bounds.size.x) + 1f;
		else
			missingCount = ((bgTemp.GetComponent<Renderer>().bounds.min.x - Camera.main.ViewportToWorldPoint(Vector3.zero).x) / bgTemp.GetComponent<Renderer>().bounds.size.x) + 1f;

		GameObject newBG;

		for (int i = 0; i < missingCount; i++)
		{
			GameObject lastBG;
			if (toRight)
				lastBG = (_bgList[_bgList.Count - 1] as GameObject);
			else
				lastBG = (_bgList[0] as GameObject);

			newBG = CreateBgGameObject(_textureLayer1[UnityEngine.Random.Range(0, _textureLayer1.Length)],
										_textureLayer2[UnityEngine.Random.Range(0, _textureLayer2.Length)],
										_textureLayer3[UnityEngine.Random.Range(0, _textureLayer3.Length)]);

			_bgList.Insert((toRight) ? _bgList.Count : 0, newBG);

			var correctXPos = (newBG.GetComponent<Renderer>().bounds.size.x + lastBG.GetComponent<Renderer>().bounds.size.x) / 2;

			newBG.transform.position = lastBG.transform.position + ((toRight) ? Vector3.right : Vector3.left) * correctXPos;
		}
	}
}