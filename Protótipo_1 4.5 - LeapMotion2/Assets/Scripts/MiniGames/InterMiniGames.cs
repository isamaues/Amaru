using UnityEngine;

//using System;

public class InterMiniGames : MonoBehaviour
{
	private const int NUMPLAT = 3;

	//const int NT1 = 4; //Numero de tipos pro primeiro quadrante.
	private const int NT1 = 2; //Numero de tipos pro primeiro quadrante.

	private const int NT2 = 3; //Numero de tipos pro segundo quadrante.
	private const int NT3 = 3; //Numero de tipos pro terceiro quadrante.
	private const int NT4 = 2; //Numero de tipos pro quarto quadrante.

	private const int TT2 = NT1 * NT2; //Total de plataformas do segundo quadrante.
	private const int TT3 = NT1 * NT3; //Total de plataformas do terceiro quadrante.
	private const int TT4 = NT1 * NT2 * NT3 * NT4; //Total de plataformas do quarto quadrante.

	public GameObject module;
	public GameObject gear;
	public GameObject trigger;
	public GameObject crianca, criancaPrefab;

	private PLATINFO[][] platInfos1, platInfos2, platInfos3, platInfos4;
	private PLATINFO[][][] platInfos;
	private Vector2[][] gearPosistions;

	private CreatPlatform cP;


