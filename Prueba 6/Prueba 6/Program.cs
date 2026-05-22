
using System;
using System.Collections.Generic;

namespace JuegoTablero
{
    class Usuario
    {
        public string Username;
        public string Password;

        public Usuario(string u, string p)
        {
            Username = u;
            Password = p;
        }
    }

    // CLASE JUGADOR

    class Jugador
    {
        public string Nombre;
        public int Puntaje;

        // Constructor
        public Jugador(string nombre)
        {
            Nombre = nombre;
            Puntaje = 0;
        }
    }

    // CLASE ABSTRACTA 
    abstract class Pieza
    {
        public string Tipo;
        public string Equipo;

        // Constructor
        public Pieza(string tipo, string equipo)
        {
            Tipo = tipo;
            Equipo = equipo;
        }

        // Método abstracto
        public abstract bool ValidarMovimiento(
            int filaOrigen,
            int columnaOrigen,
            int filaDestino,
            int columnaDestino,
            Pieza[,] tablero
        );
    }

    // CLASE REY
    class Rey : Pieza
    {
        // Constructor
        public Rey(string equipo) : base("R", equipo)
        {

        }

        // Override
        public override bool ValidarMovimiento(
            int fO,
            int cO,
            int fD,
            int cD,
            Pieza[,] tablero)
        {
            int diferenciaFila = fD - fO;
            int diferenciaColumna = cD - cO;

            if (diferenciaFila < 0 && diferenciaColumna < 0)
            {
                diferenciaFila = diferenciaFila * -1;
                diferenciaColumna = diferenciaColumna * -1;
                return true;
            }

            return false;
        }
    }


    // CLASE TORRE

    class Torre : Pieza
    {
        // Constructor
        public Torre(string equipo) : base("T", equipo)
        {

        }

