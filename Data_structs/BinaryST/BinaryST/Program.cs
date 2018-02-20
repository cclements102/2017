using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryST
{
    class Program
    {
        static void Main(string[] args)
        {
            //create random numbers to be put in the tree
            Random ran_gen = new Random();
            int ran_num = ran_gen.Next(0, 100);

            //initialize tree with initial random number
            Tree MyTree = new Tree(ran_num);

            //add random numbers to tree for a tree with 15 values
            for(int i = 0; i < 14; i++)
            {
                ran_num = ran_gen.Next(0, 100);
                MyTree.AddR(ran_num);
            }

            //print
            string TreeOut = "";
            MyTree.Print(null, ref TreeOut);
            Console.WriteLine("Binary tree traversal in ascending order: " + TreeOut);
            Console.ReadLine();
        }
    }
    class Node
    {
        public int value;
        public Node left;
        public Node right;

        public Node(int initial)
        {
            value = initial;
            left = null;
            right = null;
        }
    }

    class Tree
    {
        Node Top;

        public Tree()
        {
            //constructor for empty top node
            Top = null;
        }

        public Tree(int initial)
        {
            //constructor for top node with a value
            Top = new Node(initial);
        }

        public void Add(int value)
        {
            //non-recursive add, search for null leaf
            if(Top == null)
            {
                //empty tree, make top equal to value
                Node NewNode = new Node(value);
                Top = NewNode;
                return;
            }

            Node CurrentNode = Top;//keeps track of working node
            bool added = false;//keeps track of if the new value has been added yet
            while(!added)
            {
                //look for where to add in tree
                if(value < CurrentNode.value)
                {
                    //traverse left
                    if(CurrentNode.left == null)
                    {
                        //Add new item if left node is empty
                        Node NewNode = new Node(value);
                        CurrentNode.left = NewNode;
                        added = true;
                    }
                    else
                    {
                        CurrentNode = CurrentNode.left;
                    }
                }
                if(value >= CurrentNode.value)
                {
                    //traverse right
                    if(CurrentNode.right == null)
                    {
                        //Add new item if right node is empty
                        Node NewNode = new Node(value);
                        CurrentNode.right = NewNode;
                        added = true;
                    }
                    else
                    {
                        CurrentNode = CurrentNode.right;
                    }
                }

            }
        }
        public void AddR(int value)
        {
            //recursive add
            AddRC(ref Top, value);
        }

        private void AddRC(ref Node N, int value)
        {
            //private recursive search for where to put new node, pass n by reference to add a value to a null node
            if(N == null)
            {
                Node NewNode = new Node(value);
                N = NewNode;
                return;
            }
            if(value < N.value)
            {
                AddRC(ref N.left, value);
                return;
            }
            if(value >= N.value)
            {
                AddRC(ref N.right, value);
                return;
            }

        }

        public void Print(Node N, ref string PrintString)
        {
            //print tree in ascending order
            if(N == null)
            {
                N = Top;
            }

            if(N.left != null) //traverse the tree to the left most leaf
            {
                Print(N.left, ref PrintString);
                PrintString = PrintString + N.value.ToString().PadLeft(3);
            }
            else //when there isn't anywhere left to traverse
            {
                PrintString = PrintString + N.value.ToString().PadLeft(3);
            }
            if (N.right != null)//traverse right after traversing left
            {
                Print(N.right, ref PrintString);
            }
        }
    }
}
