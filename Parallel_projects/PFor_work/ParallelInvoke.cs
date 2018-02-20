namespace ParallelTasks
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Net;
    using System.Diagnostics;

    class ParallelInvoke
    {
        static void Main()
        {
            // Retrieve Darwin's "Origin of the Species" from Gutenberg.org.
            string[] words = CreateWordArray(@"http://www.gutenberg.org/files/2009/2009.txt");

            #region ParallelTasks
            /* 
            Perform three tasks in parallel on the source array as follows:
            Task 1: print you are in task 1 and call the GetLongestWord() method
            Task 2: print you are in task 2 and call the GetMostCommonWords() method 
            Task 3: print you are in task 2 and call the GetCountForWord() method
            Complete the Paralle.Invoke(..., ..., ...) below and also measure the execution times of each task
            and the overall parallel execution time for calling Parallel.Invoke
            */
            Stopwatch Outer_sw = new Stopwatch();
            Stopwatch inner_sw = new Stopwatch();

            Outer_sw.Start();
            Parallel.Invoke(
                //First Task
                () => {
                    inner_sw.Reset();
                    inner_sw.Start();
                    Console.WriteLine("You are in task 1");
                    GetLongestWord(words);
                    inner_sw.Stop();
                    Console.WriteLine("Task 1 runtime = {0}", inner_sw.ElapsedMilliseconds);
                },
                () =>
                {
                    //second task
                    inner_sw.Reset();
                    inner_sw.Start();
                    Console.WriteLine("You are in task 2");
                    GetMostCommonWords(words);
                    inner_sw.Stop();
                    Console.WriteLine("Task 2 runtime = {0}", inner_sw.ElapsedMilliseconds);
                    
                },
                
                () =>
                {
                    //task 3
                    inner_sw.Reset();
                    inner_sw.Start();
                    Console.WriteLine("You are in task 3");
                    GetCountForWord(words, "species");
                    inner_sw.Stop();
                    Console.WriteLine("Task 3 runtime = {0}", inner_sw.ElapsedMilliseconds);
                    
                }
                ); //close parallel.invoke

            Outer_sw.Stop();
            Console.WriteLine("Returned from Parallel.Invoke");
            Console.WriteLine("Total run time : {0}", Outer_sw.ElapsedMilliseconds);
            #endregion

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        #region HelperMethods
        private static void GetCountForWord(string[] words, string term)
        {
            var findWord = from word in words
                           where word.ToUpper().Contains(term.ToUpper())
                           select word;

            Console.WriteLine(@"Task 3 -- The word ""{0}"" occurs {1} times.",
                term, findWord.Count());
        }

        private static void GetMostCommonWords(string[] words)
        {
            var frequencyOrder = from word in words
                                 where word.Length > 6
                                 group word by word into g
                                 orderby g.Count() descending
                                 select g.Key;

            var commonWords = frequencyOrder.Take(10);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Task 2 -- The most common words are:");
            foreach (var v in commonWords)
            {
                sb.AppendLine("  " + v);
            }
            Console.WriteLine(sb.ToString());
        }

        private static string GetLongestWord(string[] words)
        {
            var longestWord = (from w in words
                               orderby w.Length descending
                               select w).First();

            Console.WriteLine("Task 1 -- The longest word is {0}", longestWord);
            return longestWord;
        }


        // An http request performed synchronously for simplicity.
        static string[] CreateWordArray(string uri)
        {
            Console.WriteLine("Retrieving from {0}", uri);

            // Download a web page the easy way.
            string s = new WebClient().DownloadString(uri);

            // Separate string into an array of words, removing some common punctuation.
            return s.Split(
                new char[] { ' ', '\u000A', ',', '.', ';', ':', '-', '_', '/' },
                StringSplitOptions.RemoveEmptyEntries);
        }
        #endregion
    }

    /* Output (May vary on each execution):
        Retrieving from http://www.gutenberg.org/dirs/etext99/otoos610.txt
        Response stream received.
        Begin first task...
        Begin second task...
        Task 2 -- The most common words are:
          species
          selection
          varieties
          natural
          animals
          between
          different
          distinct
          several
          conditions

        Begin third task...
        Task 1 -- The longest word is characteristically
        Task 3 -- The word "species" occurs 1927 times.
        Returned from Parallel.Invoke
        Press any key to exit  
     */
}