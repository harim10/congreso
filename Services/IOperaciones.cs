using Inventario.Models;
using System.Collections.Generic;
namespace Inventario.Services
{
    public interface IOperaciones{

        public Productos GetProductoById(int id);
        public List<Productos> GetProductos();
        public void AddProducto(Productos producto);
        public List<Categorias> GetCategorias();
        public void UpdateProducto(Productos producto);
        public void DeleteProducto(int id);

        public Usuarios Login(string usuario, string pwd);
    }
}