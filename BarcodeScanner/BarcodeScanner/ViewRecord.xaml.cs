using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace BarcodeScanner
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewRecord : ContentPage
    {
            public class MyTableList
            {
                public string assettag { get; set; }
                public string assettype { get; set; }
                public string devicename { get; set; }

                public string brand { get; set; }
                public string model { get; set; }
                public string sn { get; set; }
                public string department { get; set; }
                public string location { get; set; }
                public string deviceuser { get; set; }
                /*            public string datepurchased { get; set; }*/
                public float price { get; set; }
                public string HWdetail { get; set; }
                public string status { get; set; }
            public string status2 { get; set; }


        }

            SqlConnection sqlConnection;

        //public string Varfind { get; set; }



        public ViewRecord()
            {
                InitializeComponent();
                string serverdbname = "src_db";
                string servername = "192.168.100.237"; // Using wifi the mobile app can get access to SSMS
                string serverusername = "sa";
                string serverpassword = "masterfile";

                string sqlconn = $"Data Source={servername};Initial Catalog={serverdbname};User ID={serverusername};Password={serverpassword}";
                sqlConnection = new SqlConnection(sqlconn);
            }




        private async void Find_Clicked(object sender, EventArgs e)
        {
           var Varfind = UserSearch.Text;

            try
            {
                                  
                List<MyTableList> myTableLists = new List<MyTableList>();
                sqlConnection.Open();

                string queryString = $"Select * from dbo.tbldevice WHERE sn = '{UserSearch.Text}'";
       
                SqlCommand command = new SqlCommand(queryString, sqlConnection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    myTableLists.Add(new MyTableList

                    {

                        /*                        Id = Convert.ToInt32(reader["Id"]),*/
                        assettag = reader["assettag"].ToString(),
                        assettype = reader["assettype"].ToString(),
                        devicename = reader["devicename"].ToString(),

                    }
                    );
                }
                reader.Close();
                sqlConnection.Close();

                MyView.ItemsSource = myTableLists;
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Alert", ex.Message, "Ok");
                throw;
            }

        }

        private async void ConnectServer2_Clicked(object sender, EventArgs e)
        {

            sqlConnection.Open();
            await App.Current.MainPage.DisplayAlert("Alert", "Connection Establish", "Ok");
            sqlConnection.Close();
            try
            {


                // Perform database operations here
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                throw;

            }
        }

    }
 }