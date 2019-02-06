using System;

public class Resposta
{
	public string Usuario { get; set; }

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

	public Resposta(string usuario, string taskType, string escolhas, string respostaCerta, string respostaDada,
					int numeroTentativas, float latencia, bool resultado, string miniGame, int faseProcedimento, DateTime dateTime)
	{
		Usuario = usuario;
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

	public Resposta()
	{
	}
}