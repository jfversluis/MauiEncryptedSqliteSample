using SQLite;

namespace MauiEncryptedSqliteSample
{
    public class Database
    {
        private SQLiteAsyncConnection _database;
        private readonly string _databasePath;
        private async Task Init()
        {
            if (_database != null)
            {
                return;
            }

            _database = new SQLiteAsyncConnection(_databasePath);
            //var options = new SQLiteConnectionString(dbPath, true, key: "password", postKeyAction: c => { c.Execute("PRAGMA cipher_compatibility = 3"); });
            //_database = new SQLiteAsyncConnection(options);
            await _database.CreateTableAsync<Person>();
        }

        public Database(string dbPath)
        {
            _databasePath = dbPath;
        }

        public async Task<List<Person>> GetPeopleAsync()
        {
            await Init();

            return await _database.Table<Person>().ToListAsync();
        }

        public async Task<int> SavePersonAsync(Person person)
        {
            await Init();

            return await _database.InsertAsync(person);
        }

        public async Task<int> UpdatePersonAsync(Person person)
        {
            await Init();

            return await _database.UpdateAsync(person);
        }

        public async Task<int> DeletePersonAsync(Person person)
        {
            await Init();

            return await _database.DeleteAsync(person);
        }

        public async Task<List<Person>> QuerySubscribedAsync()
        {
            await Init();

            return await _database.QueryAsync<Person>("SELECT * FROM Person WHERE Subscribed = true");
        }

        public async Task<List<Person>> LinqNotSubscribedAsync()
        {
            await Init();

            return await _database.Table<Person>().Where(p => p.Subscribed == false).ToListAsync();
        }
    }
}
