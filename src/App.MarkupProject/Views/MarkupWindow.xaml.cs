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
using System.Windows.Shapes;


namespace App.MarkupProject.Views
{
    /// <summary>
    /// Логика взаимодействия для MarkupWindow.xaml
    /// </summary>
    public partial class MarkupWindow : UserControl
    {
        //private List<string> imagePaths = new List<string>(); // Список путей к изображениям
        //private int currentIndex = -1; // Индекс текущего выбранного изображения

        public MarkupWindow()
        {
            InitializeComponent();
        }
        //private void imageComboBox_DropDownOpened(object sender, EventArgs e)
        //{
        //    // Обработка события открытия выпадающего списка ComboBox
        //    OpenFileDialog openFileDialog = new OpenFileDialog();
        //    openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*";

        //    if (openFileDialog.ShowDialog() == true)
        //    {
        //        string selectedFilePath = openFileDialog.FileName;
        //        // Здесь можно обработать выбранный файл
        //    }
        //}
        //private void LoadImage(string imagePath)
        //{
        //    try
        //    {
        //        BitmapImage bitmap = new BitmapImage(new Uri(imagePath)); // Создаем новый объект BitmapImage из выбранного изображения
        //        ImageControl.Source = bitmap; // Устанавливаем источник изображения для элемента ImageControl
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Ошибка загрузки изображения: {ex.Message}");
        //    }
        //}

        //private void PreviousImage_Click(object sender, RoutedEventArgs e)
        //{
        //    if (currentIndex > 0)
        //    {
        //        currentIndex--; // Уменьшаем индекс, чтобы перейти к предыдущему изображению
        //        LoadImage(imagePaths[currentIndex]); // Загружаем выбранное изображение
        //    }
        //}

        //private void NextImage_Click(object sender, RoutedEventArgs e)
        //{
        //    if (currentIndex < imagePaths.Count - 1)
        //    {
        //        currentIndex++; // Увеличиваем индекс, чтобы перейти к следующему изображению
        //        LoadImage(imagePaths[currentIndex]); // Загружаем выбранное изображение
        //    }
        //}

    }
}
