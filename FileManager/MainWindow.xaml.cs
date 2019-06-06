using System;
using System.Windows;

namespace FileManager
{
    public partial class MainWindow : Window
    {
        private Model model;

        public MainWindow()
        {
            InitializeComponent();
            try
            {
                model = new Model();

                this.DataContext = model.GetContext();
            }
            catch (Exception) { }
        }

        private void BtnLoad_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                model.Load(fileList.SelectedItem as DBFile);
            }
            catch (Exception) { }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                model.Save();
            }
            catch (Exception) { }
        }
    }
}
