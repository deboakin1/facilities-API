using Monitor.Model;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;
using System.Text.Json;

namespace Monitor.WF
{
    public partial class Form1 : Form
    {
        public string? ConnectionString { get; set; }
        public string? APIEndPoint { get; set; }
        public Form1()
        {
            InitializeComponent();
            ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];
            APIEndPoint = ConfigurationManager.AppSettings["APIEndPoint"];
        }

        private async void Do_Work_Click(object sender, EventArgs e)
        {
            var isConfigurationSet = !string.IsNullOrEmpty(ConnectionString) && !string.IsNullOrEmpty(APIEndPoint);
            if (!isConfigurationSet)
            {
                MessageBox.Show("Please stop this application add the connection string and api endpoint in the app.config file");
                return;
            }
            await this.SendFaciltyDataToAPI();
        }

        async Task SendFaciltyDataToAPI()
        {
            try
            {
                HttpClient client = new();
                var myDonations = GetFacilityMock();
                //var myFacilities = GetFacilities();

                var dataToJson = JsonSerializer.Serialize(myDonations);

                var dataToStringContent = new StringContent(dataToJson, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(this.APIEndPoint, content: dataToStringContent);

                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                    MessageBox.Show("Data send to the api");
                else
                    MessageBox.Show("There was an issue posting the data to the api");
            }
            catch (Exception ex)
            {
                MessageBox.Show("There was an issue posting the data to the api");
            }

        }

        IEnumerable<FacilityInfo> GetFacilities()
        {
            List<FacilityInfo> facilities = new();
            using MySqlConnection connection = new(this.ConnectionString);

            MySqlCommand command = connection.CreateCommand();
            command.CommandType = System.Data.CommandType.Text;
            command.CommandText = "SELECT patient_id, form.name, DATE_FORMAT(enc.date_created,'%d-%b-%Y') AS date_created, " +
                "COUNT(*) AS submissions FROM encounter enc JOIN form ON enc.form_id = form.form_id " +
                "WHERE form.form_id IN(27,98) AND enc.date_created BETWEEN '2021-01-01' AND '2021-07-31' " +
                "GROUP BY patient_id, form.name, DATE_FORMAT(enc.date_created, '%d-%b-%Y') " +
                "ORDER BY enc.date_created";

            try
            {
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    facilities.Add(
                            new FacilityInfo()
                            {
                                PatientId = Convert.ToInt64(reader["Patient_Id"]),
                                Name = reader["Name"] is DBNull ? string.Empty : reader["Name"].ToString(),
                                DateCreated = Convert.ToDateTime(reader["Date_Created"]),
                                Submissions = Convert.ToInt16(reader["Submissions"])
                            }
                        );
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("There was an issue posting the data to the api");
            }
            return facilities;
        }

        IEnumerable<FacilityInfo> GetFacilityMock()
        {
            //var connectionString = "Server=(localdb)\\mssqllocaldb;Database=OpenalmzDatabase;Trusted_Connection=True;MultipleActiveResultSets=true";
            string queryString = "SELECT Id, DonorId, CreatedOn, Status from dbo.Donation where Status = 1";
            List<FacilityInfo> donations = new();
            using SqlConnection connection =
                    new SqlConnection(this.ConnectionString);
            SqlCommand command = new SqlCommand(queryString, connection);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    donations.Add(
                            new FacilityInfo
                            {
                                PatientId = Convert.ToInt64(reader["Id"]),
                                Name = reader["DonorId"].ToString(),
                                DateCreated = DateTime.Parse(reader["CreatedOn"].ToString()),
                                Submissions = Convert.ToInt16(reader["Status"])
                            }
                        );
                }
                reader.Close();
                return donations;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
