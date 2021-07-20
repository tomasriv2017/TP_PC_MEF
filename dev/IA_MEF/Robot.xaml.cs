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
using System.Windows.Threading;

namespace IA_MEF
{
    /// <summary>
    /// Lógica de interacción para Robot.xaml
    /// </summary>


    public enum EstadosEnum //los ESTADOS se realizaran con un enum
    {
        Busqueda,
        NuevaBusqueda,
        IrBateria,
        Recargar,
        Muerto,
        Aleatorio
    }

    public partial class Robot : UserControl
    {
        public event EventHandler<string> ActualizarDatos; //con esta variable se podra ver la informacion importante por la Ventana Principal

        Random random;
        readonly DispatcherTimer timer;
        int basuraActual = 0;
        List<Basura> basuras;
        EstacionRecarga estacionRecarga;


        public int Bateria { get; private set; } //getter y setter para Bateria
        public int X { get; private set; } //getter y setter para X
        public int Y { get; private set; }//getter y setter para Y

        public EstadosEnum Estado { get; set; } //getter y setter para los Estados

        public Robot(int x, int y)
        {
            InitializeComponent();
            random = new Random();
            RecargarBateria();
            Height = 40;
            Width = 30;

            TranslateTransform translate = new TranslateTransform(x, y);
            RenderTransform = translate;
            indicador.Fill = new SolidColorBrush(Bateria < 350 ? Colors.Red : Colors.Green);
            Estado = EstadosEnum.Busqueda; //estado inicial Busqueda
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 10); //el robot tendra un retraso de 10 milisegundos, mientras mayor sea mas lento se movera
            timer.Tick += Timer_Tick; //para que la informacion sea dinamica y pueda ir cambiando, ya sea en este caso la bateria y el estado actual.
        }

        private void RecargarBateria()
        {
            Bateria = 1000;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            ActualizarDatos(null, "Estado: " + Estado.ToString() + ", Bateria: " + Bateria);
        }

        public void IniciarRecoleccion(List<Basura> basuras, EstacionRecarga estacionRecarga)
        {
            this.basuras = basuras;
            this.estacionRecarga = estacionRecarga;

            timer.Start(); //inicio el proceso del busqueda del robot
        }
    }
}
