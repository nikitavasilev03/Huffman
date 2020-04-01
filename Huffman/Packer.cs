using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Huffman
{
    public class Packer
    {
        private Dictionary<byte, int> tableFrequency = null;
        private HuffmanTree tree = null;
        private Dictionary<byte, string> tableCodes = null;
        private byte[] m_bytes = null;

        public Dictionary<byte, int> TableFrequency { get => tableFrequency; }
        public HuffmanTree HuffmanTree { get => tree; }
        public Dictionary<byte, string> TableCodes { get => tableCodes; }
        public byte[] Bytes { get => m_bytes; }


        public void Packing(byte[] bytes)
        {
            tableFrequency = CreateTableFrequency(bytes);
            tree = CreateHuffmanTree(tableFrequency);
            tableCodes = tree.GetCodes().OrderBy(p => p.Value.Length).ToDictionary(p => p.Key, p => p.Value);
            m_bytes = Encoding(bytes, tableCodes);
        }

        public void Unpacking(Dictionary<byte, string> table, byte[] bytes)
        {
            tableFrequency = null;
            tree = null;
            tableCodes = table;
            m_bytes = Decoding(bytes, table);
        }

        private byte[] Decoding(byte[] bytes, Dictionary<byte, string> dict)
        {
            string s = recoveryAndUnionBytes(bytes);
            List<byte> lst = new List<byte>();
            int i = 0;
            do
            {
                foreach (var subStr in dict.Values)
                {
                    if (i + subStr.Length >= s.Length)
                        return lst.ToArray();
                    if (subStr == s.Substring(i, subStr.Length))
                    {
                        lst.Add(dict.FirstOrDefault(p => p.Value == subStr).Key);
                        i += subStr.Length;

                        break;
                    }
                }
            } while (i < s.Length);
            return lst.ToArray();
        }

        private string recoveryAndUnionBytes(byte[] bytes)
        {
            StringBuilder str = new StringBuilder();
            string t = "";
            int count = 0;
            foreach (var item in bytes)
            {
                t = Convert.ToString(item, 2);
                if (t.Length < 8)
                {
                    count = 8 - t.Length;
                    str.Append('0', count).Append(t);
                }
                else
                    str.Append(t);
            }
            return str.ToString();
        }

        private byte[] Encoding(byte[] array, Dictionary<byte, string> table)
        {
            string s = repairString(replaceAndUnionBytes(array, table));
            List<byte> lst = new List<byte>();
            for (int i = 0; i < s.Length; i += 8)
            {
                int x = Convert.ToInt32(s.Substring(i, 8), 2);
                lst.Add((byte)x);
            }
            return lst.ToArray();
        }
        private string repairString(string s)
        {
            if (s.Length % 8 != 0)
            {
                StringBuilder str = new StringBuilder(s);
                int count = 8 - s.Length % 8;
                str.Append('0', count);
                s = str.ToString();
            }
            return s;
        }
        private string replaceAndUnionBytes(byte[] array, Dictionary<byte, string> table)
        {
            StringBuilder str = new StringBuilder();
            foreach (var key in array)
            {
                str.Append(table[key]);
            }
            return str.ToString();
        }
        private HuffmanTree CreateHuffmanTree(Dictionary<byte, int> table)
        {
            List<HuffmanTree> trees = new List<HuffmanTree>();
            foreach (var p in table)
                trees.Add(new HuffmanTree(new Node(p.Key, p.Value)));
            while (trees.Count > 1)
            {
                HuffmanTree tree = new HuffmanTree();
                tree.AddTreeLeft(trees[0]);
                tree.AddTreeRight(trees[1]);
                trees.RemoveAt(0);
                trees.RemoveAt(0);
                trees.Add(tree);
                trees = trees.OrderBy(x => x.Frequency).ToList();
            }
            return trees.First();
        }

        private Dictionary<byte, int> CreateTableFrequency(byte[] bytes)
        {
            byte[] keys = bytes.Distinct().ToArray();
            Dictionary<byte, int> dict = new Dictionary<byte, int>();
            foreach (var key in keys)
            {
                int count = 0;
                for (int i = 0; i < bytes.Length; i++)
                    if (bytes[i] == key)
                        count++;
                dict.Add(key, count);
            }
            return dict.OrderBy(p => p.Value).ToDictionary(p => p.Key, p => p.Value);
        }
    }
}
