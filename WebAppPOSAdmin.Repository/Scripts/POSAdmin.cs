using System;
using System.Data;
using System.Data.SqlClient;

using WebAppPOSAdmin.Repository.Properties;

namespace WebAppPOSAdmin.Repository.Scripts
{
    public class POSAdmin
    {
        private static SqlConnection connectionLocal;

        public static SqlConnection getConnectionLocal()
        {
            if (connectionLocal == null)
            {
                connectionLocal = new SqlConnection(Settings.Default.pos_adminConnectionString);
                connectionLocal.Open();
            }
            return connectionLocal;
        }

        public SqlDataReader GetDataReader(string sql)
        {
            SqlCommand sqlCommand = new SqlCommand(sql, getConnectionLocal());
            SqlDataReader result = sqlCommand.ExecuteReader();
            sqlCommand.Dispose();
            return result;
        }

        public DataSet GetDataSet(string sql)
        {
            using SqlCommand sqlCommand = new SqlCommand(sql, getConnectionLocal());
            sqlCommand.CommandType = CommandType.Text;
            using SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
            DataSet dataSet = new DataSet();
            sqlDataAdapter.Fill(dataSet);
            return dataSet;
        }

        public void ExecuteSQL(string sql)
        {
            new SqlCommand(sql, getConnectionLocal()).ExecuteNonQuery();
        }
    }
}
