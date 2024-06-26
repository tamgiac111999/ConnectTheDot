using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    private int width;
    private int height;
    private List<int> gridLevel;
    private List<List<int>> answerLevel;

    void Start()
    {
        if (SaveLoadManager.instanceManager == null) return;

        for (int levelIndex = 1; levelIndex <= 8; levelIndex++)
        {
            switch (levelIndex)
            {
                case 1:
                    InitializeLevel(4, 4, new List<int>() { 1, 2, 1, 3, 0, 0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 3 },
                                    new List<List<int>>() {
                                        new List<int> { 0, 4, 8, 12, 13, 14, 10, 6, 2 },
                                        new List<int> { 1, 5, 9 },
                                        new List<int> { 3, 7, 11, 15 }
                                    });
                    break;
                case 2:
                    InitializeLevel(5, 5, new List<int>() { 1, 2, 3, 4, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 0, 1, 2, 0, 0, 0 },
                                    new List<List<int>>() {
                                        new List<int> { 0, 5, 10, 15, 20 },
                                        new List<int> { 1, 6, 11, 16, 21 },
                                        new List<int> { 2, 7, 12, 17, 22, 23, 24, 19, 14, 9, 4 },
                                        new List<int> { 3, 8, 13, 18 }
                                    });
                    break;
                case 3:
                    InitializeLevel(5, 5, new List<int>() { 1, 0, 0, 0, 1, 2, 0, 0, 0, 0, 3, 0, 0, 3, 0, 4, 0, 0, 4, 0, 2, 0, 0, 0, 0 },
                                    new List<List<int>>() {
                                        new List<int> { 0, 1, 2, 3, 4 },
                                        new List<int> { 5, 6, 7, 8, 9, 14, 19, 24, 23, 22, 21, 20 },
                                        new List<int> { 10, 11, 12, 13 },
                                        new List<int> { 15, 16, 17, 18 }
                                    });
                    break;
                case 4:
                    InitializeLevel(5, 5, new List<int>() { 1, 2, 0, 0, 3, 0, 0, 4, 0, 0, 0, 0, 5, 0, 0, 0, 0, 0, 0, 0, 1, 4, 5, 2, 3 },
                                    new List<List<int>>() {
                                        new List<int> { 0, 5, 10, 15, 20 },
                                        new List<int> { 1, 2, 3, 8, 13, 18, 23 },
                                        new List<int> { 4, 9, 14, 19, 24 },
                                        new List<int> { 7, 6, 11, 16, 21 },
                                        new List<int> { 12, 17, 22 }
                                    });
                    break;
                case 5:
                    InitializeLevel(5, 5, new List<int>() { 1, 2, 3, 2, 4, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 1, 0, 0, 0, 0, 4, 0, 0, 0, 0 },
                                    new List<List<int>>() {
                                        new List<int> { 0, 5, 10, 15 },
                                        new List<int> { 1, 6, 11, 16, 17, 18, 13, 8, 3 },
                                        new List<int> { 2, 7, 12 },
                                        new List<int> { 20, 21, 22, 23, 24, 19, 14, 9, 4 }
                                    });
                    break;
                case 6:
                    InitializeLevel(5, 5, new List<int>() { 0, 0, 0, 0, 1, 0, 3, 0, 3, 2, 0, 4, 2, 0, 0, 1, 0, 0, 0, 4, 5, 0, 0, 0, 5 },
                                    new List<List<int>>() {
                                        new List<int> { 4, 3, 2, 1, 0, 5, 10, 15 },
                                        new List<int> { 9, 14, 13, 12 },
                                        new List<int> { 6, 7, 8 },
                                        new List<int> { 11, 16, 17, 18, 19 },
                                        new List<int> { 20, 21, 22, 23, 24 }
                                    });
                    break;
                case 7:
                    InitializeLevel(5, 5, new List<int>() { 1, 0, 0, 0, 0, 2, 3, 4, 5, 0, 0, 0, 0, 0, 0, 0, 3, 4, 5, 0, 0, 2, 1, 0, 0 },
                                    new List<List<int>>() {
                                        new List<int> { 0, 1, 2, 3, 4, 9, 14, 19, 24, 23, 22 },
                                        new List<int> { 5, 10, 15, 20, 21 },
                                        new List<int> { 6, 11, 16 },
                                        new List<int> { 7, 12, 17 },
                                        new List<int> { 8, 13, 18 }
                                    });
                    break;
                case 8:
                    InitializeLevel(5, 6, new List<int>() { 1, 0, 0, 0, 1, 2, 0, 3, 0, 3, 4, 0, 5, 0, 6, 0, 0, 6, 0, 0, 0, 0, 0, 5, 0, 4, 2, 0, 0, 0 },
                                    new List<List<int>>() {
                                        new List<int> { 0, 1, 2, 3, 4 },
                                        new List<int> { 5, 6, 11, 16, 21, 26 },
                                        new List<int> { 7, 8, 9 },
                                        new List<int> { 10, 15, 20, 25 },
                                        new List<int> { 12, 13, 18, 23 },
                                        new List<int> { 14, 19, 24, 29, 28, 27, 22, 17 }
                                    });
                    break;
            }

            SaveLoadManager.instanceManager.SaveLevelDimensions(levelIndex, width, height);
            SaveLoadManager.instanceManager.SaveGridLevel(gridLevel, "gridLevel", levelIndex);
            SaveLoadManager.instanceManager.SaveAnswerLevel(answerLevel, "answerLevel", levelIndex);
        }
    }

    private void InitializeLevel(int width, int height, List<int> gridLevel, List<List<int>> answerLevel)
    {
        this.width = width;
        this.height = height;
        this.gridLevel = gridLevel;
        this.answerLevel = answerLevel;
    }
}
