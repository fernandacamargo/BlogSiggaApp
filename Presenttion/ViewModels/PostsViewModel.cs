using BlogSiggaApp.Application.Interfaces;
using BlogSiggaApp.Domain.Entities;
using BlogSiggaApp.Presenttion.ViewModels;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

public class PostsViewModel : BaseViewModel, INotifyPropertyChanged
{
    private readonly IPostService _postService;
    private readonly IApiService _apiService;
    private bool _offlineAlreadyLoaded = false;
    public ICommand RefreshCommand { get; }
    public List<Post> _listsPosts { get; set; }

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
            SetLoadingState(isRefresh);

            bool isOnline = CheckConnectivy();

            // Tentar carregar posts da API ou carregar localmente quando offline
            if (isOnline)
            {
                await LoadPostsFromApi();
            }
            else if (!_offlineAlreadyLoaded)
            {
                await LoadPostsFromLocalStorage();
                _offlineAlreadyLoaded = true;
            }
        }
        catch (Exception ex)
        {
            await HandleError(ex);
        }
        finally
        {
            // Finalizando os estados de carregamento
            ResetLoadingState();
        }
    }

    private void SetLoadingState(bool isRefresh)
    {
        if (isRefresh)
        {
            IsRefreshing = true;
        }
        else
        {
            IsBusy = true;
        }
    }

    private async Task LoadPostsFromApi()
    {
        _offlineAlreadyLoaded = false;

        // Buscando posts da API
        var postsFromApi = await _apiService.FetchPostsAsync();
        var localPosts =  _postService.GetLocalPostsAsync();

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
            await Application.Current.MainPage.DisplayAlert("Blog", "Não há novos posts no blog.", "OK");

            // Carregando os posts locais
            Posts.Clear();
            foreach (var post in localPosts)
            {
                Posts.Add(post);
            }
        }
    }

    private async Task LoadPostsFromLocalStorage()
    {
        var localPosts = _postService.GetLocalPostsAsync();

        // Carregando os posts locais
        Posts.Clear();
        foreach (var post in localPosts)
        {
            Posts.Add(post);
        }
    }

    private async Task HandleError(Exception ex)
    {
        await Application.Current.MainPage.DisplayAlert("Erro", $"Falha ao carregar posts: {ex.Message}", "OK");
    }

    private void ResetLoadingState()
    {
        IsBusy = false;
        IsRefreshing = false;
        OnPropertyChanged(nameof(IsBusy));
        OnPropertyChanged(nameof(IsRefreshing));
    }




    public void OnPostSelected(Post selectedPost)
    {   
        //Implement
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected override void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
