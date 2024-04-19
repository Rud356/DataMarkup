using System.Windows;

namespace App.AboutSection.Views
{
    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();
        }

        // Обработчик события нажатия кнопки "Назад"
        private void BackToMainWindow_Click(object sender, RoutedEventArgs e)
        {
            // Создаем новый экземпляр главного окна и показываем его
            App.Views.MainWindow mainWindow = new App.Views.MainWindow();
            mainWindow.Show();

            // Закрываем текущее окно
            Close();
        }
    }
}
