using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace XORbp
{
    public partial class Form1 : Form
    {
        public class neuron
        {
            public double[] W { get; set; }
            public double Out { get; set; }
            public double prag { get; set; }
            public neuron(int nr_elemente)
            {
                W = new double[nr_elemente];
                for(int i = 0; i < nr_elemente; i++)
                {
                    W[i] = getRandomDouble();
                }
                prag = getRandomDouble();
                Out = 0;
            }
        }
        #region generareRandom
        public static readonly Random getRandom = new Random();
        public static int getRandomNumber(int min, int max)
        {
            lock (getRandom)
            {
                return getRandom.Next(min, max);
            }
        }
        public static double getRandomDouble()
        {
            lock (getRandom)
            {
                return getRandom.NextDouble();
            }
        }
        #endregion
        public Form1()
        {
            InitializeComponent();
        }
        #region variabile Globale
        #endregion
        private void EtapaAntrenare()
        {
           int nr_unitati_intrare = 2, hidden_neurons = 2;
            double gradient = 0.5;
            double E;
            neuron[] x = new neuron[nr_unitati_intrare];
            neuron y = new neuron(0);
            neuron[] h = new neuron[hidden_neurons];
            x[0] = new neuron(hidden_neurons);
            x[1] = new neuron(hidden_neurons);
            h[0] = new neuron(1);
            h[1] = new neuron(1);
            int k = 0;
            int nr_exemple = 4;
            double[] dateX1 = new double[nr_exemple];
            double[] dateX2 = new double[nr_exemple];
            double[] dateY = new double[nr_exemple];
            string path = "input.txt";
            string[] lines = File.ReadAllLines(path);
            for (int i = 0; i < lines.Length; i++)
            {
                string[] words = lines[i].Split(' ');
                dateX1[i] = Convert.ToDouble(words[0]);
                dateX2[i] = Convert.ToDouble(words[1]);
              dateY[i] = Convert.ToDouble(words[2]);
            }
            double E_epoca = 0;
            int epoca = 0;
            do
            {
                epoca++;

                label1.Text = $"Eroare: {E_epoca} \nEpoca: {epoca}";
                label1.Update();
           
                
              E_epoca = 0;
                for (k = 0; k < nr_exemple; k++)
                {
                    x[0].Out = dateX1[k];
                    x[1].Out = dateX2[k];
                  #region labels
                    
                    X0outLabel.Text = x[0].Out.ToString();
                    X1outLabel.Text = x[1].Out.ToString();
                    H0outLabel.Text = h[0].Out.ToString();
                    H1outLabel.Text = h[1].Out.ToString();
                    YoutLabel.Text = y.Out.ToString();
                    X0W0_label.Text = x[0].W[0].ToString();
                    X0W1_label.Text = x[0].W[1].ToString();
                    X1W1_label.Text = x[1].W[1].ToString();
                    X1W0_label.Text = x[1].W[0].ToString();
                    H0W0_Label.Text = h[0].W[0].ToString();
                    H1W0_Label.Text = h[1].W[0].ToString();
                    Prag_H0.Text = h[0].prag.ToString();
                    Prag_H1.Text = h[1].prag.ToString();
                    Prag_Y.Text = y.prag.ToString();
                    H0W0_Label.Update();
                    H1W0_Label.Update();
                    X0outLabel.Update();
                    X1outLabel.Update();
                    H0outLabel.Update();
                    H1outLabel.Update();
                    panel1.Update();
                    YoutLabel.Update();
                    X0W0_label.Update();
                    X0W1_label.Update();
                    X1W0_label.Update();
                    X1W1_label.Update();
                    Prag_H0.Update();
                    Prag_H1.Update();
                    Prag_Y.Update();

                    #endregion



                   y.Out = 0;
                    for (int a = 0; a < hidden_neurons; a++)
                    {
                       h[a].Out = 0;
                        for (int i = 0; i < nr_unitati_intrare; i++)
                        {
                            h[a].Out += x[i].W[a] * x[i].Out + h[a].prag;
                        }
                        h[a].Out = F(h[a].Out);
                        y.Out += h[a].W[0] * h[a].Out + y.prag;
                    }
                    y.Out = F(y.Out);
                    E = Math.Pow((y.Out - dateY[k]), 2);
                    E_epoca += E;

                   

                    double rez = 2 * (y.Out - dateY[k]) * F_derivat(y.Out);
                    for (int a = 0; a < hidden_neurons; a++)
                    {
                        for (int i = 0; i < nr_unitati_intrare; i++)
                        {
                            x[i].W[a] = x[i].W[a] - (gradient * rez * h[a].W[0] * F_derivat(h[a].Out) * x[i].Out);
                        }
                        h[a].prag -= gradient * (rez) * h[a].W[0] * F_derivat(h[a].Out);
                        h[a].W[0] -= (gradient * rez * h[a].Out);
                    }
                    y.prag -= (gradient * rez);
                }
                //Thread.Sleep(2000);
            } while (E_epoca>Math.Pow(10,-3));
          

     


        }
        public double F(double x)
        {
            return 1.0 / (1.0 + Math.Exp(-x));
        }
        public double F_derivat(double x)
        {
            double rez = (x * (1 - x));
            return rez;
        }
       
        private void button2_Click(object sender, EventArgs e)
        {
            EtapaAntrenare();
        }
    
    }
}
