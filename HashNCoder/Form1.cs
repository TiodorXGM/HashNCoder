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
        private Color defaultTextColor = Color.FromArgb(125, 137, 149);
        public Form1()
        {
            InitializeComponent();       
        }
       
        // ------------------------------------------------------------------ ENCODING
        private async void E_Btn_Encode_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(E_Txb_CurrentText.Text))
            {
                await HighlightTextBoxAsync(E_Txb_CurrentText);
                DisplayErrorInResult(E_Txb_ResultText, "Please fill in the required fields.");
                return;
            }

            if (E_Combo_EnCodeDe.SelectedIndex == 1 && E_Combo_Algoritm.SelectedIndex == 0)
            {
                if (!Coding.IsBase64String(E_Txb_CurrentText.Text))
                {
                    await HighlightTextBoxAsync(E_Txb_CurrentText);
                    DisplayErrorInResult(E_Txb_ResultText,
                        "The string is not a valid Base64 string.\n\n" +
                        "- Contain only letters (A-Z, a-z), digits (0-9), and the characters '+' and '/';\n" +
                        "- Have a length that is a multiple of 4 (including '=' padding characters);");
                    return;
                }
            }

            string result = string.Empty;

            if (E_Combo_Algoritm.SelectedIndex == 0)
            {
                result = E_Combo_EnCodeDe.SelectedIndex == 0
                    ? Coding.EncodeBase64(E_Txb_CurrentText.Text)
                    : Coding.DecodeBase64(E_Txb_CurrentText.Text);
            }
            else if (E_Combo_Algoritm.SelectedIndex == 1)
            {
                result = E_Combo_EnCodeDe.SelectedIndex == 0
                    ? Coding.URLEncode(E_Txb_CurrentText.Text)
                    : Coding.URLDecode(E_Txb_CurrentText.Text);
            }
            else if (E_Combo_Algoritm.SelectedIndex == 2)
            {
                result = E_Combo_EnCodeDe.SelectedIndex == 0
                    ? Coding.HTMLEncode(E_Txb_CurrentText.Text)
                    : Coding.HTMLDecode(E_Txb_CurrentText.Text);
            }
            else if (E_Combo_Algoritm.SelectedIndex == 3)
            {
                result = E_Combo_EnCodeDe.SelectedIndex == 0
                    ? Coding.UnescapeEncode(E_Txb_CurrentText.Text)
                    : Coding.UnescapeDecode(E_Txb_CurrentText.Text);
            }

            DisplaySuccessResult(E_Txb_ResultText, result);
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

        
        // ------------------------------------------------------------------ AES
        private async void AES_Btn_Encode_Click(object sender, EventArgs e)
        {         
            try
            {
                bool isKeyEmpty = string.IsNullOrWhiteSpace(AES_TxtBx_Key.Text);
                bool isTextEmpty = string.IsNullOrWhiteSpace(AES_Txb_CurrentText.Text);

                if (isKeyEmpty && isTextEmpty)
                {
                    await HighlightTextBoxAsync(AES_TxtBx_Key);
                    await HighlightTextBoxAsync(AES_Txb_CurrentText);
                    throw new InvalidOperationException("Both the key and the text fields are empty. Please fill in both fields to proceed.");
                }

                if (isKeyEmpty)
                {
                    await HighlightTextBoxAsync(AES_TxtBx_Key);
                    throw new InvalidOperationException("The key field is empty. Please provide a key to proceed.");
                }

                if (isTextEmpty)
                {
                    await HighlightTextBoxAsync(AES_Txb_CurrentText);
                    throw new InvalidOperationException("The text field is empty. Please provide text to encrypt or decrypt.");
                }

                if (AES_Combo_EnCodeDe.SelectedIndex == 1) 
                {
                    if (!Coding.IsBase64String(AES_Txb_CurrentText.Text))
                    {
                        await HighlightTextBoxAsync(AES_Txb_CurrentText);
                        throw new FormatException(
                            "The string is not a valid Base64 string.\n\n" +
                            "- Contain only letters (A-Z, a-z), digits (0-9), and the characters '+' and '/';\n" +
                            "- Have a length that is a multiple of 4 (including '=' padding characters);");
                    }
                }

                CipherMode cipherMode = AES_Combo_Algoritm.SelectedIndex == 0 ? CipherMode.ECB : CipherMode.CBC;
                int keySize = int.TryParse(AES_Combo_KeySize.SelectedItem?.ToString(), out int size) ? size : 128;

                string result = AES_Combo_EnCodeDe.SelectedIndex == 0
                ? Coding.AES_Encrypt(AES_Txb_CurrentText.Text, AES_TxtBx_Key.Text, cipherMode, keySize)
                : Coding.AES_Decrypt(AES_Txb_CurrentText.Text, AES_TxtBx_Key.Text, cipherMode, keySize);

                DisplaySuccessResult(AES_Txb_ResultText, result);
            }
            catch (FormatException ex)
            {
                DisplayErrorInResult(AES_Txb_ResultText, ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                DisplayErrorInResult(AES_Txb_ResultText, ex.Message);
            }
            catch (Exception ex)
            {
                DisplayErrorInResult(AES_Txb_ResultText, $"An unexpected error occurred: {ex.Message}");
            }
        }



        private void AES_Btn_GenerateKey_Click(object sender, EventArgs e)
        {
            try
            {
                int keySize = int.TryParse(AES_Combo_KeySize.SelectedItem?.ToString(), out int size) ? size : 128;
                byte[] key = Coding.AES_GenerateKey(keySize);
                string result = Convert.ToBase64String(key);
                DisplaySuccessResult(AES_TxtBx_Key, result);
            }
            catch (Exception ex)
            {
                DisplayErrorInResult(AES_Txb_ResultText, $"Error generating key: {ex.Message}");
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

        // ------------------------------------------------------------------ HASH

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

        // ------------------------------------------------------------------ HELPERS

        private void DisplayErrorInResult(Guna2TextBox textBox, string errorMessage)
        {
            textBox.ForeColor = Color.Red;
            textBox.Text = errorMessage;
        }
        private void DisplaySuccessResult(Guna2TextBox textBox, string result)
        {
            textBox.ForeColor = defaultTextColor;
            textBox.Text = result;
        }

        private void SwapText(Guna2TextBox textBox1, Guna2TextBox textBox2)
        {
            string temp = textBox1.Text;
            textBox1.Text = textBox2.Text;
            textBox2.Text = temp;
        }

        private bool IsCopyProcessing = false;

        private async Task CopyToClipboardAsync(
            Guna2Button button,
            Guna2TextBox textBox,
            Image originalIcon,
            Image successIcon)
        {
            if (IsCopyProcessing) return;

            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                DisplayErrorInResult(textBox, "The text field is empty. There is nothing to copy.");
                await HighlightTextBoxAsync(textBox);
                return;
            }
            try
            {
                IsCopyProcessing = true;
                Clipboard.SetText(textBox.Text);
                button.Image = successIcon;
                await Task.Delay(1000);
                button.Image = originalIcon;
            }
            catch (Exception ex)
            {
                DisplayErrorInResult(textBox, ex.Message);
                await HighlightTextBoxAsync(textBox);
            }
            finally
            {
                IsCopyProcessing = false;
            }
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

      
    }
}
