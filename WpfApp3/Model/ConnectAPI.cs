using Microsoft.Data.SqlClient;
using System.Collections.ObjectModel;
using System.Data;
using WpfApp3.Model;

public class ConnectAPI
{
    public ConnectAPI(Auth auth)
    {
        this.auth = auth;
        SetConnectionString();
        connection = new SqlConnection(connectionString);
        connection.StateChange += Connection_StateChange;

    }


    #region Class Attributes
    public bool isConnected = false;
    public string statusMessage = string.Empty;
    public SqlConnection connection;

    private ObservableCollection<QuestionItem> questionsList = new ObservableCollection<QuestionItem>();

    public ObservableCollection<QuestionItem> QuestionsList
    {
        get { return questionsList; }
        set { questionsList = value; }
    }


    private string tableName = "questions_collection";

    private string connectionString;
    private Auth auth;
    #endregion

    #region Public Methods
    public void Connect()
    {

        try
        {
            if (!isConnected)
            {
                connection.Open();
            }

            FetchList();

        }
        catch (Exception)
        {
            statusMessage = $"Can't establish connection to table: {tableName}.";

            throw;
        }
    }

    public void SetConnectionString()
    {
        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();

        builder.DataSource = auth.serverName + "," + auth.port;
        builder.UserID = auth.userName;
        builder.Password = auth.password;
        builder.InitialCatalog = auth.databaseName;
        builder.TrustServerCertificate = true;
        builder.Encrypt = true;

        connectionString = builder.ConnectionString;
    }
    public void FetchList()
    {
        string sqlQuery = BuildSelectionQuery();
        if (!isConnected)
        {
            return;
        }
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
    }

  

    public void Close()
    {
        if (connection.State != ConnectionState.Closed)
        {
            statusMessage = $"Disconnected from table: {tableName}. ";
            QuestionsList?.Clear();
            connection?.Close();
        }
    }

    public void UpdateList(QuestionItem updated)
    {
        if (!isConnected)
        {
            return;
        }

        QuestionsList?.Clear();
        using (SqlCommand command = BuildUpdateQuery(updated))
        {
            command.ExecuteNonQuery();


            command.Dispose();
        }

        FetchList();
    }

    public void RemoveItem(QuestionItem item)
    {
        if (!isConnected)
        {
            return;
        }

        QuestionsList?.Clear();
        using (SqlCommand command = BuildDeleteQuery(item))
        {
            command.ExecuteNonQuery();
            command.Dispose();
        }

        FetchList();

    }
    #endregion

    #region Private Methods
    private void Connection_StateChange(object sender, StateChangeEventArgs e)
    {

        isConnected = e.CurrentState == ConnectionState.Open;
        if (isConnected)
        {
            statusMessage = $"Connected to Table: {tableName}";
        }

        statusMessage += ($"Connection state changed from {e.OriginalState} to {e.CurrentState}.");
    }


    private string BuildSelectionQuery()
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

    private SqlCommand BuildUpdateQuery(QuestionItem updated)
    {

 
        SqlCommand command = new SqlCommand($"UPDATE {tableName} " +
            "SET date= @date , " +
            "question= @question , " +
            "choices= @choices , " +
            "solution = @solution " +
            "WHERE id = @id;" +
            $"IF NOT EXISTS(SELECT 1 FROM {tableName} WHERE id = @id) " +
            $"BEGIN " +
            $"INSERT INTO {tableName} (id,date,question,choices,solution) " +
            "SELECT @id,@date,@question,@choices,@solution " +
            "END", connection);

        command.Parameters.AddWithValue("@date", updated.Date);
        command.Parameters.AddWithValue("@question", updated.Question);
        command.Parameters.AddWithValue("@choices", string.Join(";", updated.Choices));
        command.Parameters.AddWithValue("@solution", updated.Solution);
        command.Parameters.AddWithValue("@id", updated.Id);


        return command;

    }

    private SqlCommand BuildDeleteQuery(QuestionItem item)
    {

        SqlCommand command = new SqlCommand($"DELETE FROM {tableName} WHERE id=@id", connection);
        command.Parameters.AddWithValue("@id", item.Id);

        return command;
    }

    #endregion
}
