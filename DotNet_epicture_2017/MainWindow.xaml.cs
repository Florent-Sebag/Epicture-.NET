using FlickrNet;
using Microsoft.Win32;
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

namespace DotNet_epicture_2017
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Flickr flickr;
        OAuthRequestToken requestToken;
        List<Item> ItemList;
        int page;
        Boolean isConnected;
        PhotoCollection fav;

        class Item
        {
            public BitmapImage img { get; set; }
            public String id { get; set; }

            public Boolean isFavorites { get; set; }
        }

        public class PhotoDesc
        {
            public String title;

            public String file;
        }
        public MainWindow()
        {
            InitializeComponent();
            flickr = new Flickr("88d44f3a042188839bfb65a05cdd8f63");
            flickr.ApiSecret = "8734fda1ef5162da";
            ItemList = new List<Item>();
            ImgList.DataContext = this;
            ImgList.ItemsSource = ItemList;
            isConnected = false;
            page = 1;
        }

        private void refreshList()
        {
            ImgList.ClearValue(ItemsControl.ItemsSourceProperty);
            ImgList.ItemsSource = ItemList;
        }

        private Boolean isAFav(Photo pic)
        {
            foreach (Photo f in fav)
            {
                if (f.PhotoId == pic.PhotoId)
                    return (true);
            }
            return (false);
        }

        private void LoadImage(Photo pic)
        {
            var fullFilePath = pic.SmallUrl;
            Image image = new Image();
            Boolean isFav = false;

            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(fullFilePath, UriKind.Absolute);
            bitmap.EndInit();

            image.Source = bitmap;
            if (isConnected)
            {
                fav = flickr.FavoritesGetList();
                if (isAFav(pic))
                    isFav = true;
            }
            ItemList.Add(new Item { img = bitmap, id =  pic.PhotoId, isFavorites = isFav } );
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (SearchInput.Text == "")
                return;
            var options = new PhotoSearchOptions { Tags = SearchInput.Text, PerPage = 6, Page = 1, Extras = PhotoSearchExtras.SmallUrl };
            PhotoCollection res = flickr.PhotosSearch(options);
            foreach (Photo pic in res)
                LoadImage(pic);
            page = 1;
            refreshList();
        }

        private void PrevBtn_Click(object sender, RoutedEventArgs e)
        {
            if (page == 1)
                return;
            page -= 1;
            if (SearchInput.Text == "")
                return;
            var options = new PhotoSearchOptions { Tags = SearchInput.Text, PerPage = 6, Page = page, Extras = PhotoSearchExtras.SmallUrl };
            PhotoCollection res = flickr.PhotosSearch(options);
            ItemList.Clear();
            foreach (Photo pic in res)
                LoadImage(pic);
            refreshList();
        }

        private void NextBtn_Click(object sender, RoutedEventArgs e)
        {
            if (SearchInput.Text == "")
                return;
            page += 1;
            var options = new PhotoSearchOptions { Tags = SearchInput.Text, PerPage = 6, Page = page, Extras = PhotoSearchExtras.SmallUrl  };
            PhotoCollection res = flickr.PhotosSearch(options);
            ItemList.Clear();
            foreach (Photo pic in res)
                LoadImage(pic);
            refreshList();
        }

        private void UploadPic_Click(object sender, RoutedEventArgs e)
        {
            if (!isConnected)
            {
                MessageBox.Show("Please connect before upload.");
                return;
            }
            UploadDialog op = new UploadDialog();
            if (op.ShowDialog() == true)
            {
                if (op.Answer.title != "")
                    flickr.UploadPicture(op.Answer.file, op.Answer.title);
                else
                    flickr.UploadPicture(op.Answer.file);
                MessageBox.Show("Successfully added to your Gallery");
            }
        }

        private void ImgList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!isConnected)
            {
                MessageBox.Show("You need to be connected for adding to your favorites.");
                return;
            }
            if (ImgList.SelectedIndex >= 0)
            {
                if (ItemList[ImgList.SelectedIndex].isFavorites)
                {
                    flickr.FavoritesRemove(ItemList[ImgList.SelectedIndex].id);
                    MessageBox.Show("Successfully removed from favorites");
                    ItemList[ImgList.SelectedIndex].isFavorites = false;
                }
                else
                {
                    flickr.FavoritesAdd(ItemList[ImgList.SelectedIndex].id);
                    MessageBox.Show("Successfully added to favorites");
                    ItemList[ImgList.SelectedIndex].isFavorites = true;
                }
            }
        }

        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            ConnectDialog dial = new ConnectDialog();

            requestToken = flickr.OAuthGetRequestToken("oob");
            string url = flickr.OAuthCalculateAuthorizationUrl(requestToken.Token, AuthLevel.Write);
            System.Diagnostics.Process.Start(url);
            OAuthAccessToken token = null;
            if (dial.ShowDialog() == true)
            {
                token = flickr.OAuthGetAccessToken(requestToken, dial.Answer);
                isConnected = true;
                flickr.AuthToken = token.ToString();
                Console.WriteLine("Connected");
                Connect.Visibility = Visibility.Hidden;
            }
        }

        private void S_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
