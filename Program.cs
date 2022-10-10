namespace TaskCancelProject
{
    internal class Program
    {
        static void SquareNumbers(CancellationToken token)
        {
            for (int i = 1; i <= 10; i++)
            {
                if (token.IsCancellationRequested)
                {
                    Console.WriteLine("Task is cancel!");
                    return;
                }
                Console.WriteLine($"square {i} = {i * i}");
                Thread.Sleep(500);
            }
        }

        static void Square(int i, CancellationToken token)
        {
            if (token.IsCancellationRequested)
                Console.WriteLine($"cancaled on {i} numeber");
            else
                Console.WriteLine($"continue on {i} numeber");
            Thread.Sleep(500);
            Console.WriteLine($"square {i} = {i * i}");
        }
        static void Main(string[] args)
        {
            /*
            CancellationTokenSource cancelSource = new CancellationTokenSource();
            CancellationToken cancelToken = cancelSource.Token;

            Task task = new(() => 
            {
                for(int i = 1; i <= 10; i++)
                {
                    if(cancelToken.IsCancellationRequested)
                    {
                        Console.WriteLine("Task is cancel!");
                        return;
                    }
                    Console.WriteLine($"square {i} = {i * i}");
                    Thread.Sleep(500);
                }
            }, cancelToken);
            task.Start();

            Thread.Sleep(2000);
            cancelSource.Cancel();
            task.Wait();
            Console.WriteLine($"Status of task: {task.Status}");

            cancelSource.Dispose();
            */
            /*
            CancellationTokenSource cancelSource = new CancellationTokenSource();
            CancellationToken cancelToken = cancelSource.Token;

            Task task = new(() =>
            {
                for (int i = 1; i <= 10; i++)
                {
                    if (cancelToken.IsCancellationRequested)
                    {
                        cancelToken.ThrowIfCancellationRequested();
                    }
                    Console.WriteLine($"square {i} = {i * i}");
                    Thread.Sleep(500);
                }
            }, cancelToken);
            try
            {
                task.Start();

                Thread.Sleep(2000);
                cancelSource.Cancel();

                task.Wait();
            }
            catch(AggregateException e)
            {
                foreach (Exception ex in e.InnerExceptions)
                    if (ex is TaskCanceledException)
                        Console.WriteLine("task is cancel!");
                    else
                        Console.WriteLine(e);
            }
            finally
            {
                cancelSource.Dispose();
            }
            
            Console.WriteLine($"Status of task: {task.Status}");
            */

            //Task task = new(() => SquareNumbers(cancelToken), cancelToken);

            CancellationTokenSource cancelSource = new CancellationTokenSource();
            CancellationToken cancelToken = cancelSource.Token;
            new Task(() =>
            {
                Thread.Sleep(100);
                cancelSource.Cancel();
            }).Start();

            try
            {
                Parallel.ForEach(new List<int>() { 1, 2, 3, 4, 5, 6, 7 },
                                    new ParallelOptions { CancellationToken = cancelToken }, 
                                    (i) => Square(i, cancelToken));
            }
            catch (OperationCanceledException e)
            {
                
                Console.WriteLine("task is cancel!");
                Console.WriteLine(e.Message);

            }
            finally
            {
                cancelSource.Dispose();
            }

        }
    }
}