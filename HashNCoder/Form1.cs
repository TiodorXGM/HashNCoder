using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
                if (E_Combo_EnCodeDe.SelectedIndex == 0)
                {
                    E_Txb_ResultText.Text = Coding.EncodeBase64(E_Txb_CurrentText.Text);
                }
                else E_Txb_ResultText.Text = Coding.DecodeBase64(E_Txb_CurrentText.Text);

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
    }
}
