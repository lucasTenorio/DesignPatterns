public abstract class State
{
    protected Venda _venda;

    public void DefineVenda(Venda venda)
    {
        this._venda = venda;
    }

    public abstract double AplicaDesconto();
    public abstract void AlteraEstado();
}

public class Venda
{
    
    public State Estado { get; set; }
    public double Total { get; set; }

    public Venda(State estado)
    {
        this.AlterarEstado(estado);
    }

    public void AlterarEstado(State estado)
    {
        this.Estado = estado;
        this.Estado.DefineVenda(this);
    }
}

public class EmAprovação : State
{
    public override void AlteraEstado()
    {
        this._venda.AlterarEstado(new Aprovado());
    }

    public override double AplicaDesconto()
    {
        return _venda.Total = _venda.Total - (_venda.Total * 0.05);
    }
}

public class Aprovado : State
{
    public override void AlteraEstado()
    {
        this._venda.AlterarEstado(new Reprovado());
    }

    public override double AplicaDesconto()
    {
        return _venda.Total = _venda.Total - (_venda.Total * 0.02);
    }
}

public class Reprovado : State
{
    public override void AlteraEstado()
    {
        this._venda.AlterarEstado(new Finalizado());
    }

    public override double AplicaDesconto()
    {
        return _venda.Total = _venda.Total;
    }
}

public class Finalizado : State
{
    public override void AlteraEstado()
    {
        this._venda.AlterarEstado(new Aprovado());
    }

    public override double AplicaDesconto()
    {
        return _venda.Total = _venda.Total;
    }
}

public class Program
{
    static void Main(string[] args)
    {
        var venda = new Venda(new EmAprovação())
        {
            Total = 478
        };
        Console.WriteLine(venda.Total);
        venda.Estado.AplicaDesconto();
        venda.Estado.AlteraEstado();
        Console.WriteLine(venda.Total);
        venda.Estado.AplicaDesconto();
        venda.Estado.AlteraEstado();
        Console.WriteLine(venda.Total);
        venda.Estado.AplicaDesconto();
        venda.Estado.AlteraEstado();
        Console.WriteLine(venda.Total);
        venda.Estado.AplicaDesconto();
        venda.Estado.AlteraEstado();
        Console.WriteLine(venda.Total);
    }
}