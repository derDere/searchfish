using Microsoft.Win32;
using SearchFish.SqLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Tesseract;

namespace SearchFish {
  /// <summary>
  /// Interaktionslogik für MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window {
    public MainWindow() {
      InitializeComponent();

      List<string> errors = null;
      if (!Config.IsSetupDone(ref errors)) {
        RightContentSV.Content = new Controls.SetupDialog();
      }
    }

    class JustAOne : DbObject {
      public long One { get; private set; }
      public JustAOne() {}
      internal override void Load(System.Data.SQLite.SQLiteDataReader reader) {
        One = (long)reader[0];
      }
    }

    private void TestBtn_Click(object sender, RoutedEventArgs e) {


      // DB Test

      SqLite.Database.FileName = @"C:\tmp\test.db";
      if (SqLite.Database.FileExists) {
        File.Delete(SqLite.Database.FileName);
      }
      SqLite.Database.Create();
      SqLite.Database.Open();
      int? r = SqLite.Database.Select1.ExecScalar<int>();
      bool rhs = r.HasValue;
      SqLite.Database.Commit();
      string val = SqLite.Database.SelectVal.Param("@val", "New Value").ExecStr();
      SqLite.Database.Commit();
      JustAOne one = SqLite.Database.Select1.ExecObj<JustAOne>();
      SqLite.Database.Commit();
      JustAOne[] someOnes = SqLite.Database.Select1.ExecList<JustAOne>().ToArray();
      SqLite.Database.Commit();
      SqLite.Database.Nothing.Exec();
      SqLite.Database.Close();

      int i = 0;




      /*
      OpenFileDialog openFileDialog = new OpenFileDialog();
      openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";

      if (openFileDialog.ShowDialog() == true) {
        string imagePath = openFileDialog.FileName;
        try {
          // Create an instance of the TesseractEngine using the default language ("eng" for English)
          using (var engine = new TesseractEngine(@"C:\Users\phill\sources\tessdata_fast", "eng", EngineMode.Default)) {
            // Load the image
            using (var img = Pix.LoadFromFile(imagePath)) {
              // Perform OCR
              using (var page = engine.Process(img)) {
                // Get the recognized text
                string text = page.GetText();

                // Output the recognized text
                TestTxb.Text = text;
              }
            }
          }
        }
        catch (Exception ex) {
          TestTxb.Text = ex.ToString();
        }
      }
      */
    }
  }
}
