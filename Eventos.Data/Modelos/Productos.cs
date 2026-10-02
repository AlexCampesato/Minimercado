using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Eventos.Data.Modelos
{
    public class Producto
    {
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public double PesoGramos { get; set; }
        public int Stock { get; set; }
        public int IdCategoria { get; set; }

        [ForeignKey("IdCategoria")]
        public Categoria? Categoria { get; set; }
    }
}
