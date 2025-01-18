using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;


namespace HashNCoder
{
    
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
           

        }

      

        private void E_Btn_Encode_Click(object sender, EventArgs e)
        {
            if (E_Combo_Algoritm.SelectedIndex == 0)
            {
                E_Txb_ResultText.Text = E_Combo_EnCodeDe.SelectedIndex == 0
                    ? Coding.EncodeBase64(E_Txb_CurrentText.Text)
                    : Coding.DecodeBase64(E_Txb_CurrentText.Text);

            }    
            else if (E_Combo_Algoritm.SelectedIndex == 1)
            {
                E_Txb_ResultText.Text = E_Combo_EnCodeDe.SelectedIndex == 0
                    ? Coding.URLEncode(E_Txb_CurrentText.Text)
                    : Coding.URLDecode(E_Txb_CurrentText.Text);
            }
            else if (E_Combo_Algoritm.SelectedIndex == 2)
            {
                E_Txb_ResultText.Text = E_Combo_EnCodeDe.SelectedIndex == 0
                    ? Coding.HTMLEncode(E_Txb_CurrentText.Text)
                    : Coding.HTMLDecode(E_Txb_CurrentText.Text);
            }
            else if (E_Combo_Algoritm.SelectedIndex == 3)
            {
                E_Txb_ResultText.Text = E_Combo_EnCodeDe.SelectedIndex == 0
                    ? Coding.UnescapeEncode(E_Txb_CurrentText.Text)
                    : Coding.UnescapeDecode(E_Txb_CurrentText.Text);
            }

        }

     
        private void E_Combo_EnCodeDe_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (E_Combo_EnCodeDe.SelectedIndex == 0)
            {
                E_Btn_Encode.Text = "Encode";

            }
            else E_Btn_Encode.Text = "Decode";
        }

        private void E_Btn_Paste_Click(object sender, EventArgs e)
        {
            E_Txb_CurrentText.Text = Clipboard.GetText();
           
        }

        private bool EBtnCopyIsProcessing = false; 
        private async void E_Btn_Copy_ClickAsync(object sender, EventArgs e)
        {
            if (EBtnCopyIsProcessing) return;

            if (!string.IsNullOrWhiteSpace(E_Txb_ResultText.Text))
            { 
            
                Clipboard.SetText(E_Txb_ResultText.Text);
                if (EBtnCopyIsProcessing) return;

                var originalIcon = Properties.Resources.Copy_icon_30px;

                E_Btn_Copy.Image = Properties.Resources.Icon_Check_30px; 
             
                await Task.Delay(1000);
               
                E_Btn_Copy.Image = originalIcon;
                EBtnCopyIsProcessing = false;
            }
            else
            {
                MessageBox.Show("The result field is empty. There is nothing to copy.", "Warning!");
            }
            

        }

        private void E_Btn_Swap_Click(object sender, EventArgs e)
        {
            string temp = E_Txb_ResultText.Text;
            E_Txb_ResultText.Text = E_Txb_CurrentText.Text;
            E_Txb_CurrentText.Text = temp;
        }


       

        private void AES_Btn_Encode_Click(object sender, EventArgs e)
        {
            CipherMode cipherMode;
            if (AES_Combo_Algoritm.SelectedIndex == 0) cipherMode = CipherMode.ECB;
            else cipherMode = CipherMode.CBC;

            byte[] key = Encoding.UTF8.GetBytes(AES_TxtBx_Key.Text);

            int keySize = int.TryParse(AES_Combo_KeySize.SelectedItem?.ToString(), out int size) ? size : 128;


            AES_Txb_ResultText.Text = E_Combo_EnCodeDe.SelectedIndex == 0
                ? Coding.AES_Encrypt(AES_Txb_CurrentText.Text, AES_TxtBx_Key.Text, cipherMode, keySize).ToString()
                : Coding.AES_Decrypt(AES_Txb_CurrentText.Text, AES_TxtBx_Key.Text, cipherMode, keySize);
            
            
        }

        private void AES_Btn_GenerateKey_Click(object sender, EventArgs e)
        {
            int keySize = int.TryParse(AES_Combo_KeySize.SelectedItem?.ToString(), out int size) ? size : 128;
   
            byte[] key = Coding.AES_GenerateKey(keySize);
       
            AES_TxtBx_Key.Text = Convert.ToBase64String(key);

        }
    }
}
