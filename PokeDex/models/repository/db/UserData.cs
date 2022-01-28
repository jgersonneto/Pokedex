using Microsoft.Data.Sqlite;
using PokeDex.models.repository.api;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

namespace PokeDex.models.repository.db
{
    public class UserData
    {
        public UserData()
        {
            var result = Task.Run(async () => await InitializeTablePokemonDB()).Result;
            var result1 = Task.Run(async () => await InitializeTableAbilitiesDB()).Result;
            var result2 = Task.Run(async () => await InitializeTableTypesDB()).Result;
        }

        public async static Task<bool> InitializeTablePokemonDB()
        {
            bool op = false;
            await ApplicationData.Current.LocalFolder.CreateFileAsync("pokeDex.db", CreationCollisionOption.OpenIfExists);
            string pathToDB = Path.Combine(ApplicationData.Current.LocalFolder.Path, "pokeDex.db");

            using (SqliteConnection con = new SqliteConnection($"Filename={pathToDB}"))
            {
                con.Open();
                string createTableSQL = "CREATE TABLE IF NOT EXISTS " +
                                        "pokemon(id INTEGER PRIMARY KEY, " +
                                                "name NVARCHAR(100), " +
                                                "base_experience INTEGER, " +
                                                "height INTEGER, " +
                                                "weight INTEGER, " +
                                                "hp INTEGER, " +
                                                "attack INTEGER, " +
                                                "defense INTEGER, " +
                                                "special_attack INTEGER, " +
                                                "special_defense INTEGER, " +
                                                "speed INTEGER, " +
                                                "sprints NVARCHAR(200)" +
                                        ");";

                SqliteCommand commandCreateTable = new SqliteCommand(createTableSQL, con);
                commandCreateTable.ExecuteReader();

                con.Close();
                op = true;
            }
            return op;
        }
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
        public async static Task<bool> InitializeTableTypesDB()
        {
            bool op = false;
            await ApplicationData.Current.LocalFolder.CreateFileAsync("pokeDex.db", CreationCollisionOption.OpenIfExists);
            string pathToDB = Path.Combine(ApplicationData.Current.LocalFolder.Path, "pokeDex.db");

            using (SqliteConnection con = new SqliteConnection($"Filename={pathToDB}"))
            {
                con.Open();
                string createTableSQL = "CREATE TABLE IF NOT EXISTS " +
                                         "types( " +
                                             "type NVARCHAR(100), " +
                                             "id_pokemon INTEGER, " +
                                             "PRIMARY KEY (type, id_pokemon), " +
                                             "FOREIGN KEY (id_pokemon) REFERENCES pokemon(id) " +
                                         ");";

                SqliteCommand commandCreateTable = new SqliteCommand(createTableSQL, con);
                commandCreateTable.ExecuteReader();
                con.Close();
                op = true;
            }
            return op;
        }

        public static void AddPokemonToDB(Pokemon pokemon)
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


                    con.Open();

                    try
                    {
                        commandInsert.CommandText = "INSERT INTO pokemon VALUES(@id, @name, " +
                    "@base_experience, @height, @weight, @hp, @attack, @defense, " +
                    "@special_attack, @special_defense, @speed, @sprints);";

                        commandInsert.Parameters.AddWithValue("@id", pokemon.Id);
                        commandInsert.Parameters.AddWithValue("@name", pokemon.Name);
                        commandInsert.Parameters.AddWithValue("@base_experience", pokemon.Base_Experience);
                        commandInsert.Parameters.AddWithValue("@height", pokemon.Height);
                        commandInsert.Parameters.AddWithValue("@weight", pokemon.Weight);

                        commandInsert.Parameters.AddWithValue("@hp", GetStatValue(pokemon, "hp"));
                        commandInsert.Parameters.AddWithValue("@attack", GetStatValue(pokemon, "attack"));
                        commandInsert.Parameters.AddWithValue("@defense", GetStatValue(pokemon, "defense"));
                        commandInsert.Parameters.AddWithValue("@special_attack", GetStatValue(pokemon, "special-attack"));
                        commandInsert.Parameters.AddWithValue("@special_defense", GetStatValue(pokemon, "special-defense"));
                        commandInsert.Parameters.AddWithValue("@speed", GetStatValue(pokemon, "speed"));
                        commandInsert.Parameters.AddWithValue("@sprints", pokemon.Sprites.Other.Home.Front_Default);

                        commandInsert.ExecuteReader();
                    }
                    catch (SqliteException e)
                    {
                        Console.WriteLine("erro na inserção no DB " + e.Message);
                    }


