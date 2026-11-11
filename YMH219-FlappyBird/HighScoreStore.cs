namespace FlappyBird;

/// <summary>
/// En yüksek skoru yerel bir metin dosyasında saklar.
/// </summary>
public class HighScoreStore
{
    public HighScoreStore(string filePath)
    {
        FilePath = filePath;
    }

    public string FilePath { get; }

    public static HighScoreStore CreateDefault()
    {
        string folder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "YMH219-FlappyBird");
        return new HighScoreStore(Path.Combine(folder, "highscore.txt"));
    }

    public int Load()
    {
        try
        {
            if (!File.Exists(FilePath))
            {
                return 0;
            }

            string text = File.ReadAllText(FilePath).Trim();
            return int.TryParse(text, out int value) && value > 0 ? value : 0;
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            return 0;
        }
    }

    public bool TrySave(int score)
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(FilePath)!);
            File.WriteAllText(FilePath, score.ToString());
            return true;
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            return false;
        }
    }
}
