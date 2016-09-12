using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CallRequestResponseService;

namespace RUAP
{
    public partial class Form1 : Form
    {
        string a = "yes";
        string b = "no";
        bool flag = false;
        public Form1()
        {
            
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string[] data = new string[]
            {
                txtB_age.Text,
                cbB_job.Text,
                cbB_marital.Text,
                cbB_education.Text,
                cbB_default.Text,
                txtB_balance.Text,
                cbB_housing.Text,
                cbB_loan.Text,
                cbB_contact.Text,
                txtB_day.Text,
                cbB_month.Text,
                txtB_duration.Text,
                txtB_campaign.Text,
                txtB_pdays.Text,
                txtB_previous.Text,
                cbB_outcome.Text,
                "y"
            };

            RequestResponse.InvokeRequestResponseService(data).Wait();
            string predictedClass = getY(RequestResponse.Result);
            foreach (Control c in this.Controls)
            {
                if (c is TextBox)
                {
                    TextBox textBox = c as TextBox;
                    if (textBox.Text == string.Empty && flag == false)
                    {
                        MessageBox.Show("Fill in all the answers");
                        flag = true;
                    }
                }
            }
            flag = false;
            if(predictedClass.Equals(a))
            {
                MessageBox.Show("Client WOULD subscribe to a term deposit");
            }
            else if (predictedClass.Equals(b))
            {
                MessageBox.Show("Client WOULD NOT subscribe to a term deposit");
            }
         
        }
        private string getY(string output)
        {
            int n1 = output.Length - 1;
            while (output[n1] != '"')
            {
                n1--;
            }
            int n2 = n1 - 1;
            while (output[n2] != '"')
            {
                n2--;
            }
            n1 = n2 - 1;
            while (output[n1] != '"')
            {
                n1--;
            }
            n2 = n1 - 1;
            while (output[n2] != '"')
            {
                n2--;
            }
            string predictedY = output.Substring(n2 + 1, n1 - n2 - 1);
            return predictedY;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


    }
}
