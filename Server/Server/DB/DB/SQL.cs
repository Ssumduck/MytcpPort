using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;


namespace SQL
{
    public class MySQLCommand
    {
        /// <summary>
        /// Player 데이터베이스에 연결
        /// </summary>
        static MySqlConnection PlayerConnection()
        {
            return new MySqlConnection("Server=localhost;Database=Player;Uid=root;Pwd=root;");
        }

        /// <summary>
        /// GameData 데이터베이스에 연결
        /// </summary>
        static MySqlConnection GameDataConnection()
        {
            return new MySqlConnection("Server=localhost;database=GameData;Uid=root;Pwd=root;");
        }

        /// <summary>
        /// SELECT *FROM table where (where) like (like)
        /// like에 조건식을 넣어 해당 데이터의 존재여부를 리턴해주는 함수
        /// </summary>
        /// <param name="table"></param>
        public static bool SELECT(Database database, string table, string where, string like)
        {
            MySqlConnection connection = null;

            switch (database)
            {
                case Database.Player:
                    connection = PlayerConnection();
                    break;
                case Database.GameData:
                    connection = GameDataConnection();
                    break;
            }

            connection.Open();
            MySqlCommand _command = new MySqlCommand($"SELECT *FROM {table}", connection);
            MySqlDataReader reader = _command.ExecuteReader();

            while (reader.Read())
            {
                if (reader[where].ToString() == like)
                {
                    connection.Close();
                    return true;
                }
            }

            connection.Close();

            return false;
        }

        /// <summary>
        /// SELECT *FROM (table) where (where) like (like)을 통해 검색
        /// 해당 값을 가진 데이터의 열의 data를 리턴
        /// </summary>
        public static string SELECT(Database database, string table, string where, string like, string data)
        {
            MySqlConnection connection = null;

            switch (database)
            {
                case Database.Player:
                    connection = PlayerConnection();
                    break;
                case Database.GameData:
                    connection = GameDataConnection();
                    break;
            }

            connection.Open();
            MySqlCommand _command = new MySqlCommand($"SELECT *FROM {table}", connection);
            MySqlDataReader reader = _command.ExecuteReader();

            while (reader.Read())
            {
                if (reader[where].ToString() == like)
                {
                    string _data = reader[data].ToString();
                    connection.Close();
                    return _data;
                }
            }
            connection.Close();

            return null;
        }

        /// <summary>
        /// idx로 검사하여 해당 열의 데이터를 업데이트합니다.
        /// </summary>
        public static void Update(Database database, string table, int idx, Data d, string changeData)
        {
            MySqlConnection connection = null;

            switch (database)
            {
                case Database.Player:
                    connection = PlayerConnection();
                    break;
                case Database.GameData:
                    connection = GameDataConnection();
                    break;
            }

            connection.Open();
            new MySqlCommand($"update {table} set {d} = '{changeData}' where idx = {idx}", connection).ExecuteNonQuery();
            connection.Close();
        }

        public static void Update(Database database, string table, int idx, Data d, bool changeData)
        {
            MySqlConnection connection = null;

            switch (database)
            {
                case Database.Player:
                    connection = PlayerConnection();
                    break;
                case Database.GameData:
                    connection = GameDataConnection();
                    break;
            }

            connection.Open();
            new MySqlCommand($"update {table} set {d} = {changeData} where idx = {idx}", connection).ExecuteNonQuery();
            connection.Close();
        }

        public static bool Insert(Database database, string table, string where, string value)
        {
            try
            {
                MySqlConnection connection = null;

                switch (database)
                {
                    case Database.Player:
                        connection = PlayerConnection();
                        break;
                    case Database.GameData:
                        connection = GameDataConnection();
                        break;
                }

                connection.Open();
                MySqlCommand command = new MySqlCommand($"INSERT INTO {table}({where}) VALUES({value})", connection);
                command.ExecuteNonQuery();
                connection.Close();

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// update {table} set {data} = {data} + p where idx = {idx}
        /// </summary>
        public static void PlusData(Database database, Table table, int idx , string data, int p)
        {
            MySqlConnection connection = null;

            switch (database)
            {
                case Database.Player:
                     connection = PlayerConnection();
                    break;
                case Database.GameData:
                    connection = GameDataConnection();
                    break;
            }

            connection.Open();

            MySqlCommand command = new MySqlCommand($"update {table} set {data} = {data} + {p} where idx = {idx}", connection);
            command.ExecuteNonQuery();

            connection.Close();
        }
    }
}