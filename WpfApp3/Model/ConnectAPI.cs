using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic;
using WpfApp3.Model;
using System.Web;

public class ConnectAPI
{
    public bool isConnected = false; 
    public string statusMessage = string.Empty;

    string tableName = "questions_collection";

    private string connectionString;
    private Auth auth;
    private SqlConnection connection;

    private ObservableCollection<QuestionItem> questionsList = new ObservableCollection<QuestionItem>();

    public ObservableCollection<QuestionItem> QuestionsList
    {
        get { return questionsList; }
        set { questionsList = value; }
    }


    //public ObservableCollection<QuestionItem>? QuestionsList { get => questionsList; set; }

    public ConnectAPI(Auth auth)
    {
        this.auth = auth;
        SetConnectionString();

    }

    public void Connect()
    {

        try
        {
            string sql = BuildSelectionQuery(auth);
            connection = new SqlConnection(connectionString);
            connection.Open();

            FetchList(sql, connection);
            isConnected = true;
            statusMessage = $"Connected to Table: {tableName}";

        }
        catch (Exception)
        {
            isConnected = false;
            statusMessage = $"Can't establish connection to table: {tableName}";

            throw;
        }
    }

    public void SetConnectionString()
    {
        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

        builder.DataSource = auth.serverName+","+auth.port;
        builder.UserID = auth.userName;
        builder.Password = auth.password;
        builder.InitialCatalog = auth.databaseName;
        builder.TrustServerCertificate = true;
        builder.Encrypt = true;

        connectionString = builder.ConnectionString;
    }
    public void FetchList(string sqlQuery, SqlConnection connection)
    {
        QuestionsList?.Clear();
        using (SqlCommand command = new SqlCommand(sqlQuery, connection))
        {
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    QuestionItem data = new QuestionItem(id: reader.GetInt32(0),
                        date: reader.GetDateTime(1),
                        question: reader.GetString(2),
                        choices: new ObservableCollection<string>(reader.GetString(3).Split(";")),
                        solution: reader.GetInt32(4));

                    QuestionsList?.Add(data);
                }
                reader.Close();
            }

            command.Dispose();
        }
        Console.WriteLine(QuestionsList?.Count);
    }

    public void Close()
    {
        isConnected = false;
        statusMessage = $"Connected from table: {tableName}";
        QuestionsList?.Clear();
        connection?.Close();
    } 

    private string BuildSelectionQuery(Auth auth)
    {

        List<string> attributesSelections =
        [
            "id",
            "date",
            "question",
            "choices",
            "solution",
        ];

        string selections = string.Join(",", attributesSelections);

        return "Select " + selections + " from " + tableName;
    }
}
