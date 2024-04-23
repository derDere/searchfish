using SearchFish.Klassen;
using System;
using System.Collections.Generic;
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

namespace SearchFish.Controls {
  /// <summary>
  /// Interaktionslogik für SetupDialog.xaml
  /// </summary>
  public partial class SetupDialog : UserControl {
    public SetupDialog() {
      InitializeComponent();

      DbPathCC.Content = Lang.Text["UI.Setup.DbPath"];
      DefaultDbPathCb.Content = Lang.Text["UI.Setup.DefaultDbPathInfo"];
      PickupCC.Content = Lang.Text["UI.Setup.ScannerPickupPath"];
      ScannerPickupPathBtn.Content = Lang.Text["UI.Browse"];
      SaveBtn.Content = Lang.Text["UI.SaveAndClose"];
    }
  }
}
