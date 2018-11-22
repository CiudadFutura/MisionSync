using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.IO;
using WinSCP;
using System.Data;
using System.Data.OleDb;
using static MisionSync.Helpers.Log;

namespace MisionSync.Controladoras
{
    class Facturas
    {
        public event LogEventHandler EToLog;

        private static readonly string certPath = Directory.GetCurrentDirectory() + "\\pk.ppk";
        private static readonly string cfgPath = Directory.GetCurrentDirectory() + "\\ultimaExportacionFacturas.cfg";
        private static readonly string rutaFacturasWeb = ConfigurationManager.AppSettings["rutaFacturasWeb"];
        private static readonly string FacturasPath = ConfigurationManager.AppSettings["rutaFacturas"];
        private static readonly string rutaArchivosDbf = ConfigurationManager.AppSettings["rutaArchivos"];
        private static readonly string rutaTablasDbf = ConfigurationManager.AppSettings["rutaTablas"];
        private static readonly string archivoRemitosDbf = ConfigurationManager.AppSettings["dbfRemitos"];

        
        public bool CheckCfg()
        {
            Boolean rst = true;
            if (!File.Exists(certPath))
            {
                EToLog?.Invoke(this, new LogEventArgs() { Level = LogEnum.Error,  Sender = "FactUploader",
                    Message = String.Format("La Ruta al certificado es incorrecta: {0}", certPath),
                });
                rst &= false;
            }
            if (!Directory.Exists(FacturasPath))
            {
                EToLog?.Invoke(this, new LogEventArgs()
                {
                    Level = LogEnum.Error,
                    Message = String.Format("La Ruta a las facturas no existe: {0}", FacturasPath),
                    Sender = "FactUploader"
                });
                rst &= false;
            }
            if (!Directory.Exists(rutaArchivosDbf))
            {
                EToLog?.Invoke(this, new LogEventArgs()
                {
                    Level = LogEnum.Error,
                    Message = String.Format("La Ruta a los archivos Dbf no existe: {0}", rutaArchivosDbf),
                    Sender = "FactUploader"
                });
                rst &= false;
            }
            if (!Directory.Exists(rutaTablasDbf))
            {
                EToLog?.Invoke(this, new LogEventArgs()
                {
                    Level = LogEnum.Error,
                    Message = String.Format("La Ruta a las tablas Dbf no existe: {0}", rutaTablasDbf),
                    Sender = "FactUploader"
                });
                rst &= false;
            }
            if (!File.Exists(rutaArchivosDbf + archivoRemitosDbf))
            {
                EToLog?.Invoke(this, new LogEventArgs()
                {
                    Level = LogEnum.Error,
                    Message = String.Format("La Ruta al archivo de remitos Dbf no existe: {0}", rutaArchivosDbf + archivoRemitosDbf),
                    Sender = "FactUploader"
                });
                rst &= false;
            }

            return rst;
        }

        public bool CheckClave() {
            if (!File.Exists(certPath))
            {
                return false;
            }
            return true;
        }

        public DateTime GetLastUpload() {
            DateTime dt = DateTime.Now;
            if (!File.Exists(cfgPath) || !DateTime.TryParse(File.ReadAllText(cfgPath), out dt))
            {
                File.WriteAllText(cfgPath, DateTime.Now.ToString("yyyy-MM-dd"));
            }
            return dt;
        }

        public int GetCantidadDeFacturasARenombrar() {
            if (!Directory.Exists(FacturasPath)) {
                EToLog?.Invoke(this, new LogEventArgs()
                {
                    Level = LogEnum.Error,
                    Message = String.Format("La Ruta a las facturas no existe: {0}", FacturasPath),
                    Sender = "FactUploader"
                });
                return 0;
            }
            string hoy;
            DateTime dt = GetLastUpload();
            int cant = 0;
            do
            {
                dt = dt.AddDays(1);
                hoy = dt.ToString("_yyMMdd");
                cant += Directory.GetFiles(FacturasPath, "FAC0*" + hoy + "*.pdf", SearchOption.TopDirectoryOnly).Length;
            } while (dt <= DateTime.Now);
            return cant;
        }

