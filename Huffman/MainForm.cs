using System;
using System.IO;
using System.Windows.Forms;

namespace Huffman
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            openFileDialog1.InitialDirectory = Directory.GetCurrentDirectory();
            saveFileDialog1.InitialDirectory = Directory.GetCurrentDirectory();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                openFileDialog1.Filter = "";
                saveFileDialog1.Filter = "MyZIP (.myzip)|*.myzip|All files|*.*";
                if (openFileDialog1.ShowDialog(this) != DialogResult.OK)
                    return;
                if (saveFileDialog1.ShowDialog(this) != DialogResult.OK)
                    return;

                string pathOpen = openFileDialog1.FileName;
                string pathSave = saveFileDialog1.FileName;

                //Вариант 1 (Простое сжатие)
                //Packer packer = new Packer();
                //var bytes = FileWorker.ReadFile(pathOpen);
                //packer.Packing(bytes);
                //FileWorker.SavePackFile(pathSave, packer.TableCodes, packer.Bytes);

                //Вариант 2 (Многоуровневое сжатие)
                if (!FilePacker.Pack(pathOpen, pathSave, (int)numericUpDown1.Value))
                    MessageBox.Show("При сжатии произошла ошибка!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                openFileDialog1.Filter = "MyZIP (.myzip)|*.myzip|All files|*.*";
                saveFileDialog1.Filter = "";
                if (openFileDialog1.ShowDialog(this) != DialogResult.OK)
                    return;
                if (saveFileDialog1.ShowDialog(this) != DialogResult.OK)
                    return;

                string pathOpen = openFileDialog1.FileName;
                string pathSave = saveFileDialog1.FileName;

                //Вариант 1 (Простое сжатие)
                //Packer packer = new Packer();
                //FileWorker.ReadPackFile(pathOpen, out var table, out var bytes);
                //packer.Unpacking(table, bytes);
                //FileWorker.SaveFile(pathSave, packer.Bytes);

                //Вариант 2 (Многоуровневое сжатие)
                if (!FilePacker.UnPack(pathOpen, pathSave))
                    MessageBox.Show("При разжатии произошла ошибка");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
