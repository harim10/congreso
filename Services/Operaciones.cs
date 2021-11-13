using Inventario.Models;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using Dapper;
using System.Collections.Generic;
using System.Linq;

namespace Inventario.Services
{
    public class Operaciones : IOperaciones
    {
        private readonly IConfiguration _config;

        public Operaciones(IConfiguration config)
        {
            this._config = config;
        }
        public Productos GetProductoById(int id)
        {
            Productos p = new Productos();
            using(MySqlConnection db = new MySqlConnection(_config.GetConnectionString("DbMySql"))){
                string sql = $"select * from Productos where Id = {id}";
                p = db.QueryFirstOrDefault<Productos>(sql);
                return p;
            }
        }

        public List<Productos> GetProductos()
        {
             using(MySqlConnection db = new MySqlConnection(_config.GetConnectionString("DbMySql"))){
               string sql = "select p.Id, p.Nombre, p.Stock, p.Precio, p.Id_Categoria, " +
                            "c.Id as IdCategoria, c.Nombre from Productos as p inner join Categorias as c "+
                            "on p.Id_Categoria = c.Id";
               List<Productos> productos = db.Query<Productos, Categorias, Productos>(sql, (p,c) => {
                   p.Categoria = c;
                   return p;
               },
               splitOn : "IdCategoria"
               )
               .ToList();
               return productos;
            }
        }

        public Usuarios Login(string nombre, string pwd){
            using(MySqlConnection db = new MySqlConnection(_config.GetConnectionString("DbMySql"))){
               string sql = "select u.Id, u.Nombre, u.Correo_Electronico, u.Usuario, u.Pwd, u.Id_Rol, " +
                            "r.Id as IdRol, r.Rol from Usuarios as u inner join Roles as r "+
                            "on u.Id_Rol = r.Id where u.Usuario='" + nombre + "' and u.Pwd = '" + pwd +"'";
               Usuarios usuario = db.Query<Usuarios, Roles, Usuarios>(sql, (u,r) => {
                   u.Rol = r;
                   return u;
               },
               splitOn : "IdRol"
               )
               .FirstOrDefault();
               return usuario;
            }
        }

         public List<Categorias> GetCategorias(){
              using(MySqlConnection db = new MySqlConnection(_config.GetConnectionString("DbMySql"))){
               string sql = "Select * from Categorias";
               List<Categorias> categorias = db.Query<Categorias>(sql).ToList();
               return categorias;
            }
         }

        public void AddProducto(Productos producto){
            using(MySqlConnection db = new MySqlConnection(_config.GetConnectionString("DbMySql"))){
               string sql = "insert into Productos (Nombre, Precio, Stock, Id_Categoria, Imagen) " +
                            "values (@nombre, @precio, @stock, @id_categoria, @imagen)";
                var resultado = db.Execute(sql, producto);
            }
        }

        public void UpdateProducto(Productos producto){
            using(MySqlConnection db = new MySqlConnection(_config.GetConnectionString("DbMySql"))){
                string sql = "";
                if(producto.Imagen == null){
                    sql = "update Productos set Nombre=@nombre, Precio=@precio, Stock=@Stock, "
                            + "Id_Categoria = @id_categoria where Id = @id ";
                }
                else{
                    sql = "update Productos set Nombre=@nombre, Precio=@precio, Stock=@Stock, "
                            + "Id_Categoria = @id_categoria, Imagen=@imagen where Id = @id ";
                }
               
                var resultado = db.Execute(sql, producto);
            }
        }

        public void DeleteProducto(int id){
            using(MySqlConnection db = new MySqlConnection(_config.GetConnectionString("DbMySql"))){
               string sql = "delete from Productos where Id=@idproducto";
                var resultado = db.Execute(sql, new {idproducto = id});
            }
        }
    }
}