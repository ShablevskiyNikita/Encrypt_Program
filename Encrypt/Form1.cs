using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Numerics;

namespace Encrypt
{
    public partial class Form1 : Form
    {
        public string fileName;
        public string saveName;
        public Form1()
        {
            InitializeComponent();
        }

        private void Instruction_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Для того аби зашифрувати ваші дані потрібно: " +
                "\n 1. Обрати файл(у форматі txt) с текстом, що необхідно зашифрувати. " +
                "\n 2. Ввести у відповідні поля два простих числа p і q ." +
                "\n 3. Натиснути кнопку Encrypt , і обрати в який файл записати зашифровані дані " +
                "\n 4. Отриманий відкритий ключ і зашифровані дані надіслати вашому адресанту");
        }
        private void Encrypt_Click(object sender, EventArgs e)
        {
            if ((textBox_p.Text.Length > 0) && (textBox_q.Text.Length > 0))
            {
                long p = Convert.ToInt64(textBox_p.Text);
                long q = Convert.ToInt64(textBox_q.Text);
                if (IsTheNumberSimple(p) && IsTheNumberSimple(q))
                {
                    string s = "";
                    StreamReader sr = new StreamReader(fileName, Encoding.Default);
                    while (!sr.EndOfStream)
                    {
                        s += sr.ReadLine();
                    }
                   
                    sr.Close();

                    long n = p * q;
                    long m = (p - 1) * (q - 1);
                    long d = Calculate_d(m);
                    long e_ = Calculate_e(d, m);

                    List<string> result = RSA_Endoce(s, e_, n);

                    
                    saveFileDialog1.Filter = "(*.txt) | *.txt";
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        // получаем выбранный файл
                        saveName = saveFileDialog1.FileName;
                        
                    }


                    StreamWriter sw = new StreamWriter(saveName);
                    foreach (string item in result)
                    {
                        sw.WriteLine(item);
                    }
                    sw.Close();

                    textBox_d.Text = d.ToString();
                    textBox_n.Text = n.ToString();
                    MessageBox.Show("Your data was encrypted");
                }
                else
                    MessageBox.Show("p or q - are not prime number!");
            }
            else
                MessageBox.Show("Enter p or q!");
        }
        private bool IsTheNumberSimple(long n)
        {
            if (n < 2)
                return false;

            if (n == 2)
                return true;

            for (long i = 2; i < n; i++)
                if (n % i == 0)
                    return false;

            return true;
        }
        private long Calculate_d(long m)
        {
            long d = m - 1;

            for (long i = 2; i <= m; i++)
                if ((m % i == 0) && (d % i == 0)) //если имеют общие делители
                {
                    d--;
                    i = 1;
                }

            return d;
        }
        private long Calculate_e(long d, long m)
        {
            long e = 10;

            while (true)
            {
                if ((e * d) % m == 1)
                    break;
                else
                    e++;
            }

            return e;
        }
        private List<string> RSA_Endoce(string s, long e, long n)
        {
            List<string> result = new List<string>();

            BigInteger bi = new BigInteger();

            for (int i = 0; i < s.Length; i++)
            {
                char currentSymbol = s[i];
                bi = BigInteger.Pow(currentSymbol, (int)e)%n;
                result.Add(bi.ToString());
            }

            return result;
        }

        private void OpenFile_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "(*.txt) | *.txt";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                fileName = openFileDialog1.FileName;
            }
        }
        private void Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
