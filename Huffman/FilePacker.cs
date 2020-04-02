using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Huffman
{
    public static class FilePacker
    {
        public static bool Pack(string pathOpen, string pathSave = "./mypackfile.myzip", int level = 1)
        {
            try
            {
                var bytes = FileWorker.ReadFile(pathOpen);
                FileWorker.SaveFile(pathSave, pack(bytes, new Packer(), level, new BinaryFormatter()));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static bool UnPack(string pathOpen, string pathSave = "./myunpackfile.myzip")
        {
            try
            {
                var bytes = FileWorker.ReadFile(pathOpen);
                FileWorker.SaveFile(pathSave, unpack(bytes, new Packer(), new BinaryFormatter()));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static byte[] unpack(byte[] bytes, Packer packer, BinaryFormatter formatter)
        {
            byte[] buffer = bytes;
            int level = 0;
            Dictionary<byte, string> table;
            byte[] tempBytes;
            do
            {
                using (MemoryStream ms = new MemoryStream(buffer))
                {
                    level = (int)formatter.Deserialize(ms);
                    table = (Dictionary<byte, string>)formatter.Deserialize(ms);
                    tempBytes = (byte[])formatter.Deserialize(ms);
                }
                packer.Unpacking(table, tempBytes);
                buffer = packer.Bytes;
            } while (level > 0);   
            return buffer;
        }

        private static byte[] pack(byte[] bytes, Packer packer, int level, BinaryFormatter formatter)
        {
            byte[] buffer = bytes;
            for (int i = 0; i < level; i++)
            {
                packer.Packing(buffer);
                using (MemoryStream ms = new MemoryStream())
                {
                    formatter.Serialize(ms, i);
                    formatter.Serialize(ms, packer.TableCodes);
                    formatter.Serialize(ms, packer.Bytes);
                    buffer = ms.ToArray();
                }
            }
            return buffer;
        }
    }
}
