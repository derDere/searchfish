using SearchFish.Klassen;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using IO = System.IO;
using JJ = Newtonsoft.Json;

namespace SearchFish {
  internal class Config {

    #region Constants
    private const string CONFIG_FILENAME = "config.json";
    private const string APP_DIRNAME = "SearchFish";
    private const string DB_FILENAME = "searchfish.db";
    #endregion

    #region Construction
    private static Config MySelf;

    private static string GetConfigPath() {
      string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
      path = IO.Path.Combine(path, APP_DIRNAME);
      if (!IO.Directory.Exists(path)) {
        IO.Directory.CreateDirectory(path);
      }
      return IO.Path.Combine(path, CONFIG_FILENAME);
    }

    public static void Reload() {
      string path = GetConfigPath();
      if (IO.File.Exists(path)) {
        try {
          string jj = IO.File.ReadAllText(path, Encoding.UTF8);
          MySelf = JJ.JsonConvert.DeserializeObject<Config>(jj);
        }
        catch (Exception ex) {
          MessageBox.Show(ex.Message, "Config.Load", MessageBoxButton.OK, MessageBoxImage.Error);
        }
      }
      else {
        Save();
      }
    }

    public static void Save() {
      string path = GetConfigPath();
      if (MySelf == null) {
        MySelf = new Config();
      }
      try {
        string jj = JJ.JsonConvert.SerializeObject(MySelf, JJ.Formatting.Indented);
        IO.File.WriteAllText(path, jj, Encoding.UTF8);
      }
      catch (Exception ex) {
        MessageBox.Show(ex.Message, "Config.Save", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }

    static Config() {
      Reload();
    }
    #endregion

    #region Properties
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    [JJ.JsonProperty("DbFilePath")]
    private string _DbFilePath = DefaultDbFilePath();
    /// <summary>
    /// ...
    /// </summary>
    public static string DbFilePath {
      get {
        return MySelf._DbFilePath;
      }
      set {
        MySelf._DbFilePath = value;
      }
    }


    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    [JJ.JsonProperty("PickupDir")]
    private string _PickupDir = null;
    /// <summary>
    /// Stores the path to the directory in which the scanned documents are places.
    /// </summary>
    public static string PickupDir {
      get {
        return MySelf._PickupDir;
      }
      set {
        MySelf._PickupDir = value;
      }
    }
    #endregion

    #region Functions
    private static string DefaultDbFilePath() {
      string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
      path = IO.Path.Combine(path, APP_DIRNAME);
      if (!IO.Directory.Exists(path)) {
        IO.Directory.CreateDirectory(path);
      }
      return IO.Path.Combine(path, DB_FILENAME);
    }

    internal static bool IsSetupDone(ref List<string> errors) {
      errors = new List<string>();

      // Check if the database file exists
      if (!IO.File.Exists(DbFilePath)) {
        errors.Add(Lang.Text["Setup.Error.NoDbFile"]);
      }
      else {
        // Check if the database file is valid
        try {
          SqLite.Database.CheckDataBase.Exec();
        }
        catch (System.Data.SQLite.SQLiteException SqEx) {
          errors.Add(Lang.Text["Setup.Error.InvalidDb"]);
        }
      }

      // Check if the pickup directory is set
      if (PickupDir == null) {
        errors.Add(Lang.Text["Setup.Error.UnsetPickup"]);
      }
      else {
        // Check if the pickup directory exists
        if (!IO.Directory.Exists(PickupDir)) {
          errors.Add(Lang.Text["Setup.Error.PickupNotFound"]);
        }
      }

      return errors.Count == 0;
    }
    #endregion
  }
}
