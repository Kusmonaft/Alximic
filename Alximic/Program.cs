using System;
using System.Collections.Generic;
using static System.Console;

public class Alximic
{
    private static void Main()
    {
        Game();

        WriteLine("Добро пожаловать в игру Алхимик!");
        WriteLine("Доступные команды: соединить, режим, список, выход");

        while (true)
        {
            Write("\nВведите команду: ");
            string input = ReadLine().ToLower();

            Cleaner();

            switch (input)
            {
                case "соединить":
                    Mix();
                    break;
                case "режим":
                    Mode();
                    break;
                case "список":
                    PrintElements();
                    break;
                case "выход":
                    return;
                default:
                    WriteLine("Неизвестная команда.");
                    break;
            }
        }
    }
    
    private static Dictionary<string, List<string>> recipes = new Dictionary<string, List<string>>();
    private static HashSet<string> elements = new HashSet<string>();
    private static bool developerMode = false;
    
    public static void Game()
    {
        elements.Add("огонь");
        elements.Add("вода");

        recipes["пар"] = new List<string> { "огонь", "вода" };
    }

    public static void Mix()
    {
        Write("Введите первый элемент: ");
        string element1 = ReadLine().ToLower();

        Write("Введите второй элемент: ");
        string element2 = ReadLine().ToLower();

        if (!elements.Contains(element1) && !developerMode)
        {
            WriteLine($"Элемент '{element1}' не открыт!");
            return;
        }

        if (!elements.Contains(element2) && !developerMode)
        {
            WriteLine($"Элемент '{element2}' не открыт!");
            return;
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
            WriteLine("Все элементы теперь доступны!");
        }
    }

    private static void PrintElements()
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
        WriteLine("Добро пожаловать в игру Алхимик!");
        WriteLine("Доступные команды: соединить, режим, список, выход");
    }

}
