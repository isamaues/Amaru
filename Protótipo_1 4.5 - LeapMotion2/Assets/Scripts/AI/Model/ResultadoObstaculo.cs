public class ResultadoObstaculo
{
	public string Usuario { get; set; }

	public int IdModuloPrincipal { get; set; }

	public int IdModuloSecundario1 { get; set; }

	public int IdModuloSecundario2 { get; set; }

	public int IdModuloVairacao { get; set; }

	public int PosicaoParafuso { get; set; }

	public float Latencia { get; set; }

	public bool PegouItem { get; set; }

	public int Mundo { get; set; }

	public ResultadoObstaculo(string usuario, int idModulo1, int idModulo2, int idModulo3, int idModulo4, int posicaoParafuso, float latencia, bool pegouItem, int mundo)
	{
		Usuario = usuario;
		IdModuloPrincipal = idModulo1;
		IdModuloSecundario1 = idModulo2;
		IdModuloSecundario2 = idModulo3;
		IdModuloVairacao = idModulo4;
		PosicaoParafuso = posicaoParafuso;

		Latencia = latencia;

		PegouItem = pegouItem;

		Mundo = mundo;
	}

	public ResultadoObstaculo()
	{
	}
}