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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IA_MEF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Basura> basuras; //los objetos basura se almacenaran en una lista
        EstacionRecarga estacionRecarga;
        Robot robot;

        Random random;

        public MainWindow()
        {
            InitializeComponent();
            random = new Random();
            int x = random.Next(0, 700);
            int y = random.Next(0, 400);
            estacionRecarga = new EstacionRecarga(x, y); //le asigno una posicion aleatoria a la estacion de recarga
            Terreno.Children.Add(estacionRecarga); //gracias al Canvas dibujo el objeto estacion de recarga en la Ventana Principal

            x = random.Next(0, 700);
            y = random.Next(0, 700);
            robot = new Robot(x, y); //le asigno una posicion aleatoria al robot
            Terreno.Children.Add(robot); //gracias al Canvas dibujo el objeto robot en la Ventana Principal

            basuras = new List<Basura>();
            for (int i = 0; i < 10; i++) //en este caso se crearan 10 basuras
            {
                x = random.Next(0, 700);
                y = random.Next(0, 400);
                Basura basura = new Basura(x, y); //le asigno una posicion aleatoria a cada basura
                basuras.Add(basura);
                Terreno.Children.Add(basura); //gracias al Canvas dibujo cada objeto basura en la Ventana Principal
            }


        }

    }
}
