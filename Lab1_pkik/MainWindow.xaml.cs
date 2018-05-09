using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lab1_pkik
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        #region Text_Changed
        private string TextIsChanged(string text)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(text, "[^0-9]"))
            {
                text = text.Remove(text.Length - 1);
            }
            return text;
        }
        private void tbPrimeNum_TextChanged(object sender, TextChangedEventArgs e)
        {
            tbPrimeNum.Text = TextIsChanged(tbPrimeNum.Text);
        }
        private void tbRange_TextChanged(object sender, TextChangedEventArgs e)
        {
            tbRange.Text = TextIsChanged(tbRange.Text);
        }
        private void tbA_TextChanged(object sender, TextChangedEventArgs e)
        {
            tbA.Text = TextIsChanged(tbA.Text);
        }

        private void tbB_TextChanged(object sender, TextChangedEventArgs e)
        {
            tbB.Text = TextIsChanged(tbB.Text);
        }
        private void tbBn_TextChanged(object sender, TextChangedEventArgs e)
        {
            tbBn.Text = TextIsChanged(tbBn.Text);
        }
        private void tbN_TextChanged(object sender, TextChangedEventArgs e)
        {
            tbN.Text = TextIsChanged(tbN.Text);
        }
        private void tbM_TextChanged(object sender, TextChangedEventArgs e)
        {
            tbM.Text = TextIsChanged(tbM.Text);
        }
        private void tbNn_TextChanged(object sender, TextChangedEventArgs e)
        {
            tbNn.Text = TextIsChanged(tbNn.Text);
        }
        private void tbK_TextChanged(object sender, TextChangedEventArgs e)
        {
            tbK.Text = TextIsChanged(tbK.Text);
        }
        #endregion
        List<int> Sito;
        #region Click Methods
        private void bPrimeNum_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IsPrime(Int32.Parse(tbPrimeNum.Text)))
                {
                    labIsPrime.Content = "pierwsza";
                    labIsPrime.Foreground = new SolidColorBrush(Colors.Green);
                }
                else
                {
                    labIsPrime.Content = "nie pierwsza";
                    labIsPrime.Foreground = new SolidColorBrush(Colors.Red);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void bSieveOfErato_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Sito = new List<int>();
                tblPrimes.Text = "";
                int max = int.Parse(tbRange.Text) + 1;
                bool[] is_prime = MakeSieve(max);
                for (int i = 2; i < max; i++)
                    if (is_prime[i]) Sito.Add(i);
                foreach (var item in Sito)
                {
                    tblPrimes.Text += item + " ";
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void bSaveToFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Text file (*.txt)|*.txt|C# file (*.cs)|*.cs";
                if (saveFileDialog.ShowDialog() == true)
                    File.WriteAllText(saveFileDialog.FileName, "Liczby pierwsze dla przedziału (" + tbRange.Text + ") obliczone metodą sita Eratostenesa:" + Environment.NewLine + tblPrimes.Text);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        private void bCalcGCD_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                switch (combMethod.SelectedIndex)
                {
                    case 0:
                        {
                            labGCD.Content = GCD(Int32.Parse(tbA.Text), Int32.Parse(tbB.Text));
                            break;
                        }
                    case 1:
                        {
                            int[] temp = ExtendedGDC(Int32.Parse(tbA.Text), Int32.Parse(tbB.Text));
                            labGCD.Content = "k=" + temp[0] + " u=" + temp[1] + " v=" + temp[2];
                            break;
                        }
                    default: break;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        private void bCalcPowModFast_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                labPowModFast.Content = PowerModFast(Int32.Parse(tbBn.Text), Int32.Parse(tbN.Text), Int32.Parse(tbM.Text));
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        private void bCalcMiller_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MillerRabin(Int32.Parse(tbNn.Text), Int32.Parse(tbK.Text)))
                {
                    labMiller.Content = "prawdopodobnie pierwsza";
                    labMiller.Foreground = new SolidColorBrush(Colors.Green);
                }
                else
                {
                    labMiller.Content = "prawdopodobnie nie pierwsza";
                    labMiller.Foreground = new SolidColorBrush(Colors.Red);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        #endregion
        public static bool IsPrime(int _number)
        {
            if (_number == 1) return false;
            if (_number == 2) return true;
            if (_number % 2 == 0) return false;

            var boundary = (int)Math.Floor(Math.Sqrt(_number));

            for (int i = 3; i <= boundary; i += 2)
            {
                if (_number % i == 0) return false;
            }

            return true;
        }
        private bool[] MakeSieve(int _max)
        {
            // Make an array indicating whether numbers are prime.
            bool[] is_prime = new bool[_max + 1];
            for (int i = 2; i <= _max; i++) is_prime[i] = true;

            // Cross out multiples.
            for (int i = 2; i <= _max; i++)
            {
                // See if i is prime.
                if (is_prime[i])
                {
                    // Knock out multiples of i.
                    for (int j = i * 2; j <= _max; j += i)
                        is_prime[j] = false;
                }
            }
            return is_prime;
        }
        
        static int GCD(int _a, int _b)
        {
            return _b == 0 ? _a : GCD(_b, _a % _b);
        }
        static int[] ExtendedGDC(int _a, int _b)
        {
            int[] value = new int[3];
            int a = _a, b = _b, q;
            int x0 = 1, xn = 1;
            int y0 = 0, yn = 0;
            int x1 = 0;
            int y1 = 1;
            int r = a % b;
            
            while (r > 0)
            {
                q = a / b;


                xn = x0 - q * x1;
                yn = y0 - q * y1;

                x0 = x1;
                y0 = y1;
                x1 = xn;
                y1 = yn;
                a = b;
                b = r;
                r = a % b;

            }
            value[0] = b; value[1] = yn; value[2] = xn;
            return value;
        }
        public static int PowerModFast(int a, int b, int m)
        {
            int i;
            long result = 1;
            long x = a % m;

            for (i = 1; i <= b; i <<= 1)
            {
                x %= m;
                if ((b & i) != 0)
                {
                    result *= x;
                    result %= m;
                }
                x *= x;
            }

            return (int)result;
        }
       
        private static int[] powerOf2 = {
            1<<0,  1<<1,  1<<2,  1<<3,  1<<4,  1<<5,  1<<6,
            1<<7,  1<<8,  1<<9,  1<<10, 1<<11, 1<<12, 1<<13,
            1<<14, 1<<15, 1<<16, 1<<17, 1<<18, 1<<19, 1<<20,
            1<<21, 1<<22, 1<<23, 1<<24, 1<<25, 1<<26, 1<<27,
            1<<28, 1<<29, 1<<30
        };
        private static Boolean MillerRabin(int n, int k)
        {
            int s = 0;
            int s2 = 1;
            int a, d, i, r, prime;
            Random rand = new Random();

            if (n < 4)
            {
                return true;
            }
            if (n % 2 == 0)
            {
                return false;
            }

            // calculate s and d
            while ((s2 & (n - 1)) == 0)
            {
                s += 1;
                s2 <<= 1;
            }
            d = n / s2;

            // try k times
            for (i = 0; i < k; i++)
            {
                a = rand.Next(n - 1) + 1;
                if (PowerModFast(a, d, n) != 1)
                {
                    prime = 0;
                    for (r = 0; r <= s - 1; r++)
                    {
                        if (PowerModFast(a, powerOf2[r] * d, n) == n - 1)
                        {
                            prime = 1;
                            break;
                        }
                    }
                    if (prime == 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