	public void Start()
	{
		criancaPrefab = (GameObject)Resources.Load("Prefabs/Kids");

		cP = new CreatPlatform();

		platInfos = new PLATINFO[4][][];

		gearPosistions = new Vector2[4][];
		gearPosistions[0] = new Vector2[NT1];
		gearPosistions[1] = new Vector2[TT2];
		gearPosistions[2] = new Vector2[TT3];
		gearPosistions[3] = new Vector2[TT4];

		//MODULOS 1

		platInfos[0] = new PLATINFO[NT1][];
		platInfos[0][0] = new PLATINFO[1];
		platInfos[0][0][0] = new PLATINFO(false, new Vector2(0, 5), 6);
		gearPosistions[0][0] = new Vector2(3, 3);

		platInfos[0][1] = new PLATINFO[1];
		platInfos[0][1][0] = new PLATINFO(false, new Vector2(0, 5), 3);
		gearPosistions[0][1] = new Vector2(1, 3);

		//MODULOS 2

		platInfos[1] = new PLATINFO[TT2][];

		for (int i = 0; i < TT2; i++)
		{
			platInfos[1][i] = new PLATINFO[0];
			gearPosistions[1][i] = Vector2.zero;
		}

		gearPosistions[1][0] = new Vector2(3, 7);

        platInfos[1][1] = new PLATINFO[1];
		platInfos[1][1][0] = new PLATINFO(false, new Vector2(1, 9), 4);
		gearPosistions[1][1] = new Vector2(3, 10);

		platInfos[1][2] = new PLATINFO[1];
		platInfos[1][2][0] = new PLATINFO(true, new Vector2(6, 9), 3);
		gearPosistions[1][2] = new Vector2(3f, 10);

		gearPosistions[1][3] = new Vector2(1, 8);

		platInfos[1][4] = new PLATINFO[1];
		platInfos[1][4][0] = new PLATINFO(true, new Vector2(2, 9), 3);
		gearPosistions[1][4] = new Vector2(3, 10);

		platInfos[1][5] = new PLATINFO[1];
		platInfos[1][5][0] = new PLATINFO(false, new Vector2(0, 9), 3);
		gearPosistions[1][5] = new Vector2(1, 10);

        //MODULOS 3

        platInfos[2] = new PLATINFO[TT3][];

		for (int i = 0; i < TT3; i++)
		{
			platInfos[2][i] = new PLATINFO[0];
			gearPosistions[2][i] = Vector2.zero;
		}

		gearPosistions[2][0] = new Vector2(9, 10);

        platInfos[2][1] = new PLATINFO[2];
		platInfos[2][1][0] = new PLATINFO(true, new Vector2(6, 5), 3);
		platInfos[2][1][1] = new PLATINFO(false, new Vector2(7, 5), 6);
		gearPosistions[2][1] = new Vector2(10, 3);

		platInfos[2][2] = new PLATINFO[1];
		platInfos[2][2][0] = new PLATINFO(true, new Vector2(13, 5), 3);
		gearPosistions[2][2] = new Vector2(15f, 8);

		platInfos[2][4] = new PLATINFO[1];
		platInfos[2][4][0] = new PLATINFO(false, new Vector2(2, 5), 5);
		gearPosistions[2][4] = new Vector2(5.5f, 8);

		platInfos[2][5] = new PLATINFO[1];
		platInfos[2][5][0] = new PLATINFO(false, new Vector2(4, 9), 5);
		gearPosistions[2][5] = new Vector2(6.5f, 10);

		//MODULOS 4
		platInfos[3] = new PLATINFO[TT4][];

		for (int i = 0; i < TT4; i++)
		{
			platInfos[3][i] = new PLATINFO[0];
			gearPosistions[3][i] = Vector2.zero;
		}

		platInfos[3][3] = new PLATINFO[1];
		platInfos[3][3][0] = new PLATINFO(false, new Vector2(10, 9), 3);
		gearPosistions[3][3] = new Vector2(11.5f, 10);

		platInfos[3][5] = new PLATINFO[2];
		platInfos[3][5][0] = new PLATINFO(false, new Vector2(0, 5), 12);
		platInfos[3][5][1] = new PLATINFO(true, new Vector2(12, 9), 3);
		gearPosistions[3][5] = new Vector2(10f, 3);

		platInfos[3][7] = new PLATINFO[1];
		platInfos[3][7][0] = new PLATINFO(false, new Vector2(12, 9), 3);
		gearPosistions[3][7] = new Vector2(13.5f, 10);

		platInfos[3][9] = new PLATINFO[2];
		platInfos[3][9][0] = new PLATINFO(false, new Vector2(13, 9), 3);
		platInfos[3][9][1] = new PLATINFO(false, new Vector2(13, 5), 3);
		gearPosistions[3][9] = new Vector2(17, 10);

		platInfos[3][11] = new PLATINFO[2];
		platInfos[3][11][0] = new PLATINFO(false, new Vector2(6, 5), 7);
		platInfos[3][11][1] = new PLATINFO(true, new Vector2(5, 12), 3);
		gearPosistions[3][11] = new Vector2(9, 10);

		platInfos[3][13] = new PLATINFO[2];
		platInfos[3][13][0] = new PLATINFO(false, new Vector2(7, 9), 3);
		platInfos[3][13][1] = new PLATINFO(false, new Vector2(7, 5), 3);
		gearPosistions[3][13] = new Vector2(8.5f, 7.5f);

		platInfos[3][15] = new PLATINFO[2];
		platInfos[3][15][0] = new PLATINFO(false, new Vector2(13, 9), 3);
		platInfos[3][15][1] = new PLATINFO(false, new Vector2(13, 5), 3);
		gearPosistions[3][15] = new Vector2(18, 10);

		gearPosistions[3][16] = new Vector2(10, 3);

		platInfos[3][17] = new PLATINFO[1];
		platInfos[3][17][0] = new PLATINFO(false, new Vector2(7, 9), 6);
		gearPosistions[3][17] = new Vector2(10, 3);

		gearPosistions[3][18] = new Vector2(10, 9);

		platInfos[3][19] = new PLATINFO[2];
		platInfos[3][19][0] = new PLATINFO(true, new Vector2(2, 5), 3);
		platInfos[3][19][1] = new PLATINFO(false, new Vector2(3, 5), 3);
		gearPosistions[3][19] = new Vector2(4, 3);

		platInfos[3][21] = new PLATINFO[1];
		platInfos[3][21][0] = new PLATINFO(true, new Vector2(7, 9), 3);
		gearPosistions[3][21] = new Vector2(12, 10);

		platInfos[3][23] = new PLATINFO[1];
		platInfos[3][23][0] = new PLATINFO(false, new Vector2(9, 5), 3);
		//platInfos[3][23][1] = new PLATINFO(false, new Vector2(9,9),2);
		gearPosistions[3][23] = new Vector2(10f, 8);

		platInfos[3][25] = new PLATINFO[1];
		platInfos[3][25][0] = new PLATINFO(false, new Vector2(0, 9), 3);
		gearPosistions[3][25] = new Vector2(-3f, 10);

		platInfos[3][27] = new PLATINFO[1];
		platInfos[3][27][0] = new PLATINFO(false, new Vector2(2, 9), 5);
		gearPosistions[3][27] = new Vector2(5f, 7f);

		platInfos[3][29] = new PLATINFO[2];
		platInfos[3][29][0] = new PLATINFO(false, new Vector2(0, 9), 3);
		platInfos[3][29][1] = new PLATINFO(true, new Vector2(2, 12), 3);
		gearPosistions[3][29] = new Vector2(7, 10);

		platInfos[3][31] = new PLATINFO[2];
		platInfos[3][31][0] = new PLATINFO(false, new Vector2(0, 9), 3);
		platInfos[3][31][1] = new PLATINFO(true, new Vector2(2, 12), 3);
		gearPosistions[3][31] = new Vector2(7, 10);

		platInfos[3][33] = new PLATINFO[3];
		platInfos[3][33][0] = new PLATINFO(false, new Vector2(7, 9), 9);
		platInfos[3][33][1] = new PLATINFO(false, new Vector2(7, 5), 9);
		platInfos[3][33][2] = new PLATINFO(true, new Vector2(10, 9), 3);
		gearPosistions[3][33] = new Vector2(14, 7);

		platInfos[3][35] = new PLATINFO[1];
		platInfos[3][35][0] = new PLATINFO(true, new Vector2(9, 12), 3);
		gearPosistions[3][35] = new Vector2(14, 7);
    }

