using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static void Main()
    {

        // Source must be array or IList.
        var source = Enumerable.Range(0, 100000).ToArray();
        double[] results = new double[source.Length];

        Console.WriteLine("Enter default partitioner code");
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        Parallel.ForEach(source, // source collection
                                    (j, loop) => // method invoked by the loop on each iteration
                                    {
                                        results[j] = source[j] * Math.PI;
                                    }
                                    // Method to be executed when each partition has completed.
                                    // finalResult is the final value of subtotal for a particular partition.
                                    );
        stopwatch.Stop();
        Console.Error.WriteLine("Default partitioner loop time in milliseconds: {0}",
                                stopwatch.ElapsedMilliseconds);
        // Partition the entire source array.
        var rangePartitioner = Partitioner.Create(0, source.Length);

        Console.WriteLine("Enter partitioner code");
        stopwatch.Reset();
        stopwatch.Start();
        // Loop over the partitions in parallel.
        Parallel.ForEach(rangePartitioner, (range, loopState) =>
        {
            // Loop over each range element without a delegate invocation.
            for (int i = range.Item1; i < range.Item2; i++)
            {
                results[i] = source[i] * Math.PI;
            }
        });
        stopwatch.Stop();
        Console.Error.WriteLine("Partitioner loop time in milliseconds: {0}",
                                stopwatch.ElapsedMilliseconds);

        // Partition the entire source array.
        int size = source.Length / 10;
        var chunkPartitioner = Partitioner.Create(0, source.Length, size);

        Console.WriteLine("Enter defined chunk partitioner code");
        stopwatch.Reset();
        stopwatch.Start();
        // Loop over the partitions in parallel.
        Parallel.ForEach(chunkPartitioner, (range, loopState) =>
        {
            // Loop over each range element without a delegate invocation.
            for (int i = range.Item1; i < range.Item2; i++)
            {
                results[i] = source[i] * Math.PI;
            }
        });
        stopwatch.Stop();
        Console.Error.WriteLine("Defined Chunk Partitioner loop time in milliseconds: {0}",
                                stopwatch.ElapsedMilliseconds);
        Console.ReadKey();
        
    }
}