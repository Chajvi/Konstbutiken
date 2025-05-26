using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webbshoppen.Models;

namespace Webbshoppen.Menyer;
internal class VarukorgMeny
{
    private List<(Produkt produkt, int antal)> _varukorg;
    public VarukorgMeny(List<(Produkt, int)> varukorg)
    {
        _varukorg = varukorg;
    }

    public void VisaMeny()
    {
        bool igång = true;
        while (igång)
        {
            Console.Clear();
            Console.WriteLine("VARUKORG");

            if (_varukorg.Count == 0)
            {
                Console.WriteLine("Du har inte lagt till några varor än!");
            }

            else
            {
                decimal total = 0;
                int index = 1;

                foreach (var (produkt, antal) in _varukorg)
                {
                    decimal radpris = produkt.Pris * antal;
                    total += radpris;

                    Console.WriteLine($"[{index}] {produkt.Namn} x{antal} = {radpris} kr");
                }

                Console.WriteLine($"\nTotalt: {total} kr");
            }

            Console.WriteLine("\n Skriv id:t på varan du vill ändra eller 0 för att gå tillbaka");
            string input = Console.ReadLine();
            if (int.TryParse(input, out int val))
            {
                if (val == 0)
                {
                    igång = false;
                }

                else if (val >= 1 && val <= _varukorg.Count)
                {
                    var vald = _varukorg[val-1];
                    Console.Write($"Hur många {vald.Item1.Namn} vill du ha i varukorgen? Skriv 0 för att ta bort den: ");
                    if (int.TryParse(Console.ReadLine(), out int nyaMängden))
                    {
                        if (nyaMängden == 0)
                        {
                            _varukorg.RemoveAt(val - 1);
                            Console.WriteLine("Produkten har tagits bort ur din varukorg!");
                        }

                        else
                        {
                            _varukorg[val - 1] = (vald.Item1, nyaMängden);
                            Console.WriteLine("Produktmängden har uppdaterats!");
                            Console.ReadKey();
                        }
                    }
                }
            }
        }
    }
}
