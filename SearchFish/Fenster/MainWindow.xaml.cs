using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    }

    private async void TestBtn_Click(object sender, RoutedEventArgs e) {
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
    }
  }
}
