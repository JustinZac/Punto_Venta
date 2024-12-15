using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Microsoft.Data.SqlClient;

namespace PUNTOVENTA
{
    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int currentClienteId=0;
        public MainWindow()
        {
            InitializeComponent();
            cargaDataGrid();

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Productos ventanaProducto = new Productos();
            ventanaProducto.Show();
        }

        private void MenuItem_Click_Salir(object sender, RoutedEventArgs e)
        {
            this.Close();
        }



        private void Agregar_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new AppDbContext())
            {
                try
                {
                    var cliente = new Cliente
                    {

                        Nombre = txtNombre.Text,
                        Apellido = txtApellido.Text,
                        Dpi = txtDpi.Text,
                        Nit = txtNit.Text,
                        Telefono = txtCelular.Text,
                        Correo = txtCorreo.Text
                    };

                    context.Cliente.Add(cliente);
                    context.SaveChanges();
                    Console.WriteLine("Producto agregado");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ocurrió un error: {ex.Message}");
                    if (ex.InnerException != null)
                    {
                        Console.WriteLine($"Detalle: {ex.InnerException.Message}");
                    }
                }

            }

            cargaDataGrid();
        }


        public void cargaDataGrid()
        {
            using (var context = new AppDbContext())
            {
                // Obtener todos los registros de la tabla Clientes
                var clientes = context.Cliente.ToList();

                // Verificar si hay datos en la tabla
                if (clientes.Any())
                {
                    dataGridCliente.ItemsSource = clientes; 
                }
                else
                {
                    Console.WriteLine("No hay clientes en la tabla.");
                }
            }
        }

        private void dataGridCliente_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (dataGridCliente.SelectedItem is Cliente clienteSeleccionado)
            {
                txtNombre.Text = clienteSeleccionado.Nombre;
                txtApellido.Text = clienteSeleccionado.Apellido;
                txtDpi.Text = clienteSeleccionado.Dpi;
                txtNit.Text = clienteSeleccionado.Nit;
                txtCelular.Text = clienteSeleccionado.Telefono;
                txtCorreo.Text = clienteSeleccionado.Correo;

                currentClienteId = clienteSeleccionado.clienteId;
            }
        }

        private void Actualizar_Click(object sender, RoutedEventArgs e)
        {

            using (var context = new AppDbContext())
            {
                try
                {
                    // Busca el cliente en la base de datos
                    var cliente = context.Cliente.Find(currentClienteId);

                    if (cliente != null)
                    {
                        // Actualiza los campos con la información de los TextBox
                        cliente.Nombre = txtNombre.Text;
                        cliente.Apellido = txtApellido.Text;
                        cliente.Dpi = txtDpi.Text;
                        cliente.Nit = txtNit.Text;
                        cliente.Telefono = txtCelular.Text;
                        cliente.Correo = txtCorreo.Text;

                        // Guarda los cambios
                        context.SaveChanges();

                        // Refresca el DataGrid
                        cargaDataGrid();
                        LimpiarDatos();

                        MessageBox.Show("Cliente actualizado correctamente");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al actualizar: {ex.Message}");
                }
            }

        }

        private void Borrar_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new AppDbContext())
            {

                if (currentClienteId == 0)
                {
                    MessageBox.Show("Debe seleccionar primero, antes de eliminar");
                    return;
                }

                //validacion antes de eliminar
                var resultado = MessageBox.Show(
                    "Esats seguro de querer eliminar?\n",
                    "Confirmar eliminacion",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question
                    );

                if(resultado == MessageBoxResult.Yes)
                {
                    try
                    {
                        var cliente = context.Cliente.Find(currentClienteId);

                        if (cliente != null)
                        {
                            context.Cliente.Remove(cliente);
                            context.SaveChanges();

                            cargaDataGrid();
                            LimpiarDatos();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error al actualizar: {ex.Message}");
                    }
                }
            }
        }

        private void LimpiarDatos()
        {
            txtNombre.Text = string.Empty;
            txtApellido.Text = string.Empty;
            txtNit.Text = string.Empty;
            txtDpi.Text = string.Empty;
            txtCorreo.Text = string.Empty;
            txtCelular.Text = string.Empty;
        }

        private void txtnum_PreviewTextInput(object sender, TextCompositionEventArgs e)

        {
            e.Handled = !(OnlyNumberText(e.Text));

        }

        private static readonly Regex _regex = new Regex("[^0-9]");
        private static bool OnlyNumberText(string texto)
        {
            return !_regex.IsMatch(texto);
        }

        private static readonly Regex _regex1 = new Regex(@"[^a-zA-Z]");
        private static bool OnlyText(string texto)
        {
            return !_regex1.IsMatch(texto);
        }

        private void txtNombre_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !(OnlyText(e.Text));
        }
    }
}