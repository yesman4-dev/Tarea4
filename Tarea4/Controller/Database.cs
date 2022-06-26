using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using Ejercicio14.Model;
using System.Threading.Tasks;

namespace Ejercicio14.Controller
{   
    public class Database
    {

        readonly SQLiteAsyncConnection db;
       
        public Database(String pathdb)
        {
            
            db = new SQLiteAsyncConnection(pathdb);

          
            db.CreateTableAsync<Imagen>().Wait();
        }

        
        public Task<List<Imagen>> ListaImagen()
        {
            return db.Table<Imagen>().ToListAsync();

        }

      
        public Task<int> GrabarImagen(Imagen imagen)
        {
            if (imagen.id != 0)
            {
                return db.UpdateAsync(imagen);
            }
            else
            {

                return db.InsertAsync(imagen);
            }
        }

     
        public Task<int> EliminarImagen(Imagen imagen)
        {
            return db.DeleteAsync(imagen);
        }
    }
}
