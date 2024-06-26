using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class Field : MonoBehaviour
{
    public Tile[] allTiles;
    public Canvas canvas;
    public RectTransform objectRectTransform;
    public TextMeshProUGUI textLevel;

    public Fill[] allFills;
    public Color[] allColorConnection;
    public Color[] allColorMark;

    public GameObject _true;
    public GameObject _false;
    public GameObject setting;
    public GameObject _behind;
    public GameObject _grid;

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
    private RectTransform settingRectTransform;

    private Tile connectionTile;
    private Dictionary<int, TileData> temporary = new Dictionary<int, TileData>();

    private List<List<Tile>> listConnection = new List<List<Tile>>();
    private List<List<Tile>> answer = new List<List<Tile>>();

    private Dictionary<int, List<List<Tile>>> dictionary = new Dictionary<int, List<List<Tile>>>();

    void Awake()
    {
        levelIndex = PlayerPrefs.GetInt("LevelPuzzle", 1);
        textLevel.text = "Level " + levelIndex.ToString();

        if (PlayerPrefs.HasKey("gridLevel" + levelIndex))
        {
            SaveLoadManager.instanceManager.LoadLevelDimensions(levelIndex, out loadedWidth, out loadedHeight);
            loadedGridLevel = SaveLoadManager.instanceManager.LoadGridLevel("gridLevel", levelIndex);
            loadedAnswerLevel = SaveLoadManager.instanceManager.LoadAnswerLevel("answerLevel", levelIndex);
        }

        allFills = new Fill[loadedWidth * loadedHeight];
        GameObject fillPrefab = Resources.Load<GameObject>("FillPrefab");
        RectTransform behindRectTransform = _behind.GetComponent<RectTransform>();
        GridLayoutGroup behindLayout = _behind.GetComponent<GridLayoutGroup>();
        behindLayout.cellSize = new Vector2((behindRectTransform.rect.width - behindLayout.spacing.x * (loadedWidth + 1)) / loadedWidth, (behindRectTransform.rect.height - behindLayout.spacing.y * (loadedHeight + 1)) / loadedHeight);

        if (loadedWidth != loadedHeight)
        {
            if (loadedWidth > loadedWidth)
            {
                behindRectTransform.sizeDelta = new Vector2(behindRectTransform.rect.width, behindRectTransform.rect.height - (behindLayout.spacing.y + loadedHeight));
            }
            else
            {
                behindRectTransform.sizeDelta = new Vector2(behindRectTransform.rect.width - (behindLayout.spacing.x + loadedWidth), behindRectTransform.rect.height);
            }
        }

        for (int i = 0; i < loadedWidth * loadedHeight; i++)
        {
            GameObject fillInstance = Instantiate(fillPrefab, behindRectTransform);
            Fill fillComponent = fillInstance.GetComponent<Fill>();

            if (fillComponent != null)
            {
                allFills[i] = fillComponent;
            }
        }

        allTiles = new Tile[loadedWidth * loadedHeight];
        GameObject tilePrefab = Resources.Load<GameObject>("TilePrefab");
        RectTransform gridRectTransform = _grid.GetComponent<RectTransform>();
        GridLayoutGroup gridLayout = _grid.GetComponent<GridLayoutGroup>();
        gridLayout.cellSize = new Vector2((gridRectTransform.rect.width - gridLayout.spacing.x * (loadedWidth + 1)) / loadedWidth, (gridRectTransform.rect.height - gridLayout.spacing.y * (loadedHeight + 1)) / loadedHeight);
        if (loadedWidth != loadedHeight)
        {
            if (loadedWidth > loadedWidth)
            {
                gridRectTransform.sizeDelta = new Vector2(gridRectTransform.rect.width, gridRectTransform.rect.height - (gridLayout.spacing.y + loadedHeight));
            }
            else
            {
                gridRectTransform.sizeDelta = new Vector2(gridRectTransform.rect.width - (gridLayout.spacing.x + loadedWidth), gridRectTransform.rect.height);
            }
        }

        for (int i = 0; i < loadedWidth * loadedHeight; i++)
        {
            GameObject tileInstance = Instantiate(tilePrefab, gridRectTransform);

            RectTransform pipeRectTransform = tileInstance.transform.Find("Connection/Pipe").GetComponent<RectTransform>();
            pipeRectTransform.sizeDelta = new Vector2(gridLayout.cellSize.x + 5f, gridLayout.cellSize.y + 5f);
            pipeRectTransform.anchoredPosition = new Vector2(0, gridLayout.cellSize.y * 0.4f + 5f);

            RectTransform markRectTransform = tileInstance.transform.Find("Mark").GetComponent<RectTransform>();
            markRectTransform.sizeDelta = new Vector2(gridLayout.cellSize.x * 0.9f, gridLayout.cellSize.y * 0.9f);
            
            TextMeshProUGUI textMeshPro = tileInstance.transform.Find("Mark/Text").GetComponent<TextMeshProUGUI>();
            textMeshPro.fontSize = gridLayout.cellSize.x * 0.6f;

            Tile tileComponent = tileInstance.GetComponent<Tile>();

            if (tileComponent != null)
            {
                allTiles[i] = tileComponent;
            }
        }

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
        canvasRectTransform = canvas.GetComponent<RectTransform>();
        settingRectTransform = setting.GetComponent<RectTransform>();
        Vector3[] corners = new Vector3[4];
        objectRectTransform.GetWorldCorners(corners);
        dimensionX1 = (int)corners[0].x;
        dimensionY1 = (int)corners[0].y;
        dimensionX2 = (int)corners[2].x;
        dimensionY2 = (int)corners[2].y;

        dictionary.Add(1, listConnection);
    }

    void Update()
    {
        if (canDrawConnection)
        {
            UpdateMouseGridPosition();
            
            if (IsMouseOutsideGrid()) return;

            Tile hitTile = null;
            int index = 0;

            foreach (Tile tile in allTiles)
            {
                if (RectTransformUtility.RectangleContainsScreenPoint(tile.GetComponent<RectTransform>(), Input.mousePosition, canvas.worldCamera))
                {
                    hitTile = tile;
                    break;
                }

                index++;
            }

            if (hitTile != null)
            {
                ContinueSection(listConnection[value - 1], hitTile, index);
            }
        }
        else
        {
            for (int i = 0; i < listConnection.Count; i++)
            {
                CheckIsCoefficient(listConnection[i]);
            }

            temporary.Clear();
            CheckIsFillColor();

            if (CheckIfTilesMatch())
            {
                StartCoroutine(WaitAndDoSomething(1.0f));
            }
        }

        if (isSetting)
        {
            OutSetting();
        }
    }

    void UpdateMouseGridPosition()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, Input.mousePosition, canvas.worldCamera, out Vector2 canvasMousePosition);
        Vector3 canvasPosition = canvasRectTransform.anchoredPosition;
        mouseGridX = (int)(canvasPosition.x + canvasMousePosition.x);
        mouseGridY = (int)(canvasPosition.y + canvasMousePosition.y);
    }

    public void Undo()
    {
        if (dictionary.Count <= 1) return;

        List<List<Tile>> _replica = dictionary[dictionary.Count - 1];

        for (int i = 0; i < listConnection.Count; i++)
        {
            listConnection[i].Clear();
        }

        for (int i = 0; i < listConnection.Count; i++)
        {
            listConnection[i].AddRange(_replica[i]);
        }
        
        CheckIsGrid();
        dictionary.Remove(dictionary.Count);
    }

    public void CheckIsGrid()
    {
        for (int i = 0; i < allTiles.Length; i++)
        {
            allTiles[i].coefficient = 0;
            allTiles[i].ResetConnection();
        }

        for (int i = 0; i < listConnection.Count; i++)
        {
            UndoConnections(listConnection[i]);
        }

        CheckIsFillColor();
    }

    public void UndoConnections(List<Tile> _connections)
    {
        if (_connections.Count <= 1) return;

        _connections[0].coefficient = _connections[0].cid;

        for (int i = 0; i < _connections.Count - 1; i++)
        {
            Tile tile = _connections[i];
            Tile nextTile = _connections[i + 1];
            nextTile.coefficient = tile.coefficient;
            nextTile.SetConnectionColor(tile.ConnectionColor);
            Vector3[] tilePosition = FindTileCoordinates(tile);
            Vector3[] nextTilePosition = FindTileCoordinates(nextTile);

            tile.ConnectionToSide
            (
                System.Math.Abs(((int)nextTilePosition[2].y + (int)nextTilePosition[0].y) / 2) > (int)tilePosition[2].y,
                System.Math.Abs(((int)nextTilePosition[2].x + (int)nextTilePosition[0].x) / 2) > (int)tilePosition[2].x,
                System.Math.Abs(((int)nextTilePosition[2].y + (int)nextTilePosition[0].y) / 2) < (int)tilePosition[0].y,
                System.Math.Abs(((int)nextTilePosition[2].x + (int)nextTilePosition[0].x) / 2) < (int)tilePosition[0].x
            );
        }
    }

    public void CheckIsCoefficient(List<Tile> _connections)
    {
        if (_connections.Count == 1)
        {
            for (int i = 0; i < allTiles.Length; i++)
            {
                if (_connections[0] == allTiles[i])
                {
                    allTiles[i].coefficient = 0;
                }
            }
        }
    }

    public void ContinueSection(List<Tile> _connections, Tile _tile, int index)
    {
        Tile firstTile = _connections[0];

        if (_tile.cid > 0 && _tile.cid != firstTile.cid) return;

        Vector3[] connectionTilePosition = FindTileCoordinates(connectionTile);
        
        if (!CheckMouseOutsideTile(mouseGridX, mouseGridY, connectionTilePosition)) return;

        if (CheckIsShiftDiagonal(mouseGridX, mouseGridY, connectionTilePosition) || CheckIsShiftNotOnNext(mouseGridX, mouseGridY, connectionTilePosition)) return;

        if (CheckIsTurning(_connections) && _tile.coefficient != connectionTile.coefficient) return;

        allFills[index].SetFillColor(allColorConnection[firstTile.cid]);
        _tile.SetConnectionColor(connectionTile.ConnectionColor);

        connectionTile.ConnectionToSide
        (
            mouseGridY > (int)connectionTilePosition[2].y,
            mouseGridX > (int)connectionTilePosition[2].x,
            mouseGridY < (int)connectionTilePosition[0].y,
            mouseGridX < (int)connectionTilePosition[0].x
        );

        Intersecting(_tile, connectionTile);

        connectionTile = _tile;
        connectionTile.coefficient = firstTile.cid;
        _connections.Add(connectionTile);
        CheckIsConnections(_connections);

        ConnectionTemporary(_connections);

        CheckIsFillColor();

        if (CheckIfTilesMatch())
        {
            canDrawConnection = false;
        }
    }

    IEnumerator WaitAndDoSomething(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        gameObject.SetActive(false);

        Congratulations.instanceCongratulations.youBeatLevel.text = "Level " + levelIndex.ToString();
        Congratulations.instanceCongratulations.nextLevel.text = "Level " + (levelIndex + 1).ToString();
    }

    void Intersecting(Tile _tile, Tile _hoverTile)
    {
        if (_tile.coefficient > 0 && _tile.coefficient != _hoverTile.coefficient)
        {
            CheckIntersecting(listConnection[_tile.coefficient - 1], _tile);
        }
    }

    public void Hint()
    {
        List<Tile> replica = new List<Tile>();

        for (int i = 0; i < listConnection.Count; i++)
        {
            if (listConnection[i].Count != answer[i].Count)
            {
                foreach (Tile hitTile in listConnection[i])
                {
                    hitTile.coefficient = 0;
                    hitTile.ResetConnection();
                }
                
                listConnection[i].Clear();
                listConnection[i].AddRange(answer[i]);
                replica = answer[i];

                for (int j = 0; j < answer[i].Count - 1; j++)
                {
                    Intersecting(replica[j + 1], replica[j]);
                }

                UndoConnections(listConnection[i]);

                break;
            }
        }
    }

    public void ConnectionTemporary(List<Tile> _connections)
    {
        if (temporary.Count == 0) return;

        int index = temporary.Count;

        for (int i = 0; i < index; i++)
        {
            int lastKey = temporary.Keys.Max();
            Tile lastConnectionTile = temporary[lastKey].SomeConnectionTile;

            bool foundMatch = false;

            foreach (Tile tile in _connections)
            {
                if (tile == lastConnectionTile)
                {
                    foundMatch = true;
                    break;
                }
            }

            if (foundMatch)
            {
                return;
            }
            else
            {
                listConnection[temporary[lastKey].SomeCid - 1].Clear();
                listConnection[temporary[lastKey].SomeCid - 1].AddRange(temporary[lastKey].SomeConnection);

                foreach (Tile tile in temporary[lastKey].SomeConnection)
                {
                    tile.coefficient = 0;
                    tile.ResetConnection();
                }

                UndoConnections(temporary[lastKey].SomeConnection);
                temporary.Remove(lastKey);
            }
        }
    }

    public void CheckIntersecting(List<Tile> _connections, Tile _tile)
    {
        var tileData = new TileData
        {
            SomeCid = _connections[0].cid,
            SomeConnectionTile = _tile,
            SomeConnection = new List<Tile>(_connections)
        };

        int nextKey = temporary.Keys.Count > 0 ? temporary.Keys.Max() + 1 : 1;
        temporary.Add(nextKey, tileData);

        int index = _connections.IndexOf(_tile);
        _connections.Add(_connections[index]);
        CheckIsConnections(_connections);
        _connections.Add(_connections[_connections.Count - 2]);
        CheckIsConnections(_connections);
        CheckIsFillColor();
    }

    public bool CheckIfTilesMatch()
    {
        return allTiles.All((tile) => tile.coefficient > 0);
    }

    public bool CheckIsTurning(List<Tile> _connections)
    {
        return _connections.FindAll((tile) => tile.cid > 0 && tile.coefficient == tile.cid).Count == 2;
    }

    bool IsMouseOutsideGrid()
    {
        return mouseGridY > dimensionY2 || mouseGridY < dimensionY1 || mouseGridX > dimensionX2 || mouseGridX < dimensionX1;
    }

    public bool CheckIsShiftNotOnNext(int gridX, int gridY, Vector3[] tileCorners)
    {
        return  System.Math.Abs(gridX - (gridX > (int)tileCorners[2].x ? (int)tileCorners[2].x : (int)tileCorners[0].x)) > 101 || 
                System.Math.Abs(gridY - (gridY > (int)tileCorners[2].y ? (int)tileCorners[2].y : (int)tileCorners[0].y)) > 101;
    }

    public bool CheckIsShiftDiagonal(int gridX, int gridY, Vector3[] tileCorners)
    {
        return (gridX < (int)tileCorners[0].x && gridY > (int)tileCorners[2].y) || (gridX < (int)tileCorners[0].x && gridY < (int)tileCorners[0].y) || (gridX > (int)tileCorners[2].x && gridY > (int)tileCorners[2].y) || (gridX > (int)tileCorners[2].x && gridY < (int)tileCorners[0].y);
    }
    
    public void CheckIsConnections(List<Tile> _connections)
    {
        bool foundPair = true;

        while (foundPair)
        {
            foundPair = false;

            for (int i = 0; i < _connections.Count; i++)
            {
                for (int j = i + 1; j < _connections.Count; j++)
                {
                    if (_connections[i] == _connections[j])
                    {
                        for (int k = i; k <= j; k++)
                        {
                            _connections[k].ResetConnection();
                            
                            if (k + 1 < j)
                            {
                                _connections[k + 1].coefficient = 0;
                            }
                        }
                        
                        _connections.RemoveRange(i + 1, j - i);
                        foundPair = true;
                        break;
                    }
                }

                if (foundPair) break;
            }
        }
    }

    public void CheckIsFillColor()
    {
        for (int i = 0; i < allTiles.Length; i++)
        {
            allFills[i].SetFillColor(allColorConnection[allTiles[i].coefficient]);
        }
    }

    void onTileSelected(Tile _tile)
    {
        if (_tile.isSelected == true)
        {
            if (_tile.coefficient > 0 || _tile.cid > 0)
            {
                value = (_tile.coefficient > _tile.cid) ? _tile.coefficient : _tile.cid;
                SelectedTile(listConnection[value - 1], _tile);
            }
        }
        else
        {
            List<List<Tile>> newList = new List<List<Tile>>();

            for (int i = 0; i < listConnection.Count; i++)
            {
                newList.Add(new List<Tile>(listConnection[i]));
            }

            int nextKey = dictionary.Keys.Count > 0 ? dictionary.Keys.Max() + 1 : 1;
            dictionary.Add(nextKey, newList);
            canDrawConnection = false;
        }
    }

    public void SelectedTile(List<Tile> _connections, Tile _tile)
    {
        int index = Array.IndexOf(allTiles, _tile);

        if (_tile.cid > 0)
        {
            if (_connections.Count > 0)
            {
                if (_tile != _connections[0])
                {
                    _connections[0].coefficient = 0;
                    _connections.Add(_connections[0]);
                    _connections.Insert(0, _tile);
                }
                else
                {
                    _connections.Add(_connections[0]);
                }

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
        CheckIsFillColor();
        canDrawConnection = true;
    }

    public Vector3[] FindTileCoordinates(Tile _tileObject)
    {
        var tileCorners = new Vector3[4];
        _tileObject.GetComponent<RectTransform>().GetWorldCorners(tileCorners);
        return tileCorners;
    }

    public bool CheckMouseOutsideTile(int gridX, int gridY, Vector3[] tileCorners)
    {
        return gridY > (int)tileCorners[2].y || gridY < (int)tileCorners[0].y || gridX > (int)tileCorners[2].x || gridX < (int)tileCorners[0].x;
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
        setting.SetActive(true);
        isSetting = true;
    }

    public void QuitSetting()
    {
        setting.SetActive(false);
        isSetting = false;
    }

    public void OutSetting()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (IsPointerOutsideRect())
            {
                setting.SetActive(false);
                isSetting = false;
            }
        }
    }

    private bool IsPointerOutsideRect()
    {
        if (settingRectTransform == null) return false;

        Vector2 localMousePosition = settingRectTransform.InverseTransformPoint(Input.mousePosition);
        return !settingRectTransform.rect.Contains(localMousePosition);
    }

    public void ColorLabels()
    {
        if(colorLabel)
        {
            foreach (Tile tile in allTiles)
            {
                if (tile.cid > 0)
                {
                    tile.ResetMarkText();
                }
            }

            colorLabel = false;
            _true.SetActive(false);
            _false.SetActive(true);
        }
        else
        {
            foreach (Tile tile in allTiles)
            {
                if (tile.cid > 0)
                {
                    tile.SetMarkText();
                }
            }
            
            colorLabel = true;
            _false.SetActive(false);
            _true.SetActive(true);
        }
    }
}
