//
// IMPORTANT!!!: Add a reference to System.Drawing.dll
using System;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;

public class Example
{
    public static void Main()
    {
        // A simple source for demonstration purposes. Modify this path as necessary.
        String[] files = System.IO.Directory.GetFiles(@"C:\Users\Public\Pictures", "*.jpg");
        String newDir = @"C:\Users\Public\Pictures\Sample Pictures\Modified";
        System.IO.Directory.CreateDirectory(newDir);

        //Sequential
        Console.WriteLine("Enter sequential code");
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        foreach (string currentFile in files)
        {
            // The more computational work you do here, the greater 
            // the speedup compared to a sequential foreach loop.
            String filename = System.IO.Path.GetFileName(currentFile);
            var bitmap = new Bitmap(currentFile);

            bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
            bitmap.Save(Path.Combine(newDir, filename));

            // Peek behind the scenes to see how work is parallelized.
            // But be aware: Thread contention for the Console slows down parallel loops!!!

           Console.WriteLine("Processing {0} on thread {1}", filename, Thread.CurrentThread.ManagedThreadId);
            //close lambda expression and method invocation
        }
        stopwatch.Stop();
        Console.Error.WriteLine("Sequential loop time in milliseconds: {0}",
                                stopwatch.ElapsedMilliseconds);
    
        stopwatch.Reset();
        stopwatch.Start();
        
        
        var options = new ParallelOptions()
        {
            MaxDegreeOfParallelism = 2
        };

        Parallel.ForEach(files,  (currentFile) =>
            {
                String filename = System.IO.Path.GetFileName(currentFile);
                var bitmap = new Bitmap(currentFile);

                bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
                bitmap.Save(Path.Combine(newDir, filename));
                Console.WriteLine("Processing {0} on thread {1}", filename, Thread.CurrentThread.ManagedThreadId);

            });
        stopwatch.Stop();
        Console.Error.WriteLine("Parallel loop time in milliseconds: {0}",
                                stopwatch.ElapsedMilliseconds);

        // Peek behind the scenes to see how work is parallelized.
        // But be aware: Thread contention for the Console slows down parallel loops!!!


        //close lambda expression and method invocation

        Console.WriteLine("Processing complete. Press any key to exit.");
        Console.ReadKey();
    }
}