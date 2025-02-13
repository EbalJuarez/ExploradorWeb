using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Web.WebView2.Core;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;

namespace ExploradorWeb
{
    public partial class Explorador : Form
    {
        public Explorador()
        {
            InitializeComponent();
            this.Resize += new System.EventHandler(this.Form_Resize);
            //string path = @"../..Historial.txt";
            //File.WriteAllText(path, string.Empty);
        }

        private void Form_Resize(object sender, EventArgs e)
        {
            webView.Size = this.ClientSize - new System.Drawing.Size(webView.Location);
            BotonIr.Left = this.ClientSize.Width - BotonIr.Width;
            comboBox1.Width = BotonIr.Left - comboBox1.Left;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();

            string archivo1 = @"../..Historial.txt";

            FileStream stream1 = new FileStream(archivo1, FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(stream1);

            while (reader.Peek() > -1)
            {
                comboBox1.Items.Add(reader.ReadLine());
            }

            reader.Close();


        }

        private void BotonIr_Click(object sender, EventArgs e)
        {
            

            string archivo = @"../..Historial.txt";
            FileStream stream = new FileStream(archivo, FileMode.Append, FileAccess.Write);
            StreamWriter writer = new StreamWriter(stream);
            writer.WriteLine(comboBox1.Text);

            writer.Close();

            


            if (webView != null && webView.CoreWebView2 != null)
            {
                if (comboBox1.Text.Contains("https://") && comboBox1.Text.Contains(".com") || comboBox1.Text.Contains(".com/")) {
                    webView.CoreWebView2.Navigate(comboBox1.Text);
                }
                if (comboBox1.Text.Contains(".com/") || comboBox1.Text.Contains(".com"))
                {
                    webView.CoreWebView2.Navigate("https://" + comboBox1.Text);
                }
                if (comboBox1.Text.Contains("https://"))
                {
                    webView.CoreWebView2.Navigate(comboBox1.Text);
                }
                else {
                    webView.CoreWebView2.Navigate(" https://www.google.com/search?q=" + comboBox1.Text);
                }
            }
        }

        private void inicioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            webView.CoreWebView2.Navigate("https://www.google.com/");
        }

        private void haciaAtrasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            webView.CoreWebView2.GoBack();
        }

        private void haciaAdelanteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            webView.CoreWebView2.GoForward();
        }

        private void webView_Click(object sender, EventArgs e)
        {
           
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Historial_Click(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();

            string archivo1 = @"../..Historial.txt";

            FileStream stream1 = new FileStream(archivo1, FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(stream1);

            while (reader.Peek() > -1)
            {
                comboBox1.Items.Add(reader.ReadLine());
            }

            reader.Close();
            
        }
    }
}
