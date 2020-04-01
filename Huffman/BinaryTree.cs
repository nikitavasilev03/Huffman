using System.Collections.Generic;

namespace Huffman
{
    public class BinaryTree
    {
        private Node Root { get; set; }

        public int Frequency { get => sumValue(Root); }

        public BinaryTree()
        {
            Root = new Node();
        }

        public BinaryTree(Node node)
        {
            Root = node;
        }

        public void AddTreeLeft(BinaryTree tree)
        {
            Root.Left = tree.Root;
        }

        public void AddTreeRight(BinaryTree tree)
        {
            Root.Right = tree.Root;
        }

        public void SetCodes()
        {
            setCodes(Root);
        }

        public Dictionary<byte, string> GetCodes()
        {
            return getCodes(Root, new Dictionary<byte, string>(), "");
        }

        private Dictionary<byte, string> getCodes(Node node, Dictionary<byte, string> dict, string str)
        {
            str += node.Code;
            if (node.Key != null)
            {
                dict.Add((byte)node.Key, str);
            }
            if (node.Left != null)
                getCodes(node.Left, dict, str);
            if (node.Right != null)
                getCodes(node.Right, dict, str);
            return dict;
        }

        private void setCodes(Node node)
        {
            if (node.Left != null) 
            {
                node.Left.Code = "0";
                setCodes(node.Left);
            }  
            if (node.Right != null)
            {
                node.Right.Code = "1";
                setCodes(node.Right);
            }           
        }

        private int sumValue(Node node)
        {
            int sum = node.Count;
            if (node.Left != null)
                sum += sumValue(node.Left);
            if (node.Right != null)
                sum += sumValue(node.Right);
            return sum;
        }
    }
}
