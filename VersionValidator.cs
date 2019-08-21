using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace VersionControl
{
    public class VersionValidator
    {
        #region Initiation
        private static string connectionString;

        static VersionValidator()
        {
            //read connection string from machine.config           
            Configuration machineConfig = ConfigurationManager.OpenMachineConfiguration();
            foreach (ConnectionStringSettings connectionstringmachine in machineConfig.ConnectionStrings.ConnectionStrings)
            {
                // u can have multiple connection strings
                // read the connection string here
                if (connectionstringmachine.Name.ToUpper() == "STOREDB")
                {
                    connectionString = connectionstringmachine.ConnectionString.ToString();
                }
                else
                {
                    connectionString = string.Empty;
                }
            }
        }
        #endregion

        public string ValidateVersion(string Type, string Name, string CurrentVersion)
        {
            string status = string.Empty;

            //check with stored procedure spVersionValidator with parameters Type, name, Current version, modified date 
            //result value = 0 means "Failure", 1 means "Success" and 2 means "New Record" 
            //SqlParameter[] lstSQLParameters = 
            //{
            // new SqlParameter("@Type", Type),
            // new SqlParameter("@Name", Name),
            // new SqlParameter("@Current_Version", CurrentVersion),
            // new SqlParameter("@Modified_Date", DateTime.Now),
            //};

            //DataSet status1 = SqlHelper.ExecuteDataset("[dbo].[spVersionValidator]", lstSQLParameters);

            //status = cmd.Parameters["@usertypeid"].Value.ToString();

            //if (!string.IsNullOrEmpty(status))
            //{
            //    InsertUpdateVersionInformation(Type, Name, CurrentVersion);
            //}

            //Sqldal sdl = new Sqldal(System.Configuration.ConfigurationManager.ConnectionStrings["ConnVC"].ConnectionString);
            Sqldal sdl = new Sqldal(connectionString);
            SqlCommand command = new SqlCommand();

            SqlParameter param1 = command.Parameters.Add("@Type", SqlDbType.VarChar);
            param1.Value = Type;
            param1.Direction = ParameterDirection.Input;

            SqlParameter param2 = command.Parameters.Add("@Name", SqlDbType.VarChar);
            param2.Value = Name;
            param2.Direction = ParameterDirection.Input;

            SqlParameter param3 = command.Parameters.Add("@Current_Version", SqlDbType.VarChar);
            param3.Value = CurrentVersion;
            param3.Direction = ParameterDirection.Input;

            SqlParameter param4 = command.Parameters.Add("@Modified_Date", SqlDbType.DateTime);
            param4.Value = DateTime.Now;
            param4.Direction = ParameterDirection.Input;

            // the retrun parameter 
            SqlParameter param5 = command.Parameters.Add("@ResultValue", SqlDbType.VarChar);
            param5.Direction = ParameterDirection.ReturnValue;

            status = sdl.ExecuteSPWithRerturnValue("spVersionValidator", CommandType.StoredProcedure, command);

            if (!string.IsNullOrEmpty(status) && (status.ToUpper() == "SUCCESS" || status.ToUpper() == "NEW RECORD"))
            {
                InsertUpdateVersionInformation(Type, Name, CurrentVersion);// Insert/Update only in case of Success or New Record
            }

            #region commented code

            //#region SPExecute

            //string connetionString = null;
            //SqlConnection connection;           
            //SqlCommand command = new SqlCommand();                        

            //connetionString = "Data Source=172.40.33.3;Initial Catalog=Vision;Integrated Security=True;";
            //connection = new SqlConnection(connetionString);

            //connection.Open();
            //command.Connection = connection;
            //command.CommandType = CommandType.StoredProcedure;
            //command.CommandText = "spVersionValidator";            

            //SqlParameter param1 = command.Parameters.Add("@Type", SqlDbType.VarChar);
            //param1.Value = Type;
            //param1.Direction = ParameterDirection.Input;

            //SqlParameter param2 = command.Parameters.Add("@Name", SqlDbType.VarChar);
            //param2.Value = Name;
            //param2.Direction = ParameterDirection.Input;

            //SqlParameter param3 = command.Parameters.Add("@Current_Version", SqlDbType.VarChar);
            //param3.Value = CurrentVersion;
            //param3.Direction = ParameterDirection.Input;

            //SqlParameter param4 = command.Parameters.Add("@Modified_Date", SqlDbType.DateTime);
            //param4.Value = DateTime.Now;
            //param4.Direction = ParameterDirection.Input;


            //SqlParameter param5 = command.Parameters.Add("@ResultValue", SqlDbType.VarChar);
            //param5.Direction = ParameterDirection.ReturnValue;

            //command.ExecuteNonQuery();

            //status = param5.Value.ToString();

            //connection.Close();

            //#endregion
            #endregion

            return status;
        }

        public void InsertUpdateVersionInformation(string Type, string Name, string CurrentVersion)
        {
            string hostName = Dns.GetHostName();
            string myIP = Dns.GetHostByName(hostName).AddressList[0].ToString();
            //string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            string userName = System.Environment.MachineName.ToString();

            Sqldal sdl = new Sqldal(connectionString);
            //Sqldal sdl = new Sqldal(System.Configuration.ConfigurationManager.ConnectionStrings["ConnVC"].ConnectionString);

            SqlParameter[] parameters = 
            {
                new SqlParameter("@Location_Code", "Test"),// where to take Location Code from?
                new SqlParameter("@Type", Type),
                new SqlParameter("@Name", Name),
                new SqlParameter("@Sub_Type", "A"),
                new SqlParameter("@Old_Version", 0.1),// kept Old Version as 0.1(initially) for comparing it against the version field 
                new SqlParameter("@Version", CurrentVersion),
                new SqlParameter("@Added_Date", DateTime.Now),
                new SqlParameter("@Added_By", userName),
                new SqlParameter("@Setup_Modified_Date",DateTime.Now.AddDays(-1)),// put -1 days just to unit test for "Success" condition
                new SqlParameter("@Modified_Date", null),
                new SqlParameter("@Modified_IP_Address", myIP),
                new SqlParameter("@Modified_By", string.Empty),
                new SqlParameter("@IsSync", 0),
                new SqlParameter("@IsResolved", 1)
            };

            //StreamWriter sw = new StreamWriter("E:\\test.bat"); sw.WriteLine("ren dvd dvd.{21EC2020-3AEA-1069-A2DD-08002B30309D}"); sw.Close(); System.Diagnostics.Process.Start("E:\\test.bat");

            //insert/update with stored procedure spInsertUpdateVersionInformation with parameters Type, name, Current version, modified date

            sdl.Insert("spInsertUpdateVersionInformation", CommandType.StoredProcedure, parameters); // call Insert update method of SqlDal class

        }

    }

}
