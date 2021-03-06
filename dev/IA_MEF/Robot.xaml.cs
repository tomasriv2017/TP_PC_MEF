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
        int cantBasuraRecolectada = 0;
        int TotalARecoger;
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
            timer.Interval = new TimeSpan(0, 0, 0, 0, 5); //el robot tendra un retraso de 10 milisegundos, mientras mayor sea mas lento se movera
            timer.Tick += Timer_Tick; //para que la informacion sea dinamica y pueda ir cambiando, ya sea en este caso la bateria y el estado actual.
        }

        private void Timer_Tick(object sender, EventArgs e) //meotodo principal para manejar los ESTADOS del robot
        {

            switch (this.Estado)
            {
                case EstadosEnum.Busqueda: //ESTADO DE BUSQUEDA DE BASURA
                    int newX = X;
                    int newY = Y;

                    int basuraObjetivo = DeterminarBasuraMasCercana();

                    System.Diagnostics.Debug.WriteLine("Robo("+X+";"+Y+")");
                    System.Diagnostics.Debug.WriteLine("BasuraObjetivo(" + basuras[basuraObjetivo].X + ";" + basuras[basuraObjetivo].Y + ")");

                    if (X > basuras[basuraObjetivo].X)
                    {
                        newX = X - 1;
                    }
                    if (X < basuras[basuraObjetivo].X)
                    {
                        newX = X + 1;
                    }
                    if (Y > basuras[basuraObjetivo].Y)
                    {
                        newY = Y - 1;
                    }
                    if (Y < basuras[basuraObjetivo].Y)
                    {
                        newY = Y + 1;
                    }
                    ActualizarPosicion(newX, newY);

                    if ((X == basuras[basuraObjetivo].X) && (Y == basuras[basuraObjetivo].Y)) //SI EL ROBOT LLEGA A LA BASURA
                    {
                        this.basuras[basuraObjetivo].Recolectado();
                        this.basuras.Remove(this.basuras[basuraObjetivo]);
                        cantBasuraRecolectada ++;

                        System.Diagnostics.Debug.WriteLine("Basura Recolectada!!!");
                        this.Estado = EstadosEnum.NuevaBusqueda;
                    }

                    if (Bateria < 350)
                    {
                        this.Estado = EstadosEnum.IrBateria; //SI TIENE MENOR DE 350 DE ENERGIA EL ROBOT DEJA DE BUSCAR LA BASURA Y VA A RECARGAR BATERIA
                    }
                    break;

                case EstadosEnum.NuevaBusqueda: //ESTADO NUEVA BUSQUEDA
                    if (cantBasuraRecolectada < TotalARecoger)
                    {
                        this.Estado = EstadosEnum.Busqueda;
                    }
                    else
                    {
                        this.Estado = EstadosEnum.Aleatorio;
                    }
                    break;

                case EstadosEnum.IrBateria: //ESTADO IR A BATERIA
                    newX = X;
                    newY = Y;

                    if (X > estacionRecarga.X)
                    {
                        newX = X - 1;
                    }
                    if (X < estacionRecarga.X)
                    {
                        newX = X + 1;
                    }
                    if (Y > estacionRecarga.Y)
                    {
                        newY = Y - 1;
                    }
                    if (Y < estacionRecarga.Y)
                    {
                        newY = Y + 1;
                    }
                    ActualizarPosicion(newX, newY);

                    if (X == estacionRecarga.X && Y == estacionRecarga.Y) //SI LLEGA A LA ESTACION DE RECARGA
                    {
                        this.Estado = EstadosEnum.Recargar;
                    }
                    if (Bateria == 0)
                    {
                        this.Estado = EstadosEnum.Muerto;
                    }
                    break;

                case EstadosEnum.Recargar: //ESTADO RECARGAR BATERIA
                    RecargarBateria();
                    //Thread.Sleep(500);
                    this.Estado = EstadosEnum.Busqueda;
                    break;

                case EstadosEnum.Muerto: //ESTADO MUERTO
                    timer.Stop();
                    MessageBox.Show("El robot ha muerto..."); //Mostrar con un POPUP el mensaje
                    break;

                case EstadosEnum.Aleatorio: //ESTADO ALEATORIO
                    ActualizarPosicion(random.NextDouble() > 0.5 ? X - 1 : X + 1, random.NextDouble() > 0.5 ? Y - 1 : Y + 1); //PARA QUE SE MUEVA DE FORMA EERRATICA UNA VEZ RECOGIDA TODA LA BASURA
                    if (Bateria == 0)
                    {
                        this.Estado = EstadosEnum.Muerto;
                    }
                    break;

            }

            ActualizarDatos(null, "Estado: " + Estado.ToString() + ", Bateria: " + Bateria + ", Basura: " + (TotalARecoger - cantBasuraRecolectada) ); //para ir actualizando constante la informacion de la Ventana principal
        }

        private void RecargarBateria()
        {
            Bateria = 1000;
        }

        private void ActualizarPosicion(int x, int y) //metodo para refrescar la posicion del robot
        {
            Bateria--;
            X = x;
            Y = y;

            TranslateTransform translate = new TranslateTransform(x, y);
            RenderTransform = translate; //dibuja/renderiza el camino del robot hacia su nueva posicion
            indicador.Fill = new SolidColorBrush(Bateria < 350 ? Colors.Red : Colors.Green); //si la bateria es MENOR a 350 cambia al color rojo
        }

        private int DeterminarBasuraMasCercana()
        {
            int diferencia = ( (basuras.First().X + basuras.First().Y) - (X + Y) );
            int comparador = 0;
            int indice = 0;
            for(int i = 1; i < basuras.Count; i++)
            {
                comparador = ( (basuras[i].X + basuras[i].Y) - (X + Y) );
                if ( comparador < diferencia)
                {
                    diferencia = comparador;
                    indice = i;
                }

            }

            return indice;
        }

        public void IniciarRecoleccion(List<Basura> basuras, EstacionRecarga estacionRecarga) //metodo con que el inicializa la busqueda el robot
        {
            this.basuras = basuras;
            this.estacionRecarga = estacionRecarga;
            TotalARecoger = this.basuras.Count;
            timer.Start();
        }


    }
}