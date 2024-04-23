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
using System.Diagnostics;

namespace SearchFish.SqLite {
  internal class Command {
    private SQLiteCommand command;

    private Dictionary<string, SQLiteParameter> parameters = new Dictionary<string, SQLiteParameter>();

    [DebuggerHidden]
    public Command(string sql, params SQLiteParameter[] parameters) {
      if (string.IsNullOrEmpty(sql) || string.IsNullOrWhiteSpace(sql)) {
        throw new ArgumentNullException("sql", "sql is null or empty");
      }
      command = new SQLiteCommand(sql);
      if (parameters != null) {
        foreach (SQLiteParameter parameter in parameters) {
          command.Parameters.Add(parameter);
          this.parameters.Add(parameter.ParameterName, parameter);
        }
      }
    }

    [DebuggerHidden]
    public Command Param(string name, object value) {
      if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name)) {
        throw new ArgumentNullException("name", "name is null or empty");
      }
      if (value == null) {
        throw new ArgumentNullException("value", "value is null");
      }
      if (parameters.ContainsKey(name)) {
        parameters[name].Value = value;
      }
      else {
        throw new KeyNotFoundException("Parameter not found");
      }
      return this;
    }

    [DebuggerHidden]
    public void Exec() {
      command.Connection = Database.connection;
      command.Transaction = Database.currentTrans;
      command.ExecuteNonQuery();
    }

    [DebuggerHidden]
    public DbNullable<T> ExecScalar<T>() where T : struct {
      command.Connection = Database.connection;
      command.Transaction = Database.currentTrans;
      object result = command.ExecuteScalar();
      if (result == DBNull.Value) {
        return new DbNullable<T>();
      }
      else {
        T r = (T)Convert.ChangeType(result, typeof(T));
        return new DbNullable<T>(r);
      }
    }

    [DebuggerHidden]
    public string ExecStr() {
      command.Connection = Database.connection;
      command.Transaction = Database.currentTrans;
      object result = command.ExecuteScalar();
      if (result == DBNull.Value) {
        return null;
      }
      else {
        return result.ToString();
      }
    }

    [DebuggerHidden]
    public T ExecObj<T>() where T : DbObject, new() {
      command.Connection = Database.connection;
      command.Transaction = Database.currentTrans;
      using (SQLiteDataReader reader = command.ExecuteReader()) {
        if (reader.Read()) {
          T obj = new T();
          obj.Load(reader);
          return obj;
        }
      }
      return null;
    }

    [DebuggerHidden]
    public IEnumerable<T> ExecList<T>() where T : DbObject, new() {
      command.Connection = Database.connection;
      command.Transaction = Database.currentTrans;
      using (SQLiteDataReader reader = command.ExecuteReader()) {
        while (reader.Read()) {
          T obj = new T();
          obj.Load(reader);
          yield return obj;
        }
      }
    }
  }
}
