using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace ap
{
    public partial class Form3 : Form
    {
        // Proizvod[] proizvodi;
        //Bolje je sa listom raditi
        
        public Form3(User us)
        {
            
                        
            InitializeComponent();
        //    listBox1.Items.Clear();
            label4.Text = us.Name;
            label5.Text = $"{us.Unique_id}";
            label2.Text = $"{us.Age_birth}";
            label3.Text = "Korisnik";
            List<Proizvod> proizvodi = LoadProizvodi();
            foreach (Proizvod p in proizvodi) {
                checkedListBox1.Items.Add($"{p.Name_object}, Cena = {p.Cena} ");
                    }


        }

        public List<Proizvod> LoadProizvodi()
        {
            List<Proizvod> proizvodi = new List<Proizvod>();
           
            string destination = Directory.GetCurrentDirectory() + "\\Proizvodi.txt";
            StreamReader sw = new StreamReader(destination);
            string data;
           

            while ((data = sw.ReadLine()) != null)
            {
                string[] dt = data.Split(' ');

               

                proizvodi.Add(new Proizvod(int.Parse(dt[0]), dt[1],double.Parse(dt[2])));

            }
          
            sw.Close();
            return proizvodi;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            List<Proizvod> proizvodi = LoadProizvodi();
            int k = 0;
            foreach (string s in checkedListBox1.CheckedItems) {
                k++;
            }
            
            List<int> indexi = new List<int>();
            MessageBox.Show($"Izabraliste {k} proizvoda!");
            
            for (int i = 0; i < checkedListBox1.CheckedIndices.Count; i++) {
                indexi.Add(checkedListBox1.CheckedIndices[i]);

            }
            

            List <Proizvod> proz = new List<Proizvod>();
            Proizvod p;
            foreach (int i in indexi)
            {             
                p = proizvodi[i];
                proz.Add(p);
            }
             //proz sadrzi kupceve proizvode
            foreach (Proizvod pr in proz) {
                listBox1.Items.Add($"{pr.New_id} {pr.Name_object} {pr.Cena}");
            }

        }
        /// <summary>
        /// Upis Datuma i podatka
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            List<Order> pod = new List<Order>();
            pod = LoadOrders();
            double sum = 0;
            int id;
            bool pz = true;
            bool res = int.TryParse(textBox1.Text, out id);
            User us = new User(int.Parse(label5.Text), label4.Text, int.Parse(label2.Text), false);
            DateTime vr = dateTimePicker1.Value.Date;
            if (res)
            { 
                try
                {
                    if (pz)
                    {
                        foreach (Order o in pod)
                        {

                            if (o.New_id_Order == id)
                            {
                                MessageBox.Show("Ovaj id porudzbine vec postoji!!!");
                                pz = false;
                                break;

                            }
                            else pz= true;

                        }
                    }
              
                }
            catch(Exception ex){
                    MessageBox.Show(ex.Message);
                }
           
            
                if (listBox1.Items.Count > 0 && pz==true)
                {
                    List<Proizvod> proz = new List<Proizvod>();
                    foreach (string s in listBox1.Items)
                    {
                        string[] k = s.Split(' ');
                        proz.Add(new Proizvod(int.Parse(k[0]), k[1], double.Parse(k[2])));

                    }
                    listBox1.Items.Clear();
                    foreach (Proizvod p in proz)
                    {
                        sum += p.Cena;
                    }

                    Order nabavka = new Order(id, vr, us, proz, sum);


                    SaveOrder(nabavka);
                }else
                MessageBox.Show("Niste izabrali proizvode!");
            }
        }

        
        public void SaveListOrders(List<Order> sel)
        {
            string dest = Directory.GetCurrentDirectory() + "\\Porudzbine.txt";
            StreamWriter sw = new StreamWriter(dest);
            using (sw)
            {

                foreach (Order k in sel)
                {
                    sw.WriteLine(k.ToString());
                }

            }
            sw.Close();
        }
    
        /// <summary>
        /// Ocitavanje liste
        /// </summary>
        /// <returns></returns>
        public List<Order> LoadOrders() {
            List<Order> ord = new List<Order>();
            string dest = Directory.GetCurrentDirectory() + "\\Porudzbine.txt";
            StreamReader sr = new StreamReader(dest);
            string e;
            

            while ((e = sr.ReadLine()) != null)
            {
                bool k = false;
                string[] date = e.Split(' ');
                int duz_date = date.Length;
                int id = int.Parse(date[0]);
                DateTime dat = DateTime.Parse(date[1] + " " + date[2]);
                if (date[6] == "Administrator")
                {
                    k = true;
                }

                User us = new User(int.Parse(date[3]), date[4], int.Parse(date[5]), false);
                List<Proizvod> proz = new List<Proizvod>();

                Proizvod p1 = new Proizvod(int.Parse(date[7]), date[8], double.Parse(date[9]));
              

                proz.Add(p1);
                double c = double.Parse(e.Split(' ').Last());

                ord.Add(new Order(id, dat, us, proz, c));
            }
            sr.Close();
            return ord;

        }
           
        /// <summary>
        /// Sacuvanje narudzbine
        /// </summary>
        /// <param></param>

        public void SaveOrder(Order k)
        {
            string destination = Directory.GetCurrentDirectory() + "\\Porudzbine.txt";
            StreamWriter sw = new StreamWriter(destination, true);
            string data = k.ToString();
            using (sw) {
                sw.WriteLine(data);
            }
            sw.Close();
        }
    

        private void button4_Click(object sender, EventArgs e)
        {
            checkedListBox2.Items.Clear();
            List<Order> ord = LoadOrders();
            foreach (Order o in ord) {
                if (int.Parse(label5.Text) == o.Kup.Unique_id) {
                    checkedListBox2.Items.Add(o.ToString());
                }

            }
            

        }

        // BRISANJI IZ LISTBOXA I DATOTEKE!!!
        private void button2_Click(object sender, EventArgs e)
        {
            List<Order> ord = LoadOrders();
            List<int> indexi = new List<int>();
           
            List<Order> selected = new List<Order>();
            /// Pravi se lista iz checklistboxa
            foreach (string s in checkedListBox2.CheckedItems)
            {

                string[] date = s.ToString().Split(' ');
                int duz_date = date.Length;
                List<Proizvod> proz = new List<Proizvod>();
                Proizvod p1 = new Proizvod(int.Parse(date[7]), date[8], double.Parse(date[9]));


                bool type = true;

                proz.Add(p1);
                User k = new User(int.Parse(date[3]), date[4], int.Parse(date[5]), type);
                selected.Add(new Order(int.Parse(date[0]), DateTime.Parse(date[1] + " " + date[2]), k, proz, double.Parse(s.Split(' ').Last())));


            }

           
            List<Order> sendback = new List<Order>();
            for (int i = 0; i < ord.Count; i++)
            {

                for (int j = 0; j < selected.Count; j++)
                {
                    if (ord[i].New_id_Order == selected[j].New_id_Order)
                    {
                        sendback.Add(ord[i]);
                    }
                }
            }
            /// For brise is checkboxa iteme//
            foreach (var stik in checkedListBox2.CheckedItems.OfType<string>().ToList())
            {
                checkedListBox2.Items.Remove(stik);
            }

            SaveListOrders(sendback);



        }
        /// <summary>
        /// Exit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }
    }
}
