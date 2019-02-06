using System;
using System.Collections;

//[Serializable]
public class Task
{
	public int Id { get; set; }

	public DateTime DateTime { get; set; } // Mais um atributo de tempo que guarda data e hora no momento da resposta do jogador

	public int MiniGame { get; set; }

	public float Latency { get; set; }

	public double Difficulty { get; set; }

	public int CompareNumber { get; set; }

	public byte Correct { get; set; }

	public string TaskType { get; set; }

	public short Model { get; set; }

	public short TaskRole { get; set; }

	public ArrayList Choices = new ArrayList();

	public string Reforco { get; set; }

	public int MaxTentativas { get; set; }

	public Task()
	{
	}

	public string DebugInfo()
	{
		string incorrectWords = "";

		foreach (int ch in Choices)
		{
			incorrectWords += " " + ch;
		}

		string dif = String.Format("{0:0.000}", Difficulty);

		return "Id: " + Id + ";\n" +
				"MiniGame: " + MiniGame + ";\n" +
				"Latency: " + Latency + ";\n" +
				"Difficulty: " + dif + ";\n" +
				"CompareNumber: " + CompareNumber + ";\n" +
				"Correct: " + Correct + ";\n" +
				"TaskType: " + TaskType + ";\n" +
				"Model: " + Model + ";\n" +
				"Choices: {" + incorrectWords + "}";
	}
}