using System;
using System.Collections.Generic;
using System.Xml;

namespace Amaru_Controle
{
	public static class Helper
	{
		public static readonly Dictionary<int, string> MiniGameDictionary = new Dictionary<int, string>()
		{
			{0, "Aleatorio"},
			{1, "Arco e Flecha"},
			{2, "Cubo"},
			{3, "Plataforma"}
		};

		public static readonly Dictionary<string, TaskType> TipoTarefaDictionary = new Dictionary<string, TaskType>()
		{
			{"AB", new TaskType("AB","1")},
			{"AC", new TaskType("AC","2")},
			{"AD", new TaskType("AD","3")},
			{"AE", new TaskType("AE","4")},
			{"BB", new TaskType("BB","5")},
			{"BC", new TaskType("BC","6")},
			{"BD", new TaskType("BD","7")},
			{"BE", new TaskType("BE","8")},
			{"CB", new TaskType("CB","9")},
			{"CC", new TaskType("CC","10")},
			{"CD", new TaskType("CD","11")},
			{"CE", new TaskType("CE","12")},
			{"FB", new TaskType("FB","13")},
			{"FC", new TaskType("FC","14")},
			{"FD", new TaskType("FD","15")},
			{"GB", new TaskType("GB","16")},
			{"GC", new TaskType("GC","17")},
			{"GD", new TaskType("GD","18")}
		};

		public static List<string> AvatarList = new List<string>()
		{
			"amaru", "garota_1", "garota_2", "garoto_1", "garoto_2", "urama"
		};

		public static Dictionary<int, Word> PalavrasDictionary = new Dictionary<int, Word>();

		public static readonly string Path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Amaru\\";
		public static string DataPath = Path + "\\Dados\\";

		public static int LoadPalavras(string usuario)
		{
			PalavrasDictionary = new Dictionary<int, Word>();
			PalavrasDictionary.Add(0, new Word("Nenhuma", 0, ""));
			var palavrasXmlDocument = new XmlDocument();
			palavrasXmlDocument.Load(Helper.DataPath + "\\Usuarios\\" + usuario + "\\Palavras.xml");
			var palavrasList = palavrasXmlDocument.DocumentElement.SelectSingleNode("arrayList").SelectNodes("Word");
            int i = 0;
			foreach (XmlNode palavraNode in palavrasList)
			{
				//	list.Add(new Word());
				//	MessageBox.Show(palavraNode.InnerText);
				var id = int.Parse(palavraNode.SelectSingleNode("WordId").InnerText);
				PalavrasDictionary.Add(id, new Word(palavraNode.SelectSingleNode("Name").InnerText, id, palavraNode.SelectSingleNode("Resource").InnerText));
                i++;
            }
            return i;
		}
	}
}