using System.ComponentModel;

namespace Amaru_Controle
{
	public class Word : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		private int _id;
		private string _name;
		private string _imagem;

		public int Id
		{
			get { return _id; }
			set
			{
				_id = value;
				if (PropertyChanged != null)
					PropertyChanged(this, new PropertyChangedEventArgs("Id"));
			}
		}

		public string Name
		{
			get { return _name; }
			set
			{
				_name = value;
				if (PropertyChanged != null)
					PropertyChanged(this, new PropertyChangedEventArgs("Name"));
			}
		}

		public string Imagem
		{
			get { return _imagem; }
			set
			{
				_imagem = value;
				if (PropertyChanged != null)
					PropertyChanged(this, new PropertyChangedEventArgs("Imagem"));
			}
		}

		public Word(Word w)
		{
			_name = w._name;
			_id = w._id;
			_imagem = w._imagem;
		}

		public Word()
		{
		}

		public Word(string nome, int id, string imagem)
        {
            Name = nome;
			Id = id;
			Imagem = imagem;
		}
	}
}