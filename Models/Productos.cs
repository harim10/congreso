using System.ComponentModel.DataAnnotations;
namespace Inventario.Models
{
    public class Productos{

        public int Id {get; set;}
        [Required(ErrorMessage = "El nombre es requerido")]
        [MaxLength(100, ErrorMessage = "Solo se aceptan 100 caracteres")]
        [MinLength(3, ErrorMessage = "Mínimo se aceptan 3 caracteres")]
        public string Nombre {get; set;}
        [Required(ErrorMessage = "El precio es requerido")]
        public decimal Precio {get; set;}
        [Required(ErrorMessage = "El stock es requerido")]
        [Range(1,100, ErrorMessage = "Solo se acepta un rango entre 1 y 100")]
        public int Stock {get; set;}
        [Required(ErrorMessage = "La categoría es requerida")]
        public byte Id_Categoria {get; set;}

        public Categorias Categoria {get; set;}

        public byte[] Imagen{get; set;}
    }
}