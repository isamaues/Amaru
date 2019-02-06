using Microsoft.Win32;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Xml;

namespace Amaru_Controle
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		private void OnLoadingRow(object sender, DataGridRowEventArgs e)
		{
			e.Row.Header = (e.Row.GetIndex() + 1).ToString();
		}

		private void ItemContainerGenerator_ItemsChanged(object sender, ItemsChangedEventArgs e)
		{
			//get all DataGridRow elements in the visual tree
			IEnumerable<DataGridRow> rows = FindVisualChildren<DataGridRow>(TaskDataGrid);
			foreach (DataGridRow row in rows)
			{
				row.Header = (row.GetIndex() + 1).ToString();
			}
		}

		private static IEnumerable<T> FindVisualChildren<T>(DependencyObject dependencyObject)
			where T : DependencyObject
		{
			if (dependencyObject != null)
			{
				for (int i = 0; i < VisualTreeHelper.GetChildrenCount(dependencyObject); i++)
				{
					DependencyObject child = VisualTreeHelper.GetChild(dependencyObject, i);
					if (child != null && child is T)
					{
						yield return (T)child;
					}

					foreach (T childOfChild in FindVisualChildren<T>(child))
					{
						yield return childOfChild;
					}
				}
			}
		}

		public ObservableCollection<string> AvatarList { get; set; }

		public ObservableCollection<MiniGameId> MiniGames { get; set; }

		public ObservableCollection<Word> Palavras { get; set; }

		public ObservableCollection<Task> Tarefas
		{
			get
			{
				return _tarefas;
			}
			set
			{
				_tarefas = value;
				if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("Tarefas"));
			}
		}

		public ObservableCollection<TaskType> TiposTarefas { get; set; }

		private ObservableCollection<Task> _tarefas;

		private ObservableCollection<string> _usuariosCollection;

		public ObservableCollection<string> UsuariosCollection
		{
			get { return _usuariosCollection; }
			set
			{
				_usuariosCollection = value;
				if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("UsuariosCollection"));
			}
		}

		public bool CanMoveRow { get; set; }

		public MainWindow()
		{
			CanMoveRow = true;

			AvatarList = new ObservableCollection<string>(Helper.AvatarList);

			MiniGames = new ObservableCollection<MiniGameId>();
			foreach (var par in Helper.MiniGameDictionary)
			{
				MiniGames.Add(new MiniGameId(par.Key, par.Value));
			}

			UsuariosCollection = new ObservableCollection<string>(LoadProfilesXml().OrderBy(s => s.ToString()));

			Helper.LoadPalavras(UsuariosCollection[0]);
			Palavras = new ObservableCollection<Word>();
			foreach (var pal in Helper.PalavrasDictionary)
			{
				Palavras.Add(pal.Value);
			}

			TiposTarefas = new ObservableCollection<TaskType>();
			foreach (var par in Helper.TipoTarefaDictionary)
			{
				TiposTarefas.Add(par.Value);
			}

			ResetTarefasList(UsuariosCollection[0]);

			InitializeComponent();

			//NomeUsuario.Text = CurrentUser;
			TaskDataGrid.ItemContainerGenerator.ItemsChanged += ItemContainerGenerator_ItemsChanged;
		}

		private string _currentUser;

		public string CurrentUser
		{
			get
			{
				return _currentUser;
			}
			set
			{
				_currentUser = value;
				if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("CurrentUser"));
			}
		}

		private void ResetTarefasList(string usuario)
		{
			Tarefas = new ObservableCollection<Task>(LoadTarefasXml("\\Usuarios\\" + usuario + "\\Tarefas.xml").OrderBy(s => s.Id));

			UsuariosCollection = new ObservableCollection<string>(LoadProfilesXml().OrderBy(s => s.ToString()));

			CurrentUser = usuario;

			Helper.LoadPalavras(usuario);

			UsuarioAtual = LoadUsuario(usuario);
		}

		private Usuario LoadUsuario(string usuario)
		{
			Usuario novo = new Usuario();

			var usuarioXmlDocument = new XmlDocument();
			usuarioXmlDocument.Load(Helper.DataPath + "//Usuarios//" + usuario + "//Usuario.xml");

			var usuarioNode = usuarioXmlDocument.SelectSingleNode("Usuario");

			novo.Nome = usuarioNode.SelectSingleNode("participante").InnerText;
			novo.Avatar = usuarioNode.SelectSingleNode("avatar").InnerText;

			return novo;
		}

		private Usuario _usuarioAtual;

		public Usuario UsuarioAtual
		{
			get
			{
				return _usuarioAtual;
			}
			set
			{
				_usuarioAtual = value;
				if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs("UsuarioAtual"));
			}
		}

		private ObservableCollection<Task> LoadTarefasXml(string path)
		{
			var tarefas = new ObservableCollection<Task>();

			var tarefasXmlDocument = new XmlDocument();
			tarefasXmlDocument.Load(Helper.DataPath + path);

			if (tarefasXmlDocument.DocumentElement != null)
			{
				//MessageBox.Show("Nao nulo\n");
				var tarefasList = tarefasXmlDocument.DocumentElement.SelectSingleNode("arrayList").SelectNodes("Task");

				if (tarefasList != null)
				{
					//MessageBox.Show("Nao nulo 2\n");
					foreach (XmlNode tarefaNode in tarefasList)
					{
						//	MessageBox.Show(tarefaNode.InnerText);
						var choices = new List<Word>();
						var xmlNodeList = tarefaNode.SelectSingleNode("Choices").SelectNodes("anyType");

						foreach (XmlNode choiceNode in xmlNodeList)
						{
							//MessageBox.Show(choiceNode.InnerText);
							choices.Add(new Word(Helper.PalavrasDictionary[int.Parse(choiceNode.InnerText)]));
						}

						if (choices.Count < 4)
						{
							for (int u = choices.Count; u < 4; u++)
								choices.Add(new Word(Helper.PalavrasDictionary[0]));
						}

						var t = new Task()
						{
							Choices = choices,
							Correct = new Word(Helper.PalavrasDictionary[int.Parse(tarefaNode.SelectSingleNode("Correct").InnerText)]),
							Id = int.Parse(tarefaNode.SelectSingleNode("Id").InnerText),
							//MiniGame = int.Parse(tarefaNode.SelectSingleNode("MiniGame").InnerText),
							MiniGame = new MiniGameId()
								{
									ID = int.Parse(tarefaNode.SelectSingleNode("MiniGame").InnerText),
									Nome = Helper.MiniGameDictionary[int.Parse(tarefaNode.SelectSingleNode("MiniGame").InnerText)]
								},
							Model = new Word(Helper.PalavrasDictionary[int.Parse(tarefaNode.SelectSingleNode("Model").InnerText)]),
							Reforco = tarefaNode.SelectSingleNode("Reforco").InnerText,
							TaskRole = short.Parse(tarefaNode.SelectSingleNode("TaskRole").InnerText),
							TaskType = new TaskType(tarefaNode.SelectSingleNode("TaskType").InnerText, "")
						};
						//	MessageBox.Show(t.DebugInfo());
						tarefas.Add(t);
					}
				}
			}

			return tarefas;
		}

		private ObservableCollection<string> LoadProfilesXml()
		{
			var list = new ObservableCollection<string>();
			var path = Helper.DataPath + "\\Usuarios\\";
			var length = path.Length;
			if (Directory.Exists(path))
			{
				foreach (string directory in Directory.GetDirectories(path))
				{
					string name = directory.Substring(length);

					list.Add(name);
				}
			}
			CurrentUser = list[0];
			return list;
		}

		private void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			SalvarTarefaXml(CurrentUser);
		}

		private void SalvarTarefaXml(string usuario)
		{
			XmlTextWriter writer = new XmlTextWriter(Helper.DataPath + "\\Usuarios\\" + usuario + "\\Tarefas.xml", Encoding.UTF8);

			var tar = TaskDataGrid.Items;

			writer.WriteRaw("<?xml version=\"1.0\" encoding=\"utf-8\"?><!DOCTYPE TasksList []><TasksList xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">");
			//writer.WriteString();
			writer.WriteStartElement("arrayList");
			int i = 1;
			foreach (Task tarefa in Tarefas)
			{
				//	MessageBox.Show(tarefa.DebugInfo());
				writer.WriteStartElement("Task");
				writer.WriteStartElement("Choices");

				foreach (Word word in tarefa.Choices)
				{
					if (word.Id == 0) continue;
					writer.WriteRaw("<anyType xsi:type=\"xsd:int\">" + word.Id.ToString() + "</anyType>");
				}

				writer.WriteEndElement();

				writer.WriteElementString("Id", i++.ToString());
				writer.WriteElementString("MiniGame", tarefa.MiniGame.ID.ToString());
				writer.WriteElementString("Latency", "0");
				writer.WriteElementString("Difficulty", "10");
				writer.WriteElementString("CompareNumber", "0");
				writer.WriteElementString("Correct", tarefa.Correct.Id.ToString());
				writer.WriteElementString("TaskType", tarefa.TaskType.Nome);
				writer.WriteElementString("TaskRole", tarefa.TaskRole.ToString());
				writer.WriteElementString("Model", tarefa.Model.Id.ToString());
				writer.WriteElementString("Reforco", tarefa.Reforco);
				writer.WriteEndElement();
			}
			writer.WriteEndElement();
			writer.WriteRaw("</TasksList>");
			writer.Close();
		}

		private void NovaTarefa_ButtonClick1(object sender, RoutedEventArgs e)
		{
			Tarefas.Add(new Task()
			{
				Choices = new List<Word>{ new Word(Helper.PalavrasDictionary[0]), new Word(Helper.PalavrasDictionary[0])
				, new Word(Helper.PalavrasDictionary[0]), new Word(Helper.PalavrasDictionary[0]) },
				MiniGame = new MiniGameId(0, Helper.MiniGameDictionary[0]),
				Reforco = "",
				TaskRole = 0,
				Model = new Word(Helper.PalavrasDictionary[1]),
				TaskType = new TaskType(Helper.TipoTarefaDictionary["AB"])
			});
		}

		private void DataGridComboBox_OnGotFocus(object sender, RoutedEventArgs e)
		{
			//	TaskDataGrid.SetValue(VisualHelper.EnableRowsMoveProperty, false);
		}

		private void DataGridComboBox_OnLostFocus(object sender, RoutedEventArgs e)
		{
			//	TaskDataGrid.SetValue(VisualHelper.EnableRowsMoveProperty, true);
		}

		private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
		{
			for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
				if (vis is DataGridRow)
				{
					var row = (DataGridRow)vis;
					Tarefas.RemoveAt(row.GetIndex());
				}
			DeletarButton.Content = "Deletar Tarefa";
			DeletarColum.Visibility = Visibility.Hidden;
		}

		private void DeletarHabilitar_OnClick(object sender, RoutedEventArgs e)
		{
			if (DeletarColum.Visibility == Visibility.Visible)
			{
				DeletarColum.Visibility = Visibility.Hidden;
				(sender as Button).Content = "Deletar Tarefa";
			}
			else
			{
				DeletarColum.Visibility = Visibility.Visible;
				(sender as Button).Content = "Cancelar Deletar";
			}
			//DeletarColum.Visibility = Visibility.Visible;
		}

		private void CheckBox_Checked_1(object sender, RoutedEventArgs e)
		{
			SetEnableRowsMove(true);
		}

		private void CheckBox_Unchecked_1(object sender, RoutedEventArgs e)
		{
			SetEnableRowsMove(false);
		}

		private void SetEnableRowsMove(bool set)
		{
			CanMoveRow = set;
			TaskDataGrid.SetValue(VisualHelper.EnableRowsMoveProperty, !CanMoveRow);
		}

		private void TrocarUsuarioButton_Click(object sender, RoutedEventArgs e)
		{
			if (UsuariosListBox.SelectedIndex >= 0)
			{
				if (MessageBox.Show("Deseja salvar antes de abrir o novo?", "Salvar", MessageBoxButton.YesNo) ==
					MessageBoxResult.Yes)
				{
					SalvarTarefaXml(CurrentUser);
				}

				ResetTarefasList(UsuariosListBox.SelectedItem as string);
			}
		}

		private void CarregarCsv_Click_1(object sender, RoutedEventArgs e)
		{
			OpenCsvFile();
		}

		private void OpenCsvFile()
		{
			var openCsvDialog = new OpenFileDialog();
			openCsvDialog.Filter = "Csv (*.csv)|*.csv";
			if (openCsvDialog.ShowDialog() == true)
			{
				ConvertCsvToXml(openCsvDialog.FileName);
			}
		}

		private void ConvertCsvToXml(string path)
		{
			var newList = new ObservableCollection<Task>();
			string[] allLines = File.ReadAllLines(path);
			int i = -1;
			foreach (var line in allLines)
			{
				if (++i == 0) continue;

				Task newTask = new Task()
				{
					MiniGame = new MiniGameId(0, Helper.MiniGameDictionary[0]),
					Id = i,
					Correct = new Word(Helper.PalavrasDictionary[0]),
				};

				string[] a = line.Split(';');

				if (a[0] == "treino")
				{
					newTask.Reforco = "AI";
					newTask.TaskRole = 1;
				}
				else
				{
					newTask.Reforco = "";
					newTask.TaskRole = 0;
				}

				string tipo = a[1];

				newTask.TaskType = new TaskType(tipo, "");

				string modelo = a[2].Replace("-", "");
				newTask.Model = Helper.PalavrasDictionary.FirstOrDefault(x => x.Value.Name == modelo).Value;

				var choices = new List<Word>();

				for (int j = 3; j < 16; j++)
				{
                    string text = a[j].Replace("-", "");
					if (text.Equals("")) break;

					choices.Add(Helper.PalavrasDictionary.FirstOrDefault(x => x.Value.Name == text).Value);
				}
				if (choices.Count < 4)
				{
					for (int u = choices.Count; u < 4; u++)
						choices.Add(new Word(Helper.PalavrasDictionary[0]));
				}

				newTask.Choices = choices;
				newList.Add(newTask);
			}

			Tarefas = newList;
		}

		private void SalvarUsuarioButton_Click(object sender, RoutedEventArgs e)
		{
			SalvarUsuarioXml();
		}

		private void SalvarUsuarioXml()
		{
			var oldPath = Helper.DataPath + "\\Usuarios\\" + CurrentUser;
			var newPath = Helper.DataPath + "\\Usuarios\\" + UsuarioAtual.Nome;

			if (CurrentUser != UsuarioAtual.Nome)
			{
				if (Directory.Exists(newPath))
				{
					MessageBox.Show("Esse nome de usuario ja está cadastrado, tente um nome unico.");
					return;
				}
			}

			XmlTextWriter writer = new XmlTextWriter(Helper.DataPath + "\\Usuarios\\" + CurrentUser + "\\Usuario.xml", Encoding.UTF8);
			writer.WriteRaw("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n<Usuario xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">");
			writer.WriteElementString("participante", UsuarioAtual.Nome);
			writer.WriteElementString("responsavel", "");
			writer.WriteElementString("avatar", UsuarioAtual.Avatar);
			writer.WriteElementString("login", "");
			writer.WriteElementString("senha", "");
			writer.WriteRaw("</Usuario>");
			writer.Close();

			if (CurrentUser != UsuarioAtual.Nome)
			{
				if (Directory.Exists(newPath))
				{
					MessageBox.Show("Esse nome de usuario ja está cadastrado, tente um nome unico.");
					return;
				}
				Directory.CreateDirectory(newPath);

				foreach (string dirPath in Directory.GetDirectories(oldPath, "*",
				SearchOption.AllDirectories))
				{
					Directory.CreateDirectory(dirPath.Replace(oldPath, newPath));
				}

				//Copy all the files & Replaces any files with the same name
				foreach (string filePath in Directory.GetFiles(oldPath, "*.*",
					SearchOption.AllDirectories))
					File.Copy(filePath, filePath.Replace(oldPath, newPath), true);

				Directory.Delete(oldPath, true);
			}

			ResetTarefasList(UsuarioAtual.Nome);
		}

		private void CopiarUsuarioButton_OnClick(object sender, RoutedEventArgs e)
		{
			SalvarUsuarioXml();

			int i = 1;

			var oldPath = Helper.DataPath + "\\Usuarios\\" + CurrentUser;
			var newPath = Helper.DataPath + "\\Usuarios\\" + CurrentUser + "-Copia" + i;

			for (i = 1; Directory.Exists(newPath); i++)
			{
				newPath = Helper.DataPath + "\\Usuarios\\" + CurrentUser + "-Copia" + i;
			}

			Directory.CreateDirectory(newPath);

			foreach (string dirPath in Directory.GetDirectories(oldPath, "*",
			SearchOption.AllDirectories))
			{
				Directory.CreateDirectory(dirPath.Replace(oldPath, newPath));
			}

			//Copy all the files & Replaces any files with the same name
			foreach (string filePath in Directory.GetFiles(oldPath, "*.*",
				SearchOption.AllDirectories))
				File.Copy(filePath, filePath.Replace(oldPath, newPath), true);

			//	Directory.Delete(oldPath, true);

			ResetTarefasList(UsuarioAtual.Nome);

			XmlTextWriter writer = new XmlTextWriter(Helper.DataPath + "\\Usuarios\\" + UsuarioAtual.Nome + "-Copia" + i + "\\Usuario.xml", Encoding.UTF8);
			writer.WriteRaw("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n<Usuario xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">");
			writer.WriteElementString("participante", UsuarioAtual.Nome + "-Copia" + i);
			writer.WriteElementString("responsavel", "");
			writer.WriteElementString("avatar", UsuarioAtual.Avatar);
			writer.WriteElementString("login", "");
			writer.WriteElementString("senha", "");
			writer.WriteRaw("</Usuario>");
			writer.Close();

			ResetTarefasList(UsuarioAtual.Nome);

			//	if (Directory.Exists())
		}

        private void TaskDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
	}
}