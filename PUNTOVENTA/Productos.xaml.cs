using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Text;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PUNTOVENTA
{
    /// <summary>
    /// Lógica de interacción para Productos.xaml
    /// </summary>
    public partial class Productos : Window
    {
        private int currentIdCatalogo=0;
        private int currentIdProducto = 0;
        public Productos()
        {
            InitializeComponent();
            cargarCatalogo();
            cargarProductos();
        }


        public void cargarCatalogo()
        {
            using (var contextProducto = new AppDbContext())
            {
                var catalogo_producto = contextProducto.Catalogo_Producto.ToList();
                if (catalogo_producto.Any())
                {
                    comboBoxCatalogo.ItemsSource = catalogo_producto;
                    comboBoxCatalogo.DisplayMemberPath = "nombre";

                    //captura del id
                    comboBoxCatalogo.SelectedValuePath = "id";
                }
                else
                    MessageBox.Show("No hay registros de tipo producto");
            }
        }

        public void cargarProductos()
        {
            using (var context = new AppDbContext())
            {
                var producto = context.Producto.ToList();
                if (producto.Any())
                {
                    dataGridProducto.ItemsSource = producto;
                }
            }
        }

        private void Click_cliente(object sender, RoutedEventArgs e)
        {
            MainWindow principal = new MainWindow();
            principal.Show();
            this.Close();
        }

        private void Click_salir(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //TODO:
        //CONTRO PARA EVITAR EXCEPCION TIPO PRODUCTO NULL
        private void Agregar_Producto_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new AppDbContext())
            {
                var prodcto = new Producto
                {
                    nombre = txtNombre.Text,
                    stock = int.Parse(txtStock.Text),
                    descripcion = txtDescripcion.Text,
                    fecha_ingreso = DateOnly.Parse(dpFecha.Text),
                    precio = Decimal.Parse(txtPrecio.Text),
                    proveedor = txtProveedor.Text,
                    id_cat_producto = currentIdCatalogo
                }; 

                context.Producto.Add(prodcto);
                context.SaveChanges();
                MessageBox.Show("Se ha agregado con exito el producto");
                cargarProductos();



            }
        }

        private void comboBoxCatalogo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(comboBoxCatalogo.SelectedItem is Catalogo_Producto productoSelecciobado)
            {
                currentIdCatalogo = productoSelecciobado.id;
            }
        }

        private void dataGridProducto_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if(dataGridProducto.SelectedItem is Producto productoSeleccionado)
            {
                txtNombre.Text = productoSeleccionado.nombre;
                txtStock.Text = productoSeleccionado.stock.ToString();
                txtDescripcion.Text = productoSeleccionado.descripcion;
                dpFecha.Text = productoSeleccionado.fecha_ingreso.ToString();
                txtPrecio.Text = productoSeleccionado.precio.ToString();
                txtProveedor.Text = productoSeleccionado.proveedor;

                var categoriaSeleccionada = comboBoxCatalogo.Items
                .Cast<Catalogo_Producto>()
    .           FirstOrDefault(c => c.id == productoSeleccionado.id_cat_producto);

                if (categoriaSeleccionada != null)
                {
                    comboBoxCatalogo.SelectedItem = categoriaSeleccionada;
                }

                currentIdProducto = productoSeleccionado.id_producto;
                //MessageBox.Show($"El id seleccionado es {currentIdProducto}");
            }
        }

        private void Actualizar_Producto_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new AppDbContext())
            {
                var productoEncotrado  = context.Producto.Find(currentIdProducto);

                if (productoEncotrado != null) 
                {
                    productoEncotrado.nombre = txtNombre.Text;
                    productoEncotrado.stock = int.Parse(txtStock.Text);
                    productoEncotrado.descripcion = txtDescripcion.Text;
                    productoEncotrado.fecha_ingreso = DateOnly.Parse(dpFecha.Text);
                    productoEncotrado.precio = decimal.Parse(txtPrecio.Text);
                    productoEncotrado.proveedor = txtProveedor.Text;
                    productoEncotrado.id_cat_producto = currentIdCatalogo;

                    context.SaveChanges();
                    MessageBox.Show("Haz actualizado corectamente ");
                    LimpiarDatosProducto();
                    cargarProductos();
                }


            }
        }

        private void Borrar_Product_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new AppDbContext())
            {

                if(currentIdProducto == 0)
                {
                    MessageBox.Show("Debe seleccionar antes de borrar");
                    return;
                }

                var resultadoBorrar = MessageBox.Show("Estas seguro de querer eliminar?", "Confirmar Eliminacion",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (resultadoBorrar == MessageBoxResult.Yes)
                {
                    try
                    {
                        var prodcutoEliminar = context.Producto.Find(currentIdProducto);
                        if(prodcutoEliminar != null)
                        {
                            context.Producto.Remove(prodcutoEliminar);
                            context.SaveChanges();

                            cargarProductos();
                            LimpiarDatosProducto();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al actualizar: {ex.Message}");
                    }
                }


            }
        }

        private void LimpiarDatosProducto()
        {
            txtNombre.Text = string.Empty;
            txtStock.Text = string.Empty;
            txtDescripcion.Text = string.Empty;
            txtPrecio.Text = string.Empty;
            dpFecha.SelectedDate = null;
            txtPrecio.Text = string.Empty ;
            txtProveedor.Text = string.Empty;
            comboBoxCatalogo.SelectedItem = null;

        }

    }
}
