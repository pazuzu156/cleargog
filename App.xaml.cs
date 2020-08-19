using System;
using System.Windows;
using System.Threading;
using System.Reflection;

namespace cleargog
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    private static Mutex _mutex = null;
    private Version AppVersion => Assembly.GetExecutingAssembly().GetName().Version;

    public App() {
      string appName = $"ClearGOG-{AppVersion.ToString()}";
      _mutex = new Mutex(false, appName);

      if (!_mutex.WaitOne(TimeSpan.FromSeconds(0.5), false)) {
        MessageBox.Show("An instance of ClearGOG is already running! You may only have one instance up at a time",
          "ClearGOG", MessageBoxButton.OK, MessageBoxImage.Error);
        _mutex.ReleaseMutex();
        Current.Shutdown();
      }
    }
  }
}
