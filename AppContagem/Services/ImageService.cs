using AppContagem.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppContagem.Services
{
    public class ImageService
    {
        private SQLiteAsyncConnection _dbConnection;

        public async Task Init()
        {
            if (_dbConnection == null)
            {
                try
                {
                    string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Imagem.db3");
                    const SQLite.SQLiteOpenFlags Flags = SQLite.SQLiteOpenFlags.ReadWrite | SQLite.SQLiteOpenFlags.Create | SQLite.SQLiteOpenFlags.SharedCache;

                    _dbConnection = new SQLiteAsyncConnection(dbPath, Flags);

                    await _dbConnection.CreateTableAsync<image>();
                }
                catch (Exception ex)
                {

                }
            }
        }

        public async Task<int> AddImage(image imagem)
        {
            return await _dbConnection.InsertAsync(imagem);
        }

        public async Task<List<image>> GetImages()
        {
            return await _dbConnection.Table<image>().ToListAsync();
        }

        public async Task<image> GetImage(string id)
        {
            return await _dbConnection.Table<image>().FirstOrDefaultAsync(i => i.nome == id);
        }
    }
}
