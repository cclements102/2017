using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyQueue
{
    class Program
    {
        static void Main(string[] args)
        {
            Queue myQ = new Queue();
            string Dequeued;//the string being taken from the queue

            myQ.Enqueue("item 1");
            myQ.Enqueue("item 2");
            myQ.Enqueue("item 3");
            myQ.Enqueue("item 4");


            Console.WriteLine("There are {0} items in the Queue", myQ.Count);

            while(myQ.Count > 0)
            {
                Dequeued = (string)myQ.Dequeue();
                Console.WriteLine("Dequeueing item: {0}", Dequeued);
            }
            Console.ReadLine();

        }
    }
}
