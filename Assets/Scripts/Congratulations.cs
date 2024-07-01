using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Congratulations : MonoBehaviour
{
    public static Congratulations instanceCongratulations;

    public TextMeshProUGUI youBeatLevel;
    public TextMeshProUGUI nextLevel;

    void Awake()
    {
        instanceCongratulations = this;
    }

    public void NextLevel()
    {
        Puzzle.instancePuzzle.levelPuzzle = PlayerPrefs.GetInt("LevelPuzzle", 1) + 1;
        SceneManager.LoadScene(1);
    }
}
