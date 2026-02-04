using UnityEngine;
using TMPro;

public class Highscore : MonoBehaviour
{
    public int level, rows, points;
    private const string HIGHSCORE_KEY = "HIGHSCORE";
    [Header("UI")]
    public TextMeshProUGUI highscoreText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI rowText;
    public TextMeshProUGUI bestScoreText; // NEW (highest ever)

    private int bestScore;

    void Start()
    {
        level = 0;
        rows = 0;
        points = 0;

        // 🔹 Load saved highscore
        bestScore = PlayerPrefs.GetInt(HIGHSCORE_KEY, 0);

        UpdateUI();
        GetComponent<Movement>().SetNewSpeed();
    }

    // Scoring system (Tetris standard)
    public void addPointsForLines(int lines)
    {
        if (lines <= 0) return;

        switch (lines)
        {
            case 1: points += 40 * (level + 1); break;
            case 2: points += 100 * (level + 1); break;
            case 3: points += 300 * (level + 1); break;
            case 4: points += 1200 * (level + 1); break;
        }

        rows += lines;
        level = rows / 10;

        CheckHighScore();
        UpdateUI();
        GetComponent<Movement>().SetNewSpeed();
    }

    // Small reward for placing a piece
    public void addPointsForCubes()
    {
        points += 4;
        CheckHighScore();
        UpdateUI();
    }

    // 🔹 Compare & save
    void CheckHighScore()
    {
        if (points > bestScore)
        {
            bestScore = points;
            PlayerPrefs.SetInt(HIGHSCORE_KEY, bestScore);
            PlayerPrefs.Save(); // force write to disk
        }
    }

    void UpdateUI()
    {
        highscoreText.text = points.ToString();
        levelText.text = level.ToString();
        rowText.text = rows.ToString();

        if (bestScoreText != null)
            bestScoreText.text = bestScore.ToString();
    }

    // Optional: reset highscore (debug / button)
    public void ResetHighScore()
    {
        PlayerPrefs.DeleteKey(HIGHSCORE_KEY);
        bestScore = 0;
        UpdateUI();
    }
}
