public interface IDescontos
{
    double Calcula(Orcamento orcamento);
    public IDescontos Proximo { get; set; }
}

public class DescontoMaisDeCincoItens : IDescontos
{
    public IDescontos Proximo { get; set; }

    public double Calcula(Orcamento orcamento)
    {
        if (orcamento.Quantidade >= 5)
            orcamento.Total = orcamento.Total * 0.9;
        return Proximo.Calcula(orcamento);
    }

}
public class DescontoTotalMaisDe500 : IDescontos
{
    public IDescontos Proximo { get; set; }

    public double Calcula(Orcamento orcamento)
    {
        if(orcamento.Total > 500)
           orcamento.Total = orcamento.Total * 0.93;
        return Proximo.Calcula(orcamento);
    }
    
}

public class SemDesconto : IDescontos
{
    public IDescontos Proximo { get; set; }

    public double Calcula(Orcamento orcamento)
    {
        return orcamento.Total;
    }
}
public class Orcamento
{
    public double Total { get; set; }
    public int Quantidade { get; set; }

}
class Program
{
    static void Main(string[] args)
    {
        var orcamento = new Orcamento()
        {
            Quantidade = 6,
            Total = 605
        };

        var d1 = new DescontoMaisDeCincoItens();
        var d2 = new DescontoTotalMaisDe500(); 
        var d3 = new SemDesconto();

        d1.Proximo = d2;
        d2.Proximo = d3;
        var dResult = d1.Calcula(orcamento);
        Console.WriteLine(dResult);
    }
}
