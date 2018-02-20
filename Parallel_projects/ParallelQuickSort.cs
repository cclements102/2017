using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ParallelQuickSort
{
    class ParQuickSortSol
    {
        /// <summary>
        ///     Swaps the two values of the specified indexes
        /// </summary>
        /// <param name="arr">An IList where elements need to be swapped</param>
        /// <param name="i">The first index to be swapped</param>
        /// <param name="j">The second index to be swapped</param>
        private static void Swap<T>(IList<T> arr, int i, int j)
        {
            var tmp = arr[i];
            arr[i] = arr[j];
            arr[j] = tmp;
        }

        /// <summary>
        ///     Partitions an IList by defining a pivot and then comparing the other members to this pivot.
        /// </summary>
        /// <param name="arr">The IList to partition</param>
        /// <param name="low">The lowest index of the partition</param>
        /// <param name="high">The highest index of the partition</param>
        /// <returns>Returns the index of the chosen pivot</returns>
        private static int Partition<T>(IList<T> arr, int low, int high)
            where T : IComparable<T>
        {
            /*
                * Defining the pivot position, here the middle element is used but the choice of a pivot
                * is a rather complicated issue. Choosing the left element brings us to a worst-case performance,
                * which is quite a common case, that's why it's not used here.
                */
            var pivotPos = (high + low) / 2;
            var pivot = arr[pivotPos];

            // Putting the pivot on the left of the partition (lowest index) to simplify the loop
            Swap(arr, low, pivotPos);

            /** The pivot remains on the lowest index until the end of the loop
                * The left variable is here to keep track of the number of values
                * that were compared as "less than" the pivot.
                */
            var left = low;
            for (var i = low + 1; i < high; i++)
            {
                // If the value is greater than the pivot value we continue to the next index.
                if (arr[i].CompareTo(pivot) >= 0) continue;

                // If the value is less than the pivot, we increment our left counter (one more element below the pivot)
                left++;
                // and finally we swap our element on our left index.
                Swap(arr, i, left);
            }

            // The pivot is still on the lowest index, we need to put it back after all values found to be "less than" the pivot.
            Swap(arr, low, left);

            // We return the new index of our pivot
            return left;
        }
        /// <summary>
        ///     Realizes a Quicksort on an IList of IComparable items in a sequential way.
        /// </summary>
       
        private static void Quicksort<T>(IList<T> arr, int low, int high) where T : IComparable<T>
        {
            //complete implementation here 
            int pivotPos;

            //the next values to be used for low and high for the next recursive call
            int next_low;
            int next_high;

            //Console.WriteLine("Low = {0}, High = {1}", low, high);
            pivotPos = Partition(arr, low, high);
            next_high = pivotPos - 1;
            next_low = pivotPos + 1;
            //Call quicksort for the left and right partitions respectively if the remaining array to be solved has more than 1 element
            if (next_high > low)
            {
                Quicksort(arr, low, next_high);
            }
            if(high > next_low)
            {
                Quicksort(arr, next_low, high);
            }

            return;
        }

        /// <summary>
        ///     Realizes a Quicksort on an IList of IComparable items.
        ///     Left and right side of the pivot are processed in parallel.
        /// </summary>
        private static void QuicksortParallel<T>(IList<T> arr, int low, int high)
            where T : IComparable<T>
        {
            
                
            //complete implementation here
            int pivotPos;
            //the next values to be used for low and high for the next recursive call
            int next_low;
            int next_high;
            Parallel.Invoke(
                () =>
                {
                    //Console.WriteLine("Low = {0}, High = {1}", low, high);
                    pivotPos = Partition(arr, low, high);
                    next_high = pivotPos - 1;
                    next_low = pivotPos + 1;
                    //Call quicksort for the left and right partitions respectively if the remaining array to be solved has more than 1 element
                    if (next_high > low)
                    {
                        QuicksortParallel(arr, low, next_high);
                    }
                    if (high > next_low)
                    {
                        QuicksortParallel(arr, next_low, high);
                    }
                });
            return;
        }
    
    //Main method
    static void Main(string[] args)
        {
            //Test program

            //create an array of random integer values
            int size = 100;
            int[] num_list = new int[size];
            //copy array for parallel sorting
            int[] num_list2 = new int[size];
            int[] num_list3 = new int[size];

            Random randNum = new Random();
            //range of integer values in the array
            int Min = 0;
            int Max = 50;
            //fill array with random integers
            for(int i = 0; i < num_list.Length; i++)
            {
                num_list[i] = randNum.Next(Min, Max);
            }
            num_list2 = num_list;
            num_list3 = num_list;

            Stopwatch stopwatch = new Stopwatch();
            Stopwatch Par_stopwatch = new Stopwatch();

            stopwatch.Start();
            Quicksort(num_list, 0, size);
            stopwatch.Stop();
            double serial_time = stopwatch.ElapsedTicks * 3.02 / 10000;

            Console.WriteLine("Serial quicksort time in milliseconds: " + serial_time.ToString("N3"));

            var options = new ParallelOptions()
            {
                MaxDegreeOfParallelism = -1
            };

            Par_stopwatch.Start();
            QuicksortParallel(num_list2, 0, size);
            Par_stopwatch.Stop();
            /*for (int i = 0; i < size; i++)
            {
                Console.WriteLine("array[{0}] = {1}", i, num_list2[i]);
            }*/

            double speedup = serial_time / Par_stopwatch.ElapsedMilliseconds; /// Par_stopwatch.ElapsedMilliseconds;
            Console.WriteLine("Parallel quicksort time with unlimited degree of parallelism in milliseconds: {0}", Par_stopwatch.ElapsedMilliseconds);
            Console.WriteLine("Speedup with unlimited degree of parallelism: " + speedup.ToString("N4"));
            options.MaxDegreeOfParallelism = 3;

            Par_stopwatch.Reset();
            Par_stopwatch.Start();
            QuicksortParallel(num_list3, 0, size);
            Par_stopwatch.Stop();

            speedup = serial_time / Par_stopwatch.ElapsedMilliseconds;
            Console.WriteLine("Parallel quicksort time with degree of parallelism set to {0} in milliseconds: {1}", options.MaxDegreeOfParallelism, Par_stopwatch.ElapsedMilliseconds);
            Console.WriteLine("Speedup with degree of parallelism {0}: " + speedup.ToString("N4"), options.MaxDegreeOfParallelism);
            Console.Read();

            
        }
    }
}
