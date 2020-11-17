
using System;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using InterfazServiciosMisionAI.Clases;

namespace InterfazServiciosMisionAI
{
	internal class Program
	{
		private const string Productos = "products";
		private const string NombreProductos = "productos";
		private const string FechaProductos = "products_date";
		private const string Consumidores = "usuarios";
		private const string NombreConsumidores = "consumidores";
		private const string FechaConsumidores = "usuarios_date";
		private const string Productores = "suppliers";
		private const string NombreProductores = "productores";
		private const string FechaProductores = "suppliers_date";
		private const string Pedidos = "orders";
		private const string NombrePedidos = "pedidos";
		private const string FechaPedidos = "orders_date";

		private static void Main(string[] args)
		{
			var parametro = args.Length > 0 ? args[0] : "a";
			switch (parametro)
			{
				case "p":
					MainAsync(Productos, NombreProductos, FechaProductos, parametro).Wait();
					break;
				case "c":
					MainAsync(Consumidores, NombreConsumidores, FechaConsumidores, parametro).Wait();
					break;
				case "v":
					MainAsync(Productores, NombreProductores, FechaProductores, parametro).Wait();
					break;
				case "o":
					MainAsync(Pedidos, NombrePedidos, FechaPedidos, parametro).Wait();
					break;
				case "i":
					MainAsync(Pedidos, NombrePedidos, FechaPedidos, parametro, true).Wait();
					break;
				case "t":
					MainAsync(Productos, NombreProductos, FechaProductos, parametro).Wait();
					MainAsync(Consumidores, NombreConsumidores, FechaConsumidores, parametro).Wait();
					MainAsync(Productores, NombreProductores, FechaProductores, parametro).Wait();
					MainAsync(Pedidos, NombrePedidos, FechaPedidos, parametro).Wait();
					//Almacena la fecha de hoy como fecha de última consulta.
					ActualizarFechaUltimaAct();
					break;
				case "m":
					MainAsync(Productos, NombreProductos, FechaProductos, parametro).Wait();
					MainAsync(Consumidores, NombreConsumidores, FechaConsumidores, parametro).Wait();
					MainAsync(Productores, NombreProductores, FechaProductores, parametro).Wait();
					MainAsync(Pedidos, NombrePedidos, FechaPedidos, parametro, true).Wait();
					//Almacena la fecha de hoy como fecha de última consulta.
					ActualizarFechaUltimaAct();
					break;
				default:
					parametro = "a"; //Si fue cualquier otro toma el valor "a" para no cerrar la consola.
					Console.WriteLine();
					Console.WriteLine("	Ayuda de la Interfaz Misión AI - Evo. Comandos/parámetros válidos:");
					Console.WriteLine();
					Console.WriteLine("	a - Ayuda. Ejemplo: InterfazServiciosMisionAI.exe a");
					Console.WriteLine();
					Console.WriteLine("	p - Sincronizar productos. Ejemplo: InterfazServiciosMisionAI.exe p");
					Console.WriteLine();
					Console.WriteLine("	c - Sincronizar consumidores. Ejemplo: InterfazServiciosMisionAI.exe c");
					Console.WriteLine();
					Console.WriteLine("	v - Sincronizar productores. Ejemplo: InterfazServiciosMisionAI.exe v");
					Console.WriteLine();
					Console.WriteLine("	o - Sincronizar pedidos para Ciclo Ordinario. Ejemplo: InterfazServiciosMisionAI.exe o");
					Console.WriteLine();
					Console.WriteLine("	i - Sincronizar pedidos para Mini Ciclo. Ejemplo: InterfazServiciosMisionAI.exe o");
					Console.WriteLine();
					Console.WriteLine("	t - Sincronizar todos para Ciclo Ordinario. Ejemplo: InterfazServiciosMisionAI.exe t");
					Console.WriteLine();
					Console.WriteLine("	m - Sincronizar todos para Mini Ciclo. Ejemplo: InterfazServiciosMisionAI.exe m");
					Console.WriteLine();
					break;
			}

			var cerrarConsola = ConfigurationSettings.AppSettings["cerrarConsola"] ?? string.Empty;
			if (!string.IsNullOrEmpty(cerrarConsola) && cerrarConsola.Trim().Equals("1") && parametro != "a")
				Environment.Exit(0);
			
		}

		private static async Task MainAsync(string seccion, string nombreSeccion, string parametroFecha, string parametro, bool esMiniCiclo = false)
		{
			if (!string.IsNullOrEmpty(seccion) && !string.IsNullOrEmpty(nombreSeccion))
			{
				try
				{
					Console.WriteLine($"Recuperando los datos de los {nombreSeccion} desde la Misión...");

					await new InterfazServicios().ObtenerResultadoServicio(seccion, nombreSeccion, parametroFecha, esMiniCiclo);

					Console.WriteLine($"Se han sincronizado los datos de los {nombreSeccion} correctamente.");

					//Limpia el archivo de registro de errores.
					File.WriteAllText("RegistroDeErrores.txt", "Anda de primavera.");

					if (parametro != "t")
					{
						//Almacena la fecha de hoy como fecha de última consulta.
						ActualizarFechaUltimaAct();
					}
				}
				catch (Exception e)
				{
					Console.WriteLine($"Algo falló cuando se intentaban sincronizar los datos de los {nombreSeccion}.");

					//ALmacena el error en el archivo de registro de errores.
					File.WriteAllText("RegistroDeErrores.txt", "Fecha: " + DateTime.Now + Environment.NewLine + "Mensaje: " + e.Message +
						Environment.NewLine + "Fuente: " + e.Source + Environment.NewLine + "Stack trace: " + e.StackTrace);

				}
				
			}
		}

		private static void ActualizarFechaUltimaAct()
		{
			//Almacena la fecha de hoy como fecha de última consulta.
			File.WriteAllText("UltimaConsulta.txt", DateTime.Now.ToString("yyyy-MM-dd"));
		} 
	}
}
