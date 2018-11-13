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
using System.Threading;
using System.Diagnostics;

namespace PracticaMovimiento
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Stopwatch stopwatch;
        TimeSpan tiempoAnterior;
        enum EstadoJuego { Gameplay,Gameover};
        EstadoJuego estadoActual = EstadoJuego.Gameplay;
        enum Dirrecion { Arriba,Abajo,Izquierda,Derecha,Ninguna}
        Dirrecion DirreccionJugador = Dirrecion.Ninguna;
        double velocidadCarrito = 100;
        public MainWindow()
        {
            InitializeComponent();
            miCanvas.Focus();

            stopwatch = new Stopwatch();
            stopwatch.Start();
            tiempoAnterior = stopwatch.Elapsed;
            // 1. Establecer Instrucciones
            ThreadStart threadStart = new ThreadStart(Actualizar);
            // 2. Inicializar el thread
            Thread threadMoverEnemigos = new Thread(threadStart);
            // 3. Ejecutar el thread
            threadMoverEnemigos.Start();

        }
        public void moverJugador(TimeSpan deltatime)
        {
            double carritoarriba = Canvas.GetTop(carrito);
            double carritoderecha = Canvas.GetLeft(carrito);
            switch (DirreccionJugador)
            {
                case Dirrecion.Arriba:
                    Canvas.SetTop(carrito, carritoarriba - (velocidadCarrito * deltatime.TotalSeconds));
                break;
                case Dirrecion.Abajo:
                    Canvas.SetTop(carrito, carritoarriba + (velocidadCarrito * deltatime.TotalSeconds ));
                    break;
                case Dirrecion.Izquierda:
                    double nuevaPosicion = carritoderecha - (velocidadCarrito * deltatime.TotalSeconds);
                    if (nuevaPosicion + carrito.Width >= 0)
                    {
                        Canvas.SetLeft(carrito, nuevaPosicion);
                    }
                    break;
                case Dirrecion.Derecha:
                    double nuevaPosicion2 = carritoderecha + (velocidadCarrito * deltatime.TotalSeconds);
                    if (nuevaPosicion2 + carrito.Width <= 1062.167)
                    {
                        Canvas.SetLeft(carrito, nuevaPosicion2 );
                    }
                    break;
            }
                
        }
        public void Actualizar()
        {
            while (true)
            {
                Dispatcher.Invoke(
                () =>
                {
                    double carritoarriba = Canvas.GetTop(carrito);
                    double carritoderecha = Canvas.GetLeft(carrito);
                    var TiempoActual = stopwatch.Elapsed;
                    var DeltaTime = TiempoActual - tiempoAnterior;
                    moverJugador(DeltaTime);
                    if (estadoActual == EstadoJuego.Gameplay)
                    {
                        double leftCarroActual = Canvas.GetLeft(ranita);
                        Canvas.SetLeft(ranita, leftCarroActual - (140 * DeltaTime.TotalSeconds));
                        if (Canvas.GetLeft(ranita) <= -100)
                        {
                            Canvas.SetLeft(ranita, 1000);
                        }
                        double carritoo = Canvas.GetLeft(carrito);
                        double ranitaa = Canvas.GetLeft(ranita);
                        if (carritoo + carrito.Width >= ranitaa && carritoo <= ranitaa + ranita.Width)
                        {
                            lblX.Text = "SI HAY COLISIÓN EN X!!!";
                        }
                        else
                        {
                            lblX.Text = "No hay intersección en X";
                        }
                        //en y
                        double ycarrito = Canvas.GetTop(carrito);
                        double yranita = Canvas.GetTop(ranita);
                        if (yranita + ranita.Height >= ycarrito && yranita <= ycarrito + carrito.Height)
                        {
                            lblY.Text = "SI HAY INTERSECCION EN Y";
                        }
                        else
                        {
                            lblY.Text = "No hay intersección en Y";
                        }

                        if (ranitaa + ranita.Width >= carritoo && ranitaa <= carritoo + carrito.Width && yranita + ranita.Height >= ycarrito && yranita <= ycarrito + carrito.Height)
                        {
                            lblCol.Text = "Morido";
                            estadoActual = EstadoJuego.Gameover;
                        }
                        else
                        {
                            lblCol.Text = "No hay colisión";
                        }
                    }else if (estadoActual == EstadoJuego.Gameover)
                    {
                        miCanvas.Visibility = Visibility.Collapsed;
                        GameOver.Visibility = Visibility.Visible;
                    }
                   
                    tiempoAnterior = TiempoActual;
                }
                );
            }
        }
        private void miCanvas_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Right)
            {
                DirreccionJugador = Dirrecion.Derecha;
            }

            if (e.Key == Key.Left)
            {
                DirreccionJugador = Dirrecion.Izquierda;
            }

            if (e.Key == Key.Up)
            {
                DirreccionJugador = Dirrecion.Arriba;
            }
            if (e.Key == Key.Down)
            {
                DirreccionJugador = Dirrecion.Abajo;
            }
        }

        private void miCanvas_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up && DirreccionJugador == Dirrecion.Arriba)
            {
                DirreccionJugador = Dirrecion.Ninguna;
            }
            if (e.Key == Key.Down && DirreccionJugador == Dirrecion.Abajo)
            {
                DirreccionJugador = Dirrecion.Ninguna;
            }
            if (e.Key == Key.Left && DirreccionJugador == Dirrecion.Izquierda)
            {
                DirreccionJugador = Dirrecion.Ninguna;
            }
            if (e.Key == Key.Right && DirreccionJugador == Dirrecion.Derecha)
            {
                DirreccionJugador = Dirrecion.Ninguna;
            }
        }
    }
}
