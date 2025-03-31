using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

public class TextCorrector
{
  private static readonly Dictionary<string, string> ErrorDictionary = new Dictionary<string, string>()
    {
        {"првиет", "привет"},
        {"пирвет", "привет"},
        {"превед", "привет"},
        {"здарова", "здравствуйте, уважаемый"},
        {"здорово", "здравствуйте, уважаемый"}
    };

  private static readonly string PhoneNumberPattern = @"\((\d)(\d{2})\)\s*(\d{3})-(\d{2})-(\d{2})";
  private static readonly string PhoneNumberReplacement = "+38$1 $2 $3 $4 $5";

  public static void CorrectTextFilesInDirectory(string directoryPath)
  {
    if (!Directory.Exists(directoryPath))
    {
      Console.WriteLine($"Ошибка: Директория '{directoryPath}' не существует.");
      return;
    }

    string[] files = Directory.GetFiles(directoryPath, "*.txt");

    foreach (string filePath in files)
    {
      try
      {
        string fileContent = File.ReadAllText(filePath);

        fileContent = CorrectErrors(fileContent);
        fileContent = ReplacePhoneNumbers(fileContent);
        File.WriteAllText(filePath, fileContent);
        Console.WriteLine($"Файл '{filePath}' успешно обработан.");
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Ошибка при обработке файла '{filePath}': {ex.Message}");
      }
    }
  }

  private static string CorrectErrors(string text)
  {
    string correctedText = text;
    foreach (var errorEntry in ErrorDictionary)
    {
      correctedText = correctedText.Replace(errorEntry.Key, errorEntry.Value);
    }
    return correctedText;
  }

  private static string ReplacePhoneNumbers(string text)
  {
    return Regex.Replace(text, PhoneNumberPattern, PhoneNumberReplacement);
  }
}

class Program{
  public static void Main(string[] args)
  {
    Console.WriteLine("Введите путь к директории с текстовыми файлами:");
    string directoryPath = Console.ReadLine();

    CorrectTextFilesInDirectory(directoryPath);

    Console.WriteLine("Обработка завершена.  Нажмите любую клавишу для выхода.");
    Console.ReadKey();
  }
}
