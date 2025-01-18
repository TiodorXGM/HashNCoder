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
using Guna.UI2.WinForms;
using System.Security.Authentication;


namespace HashNCoder
{
    
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
           

        }

        private async void E_Btn_Encode_Click(object sender, EventArgs e)
        {
            bool hasError = false;

            if (string.IsNullOrWhiteSpace(E_Txb_CurrentText.Text))
            {
                await HighlightTextBoxAsync(E_Txb_CurrentText);
                hasError = true;
            }
            if (hasError)
            {
                MessageBox.Show("Please fill in the required fields.", "Warning");
                return;
            }

            if (E_Combo_EnCodeDe.SelectedIndex == 1 && E_Combo_Algoritm.SelectedIndex == 0)
            {
                if (!Coding.IsBase64String(E_Txb_CurrentText.Text))
                {
                    await HighlightTextBoxAsync(E_Txb_CurrentText);
                    MessageBox.Show(
                               "The string is not a valid Base64 string.\n\n" +
                               "- Contain only letters (A-Z, a-z), digits (0-9), and the characters '+' and '/';\n" +
                               "- Have a length that is a multiple of 4 (including '=' padding characters);",
                               "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;

                }
            }


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
            await CopyToClipboardAsync(
                    E_Btn_Copy,                          
                    E_Txb_ResultText,                    
                    Properties.Resources.Copy_icon_30px, 
                    Properties.Resources.Icon_Check_30px 
                );
        }

        private void E_Btn_Swap_Click(object sender, EventArgs e)
        {
            SwapText(E_Txb_ResultText, E_Txb_CurrentText);
        }

        private void SwapText(Guna2TextBox textBox1, Guna2TextBox textBox2)
        {
            string temp = textBox1.Text;
            textBox1.Text = textBox2.Text;
            textBox2.Text = temp;
        }

        private bool IsCopyProcessing = false;

        private async Task CopyToClipboardAsync(Guna2Button button,
                                                Guna2TextBox textBox,
                                                Image originalIcon,
                                                Image successIcon)
        {
            if (IsCopyProcessing) return;
            
            if (!string.IsNullOrWhiteSpace(textBox.Text))
            {
                IsCopyProcessing = true;
                Clipboard.SetText(textBox.Text);
                button.Image = successIcon;
                await Task.Delay(1000);
                button.Image = originalIcon;
                IsCopyProcessing = false;
            }
            else
            {
                MessageBox.Show("The result field is empty. There is nothing to copy.", "Warning!");
            }
        }


        private async void AES_Btn_Encode_Click(object sender, EventArgs e)
        {
            bool hasError = false;
          
            if (string.IsNullOrWhiteSpace(AES_TxtBx_Key.Text))
            {
                await HighlightTextBoxAsync(AES_TxtBx_Key);
                hasError = true;
            }

            if (string.IsNullOrWhiteSpace(AES_Txb_CurrentText.Text))
            {
                await HighlightTextBoxAsync(AES_Txb_CurrentText);
                hasError = true;
            }
            else if (AES_Combo_EnCodeDe.SelectedIndex == 1) 
            {
                if (!Coding.IsBase64String(AES_Txb_CurrentText.Text))
                {
                    await HighlightTextBoxAsync(AES_Txb_CurrentText);
                    MessageBox.Show(
                               "The string is not a valid Base64 string.\n\n" +
                               "- Contain only letters (A-Z, a-z), digits (0-9), and the characters '+' and '/';\n" +
                               "- Have a length that is a multiple of 4 (including '=' padding characters);",
                               "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                 
                }
            }

            if (hasError)
            {
                MessageBox.Show("Please fill in the required fields.", "Warning");
                return;
            }
            try
            {
                CipherMode cipherMode = AES_Combo_Algoritm.SelectedIndex == 0 ? CipherMode.ECB : CipherMode.CBC;

                int keySize = int.TryParse(AES_Combo_KeySize.SelectedItem?.ToString(), out int size) ? size : 128;

                AES_Txb_ResultText.Text = AES_Combo_EnCodeDe.SelectedIndex == 0
                    ? Coding.AES_Encrypt(AES_Txb_CurrentText.Text, AES_TxtBx_Key.Text, cipherMode, keySize)
                    : Coding.AES_Decrypt(AES_Txb_CurrentText.Text, AES_TxtBx_Key.Text, cipherMode, keySize);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AES_Btn_GenerateKey_Click(object sender, EventArgs e)
        {

            int keySize = int.TryParse(AES_Combo_KeySize.SelectedItem?.ToString(), out int size) ? size : 128;
            byte[] key = Coding.AES_GenerateKey(keySize);
            AES_TxtBx_Key.Text = Convert.ToBase64String(key);


        }



        private async Task HighlightTextBoxAsync(Guna2TextBox textBox)
        {

            var originalFillColor = textBox.FillColor;

            for (int i = 0; i < 1; i++)
            {
                textBox.FillColor = Color.Red;
                await Task.Delay(200);

                textBox.FillColor = originalFillColor; 
                await Task.Delay(200);
            }


        }

        private void AES_Btn_Paste_Click(object sender, EventArgs e)
        {
            AES_Txb_CurrentText.Text = Clipboard.GetText();
        }

        private void AES_Btn_Swap_Click(object sender, EventArgs e)
        {
            SwapText(AES_Txb_ResultText, AES_Txb_CurrentText);
        }

        private async void AES_Btn_Copy_Click(object sender, EventArgs e)
        {
            await CopyToClipboardAsync(
                   AES_Btn_Copy,                          
                   AES_Txb_ResultText,                    
                   Properties.Resources.Copy_icon_30px,
                   Properties.Resources.Icon_Check_30px 
               );
        }

        private void AES_Combo_EnCodeDe_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (AES_Combo_EnCodeDe.SelectedIndex == 0)
            {
                AES_Btn_Encode.Text = "Encode";

            }
            else AES_Btn_Encode.Text = "Decode";
        }

        private void H_Btn_GetHashes_Click(object sender, EventArgs e)
        {
            var result = new StringBuilder();
            string input = H_Txb_CurrentText.Text;

            result.AppendLine($"MD5:      {Coding.ComputeHash(input, "MD5")}");
            result.AppendLine($"SHA-1:    {Coding.ComputeHash(input, "SHA1")}");
            result.AppendLine($"SHA-256:  {Coding.ComputeHash(input, "SHA256")}");
            result.AppendLine($"SHA-384:  {Coding.ComputeHash(input, "SHA384")}");
            result.AppendLine($"SHA-512:  {Coding.ComputeHash(input, "SHA512")}");

            
            H_Txb_ResultText.Text = result.ToString();
        }

        private void H_Btn_Paste_Click(object sender, EventArgs e)
        {
            H_Txb_CurrentText.Text = Clipboard.GetText();
        }

        private async void H_Btn_Copy_Click(object sender, EventArgs e)
        {
            await CopyToClipboardAsync(
                 H_Btn_Copy,
                  H_Txb_ResultText,
                  Properties.Resources.Copy_icon_30px,
                  Properties.Resources.Icon_Check_30px
              );
        }
    }
}
