using System;
using System.IO;
using UnityEngine;

public sealed class JsonFileGameSaveRepository :
    IGameSaveRepository
{
    private readonly string _filePath;

    public bool HasSave => File.Exists(_filePath);

    public JsonFileGameSaveRepository(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            throw new ArgumentException(
                "Укажи путь к файлу сохранения.",
                nameof(filePath));
        }

        _filePath = Path.GetFullPath(filePath);
    }

    public void Save(GameSaveData data)
    {
        if (data == null)
            throw new ArgumentNullException(nameof(data));

        string json = JsonUtility.ToJson(data, true);

        string directory = Path.GetDirectoryName(_filePath);
        Directory.CreateDirectory(directory);

        string temporaryPath = _filePath + ".tmp";

        File.WriteAllText(temporaryPath, json);

        if (File.Exists(_filePath))
        {
            File.Replace(
                temporaryPath,
                _filePath,
                null);
        }
        else
        {
            File.Move(temporaryPath, _filePath);
        }
    }

    public GameSaveData Load()
    {
        if (!HasSave)
        {
            throw new FileNotFoundException(
                "Сохранение отсутствует.",
                _filePath);
        }

        string json = File.ReadAllText(_filePath);

        if (string.IsNullOrWhiteSpace(json))
        {
            throw new InvalidDataException(
                "Файл сохранения пуст.");
        }

        GameSaveData data =
            JsonUtility.FromJson<GameSaveData>(json);

        if (data == null)
        {
            throw new InvalidDataException(
                "Не удалось прочитать сохранение.");
        }

        return data;
    }
}