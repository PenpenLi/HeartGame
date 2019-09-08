using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using UnityEngine;

public class DBHelper:MonoBehaviour{
    private static SqlConnectionStringBuilder scsb;
    private static SqlConnection sc;
    private static SqlCommand com;
    private static void Init()
    {
        scsb = new SqlConnectionStringBuilder();
        scsb.DataSource = "192.168.0.103";
        scsb.UserID = "sa";
        scsb.Password = "sa";
        scsb.IntegratedSecurity = false;
        scsb.InitialCatalog = "HeartGame";
    }
    public static void Connect()
    {
        Init();
        //sc = new SqlConnection(scsb.ConnectionString);
        sc = new SqlConnection(@"server=192.168.0.103;database=HeartGame;uid=sa;pwd=sa");
        try { sc.Open(); }
        catch(Exception e) { print(e.Message); }
    }
    public static void Close()
    {
        sc.Close();
    }
    public static SqlDataReader Select(string sql)
    {
        Connect();
        com = new SqlCommand(sql, sc);
        SqlDataReader sdr = com.ExecuteReader();
        return sdr;
    }
    public static DataSet Selectds(string sql)//select * from table
    {
        Connect();
        com = new SqlCommand(sql,sc);
        SqlDataAdapter sda = new SqlDataAdapter(sql, sc);
        DataSet ds = new DataSet();
        sda.Fill(ds, "Players");
        return ds;
    }
    public static void Closedr(SqlDataReader dr)
    {
        dr.Close();
    }
    public static void  Insert(string sql)//string.format("insert into table('','')values('{0}','{1}')",xx,xx);
    {
        Connect();
        com = new SqlCommand(sql, sc);
        try { com.ExecuteNonQuery(); } catch (Exception e) { print(e.Message); }
        Close();

    }
    public static void Change(string sql)//string.format("update table set xx={0],yy={1] where zz='{2}'",xx,yy,zz);
    {
        Connect();
        com = new SqlCommand(sql, sc);
        try { com.ExecuteNonQuery(); } catch (Exception e) { print(e.Message); }
        Close();
    }
    public static void Delete(string sql)//delete from table where xx=''
    {
        Connect();
        com = new SqlCommand(sql, sc);
        try { com.ExecuteNonQuery(); } catch (Exception e) { print(e.Message); }
        Close();
    }
}
