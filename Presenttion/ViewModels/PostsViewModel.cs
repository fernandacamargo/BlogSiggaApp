using BlogSiggaApp.Application.Interfaces;
using BlogSiggaApp.Domain.Entities;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

public class PostsViewModel : INotifyPropertyChanged
{
    private readonly IPostService _postService;
    private readonly IApiService _apiService;
    private bool _offlineAlreadyLoaded = false;

    public bool IsBusy { get; set; }
    public bool IsRefreshing { get; set; }
    public string ConnectionStatusText { get; set; }
    public Color ConnectionStatusColor { get; set; }
    public ICommand RefreshCommand { get; }

    private ObservableCollection<Post> _posts = new();
    public ObservableCollection<Post> Posts
    {
        get => _posts;
        set
        {
            if (_posts != value)
            {
                _posts = value;
                OnPropertyChanged(nameof(Posts));
            }
        }
    }

    public List<Post> _listsPosts { get; set; }

    public PostsViewModel(IPostService postService, IApiService apiService)
    {
        _postService = postService;
        _apiService = apiService;
        _listsPosts = new List<Post>();
        RefreshCommand = new Command(async () => await LoadPosts(true));
    }

    public async Task LoadPosts(bool isRefresh = false)
    {
        try
        {
            // Definindo os estados de carregamento (Refresh ou Busy)
            if (isRefresh)
            {
                IsRefreshing = true;
            }
            else
            {
                IsBusy = true;
            }

            OnPropertyChanged(nameof(IsBusy));
            OnPropertyChanged(nameof(IsRefreshing));

            // Verificando o status da conexão
            var isOnline = Connectivity.Current.NetworkAccess == NetworkAccess.Internet;
            ConnectionStatusText = isOnline ? "🟢 Online" : "🔴 Offline";
            ConnectionStatusColor = isOnline ? Colors.Green : Colors.Red;
            OnPropertyChanged(nameof(ConnectionStatusText));
            OnPropertyChanged(nameof(ConnectionStatusColor));

            // Primeira tentativa de carregar da API
            if (isOnline)
            {
                _offlineAlreadyLoaded = false;

                // Buscando posts da API
                var postsFromApi = await _apiService.FetchPostsAsync();
                var localPosts = _postService.GetLocalPostsAsync();

                // Filtrando posts que ainda não existem na base local
                var novosPosts = postsFromApi
                    .Where(apiPost => !localPosts.Any(local => local.Id == apiPost.Id))
                    .ToList();

                if (novosPosts.Any())
                {
                    // Inserindo os novos posts na base local
                     _postService.InsertPostsAsync(novosPosts);

                    // Atualizando a lista de posts na UI
                    Posts.Clear();
                    foreach (var post in _postService.GetLocalPostsAsync())
                    {
                        Posts.Add(post);
                    }
                }
                else
                {
                    // Nenhum novo post, informando ao usuário
                    await Application.Current.MainPage.DisplayAlert("Blog", "Não há novos posts no blog.", "OK");

                    // Carregando os posts locais
                    Posts.Clear();
                    foreach (var post in localPosts)
                    {
                        Posts.Add(post);
                    }
                }
            }
            // Lógica quando estiver offline (mas já carregou posts anteriormente)
            else if (!_offlineAlreadyLoaded)
            {
                var posts = _postService.GetLocalPostsAsync();

                // Carregando os posts locais
                Posts.Clear();
                foreach (var post in posts)
                {
                    Posts.Add(post);
                }

                _offlineAlreadyLoaded = true;
            }
        }
        catch (Exception ex)
        {
            // Tratar exceções conforme necessário, por exemplo, logar erros
            Console.WriteLine($"Erro ao carregar posts: {ex.Message}");
        }
        finally
        {
            // Finalizando os estados de carregamento
            IsBusy = false;
            IsRefreshing = false;
            OnPropertyChanged(nameof(IsBusy));
            OnPropertyChanged(nameof(IsRefreshing));
        }
    }


    public void OnPostSelected(Post selectedPost)
    {
        Console.WriteLine($"Post selecionado: {selectedPost.Title}");
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
