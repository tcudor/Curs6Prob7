
class Cont
{
    public string Beneficiar { get; }
    public string NumarCont { get; }
    public decimal ValoareCurenta { get; private set; }
    public List<Tranzactie> Tranzactii { get; }

    public Cont(string beneficiar, string numarCont, decimal valoareCurenta)
    {
        Beneficiar = beneficiar;
        NumarCont = numarCont;
        ValoareCurenta = valoareCurenta;
        Tranzactii = new List<Tranzactie>();
    }

    public void AdaugaTranzactie(Tranzactie tranzactie)
    {
        tranzactie.NumarOrdine = Tranzactii.Count + 1;
        tranzactie.Data = DateTime.Now;
        Tranzactii.Add(tranzactie);

        if (tranzactie.TipTranzactie == TipTranzactie.Intrare)
        {
            ValoareCurenta += tranzactie.ValoareTranzactie;
        }
        else
        {
            ValoareCurenta -= tranzactie.ValoareTranzactie;
        }
    }

    public void TiparesteExtras(DateTime dataInceput, DateTime dataSfarsit)
    {
        Console.WriteLine($"Extras de cont pentru contul {NumarCont} al beneficiarului {Beneficiar}:");
        Console.WriteLine("---------------------------------------------------");
        Console.WriteLine("Data\tNr. Ordine\tTip Tranzactie\tPartener\tSold Inainte\tValoare Tranzactie\tSold Dupa");
        Console.WriteLine("---------------------------------------------------");

        var tranzactiiPerioada = Tranzactii.Where(t => t.Data >= dataInceput && t.Data <= dataSfarsit);

        foreach (var tranzactie in tranzactiiPerioada)
        {
            Console.WriteLine(tranzactie);
        }
    }
}

class Tranzactie
{
    public DateTime Data { get; set; }
    public int NumarOrdine { get; set; }
    public TipTranzactie TipTranzactie { get; }
    public string Partener { get; }
    public decimal SoldInainte { get; }
    public decimal ValoareTranzactie { get; }
    public decimal SoldDupa { get; private set; }

    public Tranzactie(TipTranzactie tipTranzactie, string partener, decimal soldInainte, decimal valoareTranzactie)
    {
        TipTranzactie = tipTranzactie;
        Partener = partener;
        SoldInainte = soldInainte;
        ValoareTranzactie = valoareTranzactie;

        if (tipTranzactie == TipTranzactie.Intrare)
        {
            SoldDupa = SoldInainte + ValoareTranzactie;
        }
        else
        {
            SoldDupa = SoldInainte - ValoareTranzactie;
        }
    }

    public override string ToString()
    {
        return $"{Data.ToShortDateString()}\t{NumarOrdine}\t{TipTranzactie}\t{Partener}\t{SoldInainte}\t{ValoareTranzactie}\t{SoldDupa}";
    }
}

enum TipTranzactie
{
    Intrare,
    Iesire
}

class Program
{
    static void Main()
    {
        Cont cont = new Cont("Tudor", "1234567890", 1000);

        cont.AdaugaTranzactie(new Tranzactie(TipTranzactie.Intrare, "Employer", cont.ValoareCurenta, 500));
        cont.AdaugaTranzactie(new Tranzactie(TipTranzactie.Iesire, "Supermarket SRL", cont.ValoareCurenta, 100));
        cont.AdaugaTranzactie(new Tranzactie(TipTranzactie.Intrare, "Client", cont.ValoareCurenta, 200));

        Console.WriteLine("Extrasul de cont pentru ultimele 7 zile:");
        cont.TiparesteExtras(DateTime.Now.AddDays(-7), DateTime.Now);
    }
}
