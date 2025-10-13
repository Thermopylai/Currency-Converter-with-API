﻿using System;
using System.Collections.Generic;
using System.IO;
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

namespace Currency_Converter_with_API
{
    /// <summary>
    /// Interaction logic for Setup.xaml
    /// </summary>
    public partial class Setup : Window
    {
        public Setup()
        {
            InitializeComponent();
            SetupHotkeys();
            txtSetup.Focus();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string key = txtSetup.Text.Trim();
            if (!string.IsNullOrEmpty(key))
            {
                File.WriteAllText("setup.ini", key);
                MainWindow.APIKey = key;
                MessageBox.Show("API key saved in 'setup.ini'", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Please enter a valid API key.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void SetupHotkeys()
        {
            try
            {
                RoutedCommand save = new RoutedCommand();
                save.InputGestures.Add(new KeyGesture(Key.Enter));
                CommandBindings.Add(new CommandBinding(save, Button_Click));

                RoutedCommand close = new RoutedCommand();
                close.InputGestures.Add(new KeyGesture(Key.X, ModifierKeys.Alt));
                CommandBindings.Add(new CommandBinding(close, windowClose));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error setting up hotkeys: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void windowClose(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
