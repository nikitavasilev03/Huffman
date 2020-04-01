using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;
using System;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Huffman
{
    public partial class MainForm : Form
    {
        BinaryTree tree = null;
        Dictionary<byte, string> codes = null;
        byte[] fileBytes;
        public MainForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            Image bmp = Image.FromFile("./1.bmp");
            fileBytes = ImageToByte(bmp);

            var pairs = GetTableBytes(fileBytes);
            
            foreach (var key in pairs.Keys)
            {
                listBox1.Items.Add($"{key} : {pairs[key]}");
            }


            tree = CreateHuffmanTree(GetNodesFromDictionary(pairs));
            tree.SetCodes();
            codes = tree.GetCodes().OrderBy(pair => pair.Value.Length).ToDictionary(pair => pair.Key, pair => pair.Value);

            foreach (var key in codes.Keys)
            {
                listBox2.Items.Add($"{key} : {codes[key]}");
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (codes == null || fileBytes == null)
            {
                MessageBox.Show("Файл не считан!!!");
                return;
            }

            var bytes = ConvertStringToBytes(CovertToString(fileBytes, codes));
            //File.WriteAllBytes("./output.myzip", bytes);
            SaveZIP("./output.myzip", codes, bytes);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var array = ReadFile("./output.myzip");
            string s = GetStringFromBytes(array);
            var bytes = Decoding(s, codes);
            SaveFileBmp("./output.bmp", bytes);
        }
        private void SaveZIP(string path, Dictionary<byte, string> dict, byte[] bytes)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, dict);
                formatter.Serialize(fs, bytes);
            }
        }
        private void SaveFileBmp(string path, byte[] bytes)
        {
            File.WriteAllBytes(path, bytes);
        }

        private byte[] Decoding(string s, Dictionary<byte, string> dict)
        {
            List<byte> bytes = new List<byte>();
            int i = 0;
            do
            {
                foreach (var subStr in dict.Values)
                {
                    if (i + subStr.Length >= s.Length)
                        return bytes.ToArray();
                    if (subStr == s.Substring(i, subStr.Length))
                    {
                        bytes.Add(dict.FirstOrDefault(p => p.Value == subStr).Key);
                        i += subStr.Length;
                        
                        break;
                    }
                }
            } while (i < s.Length);
            return bytes.ToArray();
        }

        private string GetStringFromBytes(byte[] bytes)
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
        private byte[] ReadFile(string path)
        {
            byte[] array = null;
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
                {
                    codes = (Dictionary<byte, string>)formatter.Deserialize(fs);
                    array = (byte[])formatter.Deserialize(fs);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return array;
        }

        private byte[] ConvertStringToBytes(StringBuilder str)
        {
            string s = str.ToString();
            if (s.Length % 8 != 0)
            {
                int count = 8 - s.Length % 8;
                for (int i = 0; i < count; i++)
                    str.Append("0");
                s = str.ToString();
            }
                
            List<byte> lst = new List<byte>();
            
            for (int i = 0; i < str.Length; i += 8)
            {
                int x = Convert.ToInt32(s.Substring(i, 8), 2);
                lst.Add((byte)x);
            }
            return lst.ToArray();
        }
        private StringBuilder CovertToString(byte[] array, Dictionary<byte, string> tcodes)
        {
            StringBuilder str = new StringBuilder();
            foreach (var key in array)
            {
                str.Append(tcodes[key]);
            }
            return str;
        } 
        private BinaryTree CreateHuffmanTree(List<Node> nodes)
        {
            List<BinaryTree> trees = new List<BinaryTree>();
            foreach (var node in nodes)
                trees.Add(new BinaryTree(node));
            while (trees.Count > 1)
            {
                BinaryTree tree = new BinaryTree();
                tree.AddTreeLeft(trees[0]);
                tree.AddTreeRight(trees[1]);
                trees.RemoveAt(0);
                trees.RemoveAt(0);
                trees.Add(tree);
                trees = trees.OrderBy(x => x.Frequency).ToList();
            }
            return trees.First();
        }

        private List<Node> GetNodesFromDictionary(Dictionary<byte, int> dict)
        {
            List<Node> nodes = new List<Node>();
            foreach (var key in dict.Keys)
            {
                nodes.Add(new Node(key, dict[key]));
            }
            return nodes;
        }

        private Dictionary<byte, int> GetTableBytes(byte[] bytes)
        {
            byte[] keys = bytes.Distinct().ToArray();
            //Array.Sort(keys);
            Dictionary<byte, int> pairs = new Dictionary<byte, int>();
            foreach (var key in keys)
            {
                int count = 0;
                for (int i = 0; i < bytes.Length; i++)
                    if (bytes[i] == key)
                        count++;
                pairs.Add(key, count);
            }
            var temp = pairs.OrderBy(pair => pair.Value);
            Dictionary<byte, int> sort_pairs = new Dictionary<byte, int>();
            foreach (var item in temp)
            {
                sort_pairs.Add(item.Key, item.Value);
            }
            return sort_pairs;
        }

        public static byte[] ImageToByte(Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }

    }
}
