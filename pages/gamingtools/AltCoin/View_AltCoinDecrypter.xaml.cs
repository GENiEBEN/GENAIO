using System;
using System.Collections.Generic;
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
using System.Security.Cryptography;

namespace TreeViewMenu
{
    /// <summary>
    /// Interaction logic for View_AltCoinDecrypter.xaml
    /// </summary>
    public partial class View_AltCoinDecrypter : UserControl
    {
        private readonly static byte[] Salt;

        private static byte[] keyArray;

        static View_AltCoinDecrypter()
        {
            View_AltCoinDecrypter.Salt = Encoding.UTF8.GetBytes("$field-B5484828125B10210668564D9634379E1F9CC8EE");
            View_AltCoinDecrypter.keyArray = Encoding.UTF8.GetBytes("LYtrdjzwrjLhfJE2DTgFzuG2Bmmv8CzQ");
        }

        public View_AltCoinDecrypter()
        {
            InitializeComponent();
        }

        private static string Decrypt(string toDecrypt)
        {
            byte[] numArray = Convert.FromBase64String(toDecrypt);
            RijndaelManaged rijndaelManaged = new RijndaelManaged()
            {
                Key = View_AltCoinDecrypter.keyArray,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
            ICryptoTransform cryptoTransform = rijndaelManaged.CreateDecryptor();
            byte[] numArray1 = cryptoTransform.TransformFinalBlock(numArray, 0, (int)numArray.Length);
            return Encoding.UTF8.GetString(numArray1);
        }

        private static string Encrypt(string toEncrypt)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(toEncrypt);
            RijndaelManaged rijndaelManaged = new RijndaelManaged()
            {
                Key = View_AltCoinDecrypter.keyArray,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
            ICryptoTransform cryptoTransform = rijndaelManaged.CreateEncryptor();
            byte[] numArray = cryptoTransform.TransformFinalBlock(bytes, 0, (int)bytes.Length);
            return Convert.ToBase64String(numArray, 0, (int)numArray.Length);
        }

        private void tb_In_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                tb_Out.Text = Decrypt(tb_In.Text);
            }
            catch
            {
            }
            Console.Write("Error decrypting");
        }

        private void tb_Out_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                tb_In.Text = Encrypt(tb_Out.Text);
            }
            catch
            {
            }
            Console.Write("Error encrypting");
        }
    }
}
