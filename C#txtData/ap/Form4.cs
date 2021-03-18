using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace ap
{
    public partial class Form4 : Form
    {
       
        List<Point> gornja_linija_proizvoda;
        List<Point> donja_linija_dana;


        public Form4()
        {
            


            InitializeComponent();
            gornja_linija_proizvoda = new List<Point>();
            donja_linija_dana = new List<Point>();
            this.Width = 800;
            this.Height = 350;

        }


        public List<Order> LoadOrders()
        {

            List<Order> ord = new List<Order>();
            string dest = Directory.GetCurrentDirectory() + "\\Porudzbine.txt";
            StreamReader sr = new StreamReader(dest);
            string e;


            while ((e = sr.ReadLine()) != null)
            {

                bool type = true;
                string[] date = e.Split(' ');
                int duz_date = date.Length;
                int id = int.Parse(date[0]);
                DateTime dat = DateTime.Parse(date[1] + " " + date[2]);

                User us = new User(int.Parse(date[3]), date[4], int.Parse(date[5]), type);
                List<Proizvod> proz = new List<Proizvod>();

                Proizvod p1 = new Proizvod(int.Parse(date[7]), date[8], double.Parse(date[9]));
                //  if (duz_date > 10)
                {
                    //    Proizvod p2 = new Proizvod(int.Parse(date[10]), date[11],double.Parse(date[12]));
                    //  proz.Add(p2);
                }
                //   if (duz_date > 13)
                {
                    //     Proizvod p3 = new Proizvod(int.Parse(date[13]), date[14], double.Parse(date[15]));
                    //     proz.Add(p3);
                }

                proz.Add(p1);
                double c = double.Parse(e.Split(' ').Last());

                ord.Add(new Order(id, dat, us, proz, c));
            }
            sr.Close();
            return ord;

        }




        private void button1_Click(object sender, EventArgs e)
        {


           
            List < Order > isporuci = new List<Order>();
           

            isporuci =  LoadOrders();
            int br=  0;
            foreach (int k in comboBox1.SelectedIndex.ToString()) 
            {
                foreach (Order o in isporuci) {

                if(o.Dat.Month == k)
                    {
                        Point c = new Point(o.Dat.Day, 250);
                        donja_linija_dana.Add(c);
                        Proizvod p = o.Prz.FirstOrDefault();
                        br++;
                        Point s = new Point(br, 50);
                        gornja_linija_proizvoda.Add(s);
                    }
                
                }
                this.Paint += CrtajFunk;

            }
            

            this.Refresh();
        }

        

        private void VidiPoziciju(object sender, EventArgs e)
        {
            label2.Text = "x: " + Control.MousePosition.ToString() + "y: " + Control.MousePosition.ToString();

        }

        

        public void CrtajFunk(object sender, PaintEventArgs e) {

            for (int i = 0; i < gornja_linija_proizvoda.Count; i++)
                for (int j = 0; j < donja_linija_dana.Count; j++) {
                    if (i == j) {

                        e.Graphics.DrawLine(Pens.BurlyWood, gornja_linija_proizvoda[i], donja_linija_dana[j]);
                 }
                }
            
            }
        
        


        private void Form4_Paint(object sender, PaintEventArgs e)
        {
            //x,y
            Point cosak = new Point(250, 250);
            //x.y
            Point gornja_tacka = new Point(250, 50);
            //x,y
            Point donja_tacka = new Point(560, 250);
            Pen p1 = new Pen(Color.Black);
            Pen p2 = new Pen(Color.Red);

            e.Graphics.DrawLine(p1, cosak, gornja_tacka);
            e.Graphics.DrawLine(p2, cosak, donja_tacka);

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Form4_Load(object sender, EventArgs e)
        {

        }
    }
}