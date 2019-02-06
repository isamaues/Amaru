using System;

public class TaskResponseModel
{
	public string TaskType { get; set; }

	public string Escolhas { get; set; }

	public string RespostaCerta { get; set; }

	public string RespostaDada { get; set; }

	public int NumeroTentativa { get; set; }

	public float Latencia { get; set; }

	public bool Resultado { get; set; }

	public string MiniGame { get; set; }

	public int FaseProcedimento { get; set; }

	public DateTime DateTime { get; set; }

	public TaskResponseModel(string taskType, string escolhas, string respostaCerta, string respostaDada,
					int numeroTentativas, float latencia, bool resultado, string miniGame, int faseProcedimento, DateTime dateTime)
	{
		TaskType = taskType;
		Escolhas = escolhas;
		RespostaCerta = respostaCerta;
		RespostaDada = respostaDada;
		NumeroTentativa = numeroTentativas;
		Latencia = latencia;
		Resultado = resultado;
		MiniGame = miniGame;
		FaseProcedimento = faseProcedimento;
		DateTime = dateTime;
	}

	public TaskResponseModel()
	{
	}
}