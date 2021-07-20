using System;
using System.Collections.Generic;
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

namespace IA_MEF
{
    /// <summary>
    /// Lógica de interacción para Basura.xaml
    /// </summary>
    public partial class Basura : UserControl
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        public Basura(int x, int y)
        {
            InitializeComponent();
            X = x;
            Y = y;
            TranslateTransform translate = new TranslateTransform(x, y);
            RenderTransform = translate;
            Height = 20;
            Width = 20;
        }

        public void Recolectado()
        {
            Visibility = Visibility.Hidden;
        }

    }
}
