using System.Collections.Generic;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager instanceManager;

    void Awake()
    {
        if (instanceManager == null)
        {
            instanceManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveGridLevel(List<int> gridLevel, string textGridLevel, int levelPuzzle)
    {
        PlayerPrefs.SetString(textGridLevel + levelPuzzle.ToString(), JsonUtility.ToJson(new SerializableList<int>(gridLevel)));
        PlayerPrefs.Save();
    }

    public List<int> LoadGridLevel(string textGridLevel, int levelPuzzle)
    {
        return JsonUtility.FromJson<SerializableList<int>>(PlayerPrefs.GetString(textGridLevel + levelPuzzle.ToString()))?.ToList() ?? new List<int>();
    }

    public void SaveAnswerLevel(List<List<int>> answerLevel, string textAnswerLevel, int levelPuzzle)
    {
        PlayerPrefs.SetString(textAnswerLevel + levelPuzzle.ToString(), JsonUtility.ToJson(new SerializableListOfLists<int>(answerLevel)));
        PlayerPrefs.Save();
    }

    public List<List<int>> LoadAnswerLevel(string textAnswerLevel, int levelPuzzle)
    {
        return JsonUtility.FromJson<SerializableListOfLists<int>>(PlayerPrefs.GetString(textAnswerLevel + levelPuzzle.ToString()))?.ToList() ?? new List<List<int>>();
    }

    public void SaveLevelDimensions(int levelIndex, int width, int height)
    {
        PlayerPrefs.SetInt("widthLevel" + levelIndex.ToString(), width);
        PlayerPrefs.SetInt("heightLevel" + levelIndex.ToString(), height);
        PlayerPrefs.Save();
    }

    public void LoadLevelDimensions(int levelIndex, out int width, out int height)
    {
        width = PlayerPrefs.GetInt("widthLevel" + levelIndex.ToString(), 5);
        height = PlayerPrefs.GetInt("heightLevel" + levelIndex.ToString(), 5);
    }

    [System.Serializable]
    private class SerializableListOfLists<T>
    {
        public List<SerializableList<T>> items;

        public SerializableListOfLists(List<List<T>> listOfLists)
        {
            items = new List<SerializableList<T>>();

            foreach (var list in listOfLists)
            {
                items.Add(new SerializableList<T>(list));
            }
        }

        public List<List<T>> ToList()
        {
            List<List<T>> listOfLists = new List<List<T>>();

            foreach (var serializableList in items)
            {
                listOfLists.Add(serializableList.ToList());
            }

            return listOfLists;
        }
    }

    [System.Serializable]
    private class SerializableList<T>
    {
        public List<T> items;

        public SerializableList(List<T> list)
        {
            items = list;
        }

        public List<T> ToList()
        {
            return items;
        }
    }
}
