using System;
using System.Collections.Generic;
using System.Threading;

class Program
{
    static List<Thread> threads = new List<Thread>();
    static int numSedie;
    static int numSedieLibere;
    static object lockObject = new object();
    static Random random = new Random();
    static bool giocoInCorso = true;

    static void Main(string[] args)
    {
        Console.WriteLine("Benvenuti al gioco delle sedie musicali!");
        Console.WriteLine("Inserisci il numero di bambini: ");
        int numBambini = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("Inserisci il numero di sedie: ");
        numSedie = Convert.ToInt32(Console.ReadLine());
        numSedieLibere = numSedie;

        if (numSedie < 1)
        {
            Console.WriteLine("Il numero di sedie deve essere maggiore di 0.");
            return;
        }

        // Crea un thread per ogni bambino
        for (int i = 0; i < numBambini; i++)
        {
            Thread thread = new Thread(Play);
            thread.Name = "Bambino" + i;
            threads.Add(thread);
            thread.Start();
        }

        foreach (var thread in threads)
        {
            thread.Join();
        }

        Console.WriteLine("Il gioco è finito. Tutti i bambini hanno vinto!");
        Console.ReadLine();
    }

    static void Play()
    {
        while (giocoInCorso)
        {
            lock (lockObject)
            {
                if (numSedieLibere > 0)
                {
                    Console.WriteLine($"La musica si è fermata! {Thread.CurrentThread.Name} si è seduto.");
                    numSedieLibere--;
                    Thread.Sleep(random.Next(500, 1000)); // Simula il tempo per sedersi
                }
                else
                {
                    Console.WriteLine($"La musica si è fermata! {Thread.CurrentThread.Name} è eliminato.");
                    threads.Remove(Thread.CurrentThread);
                    numSedie--;
                    numSedieLibere = numSedie;
                    break;
                }
            }
            Thread.Sleep(1000);
            if (threads.Count == numSedie + 1)
            {
                giocoInCorso = false;
                break;
            }
        }
    }
}
