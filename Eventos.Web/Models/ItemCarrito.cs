namespace Eventos.Web.Models
{
    public class ItemCarrito
    {
        public int ProductoId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public double Precio { get; set; }
        public int Cantidad { get; set; }
        public double Subtotal => Precio * Cantidad;
    }
}
