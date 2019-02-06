public class UserGameProgress
{
	public int id_Da_Ultima_Tarefa;
	public int quantidade_Parafusos;
	public int mundo_atual;

	public UserGameProgress()
	{
	}

	public static UserGameProgress PegarDadosXMLDefault()
	{
		string defaultUserGameProgressXMLPath = Paths.DATA_XMLs_FOLDER_PATH + "GeneratedUserGameProgress.xml";
		return XMLManager.LoadXML<UserGameProgress>(defaultUserGameProgressXMLPath);
	}
}