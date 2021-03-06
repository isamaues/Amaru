using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

//REVISION 126
public class WordManager : MonoBehaviour
{
	public static int WORD_NUMBER { get; set; }

	private static ArrayList wordList = new ArrayList();
	private Dictionary<string, int> wordListDict = new Dictionary<string, int>();
	private static char separation = '/';

	public WordManager()
	{
		foreach (Word w in UserManager.CurrentWordList.arrayList)
		{
			wordListDict.Add(w.Name, w.WordId);
			wordList.Add(w);
		}
	}

	public ArrayList GetWords()
	{
		return wordList;
	}

	public string GetWord(int id)
	{
		foreach (string item in wordListDict.Keys)
		{
			if (wordListDict[item] == id) return item;
		}
		return "ERRO";
	}

	#region Códigos de Leitura e Escrita

	public void displayWords()
	{
		foreach (Word p in wordList)
		{
			Debug.Log(p.WordId + " " + p.Name + " [");

			foreach (string s in p.Syllables)
			{
				Debug.Log(" " + s + " ");
			}

			Debug.Log("] " + p.SyllablesNumber + " " + p.MaxReadDif + " " + p.LearningDegreeRead
							   + " " + p.LearnedRead + " " + p.MaxWriteDif + " " + p.LearningDegreeWrite
							   + " " + p.LearnedWrite);
		}

		//Console.ReadKey();
	}

	public void WriteWord(string path)
	{
		StreamWriter sw = null;
		string aux = separation.ToString();
		try
		{
			sw = new StreamWriter(File.Create(path));

			foreach (Word i in wordList)
			{
				sw.Write(i.WordId + aux + i.Name + aux);

				for (int s = 0; s < i.Syllables.Count; s++)
				{
					if (s == i.Syllables.Count - 1)
						sw.Write(i.Syllables[s]);
					else
						sw.Write(i.Syllables[s] + "-");
				}

				sw.Write(aux + i.SyllablesNumber + aux + i.MaxReadDif + aux + i.LearningDegreeRead
						   + aux + i.LearnedRead + aux + i.MaxWriteDif + aux + i.LearningDegreeWrite
						   + aux + i.LearnedWrite);

				if (i.WordId != wordList.Count)
					sw.WriteLine();
			}
		}
		catch
		{
			Debug.Log("Erro no WriteWord!");
		}
		finally
		{
			if (sw != null)
				sw.Close();
		}
	}

	#endregion Códigos de Leitura e Escrita

	#region Códigos Selects

	public ArrayList SelectWords()
	{
		return wordList;
	}

	public ArrayList SelectWords(string nome)
	{
		ArrayList wordListSelecionadas = new ArrayList();

		foreach (Word i in wordList)
		{
			if (i.Name.Equals(nome))
			{
				wordListSelecionadas.Add(i);
			}
		}

		return wordListSelecionadas;
	}

	public ArrayList SelectWords(int id)
	{
		ArrayList wordListSelecionadas = new ArrayList();

		foreach (Word i in wordList)
		{
			if (i.WordId == id)
			{
				wordListSelecionadas.Add(i);
			}
		}

		return wordListSelecionadas;
	}

	#endregion Códigos Selects
}