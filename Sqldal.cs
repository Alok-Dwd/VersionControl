﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
/// <summary>
/// Summary description for Sqldal
/// </summary>
public class Sqldal
{
    public Sqldal()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    private string ConnectionString { get; set; }
    public Sqldal(string connectionString)
    {
        ConnectionString = connectionString;
    }
    public void CloseConnection(SqlConnection connection)
    {
        connection.Close();
    }
    public SqlParameter CreateParameter(string name, object value, DbType dbType)
    {
        return CreateParameter(name, 0, value, dbType, ParameterDirection.Input);
    }
    public SqlParameter CreateParameter(string name, int size, object value, DbType dbType)
    {
        return CreateParameter(name, size, value, dbType, ParameterDirection.Input);
    }
    public SqlParameter CreateParameter(string name, int size, object value, DbType dbType, ParameterDirection direction)
    {
        return new SqlParameter
        {
            DbType = dbType,
            ParameterName = name,
            Size = size,
            Direction = direction,
            Value = value
        };
    }

    public DataTable GetDataTable(string commandText, CommandType commandType, SqlParameter[] parameters = null)
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();
            using (var command = new SqlCommand(commandText, connection))
            {
                command.CommandType = commandType;
                if (parameters != null)
                {
                    foreach (var parameter in parameters)
                    {
                        command.Parameters.Add(parameter);
                    }
                }
                var dataset = new DataSet();
                var dataAdaper = new SqlDataAdapter(command);
                dataAdaper.Fill(dataset);
                return dataset.Tables[0];
            }
        }
    }
    public DataSet GetDataSet(string commandText, CommandType commandType, SqlParameter[] parameters = null)
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();
            using (var command = new SqlCommand(commandText, connection))
            {
                command.CommandType = commandType;
                command.CommandTimeout = 60;
                if (parameters != null)
                {
                    foreach (var parameter in parameters)
                    {
                        command.Parameters.Add(parameter);
                    }
                }
                var dataset = new DataSet();
                var dataAdaper = new SqlDataAdapter(command);
                dataAdaper.Fill(dataset);
                return dataset;
            }
        }
    }
    public IDataReader GetDataReader(string commandText, CommandType commandType, SqlParameter[] parameters, out SqlConnection connection)
    {
        IDataReader reader = null;
        connection = new SqlConnection(ConnectionString);
        connection.Open();
        var command = new SqlCommand(commandText, connection);
        command.CommandType = commandType;
        if (parameters != null)
        {
            foreach (var parameter in parameters)
            {
                command.Parameters.Add(parameter);
            }
        }
        reader = command.ExecuteReader();
        return reader;
    }
    public void Delete(string commandText, CommandType commandType, SqlParameter[] parameters = null)
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();
            using (var command = new SqlCommand(commandText, connection))
            {
                command.CommandType = commandType;
                if (parameters != null)
                {
                    foreach (var parameter in parameters)
                    {
                        command.Parameters.Add(parameter);
                    }
                }
                command.ExecuteNonQuery();
            }
        }
    }
    public void Insert(string commandText, CommandType commandType, SqlParameter[] parameters)
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();
            using (var command = new SqlCommand(commandText, connection))
            {
                command.CommandType = commandType;
                if (parameters != null)
                {
                    foreach (var parameter in parameters)
                    {
                        command.Parameters.Add(parameter);
                    }
                }
                command.ExecuteNonQuery();
            }
        }
    }
    public int Insert(string commandText, CommandType commandType, SqlParameter[] parameters, out int lastId)
    {
        lastId = 0;
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();
            using (var command = new SqlCommand(commandText, connection))
            {
                command.CommandType = commandType;
                if (parameters != null)
                {
                    foreach (var parameter in parameters)
                    {
                        command.Parameters.Add(parameter);
                    }
                }
                object newId = command.ExecuteScalar();
                lastId = Convert.ToInt32(newId);
            }
        }
        return lastId;
    }
    public long Insert(string commandText, CommandType commandType, SqlParameter[] parameters, out long lastId)
    {
        lastId = 0;
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();
            using (var command = new SqlCommand(commandText, connection))
            {
                command.CommandType = commandType;
                if (parameters != null)
                {
                    foreach (var parameter in parameters)
                    {
                        command.Parameters.Add(parameter);
                    }
                }
                object newId = command.ExecuteScalar();
                lastId = Convert.ToInt64(newId);
            }
        }
        return lastId;
    }
    public void InsertWithTransaction(string commandText, CommandType commandType, SqlParameter[] parameters)
    {
        SqlTransaction transactionScope = null;
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();
            transactionScope = connection.BeginTransaction();
            using (var command = new SqlCommand(commandText, connection))
            {
                command.CommandType = commandType;
                if (parameters != null)
                {
                    foreach (var parameter in parameters)
                    {
                        command.Parameters.Add(parameter);
                    }
                }
                try
                {
                    command.ExecuteNonQuery();
                    transactionScope.Commit();
                }
                catch (Exception)
                {
                    transactionScope.Rollback();
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
    public void InsertWithTransaction(string commandText, CommandType commandType, IsolationLevel isolationLevel, SqlParameter[] parameters)
    {
        SqlTransaction transactionScope = null;
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();
            transactionScope = connection.BeginTransaction(isolationLevel);
            using (var command = new SqlCommand(commandText, connection))
            {
                command.CommandType = commandType;
                if (parameters != null)
                {
                    foreach (var parameter in parameters)
                    {
                        command.Parameters.Add(parameter);
                    }
                }
                try
                {
                    command.ExecuteNonQuery();
                    transactionScope.Commit();
                }
                catch (Exception)
                {
                    transactionScope.Rollback();
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
    public void Update(string commandText, CommandType commandType, SqlParameter[] parameters)
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();
            using (var command = new SqlCommand(commandText, connection))
            {
                command.CommandType = commandType;
                if (parameters != null)
                {
                    foreach (var parameter in parameters)
                    {
                        command.Parameters.Add(parameter);
                    }
                }
                command.ExecuteNonQuery();
            }
        }
    }
    public void UpdateWithTransaction(string commandText, CommandType commandType, SqlParameter[]
    parameters)
    {
        SqlTransaction transactionScope = null;
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();
            transactionScope = connection.BeginTransaction();
            using (var command = new SqlCommand(commandText, connection))
            {
                command.CommandType = commandType;
                if (parameters != null)
                {
                    foreach (var parameter in parameters)
                    {
                        command.Parameters.Add(parameter);
                    }
                }
                try
                {
                    command.ExecuteNonQuery();
                    transactionScope.Commit();
                }
                catch (Exception)
                {
                    transactionScope.Rollback();
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
    public void UpdateWithTransaction(string commandText, CommandType commandType, IsolationLevel isolationLevel, SqlParameter[] parameters)
    {
        SqlTransaction transactionScope = null;
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();
            transactionScope = connection.BeginTransaction(isolationLevel);
            using (var command = new SqlCommand(commandText, connection))
            {
                command.CommandType = commandType;
                if (parameters != null)
                {
                    foreach (var parameter in parameters)
                    {
                        command.Parameters.Add(parameter);
                    }
                }
                try
                {
                    command.ExecuteNonQuery();
                    transactionScope.Commit();
                }
                catch (Exception)
                {
                    transactionScope.Rollback();
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
    public object GetScalarValue(string commandText, CommandType commandType, SqlParameter[] parameters = null)
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();
            using (var command = new SqlCommand(commandText, connection))
            {
                command.CommandType = commandType;
                if (parameters != null)
                {
                    foreach (var parameter in parameters)
                    {
                        command.Parameters.Add(parameter);
                    }
                }
                return command.ExecuteScalar();
            }
        }
    }

    #region ExecuteSPWithRerturnValue

    public string ExecuteSPWithRerturnValue(string spName, CommandType commandType, SqlCommand command)
    {
        int i = 0;
        string result = string.Empty;
        try
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                command.Connection = connection;
                command.CommandType = commandType;
                command.CommandText = spName;
                command.ExecuteNonQuery();

                i = (int)command.Parameters[4].Value;

                if (i == 0)
                {
                    result = "Failure";
                }
                else if (i == 1)
                {
                    result = "Success";
                }
                else if (i == 2)
                {
                    result = "New Record";
                }
            }
        }
        catch (Exception ex)
        {

        }
        finally
        {
            command.Dispose();
        }

        return result;
    }

    #endregion
}