using System.ComponentModel;


namespace BlogSiggaApp.Presenttion.ViewModels
{
    public class BaseViewModel
    {
        public bool IsBusy { get; set; }
        public bool IsRefreshing { get; set; }
        public string ConnectionStatusText { get; set; }
        public Color ConnectionStatusColor { get; set; }


        protected bool CheckConnectivy()
        {
            OnPropertyChanged(nameof(IsBusy));
            OnPropertyChanged(nameof(IsRefreshing));

            // Verificando o status da conexão
            var isOnline = Connectivity.Current.NetworkAccess == NetworkAccess.Internet;
            ConnectionStatusText = isOnline ? "🟢 Online" : "🔴 Offline";
            ConnectionStatusColor = isOnline ? Colors.Green : Colors.Red;
            OnPropertyChanged(nameof(ConnectionStatusText));
            OnPropertyChanged(nameof(ConnectionStatusColor));
            return isOnline;
        }

      

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
