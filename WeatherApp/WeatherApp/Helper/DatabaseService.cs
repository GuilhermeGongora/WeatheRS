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

        // Adiciona uma nova cidade ou atualiza se já existir
        public async Task<int> SaveCityAsync(SavedCity city)
        {
            var existingCity = await GetCityByNameAsync(city.Name);
            if (existingCity != null)
            {
                // A cidade já existe, atualiza suas informações
                existingCity.Temperature = city.Temperature;
                existingCity.Humidity = city.Humidity;
                existingCity.Pressure = city.Pressure;
                existingCity.WindSpeed = city.WindSpeed;
                existingCity.Cloudiness = city.Cloudiness;
                existingCity.Icon = city.Icon;
                existingCity.Date = city.Date;

                return await _database.UpdateAsync(existingCity);
            }
            else
            {
                // Insere uma nova cidade
                return await _database.InsertAsync(city);
            }
        }

        public Task<SavedCity> GetCityByNameAsync(string cityName)
        {
            return _database.Table<SavedCity>()
                            .FirstOrDefaultAsync(c => c.Name.ToLower() == cityName.ToLower());
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
