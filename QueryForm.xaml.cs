using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ArcMapClassLibrary2
{
    /// <summary>
    /// Interaction logic for QueryForm.xaml
    /// </summary>
    public partial class QueryForm 
    {
        public QueryForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            
            this.Title = "test wpf window";
        }
    }
}
