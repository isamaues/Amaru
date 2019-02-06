using UnityEngine;

public class Final : MonoBehaviour
{
	private readonly string path = "final_0";
	private string final;
	private Material scene1image, scene2image;
	private bool firstScene = true;
	
	public UnityEngine.Sprite sprite1a, sprite1b,sprite1c,sprite2a,sprite2b,sprite2c;
	private UnityEngine.Sprite used1,used2;

	public float startTime;

	private void Start()
	{
		float coletados;
		float maximo;
		if (UserManager.CurrentProgress == null)
		{
			coletados = 60;
			maximo = 100;
		}
		else
		{
			coletados = UserManager.CurrentProgress.quantidade_Parafusos;
			maximo = GearsManager.maxColectable;
		}
		startTime = Time.time;

		float porcentagem = coletados / maximo;
		Debug.Log(coletados + " /  " + maximo + " = " + porcentagem);
		if (porcentagem > 0.66)
		{
			this.transform.FindChild("Scene1").transform.FindChild("Image1").GetComponent<UnityEngine.UI.Image>().sprite = sprite1c;
			this.transform.FindChild("Scene2").transform.FindChild("Image2").GetComponent<UnityEngine.UI.Image>().sprite = sprite2c;
		}
		else if (porcentagem > 0.33)
		{
			this.transform.FindChild("Scene1").transform.FindChild("Image1").GetComponent<UnityEngine.UI.Image>().sprite = sprite1b;
			this.transform.FindChild("Scene2").transform.FindChild("Image2").GetComponent<UnityEngine.UI.Image>().sprite = sprite2b;
		}
		else
		{
			this.transform.FindChild("Scene1").transform.FindChild("Image1").GetComponent<UnityEngine.UI.Image>().sprite = sprite1a;
			this.transform.FindChild("Scene2").transform.FindChild("Image2").GetComponent<UnityEngine.UI.Image>().sprite = sprite2a;
		}
	}

}