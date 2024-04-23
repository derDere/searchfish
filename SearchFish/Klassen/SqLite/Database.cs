using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data.SqlClient;
using IO = System.IO;
using System.Data.SqlTypes;
using System.ComponentModel;
using System.Data.Entity.Infrastructure;
using System.CodeDom;

namespace SearchFish.SqLite {
  internal class Database {

    private static SQLiteParameter P(string k, object v) {
      return new SQLiteParameter(k, v);
    }

    #region Commands
    public static readonly Command CreateDataBase = new Command(Properties.Resources.CreateDataBase);
    public static readonly Command CheckDataBase = new Command(Properties.Resources.CheckDataBase);
    public static readonly Command Select1 = new Command("SELECT 1");
    public static readonly Command SelectVal = new Command("SELECT @val", P("@val", "Value"));
    public static readonly Command Nothing = new Command("PRAGMA table_info(nonexistent_table)");
    #endregion

    internal static SQLiteConnection connection { get; private set; }
    internal static SQLiteTransaction currentTrans { get; private set; }

    public static string FileName { get; set; }
    public static string Password { private get; set; }

    public static bool IsOpen {
      get {
        return connection != null;
      }
    }

    public static bool FileExists {
      get {
        return IO.File.Exists(FileName);
      }
    }

    public static void Create() {
      if (string.IsNullOrEmpty(FileName) || string.IsNullOrWhiteSpace(FileName)) {
        throw new ArgumentNullException("FileName", "FileName is null or empty");
      }
      if (IO.File.Exists(FileName)) {
        throw new IO.IOException($"File already exists: {FileName}");
      }
      SQLiteConnection.CreateFile(FileName);
    }

    public static void Open() {
      if (string.IsNullOrEmpty(FileName) || string.IsNullOrWhiteSpace(FileName)) {
        throw new ArgumentNullException("FileName", "FileName is null or empty");
      }
      if (!IO.File.Exists(FileName)) {
        throw new IO.FileNotFoundException("File not found", FileName);
      }
      if (connection == null) {
        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
        builder.DataSource = FileName;
        if (!string.IsNullOrEmpty(Password) && !string.IsNullOrWhiteSpace(Password)) {
          builder.Password = Password;
        }
        connection = new SQLiteConnection(builder.ToString());
        connection.Open();
        currentTrans = connection.BeginTransaction();
      }
      else {
        throw new InvalidOperationException("Connection already open");
      }
    }

    public static void Commit() {
      if (connection == null) {
        throw new InvalidOperationException("No connection to commit");
      }
      if (currentTrans == null) {
        throw new InvalidOperationException("No transaction to commit");
      }
      currentTrans.Commit();
      currentTrans.Dispose();
      currentTrans = connection.BeginTransaction();
    }

    public static void Rollback() {
      if (connection == null) {
        throw new InvalidOperationException("No connection to rollback");
      }
      if (currentTrans == null) {
        throw new InvalidOperationException("No transaction to rollback");
      }
      currentTrans.Rollback();
    }

    public static void Close() {
      if (connection == null) {
        throw new InvalidOperationException("No connection to close");
      }
      if (currentTrans != null) {
        currentTrans.Commit();
        currentTrans.Dispose();
        currentTrans = null;
      }
      connection.Close();
      connection.Dispose();
      connection = null;
    }
  }
}
