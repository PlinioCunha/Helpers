using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using AutoMapper;
using System.Data;
using System.Text;
using System.Reflection;

namespace MVCAPI.DataAcess
{

    public class ADOReader
    {
        public static List<T> ToListaDataReader<T>(IDataReader dataReader)
        {
            List<T> list = new List<T>();
            T instance = default(T);
            while (dataReader.Read())
            {
                instance = Activator.CreateInstance<T>();
                foreach (PropertyInfo property in instance.GetType().GetProperties())
                {
                    try
                    {
                        if (!object.Equals(dataReader[property.Name], DBNull.Value))
                        {
                            property.SetValue(instance, dataReader[property.Name], null);
                        }
                    }
                    catch (Exception e)
                    {

                    }
                }
                list.Add(instance);
            }
            return list;
        }

        public List<T> GetDataGeneric<T>(SqlParameterCollection[] parameters, string sql, bool procedure = false)
        {
            List<T> items = new List<T>();

            using (SqlConnection conn = new SqlConnection(GetConnectingString()))
            {
                using (SqlCommand command = new SqlCommand(sql, conn))
                {
                    command.CommandType = (procedure == true ? CommandType.StoredProcedure : CommandType.Text);
                    command.Parameters.Clear();
                    foreach (var param in parameters)
                    {
                        command.Parameters.Add(param);
                    }

                    conn.Open();
                    var reader = command.ExecuteReader();
                    items = ToListaDataReader<T>(reader);
                    conn.Close();
                }
            }


            return items;
        }
    

        private string GetConnectingString(string conexao = "conexao")
        {
            return ConfigurationManager.ConnectionStrings[conexao].ToString();
        }


        #region Teste
        //public List<Relatorio> GetDataTeste(int ID)
        //{
        //    StringBuilder sqlText = new StringBuilder();
        //    sqlText.AppendLine("SELECT DISTINCT B.idBairro, B.txtNomeBairro, B.txtUrlBairro");
        //    sqlText.AppendLine("FROM  tbBairro B inner join tbCidade cidade on cidade.idCidade = B.idCidade");
        //    sqlText.AppendLine("inner join tbEstado estado on estado.idEstado = cidade.idEstado WHERE(B.idCidade = @idCidade OR @idCidade = 0)");
        //    sqlText.AppendLine("ORDER BY B.txtNomeBairro ASC");

        //    // Procedure
        //    //sqlText.Append("spPortalCarregaListaBairros")

        //    Stopwatch watch = new Stopwatch();
        //    watch.Start();

        //    List<Relatorio> items = new List<Relatorio>();

        //    using (SqlConnection conn = new SqlConnection(GetConnectingString("conexao")))
        //    {
        //        using (SqlCommand command = new SqlCommand(sqlText.ToString(), conn))
        //        {
        //            // Procedure
        //            //command.CommandType = CommandType.StoredProcedure;
        //            command.CommandType = CommandType.Text;
        //            command.Parameters.Clear();
        //            command.Parameters.Add(new SqlParameter("@idCidade", ID));

        //            conn.Open();
        //            var reader = command.ExecuteReader();
        //            items = ToListaDataReader<Relatorio>(reader);

        //        }
        //    }

        //    watch.Stop();
        //    items[0].temp = watch.ElapsedMilliseconds;

        //    return items;
        //}
        #endregion


    }
}
