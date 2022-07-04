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

namespace TestKvalik.Pages
{
    /// <summary>
    /// Interaction logic for AddPage.xaml
    /// </summary>
    public partial class AddPage : Page
    {
        private Users _currentUser = new Users();

        public AddPage()
        {
            InitializeComponent();
            DataContext = _currentUser;
            //SexBox.ItemsSource = TestKvalikEntities.GetContext().Sexes.Select(s => s.SexName).ToList();
            List<string> sexList = new List<string>() { "-", "Женский", "Мужской" };
            SexBox.ItemsSource = sexList;
        }

        private void FIO_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Char.IsLetter(e.Text.Last()) && e.Text.Last() != '-')
            {
                e.Handled = true;
            }
        }

        private void BtnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.DefaultExt = ".jpg";
            openFileDialog.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif";

            var result = openFileDialog.ShowDialog();

            if (result == true)
            {
                PhotoPath.Text = openFileDialog.FileName;
                _currentUser.Photo = openFileDialog.FileName;

                BitmapImage bitImg = new BitmapImage();
                bitImg.BeginInit();
                bitImg.UriSource = new Uri(openFileDialog.FileName);
                bitImg.EndInit();

                ProfileImg.Source = bitImg;
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrEmpty(_currentUser.SecondName))
                errors.AppendLine("Укажите фамилию");
            if (string.IsNullOrEmpty(_currentUser.Name))
                errors.AppendLine("Укажите имя");
            if (string.IsNullOrEmpty(_currentUser.Patronymic))
                errors.AppendLine("Укажите отчество");
            if (string.IsNullOrEmpty(_currentUser.Email))
                errors.AppendLine("Укажите EMail");
            if (!IsValidEmail(_currentUser.Email))
                errors.Append("Введите верный EMail");
            if (string.IsNullOrEmpty(_currentUser.Phone))
                errors.AppendLine("Укажите телефон");
            if (_currentUser.BirthDate == DateTime.MinValue)
                errors.AppendLine("Укажите дату рождения");
            if (_currentUser.SexID == 0)
                errors.AppendLine("Укажите пол");
            if (string.IsNullOrEmpty(_currentUser.Photo))
                errors.AppendLine("Выберите фото");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            _currentUser.RegDate = DateTime.Today;

            TestKvalikEntities.GetContext().Users.Add(_currentUser);

            try
            {
                TestKvalikEntities.GetContext().SaveChanges();
                MessageBox.Show("Пользователь добавлен!");
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool IsValidEmail(string eMail)
        {
            bool result = false;

            try
            {
                var eMailValidator = new System.Net.Mail.MailAddress(eMail);

                result = (eMail.LastIndexOf(".") > eMail.LastIndexOf("@"));
            }
            catch
            {
                result = false;
            };

            return result;
        }
    }
}
