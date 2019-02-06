using System.ComponentModel;

namespace Amaru_Controle
{
	public class TaskType : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		private string _nome;
		private string _detalhes;

		public TaskType(TaskType taskType)
		{
			_nome = taskType.Nome;
			_detalhes = taskType.Detalhes;
		}

		public string Nome
		{
			get { return _nome; }
			set
			{
				_nome = value;
				if (PropertyChanged != null)
					PropertyChanged(this, new PropertyChangedEventArgs("Nome"));
			}
		}

		public string Detalhes
		{
			get { return _detalhes; }
			set
			{
				_detalhes = value;
				if (PropertyChanged != null)
					PropertyChanged(this, new PropertyChangedEventArgs("Detalhes"));
			}
		}

		public TaskType(string nome, string detalhes)
		{
			_nome = nome;
			_detalhes = detalhes;
		}
	}
}