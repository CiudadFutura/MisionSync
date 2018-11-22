using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using static MisionSync.Helpers.Log;

namespace MisionSync
{
    public partial class Frm_main : Form
    {
        Controladoras.Facturas fact = new Controladoras.Facturas();
        Controladoras.Sincronizar Sync = new Controladoras.Sincronizar();
        
        public Frm_main()
        {
            InitializeComponent();
            fact.EToLog += Log;
            Sync.EToLog += Log;
            fact.CheckCfg();
        }

        private void Tsmi_salir_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void Frm_main_Load(object sender, EventArgs e)
        {
            dtp_ultimaFecha.Value = fact.GetLastUpload();
        }


        #region Sync
        private void Btn_Productos_Click(object sender, EventArgs e)
        {
            Sync.SyncProductos();
        }

        private void Btn_consumidores_Click(object sender, EventArgs e)
        {
            Sync.SyncConsumidores();
        }

        private void Btn_Productores_Click(object sender, EventArgs e)
        {
            Sync.SyncProductores();
        }

        private void Btn_PedidosCiclo_Click(object sender, EventArgs e)
        {
            Sync.SyncPedidos();
        }

        private void Btn_TodoCiclo_Click(object sender, EventArgs e)
        {
            Sync.SyncTodos();
        }

        private void Btn_PedidosMiniCiclo_Click(object sender, EventArgs e)
        {
            Sync.SyncPedidosMiniCiclo();
        }

        private void Btn_TodoMiniCiclo_Click(object sender, EventArgs e)
        {
            Sync.SyncTodosMiniCiclo();
        }
#endregion
        
        #region log
        private void Log(object sender, LogEventArgs lea)
        {
           
            if (InvokeRequired) {
                Invoke((MethodInvoker) delegate {
                    Log(sender, lea);
                });
                return;
            }

            txt_log.AppendText(DateTime.UtcNow.ToString("yyyyMMddHH:mm:ss",CultureInfo.InvariantCulture), Color.Green);
            switch (lea.Level)
            {
                case LogEnum.Trace:
                    txt_log.AppendText(" TRA ", Color.Gray);
                    break;
                case LogEnum.Debug:
                    txt_log.AppendText(" DBG ", Color.Green);
                    break;
                case LogEnum.Info:
                    txt_log.AppendText(" INF ", Color.Black);
                    break;
                case LogEnum.Warning:
                    txt_log.AppendText(" WAR ", Color.Yellow);
                    break;
                case LogEnum.Error:
                    txt_log.AppendText(" ERR ", Color.Red);
                    break;
                case LogEnum.Critical:
                    txt_log.AppendText(" CRT ", Color.Violet);
                    break;
                default:
                    txt_log.AppendText(" UNK ", Color.Black);
                    break;
            }
            txt_log.AppendText(lea.Sender + " | " + lea.Message + Environment.NewLine, Color.Black);
        }

        private void GuardarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txt_log.SaveFile("log.rtf");
        }
#endregion

        #region facuras
        private void BtnSubir_Click(object sender, EventArgs e)
        {
            fact.RenombrarYSubir();
        }

        private void BtnRenombrar_Click(object sender, EventArgs e)
        {
            fact.RenombrarArchivosFacturas();
        }

        private void Btn_renameUpload_Click(object sender, EventArgs e)
        {
            fact.SubirArchivos();
        }

        private void Dtp_ultimaFecha_ValueChanged(object sender, EventArgs e)
        {
            txt_cantFact.Text = fact.GetCantidadDeFacturasARenombrar().ToString();
        }
        #endregion
    }

    public static class RichTextBoxExtensions
    {
        public static void AppendText(this RichTextBox box, string text, Color color)
        {
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;

            box.SelectionColor = color;
            box.AppendText(text);
            box.SelectionColor = box.ForeColor;
        }
    }
}
