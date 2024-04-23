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
  internal class DbNullable<T> : INullable where T : struct {
    T Value { get; }
    bool HasValue { get; }

    public bool IsNull => !HasValue;

    public DbNullable(T value) {
      Value = value;
      HasValue = true;
    }

    public DbNullable() {
      Value = default(T);
      HasValue = false;
    }

    public static implicit operator T(DbNullable<T> value) {
      return value.Value;
    }

    public static implicit operator DbNullable<T>(T value) {
      return new DbNullable<T>(value);
    }

    public static implicit operator DbNullable<T>(DBNull value) {
      return new DbNullable<T>();
    }

    public static implicit operator DBNull(DbNullable<T> value) {
      return DBNull.Value;
    }
  }
}
