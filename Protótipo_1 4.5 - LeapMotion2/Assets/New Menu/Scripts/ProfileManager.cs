using System;
using System.IO;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileManager : MonoBehaviour
{
    public Image prefabPanel;
    public Text prefabText;
    public UnityEngine.UI.Button prefabButton;
    public Image maskPanel;
    public Image amaruAvatarPanel;
    public Image girlAvatarPanel;
	public Image profilePanelPrefab;

    private List<Image> avatarsList = new List<Image>();
    private int cont = 0;
    private Vector3 posInicial = new Vector3(0,0,0);
    private string profilesPath;
	
	private Dictionary<string,UnityEngine.Sprite> sprites;

	public UnityEngine.Sprite[] spriteLoad;

    void Start()
    {
		sprites = new Dictionary<string, UnityEngine.Sprite>();

		foreach(UnityEngine.Sprite sprite in spriteLoad)
		{
			sprites.Add(sprite.name,sprite);
		}


        //posInicial = maskPanel.rectTransform.localPosition;

        avatarsList.Add(amaruAvatarPanel);
        avatarsList.Add(girlAvatarPanel);

		/*string profilesPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Amaru\\";
        XmlDocument profileUsers = new XmlDocument();
        profileUsers.Load(profilesPath + "Dados/perfis.xml");
		XmlNodeList nodeUsers = profileUsers.DocumentElement.SelectNodes("Usuario");

//		Debug.Log(nodeUsers.Count);

        List<Profile> nameList = new List<Profile>();

        foreach (XmlNode node in nodeUsers)
        {
            string nome = node.SelectSingleNode("NomePerfil").InnerText;
            string tipo = node.SelectSingleNode("Tipo").InnerText;
			string avatar = node.SelectSingleNode("Avatar").InnerText;
            XmlNode xmlNode = node.LastChild;
            nameList.Add(new Profile(nome, tipo,avatar));
        }*/

		List<Profile> nameList = new List<Profile>();
		string profilePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Amaru\\Dados\\Usuarios"; 
		if (Directory.Exists(profilePath))
		{
			foreach (string directory in Directory.GetDirectories(profilePath))
			{
				//XmlDocument profile = new XmlDocument();
				Usuario u = XMLManager.LoadXML<Usuario> (directory, "Usuario.xml");
				nameList.Add(new Profile(u.participante,u.avatar));
			}
		}

        foreach (Profile name in nameList)
        {
            CreatePanel(name);
        }
    }

	public void ChangePlayer(string nome)
	{
		Debug.Log(nome);
		TaskManagerInstance.Instance.Name = nome;
		AutoFade.LoadLevel("SplashScreen", 0.5f, 0.5f, Color.white);
	}

    public void CreatePanel(Profile oneProfile)
    {

		Image Panel2 = Instantiate(profilePanelPrefab) as Image;
		Panel2.transform.name = oneProfile.name;
		Panel2.transform.SetParent(maskPanel.transform);
		Panel2.transform.localScale = new Vector3(1, 1, 1); //define a sua escala
		Transform panel2trans = Panel2.gameObject.transform;
		panel2trans.FindChild("Nome").GetComponent<Text>().text = oneProfile.name;
		panel2trans.FindChild("Imagem").GetComponent<Image>().sprite = sprites[oneProfile.avatar];
		var b = panel2trans.FindChild("Button").GetComponent<UnityEngine.UI.Button>();
		b.onClick.AddListener(() => ChangePlayer(oneProfile.name));

        //Inicia imagem de painel de cada perfil
       /* Image Panel2 = Instantiate(prefabPanel) as Image; //qual prefab será "instanciado"
		Panel2.name = oneProfile.name;
        Panel2.transform.SetParent(maskPanel.transform); //será filho do maskPanel
        Panel2.transform.localScale = new Vector3(1, 1, 1); //define a sua escala
        Panel2.rectTransform.sizeDelta = new Vector2(170, 170); //define o tamanho da imagem
        Panel2.rectTransform.anchorMax = Panel2.rectTransform.anchorMin = new Vector2(0, 1); //define a posição das anchors do Panel2

        //Inicia o texto que exibirá o nome do usuário
        Text namePlayer = Instantiate(prefabText) as Text; //qual prefab será "instanciado"
        namePlayer.text = oneProfile.name; //recebe o nome do perfil que será criado
        namePlayer.fontStyle = FontStyle.Bold;
        namePlayer.transform.SetParent(Panel2.transform); //será filho de Panel2
        namePlayer.rectTransform.localScale = new Vector2(1, 1);
        namePlayer.rectTransform.localPosition = new Vector3( 0,-25,0 );

        //Inicia o texto que exibirá o tipo do usuário
        Text typePlayer = Instantiate(prefabText) as Text; //qual prefab será "instanciado"
        typePlayer.text = "Tipo: " + oneProfile.tipo; //recebe o texto que exibirá o tipo do usuário
        typePlayer.fontStyle = FontStyle.Bold;
        typePlayer.transform.SetParent(Panel2.transform); //será filho de Panel2
        typePlayer.rectTransform.localScale = new Vector2(1, 1);
        typePlayer.rectTransform.localPosition = new Vector3(0, -40, 0);

        //Inicia o botão de escolha do usuário
        UnityEngine.UI.Button startGame = Instantiate(prefabButton) as UnityEngine.UI.Button; //qual prefab será "instanciado"
        startGame.transform.SetParent(Panel2.transform); //será filho de Panel2
        startGame.image.rectTransform.localScale = new Vector2(1, 1); //define o tamanho do botão em relação Panel2
        startGame.image.rectTransform.localPosition = new Vector3(0, -60, 0);//define a pos do botão em relação ao pivot do Panel2
        startGame.onClick.AddListener(delegate { StartGame(oneProfile.name); }); //Define uma função será executada quando o botão for ativado

        Image avatarImg = Instantiate( avatarsList[ 0 ] ) as Image;
        avatarImg.transform.SetParent(Panel2.transform);
        avatarImg.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f); //define a sua escala
        avatarImg.rectTransform.sizeDelta = new Vector2(0.2f, 0.2f); //define o tamanho da imagem
        avatarImg.rectTransform.localPosition = new Vector3(0, 35, 0);*/

		if (cont == 4)
		{
			posInicial.y -= 190;
			cont = 0;
		}

        if (cont != 4) //Se ainda não foram criados 4 perfis em uma linha
        {
			Panel2.rectTransform.localPosition = new Vector2(20 + 190 * cont, posInicial.y - 20);
            /*Posiciona o perfil que será criado ao lado do perfil anterior 
             * de acordo com a quantidade de perfis já existentes na linha. 
             * Por exemplo: se já existirem 2 perfis (definido pela variável cont),
             * o novo perfil ficará na terceira posição*/
        }
        
        if (posInicial.y <= -400)
            maskPanel.rectTransform.sizeDelta = new Vector2( 0, maskPanel.rectTransform.sizeDelta.y + 190 );

        cont++;
    }

    void StartGame(string player)
    {
        TaskManagerInstance.Instance = new XMLTaskManager(player);
        
        if (TaskManagerInstance.Instance.GetCurrentTaskId() == 0)
            AutoFade.LoadLevel("Openning", 0, 0, Color.white);
        else
            AutoFade.LoadLevel("new_level_001", 0, 0, Color.white);
    }
    
}
