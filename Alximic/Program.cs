using static System.Console;
using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.ComponentModel.Design;
using System.Data;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks.Dataflow;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Diagnostics.SymbolStore;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Unicode;
using System.Runtime.Serialization.Json;

public class Alximic
{
    private static Dictionary<string, List<string>> recipes = new Dictionary<string, List<string>>();
    private static HashSet<string> elements = new HashSet<string>();
    private static bool developerMode = false;
    private static string saveFile = "save.json";

    public static void Main()
    {
        WriteLine("Добро пожаловать в игру Алхимик!");
        WriteLine("Выберите действие:");
        WriteLine("1. Новая игра");
        WriteLine("2. Загрузить игру");

        string choice = ReadLine();

        if (choice == "2")
        {
            LoadGame();
        }
        else
        {
            NewGame();
        }

        GameLoop();
    }

    private static void NewGame()
    {
        elements.Clear();
        elements.Add("огонь");
        elements.Add("вода");
        elements.Add("земля");
        elements.Add("воздух");
        Game();
    }

    private static void GameLoop()
    {
        while (true)
        {
            Cleaner();
            WriteLine("Доступные команды: смешать, режим, список, сохранить, выход");

            Write("\nВведите команду: ");
            string input = ReadLine().ToLower();

            switch (input)
            {
                case "смешать":
                    Mix();
                    break;
                case "режим":
                    Mode();
                    break;
                case "список":
                    Print();
                    break;
                case "сохранить":
                    SaveGame();
                    break;
                case "выход":
                    WriteLine("Сохранить перед выходом? (да/нет)");
                    if (ReadLine().ToLower() == "да")
                    {
                        SaveGame();
                    }
                    return;
                default:
                    WriteLine("Неизвестная команда.");
                    break;
            }

            ReadLine();
        }
    }

    public static void Game()
    {
        // Рецепты
        recipes["пар"] = new List<string> { "огонь", "вода" };
        recipes["грязь"] = new List<string> { "земля", "вода" };
        recipes["пыль"] = new List<string> { "земля", "воздух" };
        recipes["лава"] = new List<string> { "огонь", "земля" };
        recipes["шторм"] = new List<string> { "воздух", "вода" };
        recipes["болото"] = new List<string> { "грязь", "вода" };
        recipes["ветер"] = new List<string> { "воздух", "воздух" };
        recipes["озеро"] = new List<string> { "вода", "вода" };
        recipes["энергия"] = new List<string> { "огонь", "ветер" };
        recipes["спирт"] = new List<string> { "энергия", "вода" };
        recipes["водка"] = new List<string> { "вода", "спирт" };
        recipes["море"] = new List<string> { "озеро", "озеро" };
        recipes["облако"] = new List<string> { "пар", "воздух" };
        recipes["небо"] = new List<string> { "облако", "воздух" };
        recipes["холод"] = new List<string> { "облако", "ветер" };
        recipes["лёд"] = new List<string> { "вода", "холод" };
        recipes["жизнь"] = new List<string> { "болото", "энергия" };
        recipes["семена"] = new List<string> { "жизнь", "земля" };
        recipes["растение"] = new List<string> { "семена", "земля" };
        recipes["фрукт"] = new List<string> { "растение", "семена" };
        recipes["мороженое"] = new List<string> { "фрукт", "лёд" };
        recipes["водоросли"] = new List<string> { "жизнь", "вода" };
        recipes["нори"] = new List<string> { "водросли", "огонь" };
        recipes[""] = new List<string> { "", "" };
    }

    public static void Mix()
    {
        PrintElement();
        Write("Введите первый элемент: ");
        string element1 = ReadLine().ToLower();

        Write("Введите второй элемент: ");
        string element2 = ReadLine().ToLower();

        if (!developerMode)
        {
            if (!elements.Contains(element1))
            {
                WriteLine($"Элемент '{element1}' не открыт!");
                return;
            }

            if (!elements.Contains(element2))
            {
                WriteLine($"Элемент '{element2}' не открыт!");
                return;
            }
        }

        foreach (var recipe in recipes)
        {
            if ((recipe.Value[0] == element1 && recipe.Value[1] == element2) || (recipe.Value[0] == element2 && recipe.Value[1] == element1))
            {
                if (!elements.Contains(recipe.Key))
                {
                    elements.Add(recipe.Key);
                    WriteLine($"Вы открыли новый элемент: {recipe.Key}!");
                }
                else
                {
                    WriteLine($"Комбинация создает: {recipe.Key} (уже открыт)");
                }
                return;
            }
        }

        WriteLine("К сожалению ничего не получилось...");
    }

    private static void Mode()
    {
        developerMode = !developerMode;
        WriteLine($"Режим разработчика {(developerMode ? "включен" : "выключен")}");

        if (developerMode)
        {
            WriteLine("Теперь можно смешивать любые элементы!");
        }
    }

    private static void Print()
    {
        WriteLine("Открытые элементы:");
        foreach (var element in elements)
        {
            WriteLine($"- {element}");
        }

        if (developerMode)
        {
            WriteLine("\nВсе возможные элементы:");
            var allElements = new HashSet<string>(elements);
            foreach (var recipe in recipes)
            {
                allElements.Add(recipe.Key);
            }

            foreach (var element in allElements)
            {
                if (!elements.Contains(element))
                {
                    WriteLine($"- {element} (еще не открыт)");
                }
            }
        }
    }
    private static void PrintElement()
    {
        WriteLine("Открытые элементы:");
        foreach (var element in elements)
        {
            WriteLine($"- {element}");
        }
    }
    private static void Cleaner()
    {
        Clear();
        WriteLine("=== АЛХИМИК ===");
    }

    private static void SaveGame()
    {
        try
        {
            var saveData = new SaveData
            {
                Elements = new List<string>(elements),
                DeveloperMode = developerMode
            };

            using (var file = File.Create(saveFile))
            {
                var serializer = new DataContractJsonSerializer(typeof(SaveData));
                serializer.WriteObject(file, saveData);
            }

            WriteLine("Игра сохранена!");
        }
        catch (Exception e)
        {
            WriteLine($"Ошибка при сохранении: {e.Message}");
        }
    }

    private static void LoadGame()
    {
        if (File.Exists(saveFile))
        {
            try
            {
                using (var file = File.OpenRead(saveFile))
                {
                    var serializer = new DataContractJsonSerializer(typeof(SaveData));
                    var saveData = (SaveData)serializer.ReadObject(file);

                    elements = new HashSet<string>(saveData.Elements);
                    developerMode = saveData.DeveloperMode;
                }

                WriteLine("Игра загружена!");
            }
            catch (Exception e)
            {
                WriteLine($"Ошибка при загрузке: {e.Message}");
                WriteLine("Начинаем новую игру...");
                NewGame();
            }
        }
        else
        {
            WriteLine("Сохранение не найдено. Начинаем новую игру...");
            NewGame();
        }

        // Инициализация рецептов
        Game();
    }
}

[Serializable]
public class SaveData
{
    public List<string> Elements { get; set; }
    public bool DeveloperMode { get; set; }
}