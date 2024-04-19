using App.AboutSection.Views;
using App.MarkupProject.Views;
using App.ProjectSettings.Views;
using System.Windows;

namespace App.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // Обработчик нажатия кнопки "Открыть проект"
        private void OpenProject_Click(object sender, RoutedEventArgs e)
        {
            // Создаем новый экземпляр окна MarkupWindow и показываем его
            MarkupWindow markupWindow = new MarkupWindow();
            markupWindow.Show();
            // Закрываем текущее окно
            Close();
        }

        // Обработчик нажатия кнопки "О программе"
        private void About_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow aboutWindow = new AboutWindow();
            aboutWindow.Show();
            // Закрываем текущее окно
            Close();
        }

        // Обработчик нажатия кнопки "Редактор классов"
        private void ClassEditor_Click(object sender, RoutedEventArgs e)
        {
            // Создаем новый экземпляр окна ProjectSettingsWindow и показываем его
            ProjectSettingsWindow projectSettingsWindow = new ProjectSettingsWindow();
            projectSettingsWindow.Show();
            // Закрываем текущее окно
            Close();
        }
    }
}
