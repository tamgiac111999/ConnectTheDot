using System.Collections;
using System.Collections.Generic;
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
        get { return _levelPuzzle; }
        set
        {
            _levelPuzzle = value;
            PlayerPrefs.SetInt("LevelPuzzle", _levelPuzzle);
        }
    }

    void Awake()
    {
        instancePuzzle = this;
    }

    void Start()
    {
        int index = 1;
        
        foreach (var umbrella in broard)
        {
            umbrella.level = index;
            umbrella.transform.Find("Text").gameObject.GetComponent<TextMeshProUGUI>().text = umbrella.level.ToString();
            index++;
        }

        foreach (var umbrella in broard)
        {
            umbrella.OnRightClick.AddListener(clickedUmbrella => HandleRightClick(clickedUmbrella));
        }
    }
    
    void HandleRightClick(Umbrella clickedUmbrella)
    {
        levelPuzzle = clickedUmbrella.level;
        SceneManager.LoadScene(1);
    }
}
