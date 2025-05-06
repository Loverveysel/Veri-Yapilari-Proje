using System;
using Veri_Yapilari_Proje.Models;

class Program
{
    static void Main()
    {
        ListManager manager = new ListManager();

        manager.ProcessCommand("singly", "add", 10);
        manager.ProcessCommand("singly", "add", 20);
        manager.ProcessCommand("singly", "add", 30);

        string jsonOutput = manager.GetListAsJson("singly");

        Console.WriteLine(jsonOutput);
    }
}
