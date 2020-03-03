using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace OracleDependencyPrueba.Models
{
    public class Pipe
    {
        public static OracleConnection con = null;
        public static OracleDependency dep = null;

        public static void OraclePipeListener()
        {
            con = new OracleConnection(ConfigurationManager.ConnectionStrings["AOPEntities"].ConnectionString);
            con.Open();

            OracleCommand cmd = new OracleCommand("select * from VTMOS_EVENTOS", con);

            dep = new Oracle.ManagedDataAccess.Client.OracleDependency(cmd);
            dep.OnChange += new OnChangeEventHandler(DependencyChange);

            cmd.Notification.IsNotifiedOnce = false;
            cmd.AddRowid = true;

            cmd.ExecuteNonQuery();
            con.Close();
            con.Dispose();
        }

        public static void DependencyChange(object src, EventArgs arg)
        {
            OracleCommand cmd = new OracleCommand();
            cmd.Connection = con;
            cmd.CommandText = "select * from VTMOS_EVENTOS where PROCESADOAOP = 0";
            cmd.CommandType = CommandType.Text;
            OracleDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {

            }

        }

        public static event EventHandler OnTable_Change;
    }

    public delegate void EventHandler(object sender, EventArgs arg);
}