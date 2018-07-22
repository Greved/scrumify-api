using System;
using System.Data;
using Npgsql;

namespace Scrumify.DataAccess.Core
{
	public static class DbConnectionHelper
	{
		/// <summary>  
		/// get the db connection  
		/// </summary>  
		/// <param name="connectionString"></param>  
		/// <returns></returns> 
        public static IDbConnection OpenConnection(string connectionString)
		{
			var conn = new NpgsqlConnection(connectionString) {UserCertificateValidationCallback = delegate { return true; }}; //TODO: back certificate validation!
			conn.Open();
			return conn;
		}
    }
}