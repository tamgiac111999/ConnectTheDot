using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Field : MonoBehaviour
{
    public Tile[] allTiles;
    public Fill[] allFills;
    public Color[] allColorConnection;
    public Color[] allColorMark;
    public Canvas canvas;
    public GameObject _true;
    public GameObject _false;
    public GameObject _above;
    public GameObject _between;
    public GameObject _below;
    public GameObject _settings;

    private bool canDrawConnection = false;
    private bool isSetting = false;
    private bool colorLabel = false;
    private int value = 0;
    private int loadedWidth;
    private int loadedHeight;
    private int levelIndex;
    private int dimensionX1;
    private int dimensionY1;
    private int dimensionX2;
    private int dimensionY2;
    private int mouseGridX;
    private int mouseGridY;
    private List<int> loadedGridLevel;
    private List<List<int>> loadedAnswerLevel;
    private RectTransform canvasRectTransform;
    private RectTransform betweenRectTransform;
    private RectTransform settingRectTransform;
    private RectTransform behindRectTransform;
    private RectTransform gridRectTransform;
    private Tile connectionTile;
    private Dictionary<int, TileData> temporary = new Dictionary<int, TileData>();
    private List<List<Tile>> listConnection = new List<List<Tile>>();
    private List<List<Tile>> answer = new List<List<Tile>>();
    private Dictionary<int, List<List<Tile>>> dictionary = new Dictionary<int, List<List<Tile>>>();

    void Awake()
    {
        canvasRectTransform = canvas.GetComponent<RectTransform>();
        betweenRectTransform = _between.GetComponent<RectTransform>();
        levelIndex = PlayerPrefs.GetInt("LevelPuzzle", 1);
        _above.transform.Find("Level/TextLevel").GetComponent<TextMeshProUGUI>().text = "Level " + levelIndex.ToString();
        float factor = canvasRectTransform.rect.width / (canvasRectTransform.rect.height / 2.0f) + 0.1f;

        if (factor > 1.2f)
        {
            factor = 1.2f;
        }

        if (PlayerPrefs.HasKey("gridLevel" + levelIndex))
        {
            SaveLoadManager.instanceManager.LoadLevelDimensions(levelIndex, out loadedWidth, out loadedHeight);
            loadedGridLevel = SaveLoadManager.instanceManager.LoadGridLevel("gridLevel", levelIndex);
            loadedAnswerLevel = SaveLoadManager.instanceManager.LoadAnswerLevel("answerLevel", levelIndex);
        }

        float newSize = betweenRectTransform.rect.width / factor;

        if (newSize * factor > canvasRectTransform.rect.height / 2.0f)
        {
            newSize = canvasRectTransform.rect.height / 2.0f / factor;
        }

        ChangeRectWidthHeight(betweenRectTransform, newSize, newSize);
        InitializeFills();
        InitializeTiles();

        if (canvasRectTransform.rect.height > canvasRectTransform.rect.width)
        {
            ChangeScale(_above.transform.Find("Back").GetComponent<RectTransform>(), factor);
            ChangeScale(_above.transform.Find("Level").GetComponent<RectTransform>(), factor);
            ChangeScale(_above.transform.Find("Setting").GetComponent<RectTransform>(), factor);
            ChangeScale(_between.transform.GetComponent<RectTransform>(), factor);
            ChangeScale(_below.transform.GetComponent<RectTransform>(), factor);
            ChangeScale(_settings.transform.GetComponent<RectTransform>(), factor);
            float aspectRatio = canvasRectTransform.rect.height / canvasRectTransform.rect.width;
            float leftRightValue = aspectRatio < 1.6f ? -25.0f : aspectRatio > 2.4f ? 25.0f : 0;

            if (leftRightValue != 0)
            {
                ChangePosition(_above.transform.Find("Back").GetComponent<RectTransform>(), -leftRightValue);
                ChangePosition(_above.transform.Find("Setting").GetComponent<RectTransform>(), leftRightValue);
            }
        }
    }

    void ChangeScale(RectTransform rectTransform, float scaleFactor)
    {
        Vector3 scale = rectTransform.localScale;
        scale.x *= scaleFactor;
        scale.y *= scaleFactor;
        rectTransform.localScale = scale;
    }

    void ChangePosition(RectTransform rectTransform, float newPosition)
    {
        Vector3 position = rectTransform.anchoredPosition;
        position.x += newPosition;
        rectTransform.anchoredPosition = position;
    }

    void InitializeFills()
    {
        allFills = new Fill[loadedWidth * loadedHeight];
        GameObject fillPrefab = Resources.Load<GameObject>("FillPrefab");
        behindRectTransform = _between.transform.Find("Behind").GetComponent<RectTransform>();
        GridLayoutGroup behindLayout = _between.transform.Find("Behind").GetComponent<GridLayoutGroup>();

        behindLayout.cellSize = new Vector2(
            (behindRectTransform.rect.width - behindLayout.spacing.x * (loadedWidth + 1)) / loadedWidth,
            (behindRectTransform.rect.height - behindLayout.spacing.y * (loadedWidth + 1)) / loadedWidth
        );

        AdjustRectTransformSize(behindRectTransform, behindLayout, loadedWidth, loadedHeight);

        for (int i = 0; i < loadedWidth * loadedHeight; i++)
        {
            GameObject fillInstance = Instantiate(fillPrefab, behindRectTransform);
            Fill fillComponent = fillInstance.GetComponent<Fill>();

            if (fillComponent != null)
            {
                allFills[i] = fillComponent;
            }
        }
    }

    void InitializeTiles()
    {
        allTiles = new Tile[loadedWidth * loadedHeight];
        GameObject tilePrefab = Resources.Load<GameObject>("TilePrefab");
        gridRectTransform = _between.transform.Find("Grid").GetComponent<RectTransform>();
        GridLayoutGroup gridLayout = _between.transform.Find("Grid").GetComponent<GridLayoutGroup>();

        gridLayout.cellSize = new Vector2(
            (gridRectTransform.rect.width - gridLayout.spacing.x * (loadedWidth + 1)) / loadedWidth,
            (gridRectTransform.rect.height - gridLayout.spacing.y * (loadedWidth + 1)) / loadedWidth
        );

        AdjustRectTransformSize(gridRectTransform, gridLayout, loadedWidth, loadedHeight);

        for (int i = 0; i < loadedWidth * loadedHeight; i++)
        {
            GameObject tileInstance = Instantiate(tilePrefab, gridRectTransform);
            SetTilePrefabProperties(tileInstance, gridLayout);
            Tile tileComponent = tileInstance.GetComponent<Tile>();

            if (tileComponent != null)
            {
                allTiles[i] = tileComponent;
            }
        }

        InitializeTileProperties();
        InitializeConnections();
    }

    void ChangeRectWidthHeight(RectTransform rectTransform, float newWidth, float newHeight)
    {
        Vector2 currentSize = rectTransform.rect.size;
        Vector2 newSize = new Vector2(newWidth, newHeight);
        Vector2 sizeDifference = newSize - currentSize;
        rectTransform.sizeDelta += sizeDifference;
    }

    void AdjustRectTransformSize(RectTransform rectTransform, GridLayoutGroup layoutGroup, int width, int height)
    {
        if (width != height)
        {
            float cellSizeChange = (width > height) ? -1 : 1;
            float newSizeDelta = rectTransform.rect.height + (layoutGroup.spacing.y + (Mathf.Abs(width - height)) * layoutGroup.cellSize.y) * cellSizeChange;
            ChangeRectWidthHeight(rectTransform, rectTransform.rect.width, newSizeDelta);
        }
    }

    void SetTilePrefabProperties(GameObject tileInstance, GridLayoutGroup gridLayout)
    {
        RectTransform pipeRectTransform = tileInstance.transform.Find("Connection/Pipe").GetComponent<RectTransform>();
        pipeRectTransform.sizeDelta = new Vector2(gridLayout.cellSize.x + 4, gridLayout.cellSize.y + 4);
        pipeRectTransform.anchoredPosition = new Vector2(0, gridLayout.cellSize.y * 0.4f + 4);
        RectTransform markRectTransform = tileInstance.transform.Find("Mark").GetComponent<RectTransform>();
        markRectTransform.sizeDelta = new Vector2(gridLayout.cellSize.x * 0.9f, gridLayout.cellSize.y * 0.9f);
        TextMeshProUGUI textMeshPro = tileInstance.transform.Find("Mark/Text").GetComponent<TextMeshProUGUI>();
        textMeshPro.fontSize = gridLayout.cellSize.x * 0.6f;
    }

    void InitializeTileProperties()
    {
        for (int i = 0; i < allTiles.Length; i++)
        {
            allTiles[i].onSelected.AddListener(onTileSelected);
            allTiles[i].cid = loadedGridLevel[i];

            if (allTiles[i].cid > 0)
            {
                allTiles[i].SetConnectionColor(allColorMark[allTiles[i].cid - 1]);
                allTiles[i].SetMarkColor(allColorMark[allTiles[i].cid - 1]);
                allTiles[i].transform.Find("Mark").gameObject.transform.Find("Text").gameObject.GetComponent<TextMeshProUGUI>().text = allTiles[i].cid.ToString();
            }
        }
    }

    void InitializeConnections()
    {
        for (int i = 0; i < loadedGridLevel.Max(); i++)
        {
            listConnection.Add(new List<Tile>());
        }

        if (loadedAnswerLevel != null && loadedAnswerLevel.Count > 0)
        {
            for (int i = 0; i < loadedAnswerLevel.Count; i++)
            {
                List<int> tileIndices = loadedAnswerLevel[i];
                List<Tile> tileList = new List<Tile>();

                foreach (int index in tileIndices)
                {
                    tileList.Add(allTiles[index]);
                }

                answer.Add(tileList);
            }
        }
    }

    void Start()
    {
        settingRectTransform = _settings.GetComponent<RectTransform>();
        Vector3[] corners = new Vector3[4];
        gridRectTransform.GetWorldCorners(corners);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, corners[0]), canvas.worldCamera, out Vector2 localCorner0);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, corners[2]), canvas.worldCamera, out Vector2 localCorner2);
        dimensionX1 = (int)localCorner0.x;
        dimensionY1 = (int)localCorner0.y;
        dimensionX2 = (int)localCorner2.x;
        dimensionY2 = (int)localCorner2.y;
        dictionary.Add(1, listConnection);
    }

    void Update()
    {
        HandleSetting();

        if (canDrawConnection)
        {
            DrawConnection();
        }
        else
        {
            ResetTiles();
        }
    }

    void HandleSetting()
    {
        if (isSetting && Input.GetMouseButtonDown(0))
        {
            if (settingRectTransform != null)
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(settingRectTransform, Input.mousePosition, canvas.worldCamera, out Vector2 localMousePosition);

                if (!settingRectTransform.rect.Contains(localMousePosition))
                {
                    _settings.SetActive(false);
                    isSetting = false;
                }
            }
        }
    }

    void DrawConnection()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, Input.mousePosition, canvas.worldCamera, out Vector2 canvasMousePosition);
        mouseGridX = (int)canvasMousePosition.x;
        mouseGridY = (int)canvasMousePosition.y;

        if (mouseGridY > dimensionY2 || mouseGridY < dimensionY1 || mouseGridX > dimensionX2 || mouseGridX < dimensionX1) return;

        Tile hitTile = GetHitTile();

        if (hitTile != null)
        {
            ProcessHitTile(hitTile);
        }
    }

    Tile GetHitTile()
    {
        foreach (Tile tile in allTiles)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(tile.GetComponent<RectTransform>(), Input.mousePosition, canvas.worldCamera))
            {
                return tile;
            }
        }

        return null;
    }

    void ProcessHitTile(Tile hitTile)
    {
        Tile firstTile = listConnection[value - 1][0];

        if (hitTile.cid > 0 && hitTile.cid != firstTile.cid) return;

        Vector2[] connectionTilePosition = GetConnectionTilePosition();

        if (!IsValidPosition_1(connectionTilePosition)) return;

        if (IsValidPosition_2(connectionTilePosition)) return;

        if ((listConnection[value - 1].FindAll(isTile => isTile.cid > 0 && isTile.coefficient == isTile.cid).Count == 2) && hitTile.coefficient != connectionTile.coefficient) return;

        UpdateTileColors(hitTile, firstTile, connectionTilePosition);

        if (hitTile.coefficient > 0 && hitTile.coefficient != connectionTile.coefficient)
        {
            ProcessTemporaryConnection(hitTile);
        }

        connectionTile = hitTile;
        connectionTile.coefficient = firstTile.cid;
        listConnection[value - 1].Add(connectionTile);
        CheckIsConnections(listConnection[value - 1]);
        ProcessTemporaryList();
        UpdateAllTileColors();

        if (allTiles.All(tile => tile.coefficient > 0))
        {
            canDrawConnection = false;
        }
    }

    Vector2[] GetConnectionTilePosition()
    {
        Vector3[] tileCorners = new Vector3[4];
        connectionTile.GetComponent<RectTransform>().GetWorldCorners(tileCorners);
        Vector2[] connectionTilePosition = new Vector2[4];

        for (int i = 0; i < 4; i++)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, tileCorners[i]), canvas.worldCamera, out connectionTilePosition[i]);
        }

        return connectionTilePosition;
    }

    bool IsValidPosition_1(Vector2[] connectionTilePosition)
    {
        return  mouseGridY > (int)connectionTilePosition[2].y ||
                mouseGridY < (int)connectionTilePosition[0].y ||
                mouseGridX > (int)connectionTilePosition[2].x ||
                mouseGridX < (int)connectionTilePosition[0].x;
    }

    bool IsValidPosition_2(Vector2[] connectionTilePosition)
    {
        return  (
                    (mouseGridX < (int)connectionTilePosition[0].x && mouseGridY > (int)connectionTilePosition[2].y) || 
                    (mouseGridX < (int)connectionTilePosition[0].x && mouseGridY < (int)connectionTilePosition[0].y) || 
                    (mouseGridX > (int)connectionTilePosition[2].x && mouseGridY > (int)connectionTilePosition[2].y) || 
                    (mouseGridX > (int)connectionTilePosition[2].x && mouseGridY < (int)connectionTilePosition[0].y)
                ) ||
                (
                    Math.Abs(mouseGridX - ((mouseGridX > (int)connectionTilePosition[2].x) ? (int)connectionTilePosition[2].x : (int)connectionTilePosition[0].x)) > ((dimensionX2 * 2 - (loadedWidth + 1) * 5) / loadedWidth + 5) ||
                    Math.Abs(mouseGridY - ((mouseGridY > (int)connectionTilePosition[2].y) ? (int)connectionTilePosition[2].y : (int)connectionTilePosition[0].y)) > ((dimensionX2 * 2 - (loadedWidth + 1) * 5) / loadedWidth + 5)
                );
    }

    void UpdateTileColors(Tile hitTile, Tile firstTile, Vector2[] connectionTilePosition)
    {
        allFills[Array.IndexOf(allTiles, hitTile)].SetFillColor(allColorConnection[firstTile.cid]);
        hitTile.SetConnectionColor(connectionTile.ConnectionColor);

        connectionTile.ConnectionToSide(
            mouseGridY > (int)connectionTilePosition[2].y,
            mouseGridX > (int)connectionTilePosition[2].x,
            mouseGridY < (int)connectionTilePosition[0].y,
            mouseGridX < (int)connectionTilePosition[0].x
        );
    }

    void ProcessTemporaryConnection(Tile hitTile)
    {
        var tileData = new TileData
        {
            SomeCid = listConnection[hitTile.coefficient - 1][0].cid,
            SomeConnectionTile = hitTile,
            SomeConnection = new List<Tile>(listConnection[hitTile.coefficient - 1])
        };

        int nextKey = temporary.Keys.Count > 0 ? temporary.Keys.Max() + 1 : 1;
        temporary.Add(nextKey, tileData);
        int index = listConnection[hitTile.coefficient - 1].IndexOf(hitTile);
        listConnection[hitTile.coefficient - 1].Add(listConnection[hitTile.coefficient - 1][index]);
        CheckIsConnections(listConnection[hitTile.coefficient - 1]);
        listConnection[hitTile.coefficient - 1].Add(listConnection[hitTile.coefficient - 1][listConnection[hitTile.coefficient - 1].Count - 2]);
        CheckIsConnections(listConnection[hitTile.coefficient - 1]);
        UpdateAllTileColors();
    }

    void ProcessTemporaryList()
    {
        if (temporary.Count > 0)
        {
            int index = temporary.Count;

            for (int i = 0; i < index; i++)
            {
                int lastKey = temporary.Keys.Max();
                Tile lastConnectionTile = temporary[lastKey].SomeConnectionTile;
                bool foundMatch = listConnection[value - 1].Contains(lastConnectionTile);

                if (foundMatch)
                {
                    break;
                }
                else
                {
                    RestoreTemporaryConnection(lastKey);
                }
            }
        }
    }

    void SetTileConnections(List<Tile> connection)
    {
        if (connection.Count <= 1) return;
        
        connection[0].coefficient = connection[0].cid;

        for (int i = 0; i < connection.Count - 1; i++)
        {
            Tile tile = connection[i];
            Tile nextTile = connection[i + 1];
            nextTile.coefficient = tile.coefficient;
            nextTile.SetConnectionColor(tile.ConnectionColor);
            Vector2[] tilePosition = GetTilePosition(tile);
            Vector2[] nextTilePosition = GetTilePosition(nextTile);

            tile.ConnectionToSide(
                IsAbove(nextTilePosition, tilePosition), 
                IsRight(nextTilePosition, tilePosition), 
                IsBelow(nextTilePosition, tilePosition),
                IsLeft(nextTilePosition, tilePosition)
            );
        }
    }

    bool IsAbove(Vector2[] nextTilePosition, Vector2[] tilePosition) => ((int)nextTilePosition[2].y + (int)nextTilePosition[0].y) / 2 > (int)tilePosition[2].y;

    bool IsRight(Vector2[] nextTilePosition, Vector2[] tilePosition) => ((int)nextTilePosition[2].x + (int)nextTilePosition[0].x) / 2 > (int)tilePosition[2].x;

    bool IsBelow(Vector2[] nextTilePosition, Vector2[] tilePosition) => ((int)nextTilePosition[2].y + (int)nextTilePosition[0].y) / 2 < (int)tilePosition[0].y;

    bool IsLeft(Vector2[] nextTilePosition, Vector2[] tilePosition) => ((int)nextTilePosition[2].x + (int)nextTilePosition[0].x) / 2 < (int)tilePosition[0].x;

    void RestoreTemporaryConnection(int lastKey)
    {
        listConnection[temporary[lastKey].SomeCid - 1].Clear();
        listConnection[temporary[lastKey].SomeCid - 1].AddRange(temporary[lastKey].SomeConnection);
        ResetConnectionTiles(temporary[lastKey].SomeConnection);
        SetTileConnections(temporary[lastKey].SomeConnection);
        temporary.Remove(lastKey);
    }

    void ResetConnectionTiles(List<Tile> connection)
    {
        foreach (Tile tile in connection)
        {
            tile.coefficient = 0;
            tile.ResetConnection();
        }
    }

    Vector2[] GetTilePosition(Tile tile)
    {
        Vector3[] tileCorners = new Vector3[4];
        tile.GetComponent<RectTransform>().GetWorldCorners(tileCorners);
        Vector2[] tilePosition = new Vector2[4];

        for (int i = 0; i < 4; i++)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, RectTransformUtility.WorldToScreenPoint(canvas.worldCamera, tileCorners[i]), canvas.worldCamera, out tilePosition[i]);
        }

        return tilePosition;
    }

    void ResetTiles()
    {
        foreach (List<Tile> connection in listConnection)
        {
            if (connection.Count == 1)
            {
                foreach (Tile tile in allTiles)
                {
                    if (connection[0] == tile)
                    {
                        tile.coefficient = 0;
                    }
                }
            }
        }

        temporary.Clear();
        UpdateAllTileColors();

        if (allTiles.All(tile => tile.coefficient > 0))
        {
            StartCoroutine(WaitAndDoSomething(1.0f));
        }
    }

    void UpdateAllTileColors()
    {
        for (int i = 0; i < allTiles.Length; i++)
        {
            allFills[i].SetFillColor(allColorConnection[allTiles[i].coefficient]);
        }
    }

    IEnumerator WaitAndDoSomething(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        gameObject.SetActive(false);
        Congratulations.instanceCongratulations.youBeatLevel.text = "Level " + levelIndex.ToString();
        Congratulations.instanceCongratulations.nextLevel.text = "Level " + (levelIndex + 1).ToString();
    }

    void CheckIsConnections(List<Tile> connections)
    {
        bool foundPair;

        do
        {
            foundPair = false;

            for (int i = 0; i < connections.Count - 1; i++)
            {
                for (int j = i + 1; j < connections.Count; j++)
                {
                    if (connections[i] == connections[j])
                    {
                        connections.GetRange(i, j - i + 1).ForEach(tile =>
                        {
                            tile.ResetConnection();

                            if (connections.IndexOf(tile) + 1 < j) connections[connections.IndexOf(tile) + 1].coefficient = 0;
                        });

                        connections.RemoveRange(i + 1, j - i);
                        foundPair = true;
                        break;
                    }
                }

                if (foundPair) break;
            }
        } while (foundPair);
    }

    void onTileSelected(Tile _tile)
    {
        if (!_tile.isSelected) 
        {
            List<List<Tile>> newList = listConnection.Select(list => new List<Tile>(list)).ToList();
            int nextKey = dictionary.Keys.Count > 0 ? dictionary.Keys.Max() + 1 : 1;
            dictionary.Add(nextKey, newList);
            canDrawConnection = false;
            return;
        }

        if (_tile.coefficient == 0 && _tile.cid == 0) return;

        value = Mathf.Max(_tile.coefficient, _tile.cid);
        List<Tile> _connections = listConnection[value - 1];
        int index = Array.IndexOf(allTiles, _tile);

        if (_tile.cid > 0)
        {
            if (_connections.Count > 0)
            {
                if (_tile != _connections[0])
                {
                    _connections[0].coefficient = 0;
                    _connections.Insert(0, _tile);
                }

                _connections.Add(_connections[0]);
                CheckIsConnections(_connections);
                connectionTile = _tile;
                connectionTile.coefficient = _connections[0].cid;
            }
            else
            {
                connectionTile = _tile;
                connectionTile.coefficient = connectionTile.cid;
            }
        }
        else
        {
            connectionTile = _tile;
            connectionTile.coefficient = _connections[0].cid;
        }

        _connections.Add(connectionTile);
        CheckIsConnections(_connections);
        allFills[index].SetFillColor(allColorConnection[connectionTile.coefficient]);

        foreach (Tile tile in allTiles)
        {
            allFills[Array.IndexOf(allTiles, tile)].SetFillColor(allColorConnection[tile.coefficient]);
        }

        canDrawConnection = true;
    }

    public void Undo()
    {
        if (dictionary.Count <= 1) return;

        for (int i = 0; i < listConnection.Count; i++)
        {
            listConnection[i].Clear();
            listConnection[i].AddRange(dictionary[dictionary.Count - 1][i]);
        }
        
        foreach (Tile tile in allTiles)
        {
            tile.coefficient = 0;
            tile.ResetConnection();
        }

        foreach (var connection in listConnection)
        {
            SetTileConnections(connection);
        }

        UpdateAllTileColors();
        dictionary.Remove(dictionary.Count);
    }

    public void Hint()
    {
        for (int i = 0; i < listConnection.Count; i++)
        {
            if (listConnection[i].Count != answer[i].Count)
            {
                ResetConnectionTiles(listConnection[i]);
                listConnection[i].Clear();
                listConnection[i].AddRange(answer[i]);

                for (int j = 0; j < answer[i].Count - 1; j++)
                {
                    if (answer[i][j + 1].coefficient > 0 && answer[i][j + 1].coefficient != answer[i][j].coefficient)
                    {
                        ProcessTemporaryConnection(answer[i][j + 1]);
                    }
                }

                SetTileConnections(listConnection[i]);
                break;
            }
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void BackGame()
    {
        SceneManager.LoadScene(0);
    }

    public void SettingGame()
    {
        _settings.SetActive(true);
        isSetting = true;
    }

    public void QuitSetting()
    {
        _settings.SetActive(false);
        isSetting = false;
    }

    public void ColorLabels()
    {
        colorLabel = !colorLabel;
        
        foreach (Tile tile in allTiles)
        {
            if (tile.cid > 0)
            {
                tile.SetMarkText(colorLabel);
            }
        }

        _settings.transform.Find("ColorLabels/Button/True").gameObject.SetActive(colorLabel);
        _settings.transform.Find("ColorLabels/Button/False").gameObject.SetActive(!colorLabel);
    }
}
