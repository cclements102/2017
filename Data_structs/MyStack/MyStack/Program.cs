using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStack
{
    class Program
    {
        static void Main(string[] args)
        {
            //Demonstrate the use of the stacks class

            DataItem D = new DataItem();

            Stacks stack = new Stacks();

            stack.Push(new DataItem("Clem", "Birmingham"));
            stack.Push(new DataItem("Jake", "Los Angeles"));

            D = stack.Peek();

            Console.WriteLine(D.Name);

            stack.Pop();

            D = stack.Pop();

            Console.WriteLine(D.Name);

            Console.ReadLine();
        }
    }
}
