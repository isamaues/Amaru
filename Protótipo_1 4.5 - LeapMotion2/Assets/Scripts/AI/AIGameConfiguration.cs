using System.Collections.Generic;
using UnityEngine;

public class ConfiguracaoAI
{
	//Dados Gerais do Jogo
	public int numero_De_Tarefas_A_Serem_Geradas;//Colocar no txt

	#region Dados Referentes da Dificuldade das Tarefas

	//Dificuldade Leitura

	public bool utilizar_Algoritmo_de_Proximidade;

	public float peso_Alfa_Leitura;
	public float peso_Beta_Leitura;
	public float peso_Gama_Leitura;

	//Dificuldade Escrita

	public float peso_Alfa_Escrita;
	public float peso_Beta_Escrita;
	public float peso_Gama_Escrita;
	public float peso_Sigma_Escrita;

	//Numero de escolhas/comparacoes de uma tarefa
	public int numero_de_Escolhas_em_uma_Tarefa;

	#endregion Dados Referentes da Dificuldade das Tarefas

	//Dados Referentes a Máquina de Aprendizado
	public float alfa_Maquina_de_Aprendizado;

	public int numero_De_Interacoes_da_Maquina_de_Aprendizado;

	#region Pesos referentes ao algoritmo de proximidade

	public double Peso_Do_Alg_Prox_Da_Regra1;
	public double Peso_Do_Alg_Prox_Da_Regra2;
	public double Peso_Do_Alg_Prox_Da_Regra3;
	public double Peso_Do_Alg_Prox_Da_Regra4;
	public double Peso_Do_Alg_Prox_Da_Regra5;

	#endregion Pesos referentes ao algoritmo de proximidade

	#region Dados referentes a logica de primeira ordem

	public double Percentagem_Da_Taxa_De_Acerto;
	public double Percentagem_Da_Media_Geral;
	public double Percentagem_Da_Probabilidade_De_Acerto;

	#endregion Dados referentes a logica de primeira ordem

	//Atributos referentes a leitura do TXT

	private Dictionary<string, object> atributosTxt = new Dictionary<string, object>();

	// Use this for initialization
	public ConfiguracaoAI()
	{
	}

	public static ConfiguracaoAI PegarDadosXMLDefault()
	{
		string defaultAIGameConfigurationXMLPath = Paths.DATA_XMLs_FOLDER_PATH + "GeneratedAIConfiguration.xml";
		return XMLManager.LoadXML<ConfiguracaoAI>(defaultAIGameConfigurationXMLPath);
	}

	private void LoadAIGameConfDataFromXMLFile(string studentsLogin)
	{
		ConfiguracaoAI data;

		string studentsDyrectory = Paths.USER_PATH + studentsLogin;

		if (!System.IO.Directory.Exists(studentsDyrectory))
		{
			Debug.Log("Diretorio não existente");
			return;
		}

		data = XMLManager.LoadXML<ConfiguracaoAI>(studentsDyrectory, "Configuracoes.xml");

		numero_De_Tarefas_A_Serem_Geradas = data.numero_De_Tarefas_A_Serem_Geradas;

		utilizar_Algoritmo_de_Proximidade = data.utilizar_Algoritmo_de_Proximidade;

		peso_Alfa_Leitura = data.peso_Alfa_Leitura;
		peso_Beta_Leitura = data.peso_Beta_Leitura;
		peso_Gama_Leitura = data.peso_Gama_Leitura;

		peso_Alfa_Escrita = data.peso_Alfa_Escrita;
		peso_Beta_Escrita = data.peso_Beta_Escrita;
		peso_Gama_Escrita = data.peso_Gama_Escrita;
		peso_Sigma_Escrita = data.peso_Sigma_Escrita;

		numero_de_Escolhas_em_uma_Tarefa = data.numero_de_Escolhas_em_uma_Tarefa;

		alfa_Maquina_de_Aprendizado = data.alfa_Maquina_de_Aprendizado;
		numero_De_Interacoes_da_Maquina_de_Aprendizado = data.numero_De_Interacoes_da_Maquina_de_Aprendizado;

		Peso_Do_Alg_Prox_Da_Regra1 = data.Peso_Do_Alg_Prox_Da_Regra1;
		Peso_Do_Alg_Prox_Da_Regra2 = data.Peso_Do_Alg_Prox_Da_Regra2;
		Peso_Do_Alg_Prox_Da_Regra3 = data.Peso_Do_Alg_Prox_Da_Regra3;
		Peso_Do_Alg_Prox_Da_Regra4 = data.Peso_Do_Alg_Prox_Da_Regra4;
		Peso_Do_Alg_Prox_Da_Regra5 = data.Peso_Do_Alg_Prox_Da_Regra5;

		Percentagem_Da_Taxa_De_Acerto = data.Percentagem_Da_Taxa_De_Acerto;
		Percentagem_Da_Media_Geral = data.Percentagem_Da_Media_Geral;
		Percentagem_Da_Probabilidade_De_Acerto = data.Percentagem_Da_Probabilidade_De_Acerto;
	}
}