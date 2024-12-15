using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PUNTOVENTA
{
    public class AppDbContext : DbContext
    {
        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Catalogo_Producto> Catalogo_Producto { get; set; }
        public DbSet<Producto> Producto { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseSqlServer(@"Server=DESKTOP-18GFURM\SQLSERVER2022;Database=PUNTOVENTA;Trusted_Connection=True;TrustServerCertificate=True");

        }
    }

    public class Cliente
    {

        public int clienteId { get; set; }
        public required string Nombre { get; set; }
        public required string Apellido { get; set; }
        public required string Dpi { get; set; }
        public required string Nit { get; set; }
        public required string Telefono { get; set; }
        public required string Correo { get; set; }
    }

    public class Catalogo_Producto
    {
        public int id { get; set; }
        public string nombre { get; set; }
    }

    public class Producto
    {
        [Key]
        public int id_producto { get; set; }
        public string nombre { get; set; }
        public int stock { get; set; }
        public string descripcion { get; set; }
        public DateOnly fecha_ingreso { get; set; }
        public decimal precio { get; set; }
        public string proveedor { get; set; }
        public int id_cat_producto { get; set; }
    }
}
