using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStack
{
    class Stacks
    {
        private Node Top;

        public void Push(DataItem d)
        {
            Node TempNode = new Node();
            TempNode.Data = d;

            TempNode.Next = Top;
            Top = TempNode;
        }

        public DataItem Pop()
        {
            if(Top == null) return null; //if top of stack is null then return null

            Node Popped = Top;

            Top = Top.Next;
            Popped.Next = null;
            
            return Popped.Data;
        }

        public DataItem Peek()
        {
            return Top.Data;
        }
    }
}