    public ResultadoObstaculo CreatInterTask(GearType type, float offset = 36.5f, bool last = false)
	{
		ResultadoObstaculo retorno = new ResultadoObstaculo();

		Vector4 tipos = ChoosePattern(1, 2, 2, 1);

		retorno.IdModuloPrincipal = (int)tipos.x;
		retorno.IdModuloSecundario1 = (int)tipos.y;
		retorno.IdModuloSecundario2 = (int)tipos.z;
		retorno.IdModuloVairacao = (int)tipos.w;

		cP.LoadResources();

		PlaceModule(tipos, offset);

//		Debug.Log(type);

		if (type != GearType.Nada)
			retorno.PosicaoParafuso = ChooseGear(type, offset, tipos, 0);

		if (last)
		{
			crianca = (GameObject)Instantiate(criancaPrefab);
			crianca.transform.position = new Vector3(module.transform.position.x + 30, 1.772518f, 3);
		}

		return retorno;
	}

	private void PlaceModule(Vector4 tipos, float offset = 36.5f)
	{
		System.Collections.Generic.List<PLATINFO> a = new System.Collections.Generic.List<PLATINFO>();

		foreach (PLATINFO platInfo in platInfos[0][(int)tipos.x])
		{
			a.Add(platInfo);
		}
		foreach (PLATINFO platInfo in platInfos[1][(int)tipos.y])
		{
			a.Add(platInfo);
		}
		foreach (PLATINFO platInfo in platInfos[2][(int)tipos.z])
		{
			a.Add(platInfo);
		}
		foreach (PLATINFO platInfo in platInfos[3][(int)tipos.w])
		{
			a.Add(platInfo);
		}

		module = cP.CreatModule(a, offset, 20, 20, false);

		trigger = GameObject.CreatePrimitive(PrimitiveType.Cube);

		trigger.transform.position = new Vector3(module.transform.position.x + 30, 6, 2);
		trigger.transform.localScale = new Vector3(1f, 12f, 40);
		trigger.GetComponent<Renderer>().enabled = false;
		trigger.GetComponent<Collider>().isTrigger = true;
		trigger.name = "Trigger";

        InterMiniGameTriggerFinish it = trigger.AddComponent<InterMiniGameTriggerFinish>();
	}

	private Vector4 ChoosePattern(int x1, int x2, int x3, int x4)
	{
		Vector4 tipos;

		int rand = Random.Range(0, NT1);
		tipos.x = rand;

		int rand1 = Random.Range(0, NT2);
		tipos.y = tipos.x * NT2 + rand1;

		int rand2 = Random.Range(0, NT3);
		tipos.z = tipos.x * NT3 + rand2;

		int rand3 = Random.Range(0, 2);
		tipos.w = rand * NT4 * NT3 * NT2 + rand1 * NT4 * NT3 + rand2 * NT4 + rand3;

		/*/Apagar depois

		tipos.x = x1;
		tipos.y = tipos.x*NT2 + x2;
		tipos.z = tipos.x*NT3 + x3;
		tipos.w = x1*NT4*NT3*NT2 + x2*NT4*NT3 + x3*NT4+x4;

		//fim do apagar//*/

		/*Debug.Log("Tipo 1:" + tipos.x);
		Debug.Log("Tipo 2:" + tipos.y);
		Debug.Log("Tipo 3:" + tipos.z);
		Debug.Log("Tipo 4:" + tipos.w);
		*/
		return tipos;
	}

	private int ChooseGear(GearType type, float offset, Vector4 tipos, int quadrante)
	{
		int rand = Random.Range(0, 4);
//		int n;

		//rand = quadrante; //Exlcuir linha

		Vector2 pos;
		int[] tiposAr = new int[4] { (int)tipos.x, (int)tipos.y, (int)tipos.z, (int)tipos.w };

		do
		{
			pos = gearPosistions[rand][tiposAr[rand]];
	//		n = rand;
			rand = Random.Range(0, 4);
		} while (pos == Vector2.zero);

		//Debug.Log("Gear Pos:" + n);
		gear = cP.CreatGear(type, offset - 1.5f + pos.x, pos.y);
		gear.GetComponent<GearScript>().type = type;

		//cP.CreatGear(Camera.main.transform.position.x+0.5f+pos.x,pos.y);
		return rand;
	}

	public void DestroyInterTask()
	{
		Destroy(module);
		Destroy(gear);
		Destroy(trigger);
		Destroy(crianca);
	}
}