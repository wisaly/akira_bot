using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Text;

namespace libtest
{
    class Kv
    {
        public string k;    // key
        public string v;    // value
        public string r;    // remark
        public Kv(string ak = "", string av = "", string ar = "")
        {
            k = ak;
            v = av;
            r = ar;
        }
    }
    class KvTable
    {
        static SQLiteConnection conn_ = null;
        string tbName_ = "";
        
        public KvTable(string tableName)
        {
            if (conn_ == null)
            {
                conn_ = new SQLiteConnection($"Data Source={Path.Combine(Directory.GetCurrentDirectory(), "knowlage.db")}");
                conn_.Open();

                //execute("create table if not exists qa(k text primary key,v text,r text)");
            }

            tbName_ = tableName;

            execute($"create table if not exists {tbName_}(k text primary key,v text,r text)");
        }

        public static void close()
        {
            if (conn_ == null)
                conn_.Close();
        }

        public int execute(string sql)
        {
            int num = 0;
            SQLiteCommand command = new SQLiteCommand(sql, conn_);
            num = command.ExecuteNonQuery();
            return num;
        }

        public bool insertOrUpdate(Kv kv)
        {
            string sql = $"insert or replace into {tbName_} (k,v,r) values (:k,:v,:r)";
            SQLiteCommand command = new SQLiteCommand(sql, conn_);
            command.Parameters.Add(new SQLiteParameter(":k", kv.k));
            command.Parameters.Add(new SQLiteParameter(":v", kv.v));
            command.Parameters.Add(new SQLiteParameter(":r", kv.r));
            command.ExecuteNonQuery();
            return true;
        }

        public List<Kv> fuzzyQuery(string k)
        {
            string sql = $"select * from {tbName_} where k like :k";
            SQLiteCommand cmd = new SQLiteCommand(sql, conn_);
            cmd.Parameters.Add(new SQLiteParameter(":k", $"%{k}%"));
            SQLiteDataReader dr = cmd.ExecuteReader();
            List<Kv> result = new List<Kv>();
            if (dr != null && dr.Read())
            {
                Kv kv = new Kv();
                kv.k = dr["k"] as string;
                kv.v = dr["v"] as string;
                kv.r = dr["r"] as string;
                result.Add(kv);
            }

            return result;
        }

        public long count()
        {
            string sql = $"select count(*) from {tbName_}";
            SQLiteCommand cmd = new SQLiteCommand(sql, conn_);
            SQLiteDataReader dr = cmd.ExecuteReader();
            if (dr != null && dr.Read())
            {
                return (long)dr[0];
            }
            return -1;
        }
    }
}
