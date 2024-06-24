using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private List<int> gridLevel;
    private List<List<int>> answerLevel;

    void Start()
    {
        if (SaveLoadManager.instanceManager == null)
        {
            Debug.LogError("SaveLoadManager is not assigned or not found!");
            return;
        }

        for (int levelIndex = 1; levelIndex <= 4; levelIndex++)
        {
            // if (levelIndex == 1)
            // {
            //     gridLevel = new List<int>(){1, 2, 1, 3, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 3};
            // }

            if (levelIndex == 1)
            {
                gridLevel = new List<int>(){1, 2, 3, 4, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 1, 2, 0, 0, 0};

                answerLevel = new List<List<int>>
                {
                    new List<int> {0, 5, 10, 15, 20},
                    new List<int> {1, 6, 11, 16, 21},
                    new List<int> {2, 7, 12, 17, 22, 23, 24, 19, 14, 9, 4},
                    new List<int> {3, 8, 13, 18}
                };
            }
            else if (levelIndex == 2)
            {
                gridLevel = new List<int>(){1, 0, 0, 0, 1, 2, 0, 0, 0, 0, 3, 0, 0, 3, 0, 4, 0, 0, 4, 0, 2, 0, 0, 0, 0};

                answerLevel = new List<List<int>>
                {
                    new List<int> {0, 1, 2, 3, 4},
                    new List<int> {5, 6, 7, 8, 9, 14, 19, 24, 23, 22, 21, 20},
                    new List<int> {10, 11, 12, 13},
                    new List<int> {15, 16, 17, 18}
                };
            }
            else if (levelIndex == 3)
            {
                gridLevel = new List<int>(){1, 2, 0, 0, 3, 0, 0, 4, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 1, 4, 5, 2, 3};

                answerLevel = new List<List<int>>
                {
                    new List<int> {0, 5, 10, 15, 20},
                    new List<int> {1, 2, 3, 8, 13, 18, 23},
                    new List<int> {4, 9, 14, 19, 24},
                    new List<int> {7, 6, 11, 16, 21},
                    new List<int> {12, 17, 22}
                };
            }
            else if (levelIndex == 4)
            {
                gridLevel = new List<int>(){1, 2, 3, 2, 4, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 1, 0, 0, 0, 0, 4, 0, 0, 0, 0};

                answerLevel = new List<List<int>>
                {
                    new List<int> {0, 5, 10, 15},
                    new List<int> {1, 6, 11, 16, 17, 18, 13, 8, 3},
                    new List<int> {2, 7, 12},
                    new List<int> {20, 21, 22, 23, 24, 19, 14, 9, 4}
                };
            }

            SaveLoadManager.instanceManager.SaveGridLevel(gridLevel, "gridLevel", levelIndex);
            SaveLoadManager.instanceManager.SaveAnswerLevel(answerLevel, "answerLevel", levelIndex);
        }
    }
}
