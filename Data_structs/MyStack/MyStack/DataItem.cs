using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStack
{
    class DataItem
    {
        //Data of a node
        public string Name;
        public string City;

        public DataItem()
        {

        }
        
        public DataItem(string n, string c)
        {
            Name = n;
            City = c;
        }
    }
}
