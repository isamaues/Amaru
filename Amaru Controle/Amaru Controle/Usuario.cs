using System.ComponentModel;

namespace Amaru_Controle
{
	public class Usuario : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		private string _nome;

		private string _avatar;

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

		public string Avatar
		{
			get { return _avatar; }
			set
			{
				_avatar = value;
				if (PropertyChanged != null)
					PropertyChanged(this, new PropertyChangedEventArgs("Avatar"));
			}
		}

		public string AvatarPath
		{
			get { return "Images/" + Avatar; }
		}

		public Usuario(string nome, string avatar)
		{
			_nome = nome;
			_avatar = avatar;
		}

		public Usuario()
		{
		}
	}
}