        private void Session_FileTransferred(object sender, TransferEventArgs e)
        {
            EToLog?.Invoke(this, new LogEventArgs()
            {
                Level = LogEnum.Debug,
                Message = String.Format("Archivo {0} transferido: {1}", e.FileName, e.Error == null ? "Correcto" : e.Error.Message),
                Sender = "FactUploader"
            });
        }

        public void RenombrarArchivosFacturas()
        {
            DateTime dt = GetLastUpload();
            string hoy = dt.ToString("_yyMMdd");

            //Eliminamos copia previa de MOVART 
            File.Delete(rutaTablasDbf + archivoRemitosDbf);
            //Traemos copia de MOVART porque no podemos acceder al original
            File.Copy(rutaArchivosDbf + archivoRemitosDbf, rutaTablasDbf + archivoRemitosDbf);

            var contador = 0;

            do
            {
                var facturas = Directory.GetFiles(FacturasPath, "FAC0*" + hoy + "*.pdf", SearchOption.TopDirectoryOnly);
                dt = dt.AddDays(1);
                hoy = dt.ToString("_yyMMdd");

                foreach (var factura in facturas)
                {
                    try
                    {
                        var nroMov = factura.Substring(factura.IndexOf("\\FAC0") + 4, 12);
                        EToLog?.Invoke(this, new LogEventArgs()
                        {
                            Level = LogEnum.Trace,
                            Message = String.Format("Renombrando el archivo de factura: " + factura),
                            Sender = "FactUploader"
                        });
                        //SELECCIONAR nroMovRemito a partir de nroMovFactura
                        var sConArchivos = ConfigurationManager.AppSettings["stringConexion"].Replace("{0}", rutaTablasDbf);
                        var sqlRemitoPorNroMovFactura = string.Format(ConfigurationManager.AppSettings["remitoPorNroMovFactura"],
                            archivoRemitosDbf, nroMov);
                        var tabla = new DataTable();
                        //Console.WriteLine(sqlRemitoPorNroMovFactura);
                        var conexion = new OleDbConnection(sConArchivos);
                        conexion.Open();

                        var comando = new OleDbCommand
                        {
                            Connection = conexion,
                            CommandText = sqlRemitoPorNroMovFactura,
                            CommandType = CommandType.Text
                        };

                        var da = new OleDbDataAdapter(comando);
                        da.Fill(tabla);
                        var nroRemito = tabla.AsEnumerable().Select(r => r.Field<decimal>("NROMOVI")).FirstOrDefault();
                        conexion.Close();
                        //Console.WriteLine(nroRemito);
                        if (nroRemito == 0) continue;

                        //SELECCIONAR nroPedidoWeb a partir de nroMovRemito
                        var sConTablas = ConfigurationManager.AppSettings["stringConexion"].Replace("{0}", rutaTablasDbf);
                        var archivoDbf = ConfigurationManager.AppSettings["dbfPedidos"];
                        var sqlPedidoPorNroMov = string.Format(ConfigurationManager.AppSettings["pedidoPorNroMov"], archivoDbf,
                            nroRemito);
                        tabla = new DataTable();
                        //Console.WriteLine(sqlPedidoPorNroMov);
                        conexion = new OleDbConnection(sConTablas);
                        conexion.Open();

                        comando = new OleDbCommand
                        {
                            Connection = conexion,
                            CommandText = sqlPedidoPorNroMov,
                            CommandType = CommandType.Text
                        };
                        da = new OleDbDataAdapter(comando);
                        da.Fill(tabla);
                        var nroPedidoWeb = tabla.AsEnumerable().Select(r => r.Field<string>("NROPEDIDO").TrimEnd()).FirstOrDefault();

                        conexion.Close();
                        //Console.WriteLine(nroPedidoWeb);
                        if (!long.TryParse(nroPedidoWeb, out long nroPedidoWebNum)) continue;

                        var nuevoNombre = factura.Replace("\\FAC0", "\\FAC_" + nroPedidoWebNum + "_0");
                        File.Move(factura, nuevoNombre.Substring(0, nuevoNombre.Length - 30) + ".pdf");

                        contador += 1;
                    }
                    catch (Exception)
                    {
                        EToLog?.Invoke(this, new LogEventArgs() {
                            Level = LogEnum.Error,
                            Message = String.Format("Error renombrando el archivo de factura: {0}", factura),
                            Sender = "FactUploader"
                           
                        });
                    }
                }

            } while (dt <= DateTime.Now);

            //Eliminamos copia previa de MOVART 
            File.Delete(rutaTablasDbf + archivoRemitosDbf);

            EToLog?.Invoke(this, new LogEventArgs()
            {
                Level = LogEnum.Info,
                Message = String.Format("Se han renombrado {0} archivos de facturas.", contador),
                Sender = "FactUploader"

            });
        }