        // Override
        public override bool ValidarMovimiento(
            int fO,
            int cO,
            int fD,
            int cD,
            Pieza[,] tablero)
        {
            // Solo movimiento recto
            if (fO != fD && cO != cD)
            {
                return false;
            }

            // Movimiento horizontal
            if (fO == fD)
            {
                int inicio;
                int fin;

                if (cO < cD)
                {
                    inicio = cO + 1;
                    fin = cD;
                }
                else
                {
                    inicio = cD + 1;
                    fin = cO;
                }

                for (int i = inicio; i < fin; i++)
                {
                    if (tablero[fO, i] != null)
                    {
                        return false;
                    }
                }
            }

            // Movimiento vertical
            if (cO == cD)
            {
                int inicio;
                int fin;

                if (fO < fD)
                {
                    inicio = fO + 1;
                    fin = fD;
                }
                else
                {
                    inicio = fD + 1;
                    fin = fO;
                }

                for (int i = inicio; i < fin; i++)
                {
                    if (tablero[i, cO] != null)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }

    // CLASE SOLDADO
    class Soldado : Pieza
    {
        // Constructor
        public Soldado(string equipo) : base("S", equipo)
        {

        }

        // Override
        public override bool ValidarMovimiento(
            int fO,
            int cO,
            int fD,
            int cD,
            Pieza[,] tablero)
        {
            int direccion;

            // Abstracción sencilla
            if (Equipo == "A")
            {
                direccion = 1;
            }
            else
            {
                direccion = -1;
            }

            // Avanzar recto
            if (cO == cD &&
                fD == fO + direccion &&
                tablero[fD, cD] == null)
            {
                return true;
            }

            // Ataque diagonal
            if ((cD == cO + 1 || cD == cO - 1) &&
                fD == fO + direccion &&
                tablero[fD, cD] != null)
            {
                return true;
            }

            return false;
        }
    }

    // CLASE PRINCIPAL
    class Program
    {
        // MATRIZ
        static Pieza[,] tablero = new Pieza[8, 8];
        static Usuario[] misUsuarios = new Usuario[2];
        static bool alguienLogueado = false;

        // RECORD
        static string mejorJugador = "";
        static int mejorPuntaje = 0;

        static void Main(string[] args)
        {
            // Creamos usuarios en el código
            misUsuarios[0] = new Usuario("admin", "UsUario!8");
            misUsuarios[1] = new Usuario("jugador1", "pass");

            Console.WriteLine("BIENVENIDO AL SISTEMA");
            Login();

            if (alguienLogueado)
            {
                Menu();
            }
        }
        static void Login()
        {
            Console.WriteLine("--- INICIO DE SESIÓN ---");
            Console.Write("Usuario: ");
            string user = Console.ReadLine();
            Console.Write("Contraseña: ");
            string pass = Console.ReadLine();

            for (int i = 0; i < misUsuarios.Length; i++)
            {
                if (misUsuarios[i].Username == user && misUsuarios[i].Password == pass)
                {
                    alguienLogueado = true;
                    Console.WriteLine("¡Ingreso exitoso!");
                    return;
                }
            }
            Console.WriteLine("Datos incorrectos. Cerrando programa.");
        }

        // MENÚ
        static void Menu()
        {
            string opcion = "";

            while (opcion != "4")
            {

                Console.WriteLine("\n===== JUEGO DE TABLERO =====");
                Console.WriteLine("1. Iniciar Partida");
                Console.WriteLine("2. Ver Reglas");
                Console.WriteLine("3. Ver Puntaje Más Alto");
                Console.WriteLine("4. Salir");

                Console.Write("Seleccione: ");
                opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        IniciarPartida();
                        break;

                    case "2":
                        MostrarReglas();
                        break;

                    case "3":
                        MostrarRecord();
                        break;

                    case "4":
                        Console.WriteLine("Saliendo...");
                        break;

                    default:
                        Console.WriteLine("Opción inválida");
                        break;
                }
            }
        }

        // REGLAS
        static void MostrarReglas()
        {

            Console.WriteLine("\n===== REGLAS =====");
            Console.WriteLine("REY:");
            Console.WriteLine("- Se mueve 1 casilla");

            Console.WriteLine("\nTORRE:");
            Console.WriteLine("- Se mueve recto");

            Console.WriteLine("\nSOLDADO:");
            Console.WriteLine("- Avanza 1");
            Console.WriteLine("- Ataca diagonal");

        }

        // RECORD
        static void MostrarRecord()
        {

            if (mejorPuntaje == 0)
            {
                Console.WriteLine("\nAún no hay puntajes registrados");
            }
            else
            {
                Console.WriteLine("\nMejor jugador: " + mejorJugador);
                Console.WriteLine("Puntaje: " + mejorPuntaje);
            }

        }

        // INICIAR PARTIDA
        static void IniciarPartida()
        {

            Console.Write("\nNombre jugador 1: ");
            string nombre1 = Console.ReadLine();

            Console.Write("Nombre jugador 2: ");
            string nombre2 = Console.ReadLine();

            Jugador jugador1 = new Jugador(nombre1);
            Jugador jugador2 = new Jugador(nombre2);

            InicializarTablero();

            bool juegoTerminado = false;

            Jugador actual = jugador1;
            string equipoActual = "A";

            while (juegoTerminado == false)
            {

                DibujarTablero();

                Console.WriteLine("\nTurno de: " + actual.Nombre);

                try
                {
                    Console.Write("Fila origen: ");
                    int fO = int.Parse(Console.ReadLine());

                    Console.Write("Columna origen: ");
                    int cO = int.Parse(Console.ReadLine());

                    Console.Write("Fila destino: ");
                    int fD = int.Parse(Console.ReadLine());

                    Console.Write("Columna destino: ");
                    int cD = int.Parse(Console.ReadLine());

                    bool valido = ValidarMovimiento(
                        fO, cO, fD, cD, equipoActual);

                    if (valido)
                    {
                        Pieza victima = tablero[fD, cD];

                        // ATAQUE
                        if (victima != null)
                        {
                            Console.WriteLine("¡Pieza capturada!");

                            if (victima is Rey)
                            {
                                actual.Puntaje += 60;
                                juegoTerminado = true;
                            }
                            else
                            {
                                actual.Puntaje += 10;
                            }
                        }

                        // MOVER PIEZA
                        tablero[fD, cD] = tablero[fO, cO];
                        tablero[fO, cO] = null;

                        // CAMBIO DE TURNO
                        if (juegoTerminado == false)
                        {
                            if (actual == jugador1)
                            {
                                actual = jugador2;
                                equipoActual = "B";
                            }
                            else
                            {
                                actual = jugador1;
                                equipoActual = "A";
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Movimiento inválido");
                    }
                }
                catch
                {
                    Console.WriteLine("Ingrese solo números");
                }
            }

            Console.WriteLine("\nGANADOR: " + actual.Nombre);
            Console.WriteLine("Puntaje: " + actual.Puntaje);

            ActualizarRecord(actual.Nombre, actual.Puntaje);

        }

        // TABLERO
        static void InicializarTablero()
        {
            // Limpiar tablero
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    tablero[i, j] = null;
                }
            }

            // EQUIPO A
            tablero[0, 0] = new Torre("A");
            tablero[0, 4] = new Rey("A");
            tablero[1, 2] = new Soldado("A");
            tablero[1, 3] = new Soldado("A");
            tablero[1, 4] = new Soldado("A");
            tablero[1, 5] = new Soldado("A");
            tablero[0, 7] = new Torre("A");

            // EQUIPO B
            tablero[7, 7] = new Torre("B");
            tablero[7, 3] = new Rey("B");
            tablero[6, 2] = new Soldado("B");
            tablero[6, 3] = new Soldado("B");
            tablero[6, 4] = new Soldado("B");
            tablero[6, 5] = new Soldado("B");
            tablero[7, 0] = new Torre("B");
        }

        // DIBUJAR TABLERO
        static void DibujarTablero()
        {
            Console.WriteLine("\n     0  1  2  3  4  5  6  7");
            Console.WriteLine("   -------------------------");
            for (int i = 0; i < 8; i++)
            {
                Console.Write(i + " | ");
                for (int j = 0; j < 8; j++)
                {
                    if (tablero[i, j] == null) Console.Write(".  ");
                    else
                    {
                        string eq = (tablero[i, j].Equipo == "Equipo A") ? "A" : "B";
                        Console.Write(tablero[i, j].Tipo + eq + " ");
                    }
                }
                Console.WriteLine("|");
            }
            Console.WriteLine("   -------------------------");
        }

        // VALIDAR MOVIMIENTO
        static bool ValidarMovimiento(
            int fO,
            int cO,
            int fD,
            int cD,
            string equipo)
        {
            // Fuera de límites
            if (fO < 0 || fO > 7 ||
                cO < 0 || cO > 7 ||
                fD < 0 || fD > 7 ||
                cD < 0 || cD > 7)
            {
                return false;
            }

            // Existe pieza
            if (tablero[fO, cO] == null)
            {
                return false;
            }

            // Pieza del jugador
            if (tablero[fO, cO].Equipo != equipo)
            {
                return false;
            }

            // Casilla ocupada por aliado
            if (tablero[fD, cD] != null)
            {
                if (tablero[fD, cD].Equipo == equipo)
                {
                    return false;
                }
            }

            // POLIMORFISMO
            return tablero[fO, cO].ValidarMovimiento(
                fO,
                cO,
                fD,
                cD,
                tablero
            );
        }

        // RECORD
        static void ActualizarRecord(string nombre,int puntos)
        {
            if (puntos > mejorPuntaje)
            {
                mejorPuntaje = puntos;
                mejorJugador = nombre;
            }
        }
    }
}