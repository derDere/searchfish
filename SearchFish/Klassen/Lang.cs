using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO = System.IO;
using JJ = Newtonsoft.Json;

namespace SearchFish.Klassen {
  internal class Lang {

    #region Statics
    public const string DEFAULT = "en";
    public const string FOLDER = "lang";
    public const string EXTENSION = ".json";

    private static Dictionary<string, Lang> langs = new Dictionary<string, Lang>();

    public static string GetCurrentSystemLang() {
      CultureInfo ci = CultureInfo.CurrentCulture;
      while (ci.Parent != null) {
        ci = ci.Parent;
      }
      return ci.TwoLetterISOLanguageName;
    }

    public static string Current { get; set; } = "en";

    public static Lang Text {
      get {
        return GetOrLoadLang(Current);
      }
    }

    [DebuggerHidden]
    private static string LangPath(string key) {
      string path = new IO.FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).Directory.FullName;
      path = IO.Path.Combine(path, FOLDER);
      if (!IO.Directory.Exists(path)) {
        IO.Directory.CreateDirectory(path);
      }
      return IO.Path.Combine(path, key + EXTENSION);
    }

    private static Lang GetOrLoadLang(string key) {
      if (langs.ContainsKey(key)) {
        return langs[key];
      }
      string path = LangPath(key);
      if (!IO.File.Exists(path)) {
        if (key == DEFAULT) {
          Lang lang = new Lang();
          lang.enFnfError = true;
          return lang;
        }
        else {
          return GetOrLoadLang(DEFAULT);
        }
      }
      else {
        Lang lang = new Lang();
        string jj = IO.File.ReadAllText(path, Encoding.UTF8);
        lang.texts = JJ.JsonConvert.DeserializeObject<Dictionary<string, string>>(jj);
        langs[key] = lang;
        return lang;
      }
    }
    #endregion

    private Dictionary<string, string> texts = new Dictionary<string, string>();

    private bool enFnfError = false;

    public string this[string key] {
      get {
        if (enFnfError) {
          return "eFnf:" + key;
        }
        if (texts.ContainsKey(key)) {
          return texts[key];
        }
        return "eKnf:" + key;
      }
    }

    private Lang() { }

  }
}
