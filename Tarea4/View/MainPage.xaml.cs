using Ejercicio14.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Plugin.Media;
using Ejercicio14.Model;
using Xamarin.Essentials;
using Xamarin.Forms.Xaml;
using System.IO;
using Ejercicio14.converter;
using Tarea4;

namespace Ejercicio14
{
    public partial class MainPage : ContentPage
    {
        Plugin.Media.Abstractions.MediaFile FileFoto = null;

        public MainPage()
        {
            InitializeComponent();
        }

        private Byte[] ConvertImageToByteArray()
        {
            if (FileFoto != null)
            {
                using (MemoryStream memory = new MemoryStream())
                {
                    Stream stream = FileFoto.GetStream();
                    stream.CopyTo(memory);
                    return memory.ToArray();
                }
            }
            return null;
        }




        //BOTON TOMAR FOTO
        private async void btnFoto_Clicked(object sender, EventArgs e)
        {
            FileFoto = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "MisFotos",
                Name = "test.jpg",
                SaveToAlbum = true
            });

            await DisplayAlert("path directorio", FileFoto.Path, "OK");

            if (FileFoto != null)
            {
                foto.Source = ImageSource.FromStream(() =>
                {
                    return FileFoto.GetStream();
                });
            }
        }

        //BOTON GUARDAR
        private async void btnGuardar_Clicked(object sender, EventArgs e)
        {
            if (FileFoto == null)
            {
                await DisplayAlert("Error", "No ha tomado una fotografia", "OK");
                return;
            }

            try
            {
                var img = new Imagen
                {
                    nombre = this.nom.Text,
                    descripcion = this.des.Text,
                    foto = ConvertImageToByteArray()
                };

                var resultado = await App.BD.GrabarImagen(img);


                if (resultado == 1)
                {
                    ClearScreen();
                    await DisplayAlert("Mensaje", "Datos ingresados correctamente", "OK");
                }
                else
                {
                    await DisplayAlert("Mensaje", "Error, No se logró guardar la información", "OK");

                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Mensaje", ex.Message.ToString(), "OK");

            }

        }

        //BOTON GUARDAR
        private async void btnLista_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Lista());
        }

        //Limpiar pantalla
        private void ClearScreen()
        {
            this.nom.Text = String.Empty;
            this.des.Text = String.Empty;
            this.foto.Source = null;
        }
    }
}
