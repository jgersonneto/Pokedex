using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

namespace PokeDex.models.repository.db
{
    public class DBTypesTable
    {
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

        public static void AddTypesToDB(Pokemon pokemon)
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
        public static ObservableCollection<Pokemon> GetAllAttributesForMainPage(ObservableCollection<Pokemon> pokemons)
        {
            ObservableCollection<Pokemon> pokeList = new ObservableCollection<Pokemon>();

            string pathToDB = Path.Combine(ApplicationData.Current.LocalFolder.Path, "pokeDex.db");

            using (SqliteConnection con = new SqliteConnection($"Filename={pathToDB}"))
            {
                con.Open();

                foreach (Pokemon p in pokemons)
                {
                    string selectTypeSQL = "SELECT type, id_pokemon FROM types";
                    SqliteCommand CommandSelectType = new SqliteCommand(selectTypeSQL, con);

                    SqliteDataReader reader2 = CommandSelectType.ExecuteReader();

                    List<TypeElement> typeList = new List<TypeElement>();
                    while (reader2.Read())
                    {
                        int idPokemon = reader2.GetInt32(1);
                        if (idPokemon == p.Id)
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
                            p.Types = typeList;
                        }
                    }
                    pokeList.Add(p);
                }
                con.Close();
            }
            return pokeList;
        }
        public static ObservableCollection<Pokemon> SearchInTypesByIdForMainPage(ObservableCollection<Pokemon> pokemons)
        {
            ObservableCollection<Pokemon> pokeList = new ObservableCollection<Pokemon>();


            string pathToDB = Path.Combine(ApplicationData.Current.LocalFolder.Path, "pokeDex.db");

            using (SqliteConnection con = new SqliteConnection($"Filename={pathToDB}"))
            {
                con.Open();

                foreach (Pokemon p in pokemons)
                {
                    string selectTypeSQL = "SELECT type, id_pokemon FROM types";
                    SqliteCommand CommandSelectType = new SqliteCommand(selectTypeSQL, con);

                    SqliteDataReader reader2 = CommandSelectType.ExecuteReader();

                    List<TypeElement> typeList = new List<TypeElement>();
                    while (reader2.Read())
                    {
                        int idPokemon = reader2.GetInt32(1);
                        if (idPokemon == p.Id)
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
                            p.Types = typeList;
                        }
                    }
                    pokeList.Add(p);
                }
                con.Close();
            }
            return pokeList;
        }

        public static ObservableCollection<Pokemon> SearchInTypesByNameForMainPage(ObservableCollection<Pokemon> pokemons)
        {
            ObservableCollection<Pokemon> pokeList = new ObservableCollection<Pokemon>();


            string pathToDB = Path.Combine(ApplicationData.Current.LocalFolder.Path, "pokeDex.db");

            using (SqliteConnection con = new SqliteConnection($"Filename={pathToDB}"))
            {
                con.Open();

                foreach (Pokemon p in pokemons)
                {
                    string selectTypeSQL = "SELECT type, id_pokemon FROM types";
                    SqliteCommand CommandSelectType = new SqliteCommand(selectTypeSQL, con);

                    SqliteDataReader reader2 = CommandSelectType.ExecuteReader();

                    List<TypeElement> typeList = new List<TypeElement>();
                    while (reader2.Read())
                    {
                        int idPokemon = reader2.GetInt32(1);
                        if (idPokemon == p.Id)
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
                            p.Types = typeList;
                        }
                    }
                    pokeList.Add(p);
                }
                con.Close();
            }
            return pokeList;
        }

    }
}
