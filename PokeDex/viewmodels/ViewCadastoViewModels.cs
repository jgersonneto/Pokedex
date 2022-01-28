
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using PokeDex.models;
using PokeDex.models.Factory;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Xaml.Media.Imaging;

namespace PokeDex.viewmodels
{
    public class ViewCadastoViewModels : ObservableObject
    {
        private FactoryDB fDB = new FactoryDB();
        private FactoryPokemon fPokemon = new FactoryPokemon();

        private string nome_;
        private string tipoone_;
        private string tipotwo_;
        private string abilities_;
        private string id_;
        private string xpbase_;
        private string peso_;
        private string altura_;
        private string hp_;
        private string attack_;
        private string defense_;
        private string specialatack_;
        private string specialdefense_;
        private string speed_;
        private BitmapImage image_;
        private string outPutTextImage_;
        private StorageFile file_;
        private string pathImage_;



        public string Nome
        {
            get => nome_;
            set => SetProperty(ref nome_, value);
        }
        public string TipoOne
        {
            get => tipoone_;
            set => SetProperty(ref tipoone_, value);
        }
        public string TipoTwo
        {
            get => tipotwo_;
            set => SetProperty(ref tipotwo_, value);
        }
        public string Abilities
        {
            get => abilities_;
            set => SetProperty(ref abilities_, value);
        }
        public string XpBase
        {
            get => xpbase_;
            set => SetProperty(ref xpbase_, value);
        }
        public string Id
        {
            get => id_;
            set => SetProperty(ref id_, value);
        }
        public string Altura
        {
            get => altura_;
            set => SetProperty(ref altura_, value);
        }
        public string Peso
        {
            get => peso_;
            set => SetProperty(ref peso_, value);
        }
        public string Hp
        {
            get => hp_;
            set => SetProperty(ref hp_, value);
        }
        public string Attack
        {
            get => attack_;
            set => SetProperty(ref attack_, value);
        }
        public string Defender
        {
            get => defense_;
            set => SetProperty(ref defense_, value);
        }
        public string SpecialAttack
        {
            get => specialatack_;
            set => SetProperty(ref specialatack_, value);
        }
        public string SpecialDefense
        {
            get => specialdefense_;
            set => SetProperty(ref specialdefense_, value);
        }
        public string Speed
        {
            get => speed_;
            set => SetProperty(ref speed_, value);
        }
        public BitmapImage Image
        {
            get => image_;
            set => SetProperty(ref image_, value);
        }
        public StorageFile File
        {
            get => file_;
            set => SetProperty(ref file_, value);
        }
        public string OutPutTextImage
        {
            get => outPutTextImage_;
            set => SetProperty(ref outPutTextImage_, value);
        }
        public string PathImage
        {
            get => pathImage_;
            set => SetProperty(ref pathImage_, value);
        }

        public ICommand LoadImage { get; }

        private async void LoadImageView()
        {
            if(Id != null && !Id.Equals(""))
            {
                var picker = new FileOpenPicker();
                picker.ViewMode = PickerViewMode.Thumbnail;
                picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
                picker.FileTypeFilter.Add(".jpg");
                picker.FileTypeFilter.Add(".jpeg");
                picker.FileTypeFilter.Add(".png");

                File = await picker.PickSingleFileAsync();
                OutPutTextImage = File.Name;
                if (File != null)
                {
                    using (IRandomAccessStream fileStream = await File.OpenAsync(FileAccessMode.Read))
                    {
                        await ApplicationData.Current.LocalFolder.CreateFolderAsync("images", CreationCollisionOption.OpenIfExists);
                        StorageFolder pathPictures = await ApplicationData.Current.LocalFolder.GetFolderAsync("images");
                        await File.CopyAsync(pathPictures, string.Format("{0}.jpg", Id));

                        PathImage = Path.Combine(ApplicationData.Current.LocalFolder.Path, "images");

                        BitmapImage bitmapImage = new BitmapImage();
                        await bitmapImage.SetSourceAsync(fileStream);
                        Image = bitmapImage;
                    }

                }
                else
                {
                    OutPutTextImage = "Falha.";
                }
            }
            else
            {
                string msg = "Opção inválido! Por favor digite o ID para poder carregar a image!";
                ValidationMessege(msg);
            }
            

        }

       

        public ICommand SavePokemon { get; }


        private async void SavedPokemon()

