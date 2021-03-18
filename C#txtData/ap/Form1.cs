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


    public partial class Form1 : Form
    {
        
 
        List<User> ListOfUsers;
        Form2 f2;
        Form3 f3;
        public Form1()
        {
            InitializeComponent();
            ListOfUsers = null;
            f2 = null;
            f3 = null;

        }

        private List<User> LoadData() {
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


        private void btnLog_Click(object sender, EventArgs e)
        {
           
            /// SVI POTREBNI PODACI

            ListOfUsers = LoadData();

            int id = 0;
            string name = string.Empty;
            int age_birth = 0;
            string age_format = string.Empty;
            bool age = false;
            bool type = false;
            string Tip = string.Empty;
            bool radi = false;

            ///
            /// TRY CATCH PODACI
            /// 
            ///
            try
            {
                radi = true;
                id = int.Parse(textBox1.Text);
                age_format = textBox3.Text;
                name = textBox2.Text;
                age = age_format.Any(char.IsDigit);
                if (age_format.Length != 4 && age )
                {
                    MessageBox.Show("Godiste (4) broja!");
                    age_format = "Greska!";

                }             
                age_birth = Int32.Parse(age_format);
                Tip = textBox4.Text;
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
                else{
                    radi = false;
                    MessageBox.Show("Za tip izabrati 'Korisnik' ili 'Administrator'", "Greska", MessageBoxButtons.OK);
                    }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Greska pri Unosu", MessageBoxButtons.RetryCancel, MessageBoxIcon.Question);
                
            }

            ///
            ///         PROVERA LISTE DAL POSTOJI KORISNIK ILI ADMINISTRATOR , ILI NE!
            ///         OTVARANEJ NOVE FORME I NJIHOVIH FUNKCIJA
            ///   

            if (radi)
            {

                bool login = false;
                foreach (User c in ListOfUsers)
                {
                    User pam = new User(id, name, age_birth, type);

                    if (c.CopareUser(pam))
                    {

                        if (type)
                        {
                            login= true;
                            MessageBox.Show("Uspesno ste se ulogovali kao Administrator!");
                            if (f2 == null)
                            {
                                f2 = new Form2(pam);
                                f2.ShowDialog();
                                this.Hide();
                                this.Close();
                            }
                            break;
                        }
                        else
                        {
                            login = true;
                            MessageBox.Show("Uspesno ste se ulogovali kao Korisnik");

                            if (f3 == null)
                            {
                                f3 = new Form3(pam);
                                f3.ShowDialog();
                                this.Hide();
                                this.Close();
                            }
                            break;

                        }

                    }

                   
                }

                if (!login)
                {
                    MessageBox.Show("Pogresni podaci za korisnika");
                }



            }
                
        }
            
    }



    /// <summary>
    /// da se pokupe informacije o koriniku koji dalje ide u apl!
    /// </summary>

    public class UserID
    {
        public static int userID;
        public static string name;
        public static int age;
        public static bool type;
        
    }
}


        
       


    



    

    
















