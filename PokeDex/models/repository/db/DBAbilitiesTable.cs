using Microsoft.Data.Sqlite;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

namespace PokeDex.models.repository.db
{
    public class DBAbilitiesTable
    {
        public async static Task<bool> InitializeTableAbilitiesDB()
        {
            bool op = false;
            await ApplicationData.Current.LocalFolder.CreateFileAsync("pokeDex.db", CreationCollisionOption.OpenIfExists);
            string pathToDB = Path.Combine(ApplicationData.Current.LocalFolder.Path, "pokeDex.db");

            using (SqliteConnection con = new SqliteConnection($"Filename={pathToDB}"))
            {
                con.Open();
                string createTableSQL = "CREATE TABLE IF NOT EXISTS " +
                                         "abilities( " +
                                                 "ability NVARCHAR(100), " +
                                                 "id_pokemon INTEGER, " +
                                                 "PRIMARY KEY (ability, id_pokemon), " +
                                                 "FOREIGN KEY (id_pokemon) REFERENCES pokemon(id) " +
                                          ");";

                SqliteCommand commandCreateTable = new SqliteCommand(createTableSQL, con);
                commandCreateTable.ExecuteReader();
                con.Close();
                op = true;
            }
            return op;
        }
        public static void AddAbilitiesToDB(Pokemon pokemon)
        {
            if (pokemon != null && pokemon.Id != 0 && pokemon.Height != 0 && pokemon.Weight != 0 && !pokemon.Name.Equals(""))
            {
                string pathToDB = Path.Combine(ApplicationData.Current.LocalFolder.Path, "pokeDex.db");

                using (SqliteConnection con = new SqliteConnection($"Filename={pathToDB}"))
                {
                    SqliteCommand commandInsert = new SqliteCommand
                    {
                        Connection = con
                    };

                    foreach (var value in pokemon.Abilities)
                    {
                        try
                        {
                            commandInsert = new SqliteCommand
                            {
                                Connection = con
                            };

                            con.Open();

                            commandInsert.CommandText = "INSERT INTO abilities VALUES(@ability, @id_pokemon);";

                            commandInsert.Parameters.AddWithValue("@ability", value.Ability.Name);
                            commandInsert.Parameters.AddWithValue("@id_pokemon", pokemon.Id);
                            commandInsert.ExecuteReader();

                            con.Close();
                        }
                        catch (SqliteException e)
                        {
                            Console.WriteLine("erro na inserção no DB " + e.Message);
                        }
                    }
                }
            }
        }
        private static int GetStatValue(Pokemon pokemon, string stat)
        {
            foreach (var value in pokemon.Stats)
            {
                if (value.Stat.Name == stat)
                {
                    return value.Base_Stat;
                }
            }
            return 0;
        }
        public static ObservableCollection<Pokemon> GetAllAttributesFromPokemonTable()
        {
            ObservableCollection<Pokemon> pokeList = new ObservableCollection<Pokemon>();
            string pathToDB = Path.Combine(ApplicationData.Current.LocalFolder.Path, "pokeDex.db");

            using (SqliteConnection con = new SqliteConnection($"Filename={pathToDB}"))
            {
                con.Open();

                string selectPokemonSQL = "SELECT id, name, sprints FROM pokemon";
                SqliteCommand CommandSelectPokemon = new SqliteCommand(selectPokemonSQL, con);

                SqliteDataReader reader1 = CommandSelectPokemon.ExecuteReader();


                while (reader1.Read())
                {
                    Pokemon pokemon = new Pokemon
                    {
                        Id = reader1.GetInt16(0),
                        Name = reader1.GetString(1)
                    };

                    Sprites sprintes = new Sprites
                    {
                        Front_Default = reader1.GetString(2)
                    };
                    pokemon.Sprites = sprintes;
                    pokeList.Add(pokemon);
                }
                con.Close();
            }
            return pokeList;
        }
    }
}
