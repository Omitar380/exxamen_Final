using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using MediaVault.Models;
using MediaVault.Services;
using System;

namespace MediaVault
{
    public partial class MainWindow : Window
    {
        private readonly ColeccionServicio _servicio;
        private int _idSeleccionado = 0;

        public MainWindow()
        {
            InitializeComponent();
            _servicio = new ColeccionServicio();
            CargarLista();
        }

        private void CargarLista()
        {
            LstColeccion.ItemsSource = _servicio.Listar();
        }

        private string ObtenerComboTexto(ComboBox combo)
        {
            return (combo.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? string.Empty;
        }

        private void MostrarMensaje(string texto, bool esError)
        {
            TxtMensaje.Text = texto;
            TxtMensaje.Foreground = esError ? Brushes.Crimson : Brushes.ForestGreen;
        }

        private void LimpiarFormulario()
        {
            _idSeleccionado = 0;
            CmbTipo.SelectedIndex = 0;
            TxtTitulo.Text = string.Empty;
            TxtGenero.Text = string.Empty;
            TxtDatoExtra.Text = string.Empty;
            CmbEstado.SelectedIndex = 0;
            TxtCalificacion.Text = string.Empty;
            LstColeccion.SelectedItem = null;
        }

        private void CmbTipo_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (LblDatoExtra == null || TxtDatoExtra == null) return;

            string tipo = ObtenerComboTexto(CmbTipo);
            if (tipo == "Videojuego")
            {
                LblDatoExtra.Text = "Plataforma:";
                TxtDatoExtra.PlaceholderText = "Ej. Nintendo Switch / PC";
            }
            else
            {
                LblDatoExtra.Text = "Episodios / Estudio:";
                TxtDatoExtra.PlaceholderText = "Ej. 24 eps / Madhouse";
            }
        }

        private void BtnGuardar_Click(object? sender, RoutedEventArgs e)
        {
            try
            {
                _servicio.Registrar(
                    ObtenerComboTexto(CmbTipo),
                    TxtTitulo.Text ?? "",
                    TxtGenero.Text ?? "",
                    ObtenerComboTexto(CmbEstado),
                    TxtCalificacion.Text ?? "",
                    TxtDatoExtra.Text ?? ""
                );

                LimpiarFormulario();
                CargarLista();
                MostrarMensaje("Elemento registrado correctamente en SQLite.", false);
            }
            catch (Exception ex)
            {
                MostrarMensaje($"Error: {ex.Message}", true);
            }
        }

        private void BtnActualizar_Click(object? sender, RoutedEventArgs e)
        {
            try
            {
                _servicio.Modificar(
                    _idSeleccionado,
                    ObtenerComboTexto(CmbTipo),
                    TxtTitulo.Text ?? "",
                    TxtGenero.Text ?? "",
                    ObtenerComboTexto(CmbEstado),
                    TxtCalificacion.Text ?? "",
                    TxtDatoExtra.Text ?? ""
                );

                LimpiarFormulario();
                CargarLista();
                MostrarMensaje("Registro actualizado correctamente.", false);
            }
            catch (Exception ex)
            {
                MostrarMensaje($"Error: {ex.Message}", true);
            }
        }

        private void BtnEliminar_Click(object? sender, RoutedEventArgs e)
        {
            try
            {
                _servicio.Eliminar(_idSeleccionado);
                LimpiarFormulario();
                CargarLista();
                MostrarMensaje("Elemento eliminado de la base de datos.", false);
            }
            catch (Exception ex)
            {
                MostrarMensaje($"Error: {ex.Message}", true);
            }
        }

        private void BtnLimpiar_Click(object? sender, RoutedEventArgs e)
        {
            LimpiarFormulario();
            TxtMensaje.Text = string.Empty;
        }

        private void LstColeccion_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (LstColeccion.SelectedItem is ElementoColeccion seleccionado)
            {
                _idSeleccionado = seleccionado.Id;
                CmbTipo.SelectedIndex = seleccionado.Tipo == "Videojuego" ? 0 : 1;
                TxtTitulo.Text = seleccionado.Titulo;
                TxtGenero.Text = seleccionado.Genero;
                TxtDatoExtra.Text = seleccionado.DatoExtra;
                TxtCalificacion.Text = seleccionado.Calificacion.ToString();

                CmbEstado.SelectedIndex = seleccionado.Estado switch
                {
                    "Pendiente" => 0,
                    "En progreso" => 1,
                    "Completado" => 2,
                    _ => 0
                };
            }
        }
    }
}