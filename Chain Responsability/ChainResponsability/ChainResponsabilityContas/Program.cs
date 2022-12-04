public enum Formato
{
    XML,
    CSV,
    PORCENTO
}

public class ContaBancaria
{

    public ContaBancaria(string nome , double saldo)
    {
        NomeTitular= nome;
        Saldo= saldo;
    }
    public string NomeTitular { get; set; }
    public double Saldo { get; set; }
}

public class Requisicao
{
    public Formato Formato { get; private set; }

    public Requisicao(Formato formato)
    {
        this.Formato = formato;
    }
}

public interface IImprimeDadosBancarios
{
    IImprimeDadosBancarios ProximaImpressao { get; set; }
    string Imprimir(Requisicao requisicao, ContaBancaria conta);
}

public class ImprimeXML : IImprimeDadosBancarios
{
    public IImprimeDadosBancarios ProximaImpressao { get; set; }

    public string Imprimir(Requisicao requisicao, ContaBancaria conta)
    {
        if (requisicao.Formato.HasFlag(Formato.XML))
            return @"<conta>"+ conta.NomeTitular + "</conta> " +
                "<saldo>" + conta.Saldo.ToString() + "</saldo>";
        return ProximaImpressao.Imprimir(requisicao, conta);
    }
}

public class ImprimeCSV : IImprimeDadosBancarios
{
    public IImprimeDadosBancarios ProximaImpressao { get; set; }

    public string Imprimir(Requisicao requisicao, ContaBancaria conta)
    {
        if (requisicao.Formato.HasFlag(Formato.CSV))
            return @"" + conta.NomeTitular + ";" + conta.Saldo.ToString() + ";";
        return ProximaImpressao.Imprimir(requisicao, conta);
    }
}

public class ImprimePorCento : IImprimeDadosBancarios
{
    public IImprimeDadosBancarios ProximaImpressao { get; set; }

    public string Imprimir(Requisicao requisicao, ContaBancaria conta)
    {
        if (requisicao.Formato.HasFlag(Formato.PORCENTO))
            return conta.NomeTitular + "%" + conta.Saldo.ToString() + "%";
        return ProximaImpressao.Imprimir(requisicao, conta);
    }
}

public class ImprimeSemFormato : IImprimeDadosBancarios
{
    public IImprimeDadosBancarios ProximaImpressao { get; set; }

    public string Imprimir(Requisicao requisicao, ContaBancaria conta)
    {
        return "Formato " + requisicao.Formato + " não encontrado";
    }
}


class Program
{
    static void Main(string[] args)
    {
        var conta = new ContaBancaria[] { new ContaBancaria("lucas", 300), new ContaBancaria("Erica", 6320.21), new ContaBancaria("Fulano", 332)};

        var requisicao = new Requisicao[] { new Requisicao(Formato.CSV), new Requisicao(Formato.XML), new Requisicao(Formato.PORCENTO) };

        var ip1 = new ImprimeCSV();
        var ip2 = new ImprimePorCento();
        var ip3 = new ImprimeXML();
        var ip4 = new ImprimeSemFormato();

        ip1.ProximaImpressao = ip2;
        ip2.ProximaImpressao = ip3;
        ip3.ProximaImpressao = ip4;

        for (int i = 0; i < 3; i++)
        {
            var result = ip1.Imprimir(requisicao[i], conta[i]);

            Console.WriteLine(result);
        }



    }
}