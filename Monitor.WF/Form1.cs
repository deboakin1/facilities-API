using Monitor.Model;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using System.Text;
using System.Text.Json;

namespace Monitor.WF
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Do_Work_Click(object sender, EventArgs e)
        {
            Task.Run(async () => await this.SendFaciltyDataToAPI());
        }

        async Task SendFaciltyDataToAPI()
        {
            try
            {
                HttpClient client = new();
                var apiEndpoint = "https://localhost:44310/api/Facilities/UpdateFacilities";
                var myDonations = GetFacilityMock();
                //var myFacilities = GetFacilities();

                var dataToJson = JsonSerializer.Serialize(myDonations);

                var dataToStringContent = new StringContent(dataToJson, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(apiEndpoint, content: dataToStringContent);

                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                    Console.WriteLine("done done");
                else
                    Console.WriteLine("please try again");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }

        }

        IEnumerable<FacilityInfo> GetFacilities()
        {
            string connectionString = "server=localhost; uid=root; pwd=Nu66et; database=mysql";
            MySqlConnection connection = new(connectionString);
            List<FacilityInfo> facilities = new();
            using (connection)
            {

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
                                new FacilityInfo
                                (
                                    PatientId: Convert.ToInt64(reader["Patient_Id"]),
                                    Name: reader["Name"] is DBNull ? string.Empty : reader["Name"].ToString(),
                                    DateCreated: Convert.ToDateTime(reader["Date_Created"]),
                                    Submissions: Convert.ToInt16(reader["Submissions"])
                                )
                            );
                    }
                    reader.Close();
                    return facilities;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }
        }

        IEnumerable<FacilityInfo> GetFacilityMock()
        {
            var connectionString = "Server=(localdb)\\mssqllocaldb;Database=OpenalmzDatabase;Trusted_Connection=True;MultipleActiveResultSets=true";
            string queryString = "SELECT Id, DonorId, CreatedOn, Status from dbo.Donation where Status = 1";
            List<FacilityInfo> donations = new();
            using (SqlConnection connection =
                    new SqlConnection(connectionString))
            {
                // Create the Command and Parameter objects.
                SqlCommand command = new SqlCommand(queryString, connection);

                // Open the connection in a try/catch block.
                // Create and execute the DataReader, writing the result
                // set to the console window.
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        donations.Add(
                                new FacilityInfo
                                (
                                    PatientId: Convert.ToInt64(reader["Id"]),
                                    Name: reader["DonorId"].ToString(),
                                    DateCreated: DateTime.Parse(reader["CreatedOn"].ToString()),
                                    Submissions: Convert.ToInt16(reader["Status"])
                                )
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
}
