using System.ComponentModel;

namespace Amaru_Controle
{
	public class MiniGameId : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		private int _id;
		private string _nome;

		public int ID
		{
			get { return _id; }
			set
			{
				_id = value;
				if (PropertyChanged != null)
					PropertyChanged(this, new PropertyChangedEventArgs("ID"));
			}
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

		public MiniGameId()
		{
		}

		public MiniGameId(int id, string nome)
		{
			ID = id;
			Nome = nome;
		}

		public MiniGameId(MiniGameId miniGame)
		{
			ID = miniGame._id;
			Nome = miniGame._nome;
		}
	}
}