        public void SubirArchivos() {
            if (!CheckClave())
            {
                EToLog?.Invoke(this, new LogEventArgs()
                {
                    Level = LogEnum.Critical,
                    Message = String.Format("Falta la clave privada"),
                    Sender = "FactUploader"
                });
                return;
            }

            DateTime dt = GetLastUpload();
            string hoy = dt.ToString("_yyMMdd");
            try
            {
                // Setup session options
                SessionOptions sessionOptions = new SessionOptions
                {
                    Protocol = Protocol.Scp,
                    HostName = ConfigurationManager.AppSettings["hostName"],
                    UserName = ConfigurationManager.AppSettings["userName"],
                    PrivateKeyPassphrase = ConfigurationManager.AppSettings["privateKeyPassphrase"],
                    SshPrivateKeyPath = certPath,
                    //TlsClientCertificatePath = "pk.ppk",
                    //SshHostKeyFingerprint = "" //ssh-rsa 2048 xx:xx:xx:xx:xx:xx:xx:xx:xx:xx:xx:xx:xx:xx:xx:xx
                    GiveUpSecurityAndAcceptAnySshHostKey = true,
                };

                using (Session session = new Session())
                {
                    session.FileTransferred += Session_FileTransferred;
                    // Conectar
                    session.Open(sessionOptions);
                    EToLog?.Invoke(this, new LogEventArgs()
                    {
                        Level = LogEnum.Debug,
                        Message = String.Format("Conexion abierta"),
                        Sender = "FactUploader"
                    });
                    // Actualizar archivos
                    TransferOptions transferOptions = new TransferOptions
                    {
                        TransferMode = TransferMode.Binary,
                        OverwriteMode = OverwriteMode.Overwrite,
                    };
                    EToLog?.Invoke(this, new LogEventArgs()
                    {
                        Level = LogEnum.Debug,
                        Message = String.Format("Subiendo archivos."),
                        Sender = "FactUploader"
                    });

                    TransferOperationResult transferResult;

                    do
                    {
                        transferResult = session.PutFiles(FacturasPath + "FAC_*" + hoy + "*.pdf", rutaFacturasWeb, false, transferOptions);
                        dt = dt.AddDays(1);
                        hoy = dt.ToString("_yyMMdd");
                    } while (dt < DateTime.Now);

                    // Dispara algún error
                    transferResult.Check();

                    // Mostrar resultados
                    foreach (TransferEventArgs transfer in transferResult.Transfers)
                    {
                        EToLog?.Invoke(this, new LogEventArgs()
                        {
                            Level = LogEnum.Trace,
                            Message = String.Format("Subir {0} terminado", transfer.FileName),
                            Sender = "FactUploader"
                        });
                    }
                    EToLog?.Invoke(this, new LogEventArgs()
                    {
                        Level = LogEnum.Debug,
                        Message = String.Format("Se han enviado {0} archivos de facturas.", transferResult.Transfers.Count),
                        Sender = "FactUploader"
                    });
                }
                File.WriteAllText(cfgPath, DateTime.Now.ToString("yyyy-MM-dd"));
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e);
                EToLog?.Invoke(this, new LogEventArgs()
                {
                    Level = LogEnum.Error,
                    Message = String.Format(e.Message),
                    Sender = "FactUploader"
                });
            }
        }

        public void RenombrarYSubir() {
            RenombrarArchivosFacturas();
            SubirArchivos();
        }
    }
}
