/**
 * Autor original Diego Diaz
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.Net.Http;
using System.Threading;
using System.Web.Script.Serialization;
using MisionSync.Modelos;
using static MisionSync.Helpers.Log;

namespace MisionSync.Controladoras
{
    class Sincronizar
    {
        public event LogEventHandler EToLog;

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

        public void SyncProductos() {
            MainAsync(Productos, NombreProductos, FechaProductos, "p").Wait();
        }

        public void SyncConsumidores()
        {
            MainAsync(Consumidores, NombreConsumidores, FechaConsumidores,"c").Wait();
        }

        public void SyncProductores()
        {
            MainAsync(Productores, NombreProductores, FechaProductores,"v").Wait();
        }

        public void SyncPedidos()
        {
            MainAsync(Pedidos, NombrePedidos, FechaPedidos, "o").Wait();
        }

        public void SyncPedidosMiniCiclo()
        {
            MainAsync(Pedidos, NombrePedidos, FechaPedidos, "i", true).Wait();
        }

        public void SyncTodos()
        {
            MainAsync(Productos, NombreProductos, FechaProductos, "t").Wait();
            MainAsync(Consumidores, NombreConsumidores, FechaConsumidores, "t").Wait();
            MainAsync(Productores, NombreProductores, FechaProductores, "t").Wait();
            MainAsync(Pedidos, NombrePedidos, FechaPedidos, "t").Wait();
            //Almacena la fecha de hoy como fecha de última consulta.
            ActualizarFechaUltimaAct();
        }

        public void SyncTodosMiniCiclo()
        {
            MainAsync(Productos, NombreProductos, FechaProductos, "m").Wait();
            MainAsync(Consumidores, NombreConsumidores, FechaConsumidores, "m").Wait();
            MainAsync(Productores, NombreProductores, FechaProductores, "m").Wait();
            MainAsync(Pedidos, NombrePedidos, FechaPedidos, "m", true).Wait();
            //Almacena la fecha de hoy como fecha de última consulta.
            ActualizarFechaUltimaAct();
        }

        private async Task MainAsync(string seccion, string nombreSeccion, string parametroFecha, string parametro, bool esMiniCiclo = false)
        {
            if (!string.IsNullOrEmpty(seccion) && !string.IsNullOrEmpty(nombreSeccion))
            {
                try
                {
                    EToLog?.Invoke(this, new LogEventArgs()
                    {
                        Level = LogEnum.Info,
                        Message = String.Format($"Recuperando los datos de los {nombreSeccion} desde la Misión..."),
                        Sender = "Sync"

                    });

                    await ObtenerResultadoServicio(seccion, nombreSeccion, parametroFecha, esMiniCiclo);

                    EToLog?.Invoke(this, new LogEventArgs()
                    {
                        Level = LogEnum.Info,
                        Message = String.Format($"Se han sincronizado los datos de los {nombreSeccion} correctamente."),
                        Sender = "Sync"

                    });

                    if (parametro != "t")
                    {
                        //Almacena la fecha de hoy como fecha de última consulta.
                        ActualizarFechaUltimaAct();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Algo falló cuando se intentaban sincronizar los datos de los {nombreSeccion}.");
                    EToLog?.Invoke(this, new LogEventArgs()
                    {
                        Level = LogEnum.Error,
                        Message = String.Format($"Algo falló cuando se intentaban sincronizar los datos de los {nombreSeccion}." + Environment.NewLine + "Mensaje: " + e.Message +
                        Environment.NewLine + "Fuente: " + e.Source + Environment.NewLine + "Stack trace: " + e.StackTrace),
                        Sender = "Sync"

                    });
                }

            }
        }

        private static void ActualizarFechaUltimaAct()
        {
            //Almacena la fecha de hoy como fecha de última consulta.
            File.WriteAllText("UltimaConsulta.txt", DateTime.Now.ToString("yyyy-MM-dd"));
        }


        ////////////////////////////// ==~~~ InterfazServicios ~~~~== \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

        private readonly string UrlBase = ConfigurationManager.AppSettings["urlAPI"] + "{0}?token=" + ConfigurationManager.AppSettings["apiToken"] + "&{1}={2}";

        private string _resultado;

        internal async Task ObtenerResultadoServicio(string seccion, string nombreSeccion, string parametroFecha, bool esMiniCiclo)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("es-ES");

            var fechaUltimaConsulta = "";
            if (File.Exists("UltimaConsulta.txt"))
                fechaUltimaConsulta = File.ReadAllText("UltimaConsulta.txt");
            fechaUltimaConsulta = string.IsNullOrEmpty(fechaUltimaConsulta) ? "1900-01-01" : fechaUltimaConsulta;

            var url = string.Format(UrlBase, seccion, parametroFecha, fechaUltimaConsulta);

            using (var cliente = new HttpClient())
            {
                _resultado = await cliente.GetStringAsync(url);
            }

            EToLog?.Invoke(this, new LogEventArgs()
            {
                Level = LogEnum.Info,
                Message = String.Format($"Actualizando los datos de los {nombreSeccion} en Evo..."),
                Sender = "Sync"
            });

            var jsonSerializador = new JavaScriptSerializer { MaxJsonLength = int.MaxValue };
            var rutaTablasDbf = ConfigurationManager.AppSettings["rutaTablas"];
            var sConTablas = ConfigurationManager.AppSettings["stringConexion"].Replace("{0}", rutaTablasDbf);

            switch (seccion)
            {
                // 
                case "products":
                    var listaProductos = jsonSerializador.Deserialize<List<Producto>>(_resultado);
                    SincronizarProductos(listaProductos, sConTablas);
                    break;
                case "usuarios":
                    var listaConsumidores = jsonSerializador.Deserialize<List<Tercero>>(_resultado);
                    SincronizarConsumidores(listaConsumidores, sConTablas);
                    break;
                case "suppliers":
                    var listaProductores = jsonSerializador.Deserialize<List<Tercero>>(_resultado);
                    SincronizarProductores(listaProductores, sConTablas);
                    break;
                case "orders":
                    _resultado = _resultado.Replace("items\":\"[", "items\":[").Replace("]\"", "]").Replace("\\\"", "\"");
                    var listaPedidos = jsonSerializador.Deserialize<List<Pedido>>(_resultado);
                    SincronizarPedidos(listaPedidos, sConTablas, esMiniCiclo);
                    break;
            }

        }

        private static void SincronizarProductos(IEnumerable<Producto> listadoActualizar, string sConTablas)
        {
            var archivoDbf = ConfigurationManager.AppSettings["dbfArticulos"];
            var archivoMarcasDbf = ConfigurationManager.AppSettings["dbfMarcas"];
            var sqlExistentes = ConfigurationManager.AppSettings["productosExistentes"].Replace("{0}", archivoDbf);
            var sqlInsercion = ConfigurationManager.AppSettings["insertarProductos"].Replace("BASE", archivoDbf);
            var sqlEliminacion = ConfigurationManager.AppSettings["eliminarProductos"].Replace("BASE", archivoDbf);
            var sqlMarcaExistente = ConfigurationManager.AppSettings["marcaExistente"].Replace("BASE", archivoMarcasDbf);
            var sqlUltimaMarcaInsertada = ConfigurationManager.AppSettings["ultimaMarcaInsertada"].Replace("BASE", archivoMarcasDbf);
            var sqlInsertarMarca = ConfigurationManager.AppSettings["insertarMarca"].Replace("BASE", archivoMarcasDbf);

            var conexion = new OleDbConnection(sConTablas);

            //Habilita valores nulos en los campos no requeridos (necesario para el insert en DBF)
            var comandoHabilitar = new OleDbCommand { Connection = conexion, CommandText = "SET NULL OFF" };

            conexion.Open();

            var codigosExistentes = RecuperarExistentes(conexion, sqlExistentes, "CODART").Select(x => x.ToString().Trim()).ToList();

            foreach (var producto in listadoActualizar)
            {
                var comandoInsertar = new OleDbCommand(sqlInsercion, conexion);
                var comandoEliminar = new OleDbCommand { Connection = conexion };

                if (string.IsNullOrEmpty(producto.Id)) continue;

                //Habilita valores nulos en los campos no requeridos (necesario para el insert en DBF)
                comandoHabilitar.ExecuteNonQuery();

                var idProducto = producto.Id.PadLeft(4, '0');
                var nombreProducto = producto.Nombre != null ? producto.Nombre.Replace("'", "''") : "";
                var nombreProductoCortado = nombreProducto.Length > 40 ? nombreProducto.Substring(0, 40) : nombreProducto;
                var precioTexto = producto.Precio != null ? producto.Precio.Replace('.', ',') : "0";
                double.TryParse(precioTexto, out double precioProducto);
                var marca = !string.IsNullOrEmpty(producto.Marca) ? producto.Marca : "";
                marca = marca.Length > 30 ? marca.Substring(0, 30) : marca;
                var rubro = producto.Id == "296" || producto.Id == "1258"
                    ? "Aalmacén"
                    : ObtenerRubroProducto(!string.IsNullOrEmpty(producto.Pack) ? producto.Pack : "wholesaler");

                #region Marca

                var comandoMarca = new OleDbCommand(string.Format(sqlMarcaExistente, marca.Replace("'", "''")), conexion);
                var codigoMarca = comandoMarca.ExecuteScalar();
                // Si la marca no existe se inserta.
                if (codigoMarca == null)
                {
                    comandoMarca.CommandText = sqlUltimaMarcaInsertada;
                    var ultimaMarca = comandoMarca.ExecuteScalar() ?? "0";
                    codigoMarca = (Convert.ToInt32(ultimaMarca.ToString().Trim()) + 1).ToString().PadLeft(4, '0');

                    comandoMarca.CommandText = sqlInsertarMarca;
                    comandoMarca.Parameters.AddWithValue("?", codigoMarca);
                    comandoMarca.Parameters.AddWithValue("?", marca);

                    comandoMarca.ExecuteNonQuery();
                }

                #endregion

                if (codigosExistentes.Contains(idProducto))
                {
                    #region Eliminar producto existente para luego reinsertarlo actualizado

                    comandoEliminar.CommandText = string.Format(sqlEliminacion, idProducto);
                    comandoEliminar.ExecuteNonQuery();

                    #endregion
                }

                #region Insertar Producto

                comandoInsertar.Parameters.AddWithValue("?", idProducto);
                comandoInsertar.Parameters.AddWithValue("?", nombreProductoCortado);
                comandoInsertar.Parameters.AddWithValue("?", nombreProducto);
                comandoInsertar.Parameters.AddWithValue("?", rubro);
                comandoInsertar.Parameters.AddWithValue("?", "C/U");
                comandoInsertar.Parameters.AddWithValue("?", "PESO");
                comandoInsertar.Parameters.AddWithValue("?", precioProducto);
                comandoInsertar.Parameters.AddWithValue("?", precioProducto);
                comandoInsertar.Parameters.AddWithValue("?", 0);
                comandoInsertar.Parameters.AddWithValue("?", 4110102);
                comandoInsertar.Parameters.AddWithValue("?", 1160203);
                comandoInsertar.Parameters.AddWithValue("?", 5110102);
                comandoInsertar.Parameters.AddWithValue("?", codigoMarca);

                comandoInsertar.ExecuteNonQuery();

                #endregion
            }
            conexion.Close();
        }

        private static object ObtenerRubroProducto(string rubro)
        {
            switch (rubro)
            {
                case "wholesaler":
                    return "Almacén";
                case "fragile":
                    return "Frágil";
                case "freshes":
                    return "Frescos";
                case "vegetables":
                    return "FrutVerd";
                case "cleaning":
                    return "Limpieza";
                default:
                    return "Almacén";
            }
        }

        private static void SincronizarConsumidores(IEnumerable<Tercero> listadoActualizar, string sConTablas)
        {
            var archivoTercerosDbf = ConfigurationManager.AppSettings["dbfTerceros"];
            var sqlConsumidoresExistentes = ConfigurationManager.AppSettings["consumidoresExistentes"].Replace("{0}", archivoTercerosDbf);
            var sqlInsercionConsumidor = ConfigurationManager.AppSettings["insertarTerceros"].Replace("BASE", archivoTercerosDbf);
            var sqlActualizacionConsumidor = ConfigurationManager.AppSettings["actualizarTerceros"].Replace("BASE", archivoTercerosDbf);
            var archivoZonaDbf = ConfigurationManager.AppSettings["dbfZonas"];
            var sqlZonasExistentes = ConfigurationManager.AppSettings["zonasExistentes"].Replace("{0}", archivoZonaDbf);
            var sqlInsercionZona = ConfigurationManager.AppSettings["insertarZonas"].Replace("BASE", archivoZonaDbf);

            var conexion = new OleDbConnection(sConTablas);

            //Habilita valores nulos en los campos no requeridos (necesario para el insert en DBF)
            var comandoHabilitar = new OleDbCommand { Connection = conexion, CommandText = "SET NULL OFF" };

            conexion.Open();

            var zonasExistentes = RecuperarExistentes(conexion, sqlZonasExistentes, "CODZONA").Select(x => x.ToString().Trim()).ToList(); ;

            var codigosExistentes = RecuperarExistentes(conexion, sqlConsumidoresExistentes, "CODTER").Select(x => x.ToString().Trim()).ToList(); ;

            foreach (var consumidor in listadoActualizar)
            {
                var comandoInsertarZona = new OleDbCommand(sqlInsercionZona, conexion);
                var comInsertarConsumidor = new OleDbCommand(sqlInsercionConsumidor, conexion);
                var comActualizarConsumidor = new OleDbCommand { Connection = conexion };

                if (consumidor.Id == "5169")
                {
                    // ¿?
                }

                if (string.IsNullOrEmpty(consumidor.Id))
                    continue;

                //Habilita valores nulos en los campos no requeridos (necesario para el insert en DBF)
                comandoHabilitar.ExecuteNonQuery();

                var idConsumidor = consumidor.Id.PadLeft(5, '0');

                var nombreConsumidor = consumidor.Nombre != null ? consumidor.Nombre.Replace("'", "''") : "";

                var apellidoConsumidor = consumidor.Apellido != null ? consumidor.Apellido.Replace("'", "''") : "";
                nombreConsumidor += " " + apellidoConsumidor;
                nombreConsumidor = nombreConsumidor.Length > 50 ? nombreConsumidor.Substring(0, 50) : nombreConsumidor;

                var direccionConsumidor = (consumidor.Calle + " - " + consumidor.Ciudad).Replace("'", "''");
                direccionConsumidor = !string.IsNullOrEmpty(direccionConsumidor) ? (direccionConsumidor.Length > 30 ? direccionConsumidor.Substring(0, 30) : direccionConsumidor) : "";

                var telefonoConsumidor = consumidor.Tel1 != null ? consumidor.Tel1.Replace("-", "").Replace(" ", "").Trim() : "";
                telefonoConsumidor = telefonoConsumidor.Length > 14 ? telefonoConsumidor.Substring(0, 14) : telefonoConsumidor;

                var celularConsumidor = consumidor.Cel1 != null ? consumidor.Cel1.Replace("-", "").Replace(" ", "").Trim() : "";
                celularConsumidor = celularConsumidor.Length > 14 ? celularConsumidor.Substring(0, 14) : celularConsumidor;

                var zonaConsumidor = string.IsNullOrEmpty(consumidor.Circulo_Id) ? "INTERNO" : consumidor.Circulo_Id;

                var emailConsumidor = consumidor.Email != null ? consumidor.Email.Replace("'", "''") : "";
                emailConsumidor = emailConsumidor.Length > 200 ? emailConsumidor.Substring(0, 200) : emailConsumidor;

                if (string.IsNullOrEmpty(consumidor.Dni) || string.IsNullOrEmpty(consumidor.Dni.Trim()) || consumidor.Dni == "null" || consumidor.Dni == "0"
                        || !int.TryParse(consumidor.Dni.Trim().Replace(".", ""), out int dniConsumidor))
                    dniConsumidor = 11111113; //Si está mal el doc le pone 11111113.

                //Si no existe el círculo lo inserta
                if (!zonasExistentes.Contains(zonaConsumidor))
                {
                    comandoInsertarZona.Parameters.AddWithValue("?", zonaConsumidor);
                    comandoInsertarZona.Parameters.AddWithValue("?", zonaConsumidor);

                    comandoInsertarZona.ExecuteNonQuery();
                    zonasExistentes.Add(zonaConsumidor);
                }

                if (codigosExistentes.Contains(idConsumidor))
                {
                    #region Actualizar Consumidor

                    var queryActualizar = string.Format(sqlActualizacionConsumidor, nombreConsumidor, nombreConsumidor, direccionConsumidor, telefonoConsumidor,
                        celularConsumidor, zonaConsumidor, emailConsumidor, "", "96", dniConsumidor, "CLIENTE", idConsumidor);
                    comActualizarConsumidor.CommandText = queryActualizar;

                    comActualizarConsumidor.ExecuteNonQuery();

                    #endregion
                }
                else
                {
                    #region Insertar Consumidor

                    comInsertarConsumidor.Parameters.AddWithValue("?", "CLIENTE");
                    comInsertarConsumidor.Parameters.AddWithValue("?", idConsumidor);
                    comInsertarConsumidor.Parameters.AddWithValue("?", nombreConsumidor);
                    comInsertarConsumidor.Parameters.AddWithValue("?", nombreConsumidor);
                    comInsertarConsumidor.Parameters.AddWithValue("?", direccionConsumidor);
                    comInsertarConsumidor.Parameters.AddWithValue("?", 2000);
                    comInsertarConsumidor.Parameters.AddWithValue("?", telefonoConsumidor);
                    comInsertarConsumidor.Parameters.AddWithValue("?", celularConsumidor);
                    comInsertarConsumidor.Parameters.AddWithValue("?", zonaConsumidor);
                    comInsertarConsumidor.Parameters.AddWithValue("?", 1130101);
                    comInsertarConsumidor.Parameters.AddWithValue("?", "PESO");
                    comInsertarConsumidor.Parameters.AddWithValue("?", emailConsumidor);
                    comInsertarConsumidor.Parameters.AddWithValue("?", "");
                    comInsertarConsumidor.Parameters.AddWithValue("?", "96");
                    comInsertarConsumidor.Parameters.AddWithValue("?", dniConsumidor);
                    comInsertarConsumidor.Parameters.AddWithValue("?", "CONSUMID");

                    comInsertarConsumidor.ExecuteNonQuery();

                    #endregion
                }
            }

            conexion.Close();
        }

        private static void SincronizarProductores(IEnumerable<Tercero> listadoActualizar, string sConTablas)
        {
            var archivoDbf = ConfigurationManager.AppSettings["dbfTerceros"];
            var sqlExistentes = ConfigurationManager.AppSettings["productoresExistentes"].Replace("{0}", archivoDbf);
            var sqlInsercion = ConfigurationManager.AppSettings["insertarTerceros"].Replace("BASE", archivoDbf);
            var sqlActualizacion = ConfigurationManager.AppSettings["actualizarTerceros"].Replace("BASE", archivoDbf);

            var conexion = new OleDbConnection(sConTablas);

            //Habilita valores nulos en los campos no requeridos (necesario para el insert en DBF)
            var comandoHabilitar = new OleDbCommand { Connection = conexion, CommandText = "SET NULL OFF" };

            conexion.Open();

            var consumidoresExistentes = RecuperarExistentes(conexion, sqlExistentes, "CODTER").Select(x => x.ToString().Trim()).ToList(); ;

            foreach (var proveedor in listadoActualizar)
            {
                var comandoInsertar = new OleDbCommand(sqlInsercion, conexion);
                var comandoActualizar = new OleDbCommand { Connection = conexion };

                if (string.IsNullOrEmpty(proveedor.Id)) continue;

                //Habilita valores nulos en los campos no requeridos (necesario para el insert en DBF)
                comandoHabilitar.ExecuteNonQuery();

                var idProveedor = proveedor.Id.PadLeft(4, '0');
                var nombreProveedor = proveedor.Name != null ? proveedor.Name.Replace("'", "''") : "";
                nombreProveedor = nombreProveedor.Length > 50 ? nombreProveedor.Substring(0, 50) : nombreProveedor;
                var direccionProveedor = (proveedor.Calle + " - " + proveedor.Ciudad).Replace("'", "''");
                direccionProveedor = direccionProveedor.Length > 30 ? direccionProveedor.Substring(0, 30) : direccionProveedor;
                var telefonoProveedor = proveedor.Telefono != null ? proveedor.Telefono.Replace("-", "").Replace(" ", "").Trim() : "";
                telefonoProveedor = telefonoProveedor.Length > 14 ? telefonoProveedor.Substring(0, 14) : telefonoProveedor;
                var emailProveedor = proveedor.Email != null ? proveedor.Email.Replace("'", "''") : "";
                emailProveedor = emailProveedor.Length > 200 ? emailProveedor.Substring(0, 200) : emailProveedor;
                var nombreContacto = proveedor.Nombre_Contacto != null ? proveedor.Nombre_Contacto.Replace("'", "''") : "";
                nombreContacto = nombreContacto.Length > 40 ? nombreContacto.Substring(0, 40) : nombreContacto;

                if (consumidoresExistentes.Contains(idProveedor))
                {
                    #region Actualizar Proveedor

                    //Agrego la zona o círculo aunque es valor fijo porque se agregó para consumidores y ambos usan la query de terceros, lo mismo para tipo y nro. de doc.
                    var queryActualizar = string.Format(sqlActualizacion, nombreProveedor, nombreProveedor, direccionProveedor, telefonoProveedor, "", "INTERNO",
                        emailProveedor, nombreContacto, "PROVEEDO", idProveedor, "80", 0);
                    comandoActualizar.CommandText = queryActualizar;

                    comandoActualizar.ExecuteNonQuery();

                    #endregion
                }
                else
                {
                    #region Insertar Proveedor

                    comandoInsertar.Parameters.AddWithValue("?", "PROVEEDO");
                    comandoInsertar.Parameters.AddWithValue("?", idProveedor);
                    comandoInsertar.Parameters.AddWithValue("?", nombreProveedor);
                    comandoInsertar.Parameters.AddWithValue("?", nombreProveedor);
                    comandoInsertar.Parameters.AddWithValue("?", direccionProveedor);
                    comandoInsertar.Parameters.AddWithValue("?", 2000);
                    comandoInsertar.Parameters.AddWithValue("?", proveedor.Telefono);
                    comandoInsertar.Parameters.AddWithValue("?", "");
                    comandoInsertar.Parameters.AddWithValue("?", "INTERNO");
                    comandoInsertar.Parameters.AddWithValue("?", 2110101);
                    comandoInsertar.Parameters.AddWithValue("?", "PESO");
                    comandoInsertar.Parameters.AddWithValue("?", emailProveedor);
                    comandoInsertar.Parameters.AddWithValue("?", nombreContacto);
                    comandoInsertar.Parameters.AddWithValue("?", "80");
                    comandoInsertar.Parameters.AddWithValue("?", 0);
                    comandoInsertar.Parameters.AddWithValue("?", "INSCRIPT");

                    comandoInsertar.ExecuteNonQuery();

                    #endregion
                }
            }

            conexion.Close();
        }

        private static void SincronizarPedidos(IEnumerable<Pedido> listaPedidos, string sConTablas, bool esMiniCiclo)
        {
            var archivoDbf = ConfigurationManager.AppSettings["dbfPedidos"];
            var sqlExistentes = ConfigurationManager.AppSettings["pedidosExistentes"].Replace("{0}", archivoDbf);
            var sqlInsercion = ConfigurationManager.AppSettings["insertarPedidos"].Replace("BASE", archivoDbf);
            var sqlEliminacion = ConfigurationManager.AppSettings["borrarPedidos"].Replace("BASE", archivoDbf);

            var conexion = new OleDbConnection(sConTablas);
            var comandoEliminar = new OleDbCommand { Connection = conexion };
            conexion.Open();

            var codigosExistentesYaProcesados = RecuperarExistentes(conexion, sqlExistentes + " CODTIPMOV <> '';", "NROPEDIDO").Select(x => x.ToString().Trim()).ToList(); ;
            var codigosExistentesActualizables = RecuperarExistentes(conexion, sqlExistentes + " CODTIPMOV = '';", "NROPEDIDO").Select(x => x.ToString().Trim()).ToList(); ;
            var codigoCuotaSocial = !esMiniCiclo ? "0296" : "1258"; // 296 = Cuota Social Ordinaria y 1258 = Cuota Social Miniciclo
            var ciclos = RecuperarCiclos(conexion);

            //Habilita valores nulos en los campos no requeridos (necesario para el insert en DBF)
            var comandoHabilitar = new OleDbCommand { Connection = conexion, CommandText = "SET NULL OFF" };

            foreach (var pedido in listaPedidos)
            {
                if (string.IsNullOrEmpty(pedido.Id) || pedido.Items.Count == 0) continue;

                var idPedido = pedido.Id.PadLeft(12, '0');
                var idConsumidor = pedido.Usuario_Id.PadLeft(5, '0');
                var fechaPedido = !string.IsNullOrEmpty(pedido.Updated_At) ? Convert.ToDateTime(pedido.Updated_At) : DateTime.Now;
                var ciclo = pedido.Compra_Id ?? "";

                if (string.IsNullOrEmpty(pedido.Circulo_Id) || pedido.Circulo_Id == "null")
                {
                    if (esMiniCiclo)
                        pedido.Circulo_Id = "-1"; //Se han abierto las compras en los mini ciclos para personas que carecen de círculo.
                    else
                        throw new Exception("Pedido de consumidor (ID: " + idConsumidor + ") sin círculo."); //En ciclos ordinarios un pedido sin círculo dispara una excepción.
                }

                if (codigosExistentesYaProcesados.Contains(idPedido)) continue; //Si el pedido existe y ya fue procesado no se actualiza.

                if (codigosExistentesActualizables.Contains(idPedido)) //Si el pedido existe y aun no ha sido procesado, se eliminan sus líneas para luego insertar las actualizadas.
                {
                    #region Eliminar Pedido (todas las líneas) para luego reinsertar

                    comandoEliminar.CommandText = string.Format(sqlEliminacion, idPedido);
                    comandoEliminar.ExecuteNonQuery();

                    #endregion
                }

                //Habilita valores nulos en los campos no requeridos (necesario para el insert en DBF)
                comandoHabilitar.ExecuteNonQuery();

                if (DebeAgregarCuotaSocial(esMiniCiclo, pedido.Circulo_Id, conexion, ciclos, idConsumidor, ciclo))
                {
                    #region Insertar Linea Pedido "Cuota Social"
                    var comandoInsertar = new OleDbCommand(sqlInsercion, conexion);

                    comandoInsertar.Parameters.AddWithValue("?", idPedido);
                    comandoInsertar.Parameters.AddWithValue("?", idConsumidor);
                    comandoInsertar.Parameters.AddWithValue("?", fechaPedido);
                    comandoInsertar.Parameters.AddWithValue("?", codigoCuotaSocial);
                    comandoInsertar.Parameters.AddWithValue("?", "1");
                    comandoInsertar.Parameters.AddWithValue("?", Convert.ToDecimal(pedido.Circulo_Id));
                    comandoInsertar.Parameters.AddWithValue("?", ciclo);

                    comandoInsertar.ExecuteNonQuery();

                    #endregion
                }

                //Se insertan las líneas del pedido
                foreach (var linea in pedido.Items)
                {

                    #region Insertar Linea Pedido
                    var comandoInsertar = new OleDbCommand(sqlInsercion, conexion);

                    comandoInsertar.Parameters.AddWithValue("?", idPedido);
                    comandoInsertar.Parameters.AddWithValue("?", idConsumidor);
                    comandoInsertar.Parameters.AddWithValue("?", fechaPedido);
                    comandoInsertar.Parameters.AddWithValue("?", linea.Producto_Id.PadLeft(4, '0'));
                    comandoInsertar.Parameters.AddWithValue("?", linea.Cantidad);
                    comandoInsertar.Parameters.AddWithValue("?", Convert.ToDecimal(pedido.Circulo_Id));
                    comandoInsertar.Parameters.AddWithValue("?", ciclo);

                    comandoInsertar.ExecuteNonQuery();

                    #endregion

                }
            }

            conexion.Close();
        }

        private static List<Ciclo> RecuperarCiclos(OleDbConnection conexion)
        {
            var dt = new DataTable();

            if (conexion.State != ConnectionState.Open) return new List<Ciclo>();

            var comando = new OleDbCommand
            {
                Connection = conexion,
                CommandText = ConfigurationManager.AppSettings["CiclosMes"],
                CommandType = CommandType.Text
            };
            var da = new OleDbDataAdapter(comando);
            da.Fill(dt);

            return dt.AsEnumerable().Select(row => new Ciclo { CicloId = row.Field<string>(0), Mes = row.Field<string>(1) }).ToList();
        }

        private static bool DebeAgregarCuotaSocial(bool esMiniCiclo, string circuloId,
            OleDbConnection conexion, IEnumerable<Ciclo> ciclos, string idConsumidor, string ciclo)
        {
            string consulta;
            if (!esMiniCiclo)
            {
                if (circuloId == "-1")
                    return true; //En mini ciclo a la persona que no tiene círculo se le asigna la cuota social.

                //Ciclo ordinario: Si es el primer pedido de un círculo se le argrega el item Cuota social ordinaria.
                consulta = string.Format(ConfigurationManager.AppSettings["CuotaSocial"], ciclo, circuloId);
            }
            else
            {
                //Mini ciclo: Si ningún integrante del círculo ha realizado compra ordinaria y no pagó ya la mini cuota social, se le argrega el item Cuota social mini ciclos.
                var mes = ciclos.Where(x => x.CicloId.Trim() == ciclo).Select(y => y.Mes).FirstOrDefault();
                var ciclosParaMes = "'" + string.Join("','", ciclos.Where(x => x.Mes == mes).Select(y => y.CicloId.Trim())) + "'";

                consulta = string.Format(ConfigurationManager.AppSettings["MiniCuotaSocial"], ciclosParaMes, circuloId, idConsumidor);
            }

            var cuotasExistentes = RecuperarExistentes(conexion, consulta, "CANTCUOTAS").FirstOrDefault();
            return Convert.ToInt32(cuotasExistentes) == 0;

        }

        private static List<object> RecuperarExistentes(OleDbConnection conexion, string sqlExistentes, string columnaCodigo)
        {
            var dt = new DataTable();

            if (conexion.State != ConnectionState.Open) throw new Exception("Error en la conexión con la tabla.");

            var comando = new OleDbCommand
            {
                Connection = conexion,
                CommandText = sqlExistentes,
                CommandType = CommandType.Text
            };
            var da = new OleDbDataAdapter(comando);
            da.Fill(dt);

            return dt.AsEnumerable().Select(r => r.Field<object>(columnaCodigo)).ToList();
        }

        //private static void AuxiliarActualizarCirculoCiclo(IEnumerable<Pedido> listaPedidos, string sConTablas)
        //{
        //	var archivoDbf = ConfigurationSettings.AppSettings["dbfPedidos"];
        //	var sqlActualizar = ConfigurationSettings.AppSettings["actualizarPedidos"].Replace("BASE", archivoDbf);

        //	var conexion = new OleDbConnection(sConTablas);
        //	conexion.Open();

        //	foreach (var pedido in listaPedidos)
        //	{
        //		if (string.IsNullOrEmpty(pedido.Id) || pedido.Items.Count == 0) continue;

        //		var idPedido = pedido.Id.PadLeft(12, '0');
        //		var ciclo = pedido.Compra_Id ?? "";
        //		var circulo = pedido.Circulo_Id ?? "";

        //		var queryActualizar = string.Format(sqlActualizar, circulo, ciclo, idPedido);
        //		var comActualizarConsumidor = new OleDbCommand
        //		{
        //			Connection = conexion,
        //			CommandText = queryActualizar
        //		};

        //		comActualizarConsumidor.ExecuteNonQuery();

        //	}

        //	conexion.Close();
        //}


    }
}
