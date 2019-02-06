using System.Collections;
using System;
using UnityEngine;

public class TutorialScript : MonoBehaviour
{
	public GameObject[] CenasTutorial;
	private int currentLevel;

	private void Start() {
        if (TaskManagerInstance.Instance != null) {
            ITaskManager taskManager = TaskManagerInstance.Instance;
            if (taskManager is XMLTaskManager) {
                ((XMLTaskManager)taskManager).ResetXML();

                var Path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Amaru\\Dados\\Usuarios\\" + UserManager.CurrentUser.participante;
                System.IO.File.Delete(Path + "\\Respostas.xml");
                System.IO.File.Copy(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Amaru\\Dados\\PadroesXML\\PadraoRespostas.xml", Path + "\\Respostas.xml");
                System.IO.File.Delete(Path + "\\ResultadoObstaculos.xml");
                System.IO.File.Copy(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Amaru\\Dados\\PadroesXML\\PadraoResultadoObstaculos.xml", Path + "\\ResultadoObstaculos.xml");

                Debug.Log("XML Resetado");
            }
        }

        foreach (GameObject scene in CenasTutorial)
            scene.SetActive(false);

		currentLevel = 0;
		CenasTutorial[currentLevel].SetActive(true);
	}

	// Update is called once per frame
	private void Update()
	{
		if (Input.anyKeyDown)
		{
			if (currentLevel == CenasTutorial.Length - 1)
			{
				AutoFade.LoadLevel("new_level_001", 1, 1, Color.white);
			}
			else
			{
				CenasTutorial[currentLevel].SetActive(false);
				CenasTutorial[++currentLevel].SetActive(true);
			}
		}
	}
}