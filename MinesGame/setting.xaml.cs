using MinesGame.Validation;
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

namespace MinesGame
{
    /// <summary>
    /// setting.xaml 的交互逻辑
    /// </summary>
    public partial class setting : Window
    {
        public setting()
        {
            InitializeComponent();

            Binding binding = new Binding("Value") { Source = this.SW };
            binding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            ValidationWidth validationWeight = new ValidationWidth();
            binding.ValidationRules.Add(validationWeight);
            this.Width_T.SetBinding(TextBox.TextProperty, binding);

            Binding binding2 = new Binding("Value") { Source = this.SH };
            binding2.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            ValidationHeight validationHeight = new ValidationHeight();
            binding2.ValidationRules.Add(validationHeight);
            this.Height_T.SetBinding(TextBox.TextProperty, binding2);

        }

        private void SliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (SH == null || SW == null || SM==null)
                return;
            int w_num = (int)this.SW.Value;
            int h_num = (int)this.SH.Value;
            this.SM.Maximum = w_num * h_num*3/5;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.main.MaxRow = (int)this.SH.Value;
            MainWindow.main.MaxCol = (int)this.SW.Value;
            MainWindow.main.MineNum = (int)this.SM.Value;
            //DialogResult = false;
            this.Close();
        }
    }
}
