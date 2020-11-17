
using System.Collections.Generic;

namespace InterfazServiciosMisionAI.Modelos
{
	public class Pedido : Seccion
	{
		public string Id { get; set; }
		public string Usuario_Id { get; set; }
		public string Updated_At { get; set; }
		public string Circulo_Id { get; set; }
		public string Compra_Id { get; set; }
		public List<LineaPedido> Items { get; set; }
	}

	public class LineaPedido
	{
		public string Producto_Id { get; set; }
		public string Cantidad { get; set; }
		public string Rubro { get; set; }
	}
}
