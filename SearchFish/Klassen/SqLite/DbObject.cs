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
  internal abstract class DbObject {
    internal abstract void Load(SQLiteDataReader reader);
  }
}
