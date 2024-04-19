using System.Windows;
using App.ProjectSettings;
namespace App.ProjectSettings.Views
{
    public partial class ProjectSettingsWindow : Window
    {
        public ProjectSettingsWindow()
        {
            InitializeComponent();
        }

        // Обработчик события для кнопки "Назад"
        private void BackToMainWindow_Click(object sender, RoutedEventArgs e)
        {
            // Создаем новый экземпляр главного окна
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show(); // Показываем главное окно
            Close(); // Закрываем текущее окно
        }

        // Другие методы и события окна
    }
}
