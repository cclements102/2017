using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkedList
{
    class Program
    {
        static void Main(string[] args)
        {
            //create list and add to it
            MyList list = new MyList();
            list.AddToEnd(9);
            list.AddToEnd(2);
            list.AddToBeginning(3);
            list.AddToEnd(6);
            list.Print();
            Console.ReadLine();
        }
    }

    public class Node
    {
        public int data;
        public Node next;

        public Node(int i)
        {
            data = i;
            next = null;
        }
        public void Print()
        {
            Console.WriteLine("|" + data + "|");
            if (next != null)
            {
                next.Print();
            }
        }
        public void AddToEnd(int data)
        {
            if (next == null) //end of the list
            {
                next = new Node(data);
            }
            else
            {
                next.AddToEnd(data);
            }
        }
    }

    public class MyList
    {
        public Node HeadNode;

        public MyList()
        {
            HeadNode = null;
        }

        public void AddToEnd(int data)
        {
            if(HeadNode == null)
            {
                HeadNode = new Node(data);
            }
            else
            {
                HeadNode.AddToEnd(data);
            }
        }

        public void AddToBeginning(int data)
        {
            if(HeadNode == null)
            {
                HeadNode = new Node(data);
            }
            else
            {
                Node temp = new Node(data);
                temp.next = HeadNode;
                HeadNode = temp;
            }
        }
        
        public void Print()
        {
            if(HeadNode != null)
            {
                HeadNode.Print();
            }

        }
    }
}
