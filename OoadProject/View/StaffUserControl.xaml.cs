﻿using System;
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

namespace OoadProject.View
{
    /// <summary>
    /// Interaction logic for NhanVienUserControl.xaml
    /// </summary>
    public partial class StaffUserControl : UserControl
    {
        public StaffUserControl()
        {
            InitializeComponent();
        }

        private void btnAddUser_Click(object sender, RoutedEventArgs e)
        {
            var window = new AddStaffWindow();
            window.ShowDialog();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Xác nhận xóa nhân viên này?", "Xác nhận", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
            var command = ((Button)sender).Command;

            if (result == MessageBoxResult.OK && command.CanExecute(null))
            {
                command.Execute(true);
            }
            else if (result != MessageBoxResult.OK && command.CanExecute(null))
            {
                command.Execute(false);
            }
        }

        private void btnGrantNewPassword_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Xác nhận cấp mật khẩu mới cho nhân viên?", "Xác nhận", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            var command = ((Button)sender).Command;

            if (result == MessageBoxResult.OK && command.CanExecute(null))
            {
                command.Execute(true);
            }
            else if (result != MessageBoxResult.OK && command.CanExecute(null))
            {
                command.Execute(false);
            }
        }
    }
}
