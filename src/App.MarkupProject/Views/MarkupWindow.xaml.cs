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
using System.Windows.Shapes;


namespace App.MarkupProject.Views
{
    /// <summary>
    /// Логика взаимодействия для MarkupWindow.xaml
    /// </summary>
    public partial class MarkupWindow : Window
    {
        public MarkupWindow()
        {
            InitializeComponent();
        }
        private void BackToMainWindow_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow(); // Создаем новый экземпляр главного окна
            mainWindow.Show(); // Показываем главное окно
            Close(); // Закрываем текущее окно
        }

        private void NextImage_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
