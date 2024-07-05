using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System;

public class Puzzle : MonoBehaviour
{
    public Umbrella[] allUmbrellas;
    public static Puzzle instancePuzzle;
    public Canvas canvas;
    public GameObject _outline;
    public GameObject _puzzleText;
    public GameObject _board;

    private int _levelPuzzle;
    private RectTransform canvasRectTransform;
    private RectTransform boardRectTransform;

    public int levelPuzzle
    {
        get => _levelPuzzle;
        set
        {
            _levelPuzzle = value;
            PlayerPrefs.SetInt("LevelPuzzle", _levelPuzzle);
        }
    }

    void Awake()
    {
        instancePuzzle = this;
        allUmbrellas = new Umbrella[21];
        canvasRectTransform = canvas.GetComponent<RectTransform>();
        boardRectTransform = _board.GetComponent<RectTransform>();
        float factor = canvasRectTransform.rect.width / (canvasRectTransform.rect.height / 2.0f) + 0.1f;
        AdjustLayout(factor);
        CreateUmbrellas(factor);
    }

    void AdjustLayout(float index)
    {
        GridLayoutGroup boardLayout = _board.GetComponent<GridLayoutGroup>();

        if (canvasRectTransform.rect.height > canvasRectTransform.rect.width)
        {
            ChangeScale(_puzzleText.transform.Find("Text").GetComponent<RectTransform>(), index);

            float aspectRatio = canvasRectTransform.rect.height / canvasRectTransform.rect.width;
            float leftRightValue = aspectRatio < 1.6f ? -50.0f : aspectRatio > 2.4f ? 50.0f : 0;

            if (leftRightValue != 0)
            {
                ChangeLeftRight(_outline.GetComponent<RectTransform>(), leftRightValue / 2);
                ChangeLeftRight(_puzzleText.GetComponent<RectTransform>(), leftRightValue);
                ChangeLeftRight(_board.GetComponent<RectTransform>(), leftRightValue);
            }
        }
    }

    void CreateUmbrellas(float index)
    {
        GameObject umbrellaPrefab = Resources.Load<GameObject>("UmbrellaPrefab");

        for (int i = 0; i < allUmbrellas.Length; i++)
        {
            GameObject umbrellaInstance = Instantiate(umbrellaPrefab, boardRectTransform);
            Umbrella umbrellaComponent = umbrellaInstance.GetComponent<Umbrella>();

            if (umbrellaComponent != null)
            {
                allUmbrellas[i] = umbrellaComponent;
                allUmbrellas[i].level = i + 1;
                allUmbrellas[i].transform.Find("Text").GetComponent<TextMeshProUGUI>().text = allUmbrellas[i].level.ToString();
                allUmbrellas[i].OnRightClick.AddListener(HandleRightClick);
            }
        }
    }

    void ChangeScale(RectTransform rectTransform, float newScale)
    {
        Vector3 scale = rectTransform.localScale;
        scale.x *= newScale;
        scale.y *= newScale;
        rectTransform.localScale = scale;
    }

    void ChangeLeftRight(RectTransform rectTransform, float additionalValue)
    {
        Vector2 sizeDelta = rectTransform.sizeDelta;
        rectTransform.sizeDelta = new Vector2(sizeDelta.x + additionalValue * 2.0f, sizeDelta.y);
    }
    
    void HandleRightClick(Umbrella clickedUmbrella)
    {
        levelPuzzle = clickedUmbrella.level;
        SceneManager.LoadScene(1);
    }
}
