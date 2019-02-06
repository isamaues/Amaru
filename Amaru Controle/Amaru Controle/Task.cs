using System.Collections.Generic;
using System.ComponentModel;

namespace Amaru_Controle
{
	public class Task : INotifyPropertyChanged
	{
		private int _id;
		private MiniGameId _miniGameId;
		private Word _correct;
		private TaskType _taskType;

		public event PropertyChangedEventHandler PropertyChanged;

		public int Id
		{
			get { return _id; }
			set
			{
				_id = value;
				if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Id"));
			}
		}

		public MiniGameId MiniGame
		{
			get { return _miniGameId; }
			set
			{
				_miniGameId = value;
				if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("MiniGame"));
			}
		}

		public Word Correct
		{
			get { return _correct; }
			set
			{
				_correct = value;
				if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Correct"));
			}
		}

		public TaskType TaskType
		{
			get { return _taskType; }
			set
			{
				_taskType = value;
				if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("TaskType"));
			}
		}

		private Word _model;

		public Word Model
		{
			get { return _model; }
			set
			{
				_model = value;
				if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Model"));
			}
		}

		private short _taskRole;

		public short TaskRole
		{
			get { return _taskRole; }
			set
			{
				_taskRole = value;
				if (PropertyChanged != null)
				{
					PropertyChanged(this, new PropertyChangedEventArgs("TaskRole"));
					PropertyChanged(this, new PropertyChangedEventArgs("TaskRoleBool"));
				}
			}
		}

		public bool TaskRoleBool
		{
			get { return TaskRole == 0; }
			set { TaskRole = value ? (short)0 : (short)1; }
		}

		private string _reforco;

		public string Reforco
		{
			get { return _reforco; }
			set
			{
				_reforco = value;
				if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Reforco"));
			}
		}

		public bool ReforcoAudio
		{
			get { return Reforco.Contains("A"); }
			set
			{
				string a = value ? "A" : "";
				Reforco = Reforco.Contains("I") ? a + "I" : a;
			}
		}

		public bool ReforcoImagem
		{
			get { return Reforco.Contains("I"); }
			set
			{
				string i = value ? "I" : "";
				Reforco = Reforco.Contains("A") ? "A" + i : i;
			}
		}

		private List<Word> _choices = new List<Word>();

		public List<Word> Choices
		{
			get { return _choices; }
			set
			{
				_choices = value;
				if (PropertyChanged != null)
				{
					PropertyChanged(this, new PropertyChangedEventArgs("Choices"));
					PropertyChanged(this, new PropertyChangedEventArgs("ChoicesString"));
				}
			}
		}

		public string ChoicesString
		{
			get
			{
				string a = " ";
				foreach (Word word in Choices)
				{
					a += word.Name + "\n";
				}
				return a;
			}
		}

		public Task()
		{
		}

		public string DebugInfo()
		{
			var incorrectWords = "";

			foreach (Word ch in Choices)
			{
				incorrectWords += " " + ch.Name;
			}

			return "Id: " + Id + ";\n" +
				   "MiniGame: " + MiniGame + ";\n" +
				   "Correct: " + Correct + ";\n" +
				   "TaskType: " + TaskType + ";\n" +
				   "Model: " + Model + ";\n" +
				   "Choices: {" + incorrectWords + "}";
		}
	}
}