                    con.Close();

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

                    foreach (var value in pokemon.Types)
                    {
                        try
                        {
                            commandInsert = new SqliteCommand
                            {
                                Connection = con
                            };

                            con.Open();

                            commandInsert.CommandText = "INSERT INTO types VALUES(@type, @id_pokemon);";

                            commandInsert.Parameters.AddWithValue("@type", value.Type.Name);
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
        public static ObservableCollection<Pokemon> GetAllPokemon()
        {
            //requestResult();
            //List<Pokemon> pokeList = new List<Pokemon>();
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
                    string selectTypeSQL = "SELECT type, id_pokemon FROM types";
                    SqliteCommand CommandSelectType = new SqliteCommand(selectTypeSQL, con);

                    SqliteDataReader reader2 = CommandSelectType.ExecuteReader();

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
                    List<TypeElement> typeList = new List<TypeElement>();
                    while (reader2.Read())
                    {
                        int idPokemon = reader2.GetInt32(1);
                        if (idPokemon == pokemon.Id)
                        {
                            TypeClass nameType = new TypeClass
                            {
                                Name = reader2.GetString(0)
                            };
                            TypeElement type = new TypeElement
                            {
                                Type = nameType
                            };
                            typeList.Add(type);
                            pokemon.Types = typeList;
                        }

                    }
                    pokeList.Add(pokemon);
                }
                con.Close();
            }
            return pokeList;
        }
        public static void RequestResult()
        {
            int termo = 1;

            while (termo != 21)
            {
                var result = Task.Run(async () => await ApiService.ApiPokeById(termo)).Result;

                AddPokemonToDB(result);
                termo++;
            }
        }

        public static Pokemon SearchInApiForPokemonById(int Id)
        {
            var result = Task.Run(async () => await ApiService.ApiPokeById(Id)).Result;
            AddPokemonToDB(result);

            return result;
        }
        public static ObservableCollection<Pokemon> SearchInDBForPokemonById(int Ida)
        {
            ObservableCollection<Pokemon> pokeList = new ObservableCollection<Pokemon>();


            string pathToDB = Path.Combine(ApplicationData.Current.LocalFolder.Path, "pokeDex.db");

            using (SqliteConnection con = new SqliteConnection($"Filename={pathToDB}"))
            {
                con.Open();

                string selectPokemonSQL = "SELECT id, name, sprints FROM pokemon WHERE id LIKE '" + Ida + "%'";
                SqliteCommand CommandSelectPokemon = new SqliteCommand(selectPokemonSQL, con);

                SqliteDataReader reader1 = CommandSelectPokemon.ExecuteReader();



                while (reader1.Read())
                {
                    string selectTypeSQL = "SELECT type, id_pokemon FROM types";
                    SqliteCommand CommandSelectType = new SqliteCommand(selectTypeSQL, con);

                    SqliteDataReader reader2 = CommandSelectType.ExecuteReader();

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
                    List<TypeElement> typeList = new List<TypeElement>();
                    while (reader2.Read())
                    {
                        int idPokemon = reader2.GetInt32(1);
                        if (idPokemon == pokemon.Id)
                        {
                            TypeClass nameType = new TypeClass
                            {
                                Name = reader2.GetString(0)
                            };
                            TypeElement type = new TypeElement
                            {
                                Type = nameType
                            };
                            typeList.Add(type);
                            pokemon.Types = typeList;
                        }

                    }
                    pokeList.Add(pokemon);
                }
                con.Close();
            }
            return pokeList;
            //Falta fazer a implemantação para chamar no DB

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


    }
}
