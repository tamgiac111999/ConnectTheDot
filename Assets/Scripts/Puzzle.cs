using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Puzzle : MonoBehaviour
{
    public Umbrella[] broard;
    public static Puzzle instancePuzzle;
    private int _levelPuzzle;

    public int levelPuzzle
    {
        get => _levelPuzzle;
        set
        {
            _levelPuzzle = value;
            PlayerPrefs.SetInt("LevelPuzzle", _levelPuzzle);
        }
    }

    void Awake() => instancePuzzle = this;

    void Start()
    {
        for (int i = 0; i < broard.Length; i++)
        {
            broard[i].level = i + 1;
            broard[i].transform.Find("Text").GetComponent<TextMeshProUGUI>().text = broard[i].level.ToString();
            broard[i].OnRightClick.AddListener(HandleRightClick);
        }
    }
    
    void HandleRightClick(Umbrella clickedUmbrella)
    {
        levelPuzzle = clickedUmbrella.level;
        SceneManager.LoadScene(1);
    }
}
