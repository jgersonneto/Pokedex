using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

namespace PokeDex.models.repository.db
{
    public class DBPokemonTable
    {
        public DBPokemonTable()
        {
            var result = Task.Run(async () => await InitializeTablePokemonDB()).Result;
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
        public static void AddPokemonToDB(Pokemon pokemon)
        {
            if (pokemon != null && pokemon.Id != 0 && !pokemon.Name.Equals(""))
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
        public static ObservableCollection<Pokemon> GetAllAttributesForMainPage()
        {
            ObservableCollection<Pokemon> pokeList = new ObservableCollection<Pokemon>();
            string pathToDB = Path.Combine(ApplicationData.Current.LocalFolder.Path, "pokeDex.db");

            using (SqliteConnection con = new SqliteConnection($"Filename={pathToDB}"))
            {
                con.Open();

                string selectPokemonSQL = "SELECT id, name, base_experience, height, weight, hp, attack, defense, special_attack, special_defense, speed, sprints FROM pokemon";
                SqliteCommand CommandSelectPokemon = new SqliteCommand(selectPokemonSQL, con);

                SqliteDataReader reader1 = CommandSelectPokemon.ExecuteReader();


                while (reader1.Read())
                {
                    Pokemon pokemon = new Pokemon
                    {
                        Id = reader1.GetInt16(0),
                        Name = reader1.GetString(1),
                        Base_Experience = reader1.GetInt16(2),
                        Height = reader1.GetInt16(3),
                        Weight = reader1.GetInt16(4),

                    };
                    int i = 5;
                    List<StatElement> listStats = new List<StatElement>();
                    while (i <= 10)
                    {
                        StatStat statStat = new StatStat()
                        {
                            Name = reader1.GetString(i),
                        };
                        StatElement Stats = new StatElement()
                        {
                            Base_Stat = reader1.GetInt16(i),
                            Stat = statStat
                        };
                        listStats.Add(Stats);
                        i++;
                    }
                    pokemon.Stats = listStats;

                    Sprites sprintes = new Sprites
                    {
                        Front_Default = reader1.GetString(11)
                    };
                    pokemon.Sprites = sprintes;
                    pokeList.Add(pokemon);
                }
                con.Close();
            }
            return pokeList;
        }
        public static ObservableCollection<Pokemon> SearchPokemonByIdForMainPage(int id)
        {
            ObservableCollection<Pokemon> pokeList = new ObservableCollection<Pokemon>();
            string pathToDB = Path.Combine(ApplicationData.Current.LocalFolder.Path, "pokeDex.db");

            using (SqliteConnection con = new SqliteConnection($"Filename={pathToDB}"))
            {
                con.Open();

                string selectPokemonSQL = "SELECT id, name, base_experience, height, weight, hp, attack, defense, special_attack, special_defense, speed, sprints FROM pokemon WHERE id LIKE '" + id + "%'";
                SqliteCommand CommandSelectPokemon = new SqliteCommand(selectPokemonSQL, con);

                SqliteDataReader reader1 = CommandSelectPokemon.ExecuteReader();

                while (reader1.Read())
                {
                    Pokemon pokemon = new Pokemon
                    {
                        Id = reader1.GetInt16(0),
                        Name = reader1.GetString(1),
                        Base_Experience = reader1.GetInt16(2),
                        Height = reader1.GetInt16(3),
                        Weight = reader1.GetInt16(4),

                    };
                    int i = 5;
                    List<StatElement> listStats = new List<StatElement>();
                    while (i <= 10)
                    {
                        StatStat statStat = new StatStat()
                        {
                            Name = reader1.GetString(i),
                        };
                        StatElement Stats = new StatElement()
                        {
                            Base_Stat = reader1.GetInt16(i),
                            Stat = statStat
                        };
                        listStats.Add(Stats);
                        i++;
                    }
                    pokemon.Stats = listStats;

                    Sprites sprintes = new Sprites
                    {
                        Front_Default = reader1.GetString(11)
                    };
                    pokemon.Sprites = sprintes;
                    pokeList.Add(pokemon);
                }
                con.Close();
            }
            return pokeList;
        }
        public static ObservableCollection<Pokemon> SearchOnePokemonById(int id)
        {
            ObservableCollection<Pokemon> pokeList = new ObservableCollection<Pokemon>();
            string pathToDB = Path.Combine(ApplicationData.Current.LocalFolder.Path, "pokeDex.db");

            using (SqliteConnection con = new SqliteConnection($"Filename={pathToDB}"))
            {
                con.Open();

                string selectPokemonSQL = "SELECT id, name, base_experience, height, weight, hp, attack, defense, special_attack, special_defense, speed, sprints FROM pokemon WHERE id = " + id;
                SqliteCommand CommandSelectPokemon = new SqliteCommand(selectPokemonSQL, con);

                SqliteDataReader reader1 = CommandSelectPokemon.ExecuteReader();

                while (reader1.Read())
                {
                    Pokemon pokemon = new Pokemon
                    {
                        Id = reader1.GetInt16(0),
                        Name = reader1.GetString(1),
                        Base_Experience = reader1.GetInt16(2),
                        Height = reader1.GetInt16(3),
                        Weight = reader1.GetInt16(4),

                    };
                    int i = 5;
                    List<StatElement> listStats = new List<StatElement>();
                    while (i <= 10)
                    {
                        StatStat statStat = new StatStat()
                        {
                            Name = reader1.GetString(i),
                        };
                        StatElement Stats = new StatElement()
                        {
                            Base_Stat = reader1.GetInt16(i),
                            Stat = statStat
                        };
                        listStats.Add(Stats);
                        i++;
                    }
                    pokemon.Stats = listStats;

                    Sprites sprintes = new Sprites
                    {
                        Front_Default = reader1.GetString(11)
                    };
                    pokemon.Sprites = sprintes;
                    pokeList.Add(pokemon);
                }
                con.Close();
            }
            return pokeList;
        }
        public static ObservableCollection<Pokemon> SearchOnePokemonByName(string name)
        {
            ObservableCollection<Pokemon> pokeList = new ObservableCollection<Pokemon>();
            string pathToDB = Path.Combine(ApplicationData.Current.LocalFolder.Path, "pokeDex.db");

            using (SqliteConnection con = new SqliteConnection($"Filename={pathToDB}"))
            {
                con.Open();

                string selectPokemonSQL = "SELECT id, name, base_experience, height, weight, hp, attack, defense, special_attack, special_defense, speed, sprints FROM pokemon WHERE name = '" + name + "'";
                SqliteCommand CommandSelectPokemon = new SqliteCommand(selectPokemonSQL, con);

                SqliteDataReader reader1 = CommandSelectPokemon.ExecuteReader();

                while (reader1.Read())
                {
                    Pokemon pokemon = new Pokemon
                    {
                        Id = reader1.GetInt16(0),
                        Name = reader1.GetString(1),
                        Base_Experience = reader1.GetInt16(2),
                        Height = reader1.GetInt16(3),
                        Weight = reader1.GetInt16(4),

                    };
                    int i = 5;
                    List<StatElement> listStats = new List<StatElement>();
                    while (i <= 10)
                    {
                        StatStat statStat = new StatStat()
                        {
                            Name = reader1.GetString(i),
                        };
                        StatElement Stats = new StatElement()
                        {
                            Base_Stat = reader1.GetInt16(i),
                            Stat = statStat
                        };
                        listStats.Add(Stats);
                        i++;
                    }
                    pokemon.Stats = listStats;

                    Sprites sprintes = new Sprites
                    {
                        Front_Default = reader1.GetString(11)
                    };
                    pokemon.Sprites = sprintes;
                    pokeList.Add(pokemon);
                }
                con.Close();
            }
            return pokeList;
        }
        public static ObservableCollection<Pokemon> SearchPokemonByNameForMainPage(string name)

        {
            ObservableCollection<Pokemon> pokeList = new ObservableCollection<Pokemon>();
            string pathToDB = Path.Combine(ApplicationData.Current.LocalFolder.Path, "pokeDex.db");

            using (SqliteConnection con = new SqliteConnection($"Filename={pathToDB}"))
            {
                con.Open();

                string selectPokemonSQL = "SELECT id, name, base_experience, height, weight, hp, attack, defense, special_attack, special_defense, speed, sprints FROM pokemon WHERE name LIKE '" + name + "%'";

                SqliteCommand CommandSelectPokemon = new SqliteCommand(selectPokemonSQL, con);

                SqliteDataReader reader1 = CommandSelectPokemon.ExecuteReader();

                while (reader1.Read())
                {
                    Pokemon pokemon = new Pokemon
                    {
                        Id = reader1.GetInt16(0),
                        Name = reader1.GetString(1),
                        Base_Experience = reader1.GetInt16(2),
                        Height = reader1.GetInt16(3),
                        Weight = reader1.GetInt16(4),

                    };
                    int i = 5;
                    List<StatElement> listStats = new List<StatElement>();
                    while (i <= 10)
                    {
                        StatStat statStat = new StatStat()
                        {
                            Name = reader1.GetString(i),
                        };
                        StatElement Stats = new StatElement()
                        {
                            Base_Stat = reader1.GetInt16(i),
                            Stat = statStat
                        };
                        listStats.Add(Stats);
                        i++;
                    }
                    pokemon.Stats = listStats;

                    Sprites sprintes = new Sprites
                    {
                        Front_Default = reader1.GetString(11)
                    };

                    pokemon.Sprites = sprintes;



                    pokeList.Add(pokemon);
                }
                con.Close();
            }
            return pokeList;
        }
        public static ObservableCollection<Pokemon> SearchPokemonByType(string type)
        {
            ObservableCollection<Pokemon> pokeList = new ObservableCollection<Pokemon>();
            string pathToDB = Path.Combine(ApplicationData.Current.LocalFolder.Path, "pokeDex.db");

            using (SqliteConnection con = new SqliteConnection($"Filename={pathToDB}"))
            {
                con.Open();

                string selectPokemonSQL = "SELECT id, name, base_experience, height, weight, hp, attack, defense, special_attack, special_defense, speed, sprints FROM pokemon AS P INNER JOIN types AS T ON T.id_pokemon = P.id WHERE T.type = '" + type + "'";
                SqliteCommand CommandSelectPokemon = new SqliteCommand(selectPokemonSQL, con);

                SqliteDataReader reader1 = CommandSelectPokemon.ExecuteReader();

                while (reader1.Read())
                {
                    Pokemon pokemon = new Pokemon
                    {
                        Id = reader1.GetInt16(0),
                        Name = reader1.GetString(1),
                        Base_Experience = reader1.GetInt16(2),
                        Height = reader1.GetInt16(3),
                        Weight = reader1.GetInt16(4),

                    };
                    int i = 5;
                    List<StatElement> listStats = new List<StatElement>();
                    while (i <= 10)
                    {
                        StatStat statStat = new StatStat()
                        {
                            Name = reader1.GetString(i),
                        };
                        StatElement Stats = new StatElement()
                        {
                            Base_Stat = reader1.GetInt16(i),
                            Stat = statStat
                        };
                        listStats.Add(Stats);
                        i++;
                    }
                    pokemon.Stats = listStats;

                    Sprites sprintes = new Sprites
                    {
                        Front_Default = reader1.GetString(11)
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
