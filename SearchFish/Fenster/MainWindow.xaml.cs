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

    private async void TestGoogleBtn_Click(object sender, RoutedEventArgs e) {
      OpenFileDialog openFileDialog = new OpenFileDialog();
      openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";

      if (openFileDialog.ShowDialog() == true) {
        string imagePath = openFileDialog.FileName;









        //var Ocr = new IronTesseract();
        //using (var Input = new OcrInput(imagePath)) {
        //  Input.EnhanceResolution();
        //  Input.DeNoise();
        //  var result = Ocr.Read(Input);
        //  GoogleTxb.Text = result.Text;
        //}












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
                GoogleTxb.Text = text;
              }
            }
          }
        }
        catch (Exception ex) {
          GoogleTxb.Text = ex.ToString();
        }











        //// Load image into MemoryStream
        //using (MemoryStream imageStream = new MemoryStream(File.ReadAllBytes(imagePath))) {
        //  // Initialize the ImageAnnotatorClient
        //  ImageAnnotatorClient client = await ImageAnnotatorClient.CreateAsync();

        //  Google.Cloud.Vision.V1.Image image = Google.Cloud.Vision.V1.Image.FromStream(imageStream);

        //  // Perform OCR on the image
        //  IReadOnlyList<EntityAnnotation> response = await client.DetectTextAsync(image);

        //  // Extract text from response
        //  string extractedText = response?.FirstOrDefault()?.Description;

        //  // Display extracted text in TextBox
        //  GoogleTxb.Text = extractedText;

        //  // Perform label detection on the image
        //  IReadOnlyList<EntityAnnotation> labelAnnotations = await client.DetectLabelsAsync(image);

        //  // Extract labels from response
        //  List<string> labels = labelAnnotations?.Select(annotation => annotation.Description).ToList();

        //  // Display labels in a separate TextBox
        //  GoogleTxb.Text += "\n\n\n\n/* ------------------------------------------ */" + string.Join(", ", labels);
        //}
      }
    }
  }
}
