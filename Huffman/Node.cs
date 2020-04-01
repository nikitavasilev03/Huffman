using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Huffman
{
    public class Node
    {
        public byte? Key { get; set; } = null;
        public int Count { get; set; } = 0;
        public string Code { get; set; } = "";
        public Node Left { get; set; } = null;
        public Node Right { get; set; } = null;
        public Node()
        {

        }
        public Node(byte key, int count)
        {
            Key = key;
            Count = count;
        }
    }
}