        {

            Pokemon pokemon_ = new Pokemon();

            if (Nome.All(char.IsLetter))
            {
                pokemon_.Name = Nome;
                Nome = "";
            }
            else
            {
                pokemon_ = null;
                Nome = "";
                string msg = "Nome do Pokemon inválido! Por favor digite apenas Letras!";
                ValidationMessege(msg);
            }
            if (Id.All(char.IsDigit) && !Id.Equals(""))
            {
                var auxId = short.Parse(Id);
                if (auxId >= 260 && !fPokemon.ThisPokemonExist(auxId))
                {
                    pokemon_.Id = auxId;
                    Id = "";
                }
                else
                {
                    pokemon_ = null;
                    Id = "";
                    string msg = "Id do Pokemon inválido! Por favor digite apenas Id acima de 260 ou já existe um Pokemon com esse Id!";
                    ValidationMessege(msg);
                }
            }
            else
            {
                pokemon_ = null;
                Id = "";
                string msg = "Id do Pokemon inválido! Por favor digite apenas Numero!";
                ValidationMessege(msg);
            }

            if (Altura.All(char.IsDigit) && !Altura.Equals(""))
            {
                var auxAltura = short.Parse(Altura);
                pokemon_.Height = auxAltura;
                Altura = "";
            }
            else
            {
                pokemon_ = null;
                Altura = "";
                string msg = "Altura do Pokemon inválido! Por favor digite apenas Numero!";
                ValidationMessege(msg);
            }

            if (Peso.All(char.IsDigit) && !Peso.Equals(""))
            {
                var auxPeso = short.Parse(Peso);
                pokemon_.Weight = auxPeso;
                Peso = "";
            }
            else
            {
                pokemon_ = null;
                Peso = "";
                string msg = "Peso do Pokemon inválido! Por favor digite apenas Numero!";
                ValidationMessege(msg);
            }
            if (XpBase.All(char.IsDigit) && !XpBase.Equals(""))
            {
                var auxXpBase = short.Parse(XpBase);
                pokemon_.Base_Experience = auxXpBase;
                XpBase = "";
            }
            else
            {
                pokemon_ = null;
                XpBase = "";
                string msg = "Xp base do Pokemon inválido! Por favor digite apenas Numero!";
                ValidationMessege(msg);
            }


            pokemon_.Types = new List<TypeElement>();
            pokemon_.Stats = new List<StatElement>();


            for (int i = 0; i <= 1; i++)
            {
                if (i == 0)
                {
                    if (TipoOne.All(char.IsLetter)
                       && (TipoOne.Equals("normal") || TipoOne.Equals("fighting") || TipoOne.Equals("flying") || TipoOne.Equals("poison")
                       || TipoOne.Equals("ground") || TipoOne.Equals("rock") || TipoOne.Equals("bug") || TipoOne.Equals("ghost")
                       || TipoOne.Equals("steel") || TipoOne.Equals("fire") || TipoOne.Equals("water") || TipoOne.Equals("grass")
                       || TipoOne.Equals("electric") || TipoOne.Equals("psychic") || TipoOne.Equals("ice") || TipoOne.Equals("dragon")
                       || TipoOne.Equals("dark") || TipoOne.Equals("fairy") || TipoOne.Equals("unknown") || TipoOne.Equals("shadow")))
                    {
                        TypeClass type = new TypeClass();
                        type.Name = TipoOne;
                        TypeElement listType = new TypeElement();
                        listType.Type = type;
                        pokemon_.Types.Add(listType);
                        TipoOne = "";
                    }
                    else
                    {
                        pokemon_ = null;
                        TipoOne = "";
                        string msg = "Tipo primario do Pokemon inválido! Tipo não reconhecido";
                        ValidationMessege(msg);
                    }

                }
                else
                {
                    TipoTwo = "";
                    if (TipoTwo != "")
                    {
                        if (TipoTwo.All(char.IsLetter)
                            && (TipoTwo.Equals("normal") || TipoTwo.Equals("fighting") || TipoTwo.Equals("flying") || TipoTwo.Equals("poison")
                            || TipoTwo.Equals("ground") || TipoTwo.Equals("rock") || TipoTwo.Equals("bug") || TipoTwo.Equals("ghost")
                            || TipoTwo.Equals("steel") || TipoTwo.Equals("fire") || TipoTwo.Equals("water") || TipoTwo.Equals("grass")
                            || TipoTwo.Equals("electric") || TipoTwo.Equals("psychic") || TipoTwo.Equals("ice") || TipoTwo.Equals("dragon")
                            || TipoTwo.Equals("dark") || TipoTwo.Equals("fairy") || TipoTwo.Equals("unknown") || TipoTwo.Equals("shadow")))
                        {
                            TypeClass type = new TypeClass();
                            type.Name = TipoTwo;
                            TypeElement listType = new TypeElement();
                            listType.Type = type;
                            pokemon_.Types.Add(listType);
                            TipoTwo = "";
                        }
                        else
                        {
                            pokemon_ = null;
                            TipoTwo = "";
                            string msg = "Tipo secudario do Pokemon inválido! Tipo não reconhecido";
                            ValidationMessege(msg);
                        }


                    }

                }
            }

            for (int i = 1; i <= 6; i++)
            {
                StatStat stat = new StatStat();
                StatElement listStat = new StatElement();
                switch (i)
                {
                    case 1:
                        if (Hp.All(char.IsDigit) && !Hp.Equals(""))
                        {
                            stat.Name = "hp";
                            var auxHp = short.Parse(Hp);
                            listStat.Base_Stat = auxHp;
                            listStat.Stat = stat;
                            pokemon_.Stats.Add(listStat);
                            Hp = "";
                        }
                        else
                        {
                            pokemon_ = null;
                            Hp = "";
                            string msg = "Vida do Pokemon inválido! Por favor digite apenas Numero!";
                            ValidationMessege(msg);
                        }


                        break;
                    case 2:
                        if (Attack.All(char.IsDigit) && !Attack.Equals(""))
                        {
                            stat.Name = "attack";
                            var auxAttack = short.Parse(Attack);
                            listStat.Base_Stat = auxAttack;
                            listStat.Stat = stat;
                            pokemon_.Stats.Add(listStat);
                            Attack = "";
                        }
                        else
                        {
                            pokemon_ = null;
                            Attack = "";
                            string msg = "Ataque do Pokemon inválido! Por favor digite apenas Numero!";
                            ValidationMessege(msg);
                        }

                        break;
                    case 3:
                        if (Defender.All(char.IsDigit) && !Defender.Equals(""))
                        {
                            stat.Name = "defense";
                            var auxDefense = short.Parse(Defender);
                            listStat.Base_Stat = auxDefense;
                            listStat.Stat = stat;
                            pokemon_.Stats.Add(listStat);
                            Defender = "";
                        }
                        else
                        {
                            pokemon_ = null;
                            Defender = "";
                            string msg = "Defesa do Pokemon inválido! Por favor digite apenas Numero!";
                            ValidationMessege(msg);
                        }


                        break;
                    case 4:
                        if (SpecialAttack.All(char.IsDigit) && !SpecialAttack.Equals(""))
                        {
                            stat.Name = "special-attack";
                            var auxSAttack = short.Parse(SpecialAttack);
                            listStat.Base_Stat = auxSAttack;
                            listStat.Stat = stat;
                            pokemon_.Stats.Add(listStat);
                            SpecialAttack = "";
                        }
                        else
                        {
                            pokemon_ = null;
                            SpecialAttack = "";
                            string msg = "Especial ataque do Pokemon inválido! Por favor digite apenas Numero!";
                            ValidationMessege(msg);
                        }


                        break;
                    case 5:
                        if (SpecialDefense.All(char.IsDigit) && !SpecialDefense.Equals(""))
                        {

                            stat.Name = "special-defense";
                            var auxSDefense = short.Parse(SpecialDefense);
                            listStat.Base_Stat = auxSDefense;
                            listStat.Stat = stat;
                            pokemon_.Stats.Add(listStat);
                            SpecialDefense = "";
                        }
                        else
                        {
                            pokemon_ = null;
                            SpecialDefense = "";
                            string msg = "Especial defesa do Pokemon inválido! Por favor digite apenas Numero!";
                            ValidationMessege(msg);
                        }
                        break;
                    case 6:
                        if (Speed.All(char.IsDigit) && !Speed.Equals(""))
                        {
                            stat.Name = "speed";
                            var auxSpeed = short.Parse(Speed);
                            listStat.Base_Stat = auxSpeed;
                            listStat.Stat = stat;
                            pokemon_.Stats.Add(listStat);
                            Speed = "";
                        }
                        else
                        {
                            pokemon_ = null;
                            Speed = "";
                            string msg = "Velocidade do Pokemon inválido! Por favor digite apenas Numero!";
                            ValidationMessege(msg);
                        }
                        break;
                }

            }

            
            Sprites sprites = new Sprites();
            Other other = new Other();
            Home home = new Home();
            home.Front_Default = PathImage + "\\" + pokemon_.Id + ".jpg";
            other.Home = home;
            sprites.Other = other;
            pokemon_.Sprites = sprites;
            Debug.WriteLine(pokemon_.Sprites.Other.Home.Front_Default);
            AbilityElement abilityElement = new AbilityElement();
            TypeClass typeClass = new TypeClass();

            typeClass.Name = "";
            abilityElement.Ability = typeClass;
            List<AbilityElement> abilities = new List<AbilityElement>();
            abilities.Add(abilityElement);
            pokemon_.Abilities = abilities;

            if (pokemon_ != null)
            {
                fDB.AddPokemonToDB(pokemon_);

            }


            Image = null;
            OutPutTextImage = "";
        }


        private async void ValidationMessege(string msg)
        {
            var dialog = new MessageDialog(msg);
            await dialog.ShowAsync();
        }
        public ViewCadastoViewModels()
        {
            SavePokemon = new RelayCommand(SavedPokemon);
            LoadImage = new RelayCommand(LoadImageView);

        }

        

    }



}