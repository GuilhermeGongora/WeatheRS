using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;
using WeatherApp.Models;

namespace WeatherApp.Helper
{
    public class DatabaseService
    {
        private readonly SQLiteAsyncConnection _database;

        public DatabaseService(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<SavedCity>().Wait(); // Cria a tabela de cidades, caso ainda não exista
        }

        // Adiciona uma nova cidade
        public Task<int> AddCityAsync(SavedCity city)
        {
            return _database.InsertAsync(city);
        }

        // Retorna todas as cidades salvas
        public Task<List<SavedCity>> GetCitiesAsync()
        {
            return _database.Table<SavedCity>().ToListAsync();
        }

        // Exclui uma cidade
        public Task<int> DeleteCityAsync(SavedCity city)
        {
            return _database.DeleteAsync(city);
        }
    }
}
