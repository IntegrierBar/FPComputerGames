using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Godot;

public static class DungeonHelper
{
    public static List<Dungeon> LoadDungeonsFromFile(string filePath)
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            Converters = { new Vector2IJsonConverter() }
        };

        try
        {
            string jsonContent = File.ReadAllText(ProjectSettings.GlobalizePath(filePath));
            List<Dungeon> dungeons = JsonSerializer.Deserialize<List<Dungeon>>(jsonContent, options);
            return dungeons;
        }
        catch (Exception e)
        {
            GD.PrintErr($"Failed to load dungeons from {filePath}: {e.Message}");
            return new List<Dungeon>();
        }
    }
}
