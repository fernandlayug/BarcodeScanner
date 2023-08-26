using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace BarcodeScanner
{
    public partial class MainPage : ContentPage
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
        }

        SqlConnection sqlConnection;
        public MainPage()
        {
            InitializeComponent();
            string serverdbname = "src_db";
            string servername = "192.168.100.237"; // Using wifi the mobile app can get access to SSMS
            string serverusername = "sa";
            string serverpassword = "masterfile";

            string sqlconn = $"Data Source={servername};Initial Catalog={serverdbname};User ID={serverusername};Password={serverpassword}";
            sqlConnection = new SqlConnection(sqlconn);
        }

        private async void ConnectServer_Clicked(object sender, EventArgs e)
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

        private async void MyScanner_Clicked(object sender, EventArgs e)
        {


            var scan = new ZXingScannerPage();
            await Navigation.PushAsync(scan);
            scan.OnScanResult += (result) =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Navigation.PopAsync();
                   /* MyScanner.Text = result.Text;*/
                    Usersn.Text = result.Text;
                });
            };
        }

        private async void ViewRecord_Clicked(object sender, EventArgs e)
        { 
            try
            {
                List<MyTableList> myTableLists = new List<MyTableList>();
                sqlConnection.Open();
                string queryString = "Select * from dbo.tbldevice";
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

                MyListView.ItemsSource = myTableLists;
            }   
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Alert",ex.Message,"Ok");   
                throw;
            }
                     
        }

        private async void AddRecord_Clicked(object sender, EventArgs e)
        {
            try
            {
                
                sqlConnection.Open();
                using (SqlCommand command = new SqlCommand("INSERT INTO dbo.tbldevice (assettag, assettype, devicename, brand, model, sn, department, location, deviceuser, price, HWdetail, status)" +
                    " VALUES (@assettag, @assettype, @devicename, @brand, @model, @sn, @department, @location, @deviceuser, @price, @HWdetail, @status)", sqlConnection))
                {
                    command.Parameters.Add(new SqlParameter("assettag", Userassettag.Text));
                    command.Parameters.Add(new SqlParameter("assettype", Userassettype.Text));
                    command.Parameters.Add(new SqlParameter("devicename", Userdevicename.Text));
                    command.Parameters.Add(new SqlParameter("brand", Userbrand.Text));
                    command.Parameters.Add(new SqlParameter("model", Usermodel.Text));
                    command.Parameters.Add(new SqlParameter("sn", Usersn.Text));
                    command.Parameters.Add(new SqlParameter("department", Userdepartment.Text));
                    command.Parameters.Add(new SqlParameter("location", Userlocation.Text));
                    command.Parameters.Add(new SqlParameter("deviceuser", Userdeviceuser.Text));
/*                    command.Parameters.Add(new SqlParameter("datepurchased", Userdatepurchased2.Text));*/
                    command.Parameters.Add(new SqlParameter("price", Userprice.Text));
                    command.Parameters.Add(new SqlParameter("HWdetail", UserHWdetail.Text));
                    command.Parameters.Add(new SqlParameter("status", Userstatus.Text));
                    command.ExecuteNonQuery();

                }
                sqlConnection.Close();
                await App.Current.MainPage.DisplayAlert("Alert", "Congrats you just posted data", "Ok");


            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Alert", ex.Message, "Ok");
                throw;
            }

        }

/*        private async void UpdateRecord_Clicked(object sender, EventArgs e)
        {
            try
            {
                sqlConnection.Open();

                int IdtoBeUpdated = Convert.ToInt32(UserId.Text);
                string TitleTobeUpdated = UserTitle.Text;
                string BodyTobeUpdated = UserBody.Text;

                string qerystr = $"UPDATE dbo.mytable SET Id='{IdtoBeUpdated}' ,  Title ='{TitleTobeUpdated}', Body ='{BodyTobeUpdated}' WHERE Id='{IdtoBeUpdated}'";


                using (SqlCommand command = new SqlCommand (qerystr, sqlConnection))
                {

                    command.ExecuteNonQuery();
                    sqlConnection.Close();

                }

                await App.Current.MainPage.DisplayAlert("Alert", "Congrats you have Successfully Updated the row item", "Ok");

            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Alert", ex.Message, "Ok");
                throw;
            }
        }  */ 

        private void ViewLogs_Clicked(object sender, EventArgs e)
        {

        }

        private void Exit_Clicked(object sender, EventArgs e)
        {

        }

        private async void View_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ViewRecord());
        }


    }
}
