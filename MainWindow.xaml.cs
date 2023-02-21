using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using ModernWpf.Controls;

namespace cleargog
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    readonly string smPath = @"C:\ProgramData\Microsoft\Windows\Start Menu\Programs";
    bool changed = false;
    bool selectedAll = false;
    private readonly ObservableCollection<Item> itemsToRemove = new();

    public MainWindow() {
      InitializeComponent();
      GetGameList();

      f1bYes.Click += (sender, e) => {
        DeleteItems();
        Flyout1.Hide();
      };
      f1bNo.Click += (sender, e) => {
        Flyout1.Hide();
      };
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
      changed = true;

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

    private void BCommit_Click(object sender, RoutedEventArgs e) {
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

        tbItemsToRemove.Text = $"Are you sure you want to remove the following\nfrom the Start Menu?\n\n{t}";
        f1bNo.Content = "No";
        f1bYes.Visibility = Visibility.Visible;
      } else {
        tbItemsToRemove.Text = "There's nothing selected to remove";
        f1bYes.Visibility = Visibility.Hidden;
        f1bNo.Content = "Okay";
      }
    }

    private void DeleteItems() {
      foreach (var item in itemsToRemove) {
        Directory.Delete(item.Path, true);
      }

      lvGameList.ItemsSource= null;
      GetGameList();
      lvGameList.Items.Refresh();
      bSelectAll.Content = "Select All";
      changed = false;
      selectedAll = false;
    }

    private void BSelectAll_Click(object sender, RoutedEventArgs e) {
      if (!selectedAll) {
        foreach (Item item in lvGameList.ItemsSource) {
          if (!item.Remove) {
            item.Remove = true;
          }
        }

        bSelectAll.Content = "Unselect All";
        changed = true;
        selectedAll = true;
      } else {
        foreach (Item item in lvGameList.ItemsSource) {
          if (item.Remove) {
            item.Remove = false;
          }
        }

        bSelectAll.Content = "Select All";
        changed = false;
        selectedAll = false;
      }

      lvGameList.Items.Refresh();
    }

    private void BClose_Click(object sender, RoutedEventArgs e) {
      if (changed) {
        f2bYes.Click += (sender, e) => {
          App.Current.Shutdown();
        };
        f2bNo.Click += (sender, e) => {
          Flyout2.Hide();
        };
      } else {
        App.Current.Shutdown();
      }
    }
  }
}
