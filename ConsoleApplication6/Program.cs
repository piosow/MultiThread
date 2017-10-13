using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwaitExample
{
    class Program
    {
        static void Main()
        {
            int threadCount;
            List<AsyncAwaitDemo> threadsList = new List<AsyncAwaitDemo>();
            bool mainThread = true;

            Console.WriteLine("Witamy w prezentacji obsługi wielu wątków.");
            Console.WriteLine("Główny wątek aplikacji będzie wyświelał się co sekundę jako ciąg ***.");
            Console.WriteLine("Aplikacja poprosi użytkownika o podanie ilości wątków które ma uruchomić niezależnie od siebie.");
            Console.WriteLine("Każdy wątek będzie wyświetlał 10 razy napis w interwale podanym przez użytkownika (w milisekundach).");
            Console.ReadLine();
            Console.WriteLine("Proszę podać ile wątków ma pracować jednocześnie: ");
            threadCount = Convert.ToInt32(Console.ReadLine());
            for (int i = 0; i < threadCount; i++)
            {
                Console.WriteLine($"Podaj interwał wyświetlania komunikatu (w ms) dla {i +1} wątku: ");
                threadsList.Add(new AsyncAwaitDemo(Console.ReadLine(), i + 1));
            }

            foreach (var item in threadsList)
            {
                item.DoStuff();
            }

            while (mainThread)
            {
                Thread.Sleep(1000);
                Console.WriteLine("*******************************************************************");
                if (threadsList.Count(c => c.isDone) == threadCount) mainThread = false;
                
            }
            Console.WriteLine("Koniec wszystkich wątków.");
            Console.ReadKey();
        }
    }

    public class AsyncAwaitDemo
    {
        private int _interval = 1000;
        private int _threadNumber = 1;
        public bool isDone = false;

        public AsyncAwaitDemo(string interval, int threadNumber)
        {
            _interval = Convert.ToInt32(interval);
            _threadNumber = threadNumber;
        }

        public async Task DoStuff()
        {
            await Task.Run(() => LongRunningOperation());
            
        }

        public async Task<string> LongRunningOperation()
        {
            int counter;

            for (counter = 0; counter < 10; counter++)
            {
                Thread.Sleep(_interval);
                Console.WriteLine($"({counter + 1})Wątek nr {_threadNumber} o interwale {_interval}");
            }
            Console.WriteLine($"KONIEC WĄTKU {_threadNumber}");
            isDone = true;
            return "";
        }
    }
}