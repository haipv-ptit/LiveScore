﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportNews.Utility
{
    public class DbUtil
    {
        public static MySqlConnection GetDBConnection()
        {
            string host = "localhost";
            int port = 3306;
            string database = "sportnews";
            string username = "root";
            string password = "Abcd@1234";

            return DbMySqlUtil.GetDBConnection(host, port, database, username, password);
        }
    }
}