using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Web.WebView2.Core;
using Newtonsoft.Json;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;

namespace ExploradorWeb
{
    public partial class Explorador : Form
    {
        public static string url;
        public static int cont;
        public static DateTime fec;
        //URL urls = new URL(url, cont, fec);
        List<URL> histo = new List<URL> ();
        public Explorador()
        {
            InitializeComponent();
            this.Resize += new System.EventHandler(this.Form_Resize);
            //string path = @"../..Historial.txt";
            //File.WriteAllText(path, string.Empty);
        }

        private void CargarHistorial()
        {
            string filename = @"../..Historial.json";
            FileStream stream = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Read);
            StreamReader reader = new StreamReader(stream);
            while (reader.Peek() > -1)
            {
                URL url = new URL();
                url.url = reader.ReadLine();
                url.contador = int.Parse(reader.ReadLine());
                url.fecha = Convert.ToDateTime(reader.ReadLine());

            }
            reader.Close();

            ActualizarComboBox();
        }

        private void ActualizarComboBox()
        {
            comboBox1.Items.Clear();
            foreach (var urls in histo) 
            {
                comboBox1.Items.Add(histo);
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
            GuardarJson("../../Historial.json");
            LeerJson("../../Historial.json");
        }

        private void GuardarHistorial()
        {
            string filename = @"../../Historial.json";
            FileStream stream = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter writer = new StreamWriter(stream);
            foreach (URL urls in histo)
            {
                writer.WriteLine(urls.url);
                writer.WriteLine(urls.contador);
                writer.WriteLine(urls.fecha);
            }
            writer.Close();
        }

        private void GuardarJson(String filename)
        {
            string json = JsonConvert.SerializeObject(histo);
            //string archivo = filename;

            System.IO.File.WriteAllText(filename, json);
        }

        private void LeerJson(string filename)
        {
            StreamReader jsonStream = File.OpenText(filename);
            string json = jsonStream.ReadToEnd();
            jsonStream.Close();

            histo = JsonConvert.DeserializeObject<List<URL>>(json);
        }

        private void Historial_Click(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem != null)
            {
                comboBox1.Items.Clear();
                if (comboBox2.SelectedItem.ToString() == "Por fecha")
                {
                    histo = histo.OrderBy(a =>a.fecha < DateTime.Now).ToList();
                    foreach (URL urls in histo)
                    {
                        comboBox1.Items.Add(urls);
                    }
                }
                else if (comboBox2.SelectedItem.ToString() == "Por visitas")
                {
                    var historialPorVisitas = histo.OrderByDescending(x => x.contador ).ToList();
                    foreach (URL urls in histo)
                    {
                        comboBox1.Items.Add(urls);
                    }
                }
            }
        }

        //Boton de ir, 
        private void BotonIr_Click(object sender, EventArgs e)

        {
            URL urls = new URL();
            string urlVisitada = comboBox1.Text;

            
                if (webView != null && webView.CoreWebView2 != null)
                {
                    if (urlVisitada.Contains("https://") && (urlVisitada.Contains(".com") || urlVisitada.Contains(".org")))
                    {
                        urls.url = urlVisitada;
                        foreach (var url in histo)
                        {
                            if (urls.url.Equals(urlVisitada))
                            {
                                urls.contador++;
                            }
                            else
                            {
                                histo.Add(urls);
                            }
                        }
                        webView.CoreWebView2.Navigate(urlVisitada);
                    }
                    else
                    {
                        urlVisitada = "https://www.google.com/search?q=" + urlVisitada;
                        urls.url = urlVisitada;
                        foreach (var url in histo)
                        {
                            if (urls.url.Equals(urlVisitada))
                            {
                                urls.contador++;
                            }
                            else
                            {
                                histo.Add(urls);
                            }
                        }
                        webView.CoreWebView2.Navigate(urlVisitada);

                    }
                    GuardarJson("../../Historial.json");
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
            histo.RemoveAll(l => l.url == urlBorrar);

        }
    }
}
