using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;

namespace cleargog
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    readonly string smPath = @"C:\ProgramData\Microsoft\Windows\Start Menu\Programs";

    public MainWindow() {
      InitializeComponent();
      GetGameList();
    }

    private void GetGameList() {
      var dirs = Directory.GetDirectories(smPath, "*[GOG.com]");
      var lItems = new ObservableCollection<Item>();

      foreach (var dir in dirs) {
        string title = dir.Split('\\').Last();
        lItems.Add(new Item { Title = title, Remove = false, Path = dir });
      }

      lvGameList.ItemsSource = lItems;
    }

    private void LvGameList_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e) {
      if (((FrameworkElement)e.OriginalSource).DataContext is Item item) {
        item.Remove = !item.Remove;
        lvGameList.Items.Refresh();
      }
    }

    public class Item
    {
      public string Title { get; set; }
      public bool Remove { get; set; }
      public string Path { get; set; }
    }

    private void MiExit_Click(object sender, RoutedEventArgs e) {
      this.Close();
    }

    private void BCommit_Click(object sender, RoutedEventArgs e) {
      var itemsToRemove = new ObservableCollection<Item>();
      foreach (Item item in lvGameList.Items) {
        if (item.Remove) {
          itemsToRemove.Add(item);
        }
      }

      if (itemsToRemove.Count > 0) {
        string t = "";
        foreach (var item in itemsToRemove) {
          t += item.Title + "\n";
        }
        var r = MessageBox.Show($"Are you sure you want to remove the following from the Start Menu?\n\n{t}", "Confirmation",
            MessageBoxButton.YesNo, MessageBoxImage.Warning);

        if (r == MessageBoxResult.Yes) {
          foreach (var item in itemsToRemove) {
            Directory.Delete(item.Path, true);
          }

          lvGameList.ItemsSource = null;
          GetGameList();
          lvGameList.Items.Refresh();
        }
      } else {
        MessageBox.Show("There's nothing selected to remove", "Nothing To Remove", MessageBoxButton.OK, MessageBoxImage.Information);
      }
    }

    private void MiAbout_Click(object sender, RoutedEventArgs e) {
      MessageBox.Show("Simple tool for removing unwanted Start Menu items for GOG Galaxy installed games.",
        "About ClearGOG", MessageBoxButton.OK, MessageBoxImage.Information);
    }
  }
}
