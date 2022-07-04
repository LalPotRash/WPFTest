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

namespace TestKvalik.Pages
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        private int _currentPage = 1;
        private int _pageSize = 1000;
        private IEnumerable<Users> _userSource;

        public MainPage()
        {
            InitializeComponent();
            _userSource = TestKvalikEntities.GetContext().Users.ToList().Take(_pageSize);
            DGridUsers.ItemsSource = _userSource;
            List<string> rowOnPage = new List<string>() { "Все", "200", "50", "10" };
            RowOnPage.ItemsSource = rowOnPage;
            List<string> sexes = new List<string>() { "Все", "Мужской", "Женский" };
            SexFilter.ItemsSource = sexes;
        }

        public void ChangePageSize(int pageSize)
        {
            _pageSize = pageSize;
            DGridUsers.ItemsSource = _userSource.ToList().Take(_pageSize);
        }

        public void PreviousPage(object sender, RoutedEventArgs e)
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                DGridUsers.ItemsSource = _userSource.ToList().Skip((_currentPage - 1) * _pageSize).Take(_pageSize);
            }
            if (_currentPage == 1)
                DGridUsers.ItemsSource = _userSource.ToList().Take(_pageSize);
        }

        public void NextPage(object sender, RoutedEventArgs e)
        {
            if ((_currentPage + 1) * _pageSize <= _userSource.Count())
            {
                DGridUsers.ItemsSource = _userSource.ToList().Skip(_currentPage * _pageSize).Take(_pageSize);
                _currentPage++;
            }
        }

        private void RowOnPage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RowOnPage.SelectedIndex == 0)
                _pageSize = 1000;
            else
                _pageSize = Convert.ToInt32(RowOnPage.SelectedItem);

            DGridUsers.ItemsSource = _userSource.ToList().Take(_pageSize);
        }

        private void SexFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SexFilter.SelectedIndex == 0)
                _userSource = TestKvalikEntities.GetContext().Users.ToList().Take(_pageSize);
            else
                _userSource = TestKvalikEntities.GetContext().Users.Where(u => u.Sexes.SexName == SexFilter.SelectedItem.ToString()).ToList().Take(_pageSize);

            DGridUsers.ItemsSource = _userSource;
        }

        public void AddPage(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddPage());
        }

        private void DGridUsers_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedUser = (Users)DGridUsers.SelectedItem;
            NavigationService.Navigate(new EditPage(selectedUser));
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                TestKvalikEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                _userSource = TestKvalikEntities.GetContext().Users.ToList().Take(_pageSize);
                DGridUsers.ItemsSource = _userSource;
            }
        }
    }
}
