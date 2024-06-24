using System.Collections;
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
        string json = JsonUtility.ToJson(new SerializableList<int>(gridLevel));
        PlayerPrefs.SetString(textGridLevel + levelPuzzle.ToString(), json);
        PlayerPrefs.Save();
    }

    public List<int> LoadGridLevel(string textGridLevel, int levelPuzzle)
    {
        string json = PlayerPrefs.GetString(textGridLevel + levelPuzzle.ToString());
        SerializableList<int> serializedList = JsonUtility.FromJson<SerializableList<int>>(json);

        if (serializedList != null)
        {
            return serializedList.ToList();
        }

        return new List<int>();
    }

    public void SaveAnswerLevel(List<List<int>> answerLevel, string textAnswerLevel, int levelPuzzle)
    {
        string json = JsonUtility.ToJson(new SerializableListOfLists<int>(answerLevel));
        PlayerPrefs.SetString(textAnswerLevel + levelPuzzle.ToString(), json);
        PlayerPrefs.Save();
    }

    public List<List<int>> LoadAnswerLevel(string textAnswerLevel, int levelPuzzle)
    {
        string json = PlayerPrefs.GetString(textAnswerLevel + levelPuzzle.ToString());
        SerializableListOfLists<int> serializedList = JsonUtility.FromJson<SerializableListOfLists<int>>(json);

        if (serializedList != null)
        {
            return serializedList.ToList();
        }

        return new List<List<int>>();
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
