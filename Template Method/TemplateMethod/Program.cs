/* Utiliza de uma classe abstrata 
 * (As classes abstratas são as que não permitem realizar qualquer tipo de instância. 
 * São classes feitas especialmente para serem modelos para suas classes derivadas. 
 * As classes derivadas, via de regra, deverão sobrescrever os métodos para realizar a implementação dos mesmos.)
 * , que vai sobreescrever executar um método que as classes que 
 * herdam utilizam diferentes regras para o template definido na classe abstrada dai vem o nome
 * TemplateMethod
 * */
interface IImposto
{
    double Calcula(Orcamento valor);
}

class Orcamento
{
    public double Total { get; set;}
    public List<Item> Items { get; set;}
}

class Item
{
    public string Name { get; set;}
    public double Valor { get; set; }
}

class ICPP : TemplateDeImpostoCondicional
{

    public override bool DeveUsarMaximaTaxacao(Orcamento orcamento)
    {
        return orcamento.Total >= 500;
    }

    public override double MaximaTaxacao(Orcamento orcamento)
    {
        return orcamento.Total * 0.7;
    }

    public override double MinimaTaxacao(Orcamento orcamento)
    {
        return orcamento.Total * 0.05;
    }
}

class IHITT : TemplateDeImpostoCondicional
{

    public override bool DeveUsarMaximaTaxacao(Orcamento orcamento)
    {
        return orcamento.Items.GroupBy(x => x.Name).Any( g => g.Count() > 1 );
    }

    public override double MaximaTaxacao(Orcamento orcamento)
    {
        return (orcamento.Total * 0.13) + 100;
    }

    public override double MinimaTaxacao(Orcamento orcamento)
    {
        return orcamento.Total * (0.01 + orcamento.Items.Count);
    }
}

class IKCV : TemplateDeImpostoCondicional
{

    public override bool DeveUsarMaximaTaxacao(Orcamento orcamento)
    {
        return orcamento.Total > 500 && TemItemMaiorQue100ReaisNo(orcamento);
    }

    public override double MaximaTaxacao(Orcamento orcamento)
    {
        return orcamento.Total * 0.10;
    }

    public override double MinimaTaxacao(Orcamento orcamento)
    {
        return orcamento.Total * 0.06;
    }

    private bool TemItemMaiorQue100ReaisNo(Orcamento orcamento)
    {
        foreach (Item item in orcamento.Items)
        {
            if (item.Valor > 100) return true;
        }
        return false;
    }
}

abstract class TemplateDeImpostoCondicional : IImposto
{
    public double Calcula(Orcamento orcamento)
    {
        if (DeveUsarMaximaTaxacao(orcamento))
        {
            return MaximaTaxacao(orcamento);
        }
        else
        {
            return MinimaTaxacao(orcamento);
        }
    }

    public abstract bool DeveUsarMaximaTaxacao(Orcamento orcamento);
    public abstract double MaximaTaxacao(Orcamento orcamento);
    public abstract double MinimaTaxacao(Orcamento orcamento);
}