namespace Inventario.Models
{
    public class Usuarios{
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Correo_Electronico { get; set; }
        public string Usuario { get; set; }
        public string Pwd { get; set; }
        public byte Id_Rol { get; set; }
        public Roles Rol { get; set; }
    }
}