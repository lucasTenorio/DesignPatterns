using System.Collections.Generic;

public abstract class Imposto
{
    public Imposto? OutroImposto { get; set; }
    
    public Imposto(Imposto outroImposto)
    {
        this.OutroImposto = outroImposto;
    }
    public Imposto()
    {
        this.OutroImposto = null;
    }
    
    public abstract double Calcula(Orcamento orcamento);
    protected double CalculoDoOutroImposto(Orcamento orcamento)
    {
        if (OutroImposto == null) return 0;
        return OutroImposto.Calcula(orcamento);
    }
    
}


public class ISS : Imposto
{

    public ISS(Imposto outroImposto) : base(outroImposto) { }
    public ISS() : base()
    {

    }
    public override double Calcula(Orcamento orcamento)
    {
        return orcamento.Total * 0.06 + CalculoDoOutroImposto(orcamento);
    }

}

public class IOF : Imposto
{

    public IOF(Imposto outroImposto) : base(outroImposto) { }
    public IOF() : base()
    {

    }
    public override double Calcula(Orcamento orcamento)
    {
        return orcamento.Total * 0.2 + CalculoDoOutroImposto(orcamento);
    }

}

public class ICMS : Imposto
{
    public ICMS(Imposto outroImposto) : base(outroImposto) { }
    public ICMS() : base()
    {

    }
    public override double Calcula(Orcamento orcamento)
    {
        return orcamento.Total * 0.1 + CalculoDoOutroImposto(orcamento);
    }

}

public class Orcamento
{
    public IList<Item>? Items { get; set; }
    public double Total { get; set; }

}
public class Item
{
    public string? Name { get; set; }
    public double Valor { get; set; }
}

public class Program
{
    static void Main(string[] args)
    {
        //Imposto iss = new ISS(new ICMS(new IOF()));

        //Orcamento orcamento = new Orcamento() {Total = 500};

        //var valor = iss.Calcula(orcamento);

        //Console.WriteLine(valor);

        var filtros = new ContasComSaldoMenorQue100(new ContasComSaldoMaiorQue500(new ContasComDataAberturaMesVigente()));
        var listas = new List<Conta>()
        {
            new()
            {
                DataAbertura = DateTime.Now,
                Saldo = 2000
            },
            new()
            {
                DataAbertura = DateTime.Now.AddMonths(-1),
                Saldo = 30000
            },
            new()
            {
                DataAbertura= DateTime.Now.AddMonths(-3),
                Saldo = 10000000000
            },
            new()
            {
                DataAbertura= DateTime.Now.AddMonths(-4),
                Saldo = 10
            }

        };

        var res = filtros.Filtra(listas);
        for (int i = 0; i < res.Count; i++)
        {
            Console.WriteLine(res[i].Saldo);
        }

    }
}

public abstract class Filtro
{
    public Filtro OutroFiltro { get; set; }
    public IList<Conta> ListaFiltrada { get; set; } = new List<Conta>();

    public Filtro(Filtro outroFiltro)
    {
        OutroFiltro = outroFiltro;
    }
    public Filtro()
    {

    }
    public abstract IList<Conta> Filtra(IList<Conta> contas);
    protected IList<Conta> ProximoFiltro(IList<Conta> contas)
    {
        if(OutroFiltro == null)
        {
            return ListaFiltrada;
        }
        return OutroFiltro.Filtra(contas);
    }
    protected void AdicionaListaFiltrada(IList<Conta> contas)
    {
        var lista = new List<Conta>();
        
        lista.AddRange(ListaFiltrada);
        lista.AddRange(contas);
        
        ListaFiltrada = lista;
    }
}

public class ContasComSaldoMenorQue100 : Filtro
{
    public ContasComSaldoMenorQue100(Filtro outroFiltro) : base(outroFiltro) { }
    public ContasComSaldoMenorQue100() : base(){}
    public override IList<Conta> Filtra(IList<Conta> contas)
    {
        var list = contas.Where(conta => conta.Saldo < 100).ToList();
        
        list.AddRange(ProximoFiltro(contas));
        return list;
    }
}

public class ContasComSaldoMaiorQue500 : Filtro
{
    public ContasComSaldoMaiorQue500(Filtro outroFiltro) : base(outroFiltro){}
    public ContasComSaldoMaiorQue500() : base(){}
    public override IList<Conta> Filtra(IList<Conta> contas)
    {
        var list = contas.Where(conta => conta.Saldo > 500000).ToList();
        list.AddRange(ProximoFiltro(contas));
        return list;
    }
}

public class ContasComDataAberturaMesVigente : Filtro
{
    public ContasComDataAberturaMesVigente(Filtro outroFiltro) : base(outroFiltro){}
    public ContasComDataAberturaMesVigente() : base() { }
    public override IList<Conta> Filtra(IList<Conta> contas)
    {
        var list = contas.Where(conta => conta.DataAbertura.Month.Equals(DateTime.Now.Month)).ToList();
        
        list.AddRange(ProximoFiltro(contas));

        return list;
    }
}

public class Conta
{
    public double Saldo { get; set; }
    public DateTime DataAbertura { get; set; }
}