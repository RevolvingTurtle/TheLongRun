[System.Serializable]
public class HighScoreEntry
{
    public string playerName;
    public int score;

    public HighScoreEntry(string name, int scoreValue)
    {
        playerName = name;
        score = scoreValue;
    }
}