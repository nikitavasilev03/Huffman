using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Huffman
{
    public static class FileWorker
    {
        public static void SavePackFile(string path, Dictionary<byte, string> table, byte[] bytes)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, 0);         //Глубина сжатия
                formatter.Serialize(fs, table);
                formatter.Serialize(fs, bytes);
            }
        }
        public static void SaveFile(string path, byte[] bytes)
        {
            File.WriteAllBytes(path, bytes);
        }
        public static void ReadPackFile(string path, out Dictionary<byte, string> table, out byte[] bytes)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                int level = (int)formatter.Deserialize(fs);         //Глубина сжатия
                table = (Dictionary<byte, string>)formatter.Deserialize(fs);
                bytes = (byte[])formatter.Deserialize(fs);
            }
        }
        public static byte[] ReadFile(string path)
        {
            return File.ReadAllBytes(path);
        }
    }
}
