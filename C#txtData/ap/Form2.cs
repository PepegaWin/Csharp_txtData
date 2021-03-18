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
    public partial class Form2 : Form
    {

        User u = new User();

        public Form2(User us)
        {
            Form3 fr = new Form3(us);

            InitializeComponent();
            label4.Text = us.Name;
            label2.Text = $"{us.Unique_id}";
            label12.Text = $"{us.Age_birth}";
            label3.Text = "Administrator";
            List<Proizvod> proizvodi = fr.LoadProizvodi();
            foreach (Proizvod p in proizvodi)
            {
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



                proizvodi.Add(new Proizvod(int.Parse(dt[0]), dt[1], double.Parse(dt[2])));

            }

            sw.Close();
            return proizvodi;

        }

        private void button6_Click(object sender, EventArgs e)
        {

            List<Proizvod> proizvodi = LoadProizvodi();
            int k = 0;
            foreach (string s in checkedListBox1.CheckedItems)
            {
                k++;
            }

            List<int> indexi = new List<int>();
            MessageBox.Show($"Izabraliste {k} proizvoda!");

            for (int i = 0; i < checkedListBox1.CheckedIndices.Count; i++)
            {
                indexi.Add(checkedListBox1.CheckedIndices[i]);

            }


            List<Proizvod> proz = new List<Proizvod>();
            Proizvod p;
            foreach (int i in indexi)
            {
                p = proizvodi[i];
                proz.Add(p);
            }
            //proz sadrzi kupceve proizvode
            foreach (Proizvod pr in proz)
            {
                listBox1.Items.Add($"{pr.New_id} {pr.Name_object} {pr.Cena}");
            }

        }
        /// <summary>
        /// Upis Datuma i podatka
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click_1(object sender, EventArgs e)
        {
            List<Order> pod = new List<Order>();
            pod = LoadOrders();
            double sum = 0;
            int id;
            bool z= true;
            bool res = int.TryParse(textBox1.Text,out id);
            User us = new User(int.Parse(label2.Text), label4.Text, int.Parse(label12.Text), true);
            DateTime vr = dateTimePicker1.Value.Date;
            if (res) { 
            try
            {

                    foreach (Order o in pod)
                    {
                        if (z)
                        {
                            if (o.New_id_Order == id)
                            {
                                MessageBox.Show("Ovaj id porudzbine vec postoji!!!");
                                z = false;
                                throw new Exception("Promeni id!");
                               
                            }

                        }
                    }
            }
            catch
           (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
                if (z)
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
                }
            }
            else
                MessageBox.Show("Los ukucan id");

        }

        /// <summary>
        /// Ocitavanje liste
        /// </summary>
        /// <returns></returns>


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

        /// <summary>
        /// Sacuvanje narudzbine
        /// </summary>
        /// <param></param>
        public void SaveListOrders(List<Order> sel) {


            string dest = Directory.GetCurrentDirectory() + "\\Porudzbine.txt";
            StreamWriter sw = new StreamWriter(dest);
            using (sw) {

                foreach (Order k in sel) {
                    sw.WriteLine(k.ToString());
                }

            }
            sw.Close();
        }
        public void SaveOrder(Order k)
        {
            string destination = Directory.GetCurrentDirectory() + "\\Porudzbine.txt";
            StreamWriter sw = new StreamWriter(destination, true);
            string data = k.ToString();
            using (sw)
            {
                sw.WriteLine(data);
            }
            sw.Close();
        }
        /// <summary>
        /// Exit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            checkedListBox2.Items.Clear();
            List<Order> ord = LoadOrders();
            foreach (Order o in ord)
            {
                
                if (int.Parse(label2.Text) == o.Kup.Unique_id)
                {
                    checkedListBox2.Items.Add(o.ToString());
                }

            }
            

        }

        // BRISANJI IZ LISTBOXA I DATOTEKE!!!
        private void button2_Click_1(object sender, EventArgs e)
        {
            //->Order1 / Order2/Order3/Order(id,datetime,User,List<proizod>,sum)
            Order or;
            List<Order> ord = LoadOrders();


            List<Order> selected = new List<Order>();
            /// Pravi se lista iz checklistboxa
            if (checkedListBox2.Items.Count > 0)
            {
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
                List<int> a = new List<int>();
                foreach (int item in checkedListBox2.CheckedIndices)
                {
                    a.Add(item);

                }

                List<Order> sendback = new List<Order>();
                for (int i = 0; i < ord.Count; i++)
                {

                    for (int j = 0; j < selected.Count; j++)
                    {
                        if (ord[i].New_id_Order != selected[j].New_id_Order)
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
            else
                MessageBox.Show("Nemate nista da otkazete!");
        }





        private List<User> LoadData()
        {
            List<User> ListOfUsers = new List<User>();
            string destination = Directory.GetCurrentDirectory() + "\\Users.txt";
            StreamReader sr = new StreamReader(destination);
            string data;
            bool type = false;
            while ((data = sr.ReadLine()) != null)
            {
                string[] dt = data.Split(' ');



                ListOfUsers.Add(new User(int.Parse(dt[0]), dt[1], int.Parse(dt[2]), bool.Parse(dt[3])));

            }
            sr.Close();
            return ListOfUsers;

        }  
        /// <summary>
        /// Pravljenje Usera
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button7_Click(object sender, EventArgs e)
        {
            List<User> ListOfUsers = new List<User>();
            ListOfUsers = LoadData();
            User k;
            int id = 0;
            string name = string.Empty;
            int age_birth = 0;
            string age_format = string.Empty;
            bool age = false;
            bool type = false;
            bool radi = false;
            string Tip = string.Empty;
            try
            {
                radi = true;
                id = int.Parse(txtID.Text);
                foreach (User u in ListOfUsers)
                {
                    if (u.Unique_id == id)
                    {
                        throw new Exception("Korisnik sa ovim ID-om Postiji!!!");

                    }
                }
                age_format = txtGod.Text;
                name = txtIme.Text;
                age = age_format.Any(char.IsDigit);
                if (age_format.Length != 4 && age)
                {
                    MessageBox.Show("Godiste (4) broja!");
                    age_format = "Greska!";

                }
                age_birth = Int32.Parse(age_format);
                Tip = txtTip.Text;
                if (Tip == "Administrator")
                {
                    type = true;
                    radi = true;
                }
                else if (Tip == "Korisnik")
                {
                    type = false;
                    radi = true;
                }
                else
                    radi = false;


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Greska pri Unosu", MessageBoxButtons.RetryCancel, MessageBoxIcon.Question);
                radi = false;
            }

            if (radi)
            {
                string destination = Directory.GetCurrentDirectory() + "\\Users.txt";
                StreamWriter sw = new StreamWriter(destination, true);
                string data = $"{id} {name} {age_birth} {type}";
                using (sw)
                {
                    sw.WriteLine(data);
                }
                sw.Close();
                radi = false;
                MessageBox.Show("Usepesno ste dodali korisnika");
            }
            else
                MessageBox.Show("Niste dodali korisnika");
        }
        /// <summary>
        /// Menjanje usera
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button9_Click(object sender, EventArgs e)
        {
            List<User> listUsers = LoadData();
            User k;
            bool radi = false;
            int a = 0;
            foreach (User u in listUsers)
            {
                a++;
                if (u.Unique_id == int.Parse(txtID.Text))
                {
                    string age_format;
                    bool age;
                    radi = false;
                    try
                    {
                        age_format = txtGod.Text;
                        u.Name = txtIme.Text;
                        age = age_format.Any(char.IsDigit);
                        if (age_format.Length != 4 && age)
                        {
                            MessageBox.Show("Godiste (4) broja!");
                            age_format = "Greska!";

                        }
                        u.Age_birth = Int32.Parse(age_format);
                        if ("Administrator" == txtTip.Text)
                        {
                            u.Type = true;
                            radi = true;

                        }
                        else if (txtTip.Text == "Korisnik")
                        {
                            u.Type = false;
                            radi = true;
                        }
                        else
                            radi = false;


                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Greska pri Unosu", MessageBoxButtons.RetryCancel, MessageBoxIcon.Question);
                        radi = false;
                    }


                }
                
            }
            if (radi)
            {
                string destination = Directory.GetCurrentDirectory() + "\\Users.txt";
                StreamWriter sw = new StreamWriter(destination);
                string data;
                foreach (User u in listUsers)
                {


                    data = $"{u.Unique_id} {u.Name} {u.Age_birth} {u.Type}";
                    sw.WriteLine(data);
                }

                sw.Close();
            }
            else
                MessageBox.Show("Niste promenili korisnika!");
        }
        /// <summary>
        /// Gasenje Forme
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();

        }
        /// <summary>
        /// Brisanje korisnika
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button8_Click(object sender, EventArgs e)
        {
            int id = int.Parse(textBox6.Text);
            List<User> us = new List<User>();
            us = LoadData();

            var osoba = us.SingleOrDefault(u => u.Unique_id == id);
            if (osoba != null)
                us.Remove(osoba);

            string destination = Directory.GetCurrentDirectory() + "\\Users.txt";
            StreamWriter sw = new StreamWriter(destination);
            string data;
            foreach (User u in us)
            {


                data = $"{u.Unique_id} {u.Name} {u.Age_birth} {u.Type}";
                sw.WriteLine(data);
            }
            sw.Close();

        }
        private List<Proizvod> LoadProizvod()
        {
            List<Proizvod> ListOProizvods = new List<Proizvod>();
            string destination = Directory.GetCurrentDirectory() + "\\Proizvodi.txt";
            StreamReader sr = new StreamReader(destination);
            string data;
            using (sr)
            {
                while ((data = sr.ReadLine()) != null)
                {
                    string[] dt = data.Split(' ');

                    ListOProizvods.Add(new Proizvod(int.Parse(dt[0]), dt[1], double.Parse(dt[2])));

                }
            }
            sr.Close();
            return ListOProizvods;
        }
        /// <summary>
        /// Pravljenje Proizvoda
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ///
        private void button12_Click(object sender, EventArgs e)
        {
            List<Proizvod> ListOfProizvod = new List<Proizvod>();
            ListOfProizvod = LoadProizvod();
            Proizvod p;
            int id = 0;
            string name = string.Empty;
            double cena = 0.0;
            bool radi;
            try
            {
                radi = true;
                id = int.Parse(txtID_proz.Text);
                foreach (Proizvod u in ListOfProizvod)
                {
                    if (u.New_id == id)
                    {
                        throw new Exception("Proizovd sa ovim ID-om Postiji!!!");

                    }
                }
                name = txtIme_proz.Text;

                cena = double.Parse(txtCena_proz.Text);


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Greska pri Unosu", MessageBoxButtons.RetryCancel, MessageBoxIcon.Question);
                radi = false;
            }

            if (radi)
            {
                string destination = Directory.GetCurrentDirectory() + "\\Proizvodi.txt";
                StreamWriter sw = new StreamWriter(destination, true);
                string data = $"{id} {name} {cena}";
                sw.WriteLine(data);
                sw.Close();
                radi = false;
            }

        }
        /// <summary>
        /// Promena proizvoda
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button10_Click(object sender, EventArgs e)
        {
            List<Proizvod> us = new List<Proizvod>();
            us = LoadProizvod();
            Proizvod p;
            int id = 0;
            string name = string.Empty;
            double cena = 0.0;
            bool radi = false;
            try
            {
                radi = true;
                id = int.Parse(txtID_proz.Text);

                name = txtIme_proz.Text;

                cena = double.Parse(txtCena_proz.Text);
               
                us.RemoveAll(x => x.New_id == id);
                us.Add(new Proizvod(id, name, cena));
                
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Greska pri Unosu", MessageBoxButtons.RetryCancel, MessageBoxIcon.Question);
                radi = false;
            }


            if (radi)
            {
                string destination = Directory.GetCurrentDirectory() + "\\Proizvodi.txt";
                StreamWriter sw = new StreamWriter(destination);
                string data;
                using (sw)
                {
                    foreach (Proizvod u in us)
                    {


                        data = $"{u.New_id} {u.Name_object} {u.Cena}";
                        sw.WriteLine(data);
                    }
                }
                sw.Close();
            }

        }
        /// <summary>
        /// Obrisi Proizvod
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button11_Click(object sender, EventArgs e)
        {
            int id = int.Parse(textBox10.Text);
            List<Proizvod> us = LoadProizvod();


            us.RemoveAll(p => p.New_id == id);





            string destination = Directory.GetCurrentDirectory() + "\\Proizvodi.txt";
            StreamWriter sw = new StreamWriter(destination);
            string data;
            using (sw)
            {
                foreach (Proizvod u in us)
                {


                    data = $"{u.New_id} {u.Name_object} {u.Cena}";
                    sw.WriteLine(data);
                }
            }
            sw.Close();
        }
        
        private void button13_Click(object sender, EventArgs e)
        {


            Form4 fr4 = new Form4();
            fr4.Show();
           
            
        }

        
    }
}
