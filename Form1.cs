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
        public static string url;
        public static int cont;
        public static DateTime fec;
        URL urls = new URL(url, cont, fec);
        Dictionary<string, (int contador, DateTime fecha)> historial = new Dictionary<string, (int, DateTime)>();
        public Explorador()
        {
            InitializeComponent();
            this.Resize += new System.EventHandler(this.Form_Resize);
            //string path = @"../..Historial.txt";
            //File.WriteAllText(path, string.Empty);
        }

        private void CargarHistorial()
        {
            comboBox1.Items.Clear();
            string archivo1 = @"../..Historial.txt";
            if (File.Exists(archivo1))
            {
                using (StreamReader reader = new StreamReader(archivo1))
                {
                    while (reader.Peek() > -1)
                    {
                        string url = reader.ReadLine();
                        if (!historial.ContainsKey(url))
                        {
                            historial[url] = (1, DateTime.Now); 
                        }
                        else
                        {
                            historial[url] = (historial[url].contador + 1, DateTime.Now); 
                        }
                    }
                }
            }

            ActualizarComboBox();
        }

        private void ActualizarComboBox()
        {
            comboBox1.Items.Clear();
            foreach (var entry in historial.OrderByDescending(x => x.Value.contador)) 
            {
                comboBox1.Items.Add(entry.Key);
            }
        }

        private void Form_Resize(object sender, EventArgs e)
        {
            webView.Size = this.ClientSize - new System.Drawing.Size(webView.Location);
            BotonIr.Left = this.ClientSize.Width - BotonIr.Width;
            comboBox1.Width = BotonIr.Left - comboBox1.Left;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CargarHistorial();


        }

        private void GuardarHistorial()
        {
            string archivo = @"../..Historial.txt";
            using (StreamWriter writer = new StreamWriter(archivo, false))
            {
                foreach (var entry in historial.OrderByDescending(x => x.Value.fecha)) 
                {
                    writer.WriteLine(entry.Key);
                }
            }
        }

        private void Historial_Click(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem != null)
            {
                comboBox1.Items.Clear();
                if (comboBox2.SelectedItem.ToString() == "Por fecha")
                {
                    var historialPorFecha = historial.OrderByDescending(x => x.Value.fecha).ToList();
                    foreach (var entry in historialPorFecha)
                    {
                        comboBox1.Items.Add(entry.Key);
                    }
                }
                else if (comboBox2.SelectedItem.ToString() == "Por visitas")
                {
                    var historialPorVisitas = historial.OrderByDescending(x => x.Value.contador).ToList();
                    foreach (var entry in historialPorVisitas)
                    {
                        comboBox1.Items.Add(entry.Key);
                    }
                }
            }
        }
        private void BotonIr_Click(object sender, EventArgs e)
        {
            string urlVisitada = comboBox1.Text;
            if (!historial.ContainsKey(urlVisitada))
            {
                historial[urlVisitada] = (1, DateTime.Now); 
            }
            else
            {
                historial[urlVisitada] = (historial[urlVisitada].contador + 1, DateTime.Now); 
            }

           
            GuardarHistorial();

            if (webView != null && webView.CoreWebView2 != null)
            {
                if (urlVisitada.Contains("https://") && (urlVisitada.Contains(".com") || urlVisitada.Contains(".org")))
                {
                    webView.CoreWebView2.Navigate(urlVisitada);
                }
                else
                {
                    webView.CoreWebView2.Navigate("https://www.google.com/search?q=" + urlVisitada);
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

        

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Borrar_Click(object sender, EventArgs e)
        {
            string urlBorrar = comboBox1.Text;
            if (historial.ContainsKey(urlBorrar))
            {
                historial.Remove(urlBorrar); 
                GuardarHistorial(); 
                ActualizarComboBox();

            }

        }
    }
}
