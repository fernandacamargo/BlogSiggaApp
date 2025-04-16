using BlogSiggaApp.Domain.Entities;

namespace BlogSiggaApp
{
    public partial class MainPage : ContentPage
    {
        private readonly PostsViewModel _viewModel;
        private Frame _previousSelectedFrame = null;

        public MainPage(PostsViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
        }


        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (!_viewModel.Posts.Any())
            {
                await _viewModel.LoadPosts();
            }
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedPost = e.CurrentSelection.FirstOrDefault() as Post;
            if (selectedPost != null)
            {
                _viewModel.OnPostSelected(selectedPost);
            }

            // Desmarcar o item selecionado
            ((CollectionView)sender).SelectedItem = null;
        }

        private void OnPostTapped(object sender, EventArgs e)
        {
            var tappedFrame = sender as Frame;
            var tappedPost = tappedFrame?.BindingContext as Post;

            if (tappedPost != null)
            {

                tappedFrame.BackgroundColor = Color.FromRgba("#FF5733");


                if (_previousSelectedFrame != null && _previousSelectedFrame != tappedFrame)
                {
                    _previousSelectedFrame.BackgroundColor = Color.FromRgba("#FFFFFF");
                }

                _previousSelectedFrame = tappedFrame;

            }
        }


        private async Task ShowAlert(string title, string message, string cancel)
        {
            await DisplayAlert(title, message, cancel);
        }
    }
}
