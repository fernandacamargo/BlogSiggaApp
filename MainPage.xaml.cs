using BlogSiggaApp.Domain.Entities;
using Microsoft.Maui.Controls;

namespace BlogSiggaApp
{
    public partial class MainPage : ContentPage
    {
        private readonly PostsViewModel _viewModel;


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


            ((CollectionView)sender).SelectedItem = null;
        }
    }
}
