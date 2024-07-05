using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    private int width;
    private int height;
    private List<int> gridLevel;
    private List<int> wallLevel;
    private List<List<int>> answerLevel;

    void Start()
    {
        if (SaveLoadManager.instanceManager == null) return;

        for (int levelIndex = 1; levelIndex <= 21; levelIndex++)
        {
            switch (levelIndex)
            {
                case 1:
                    InitializeLevel(4, 4,   new List<int>() {
                                                1, 2, 1, 3, 
                                                0, 0, 0, 0,
                                                0, 2, 0, 0,
                                                0, 0, 0, 3
                                            },
                                            new List<List<int>>() {
                                                new List<int> { 0, 4, 8, 12, 13, 14, 10, 6, 2 },
                                                new List<int> { 1, 5, 9 },
                                                new List<int> { 3, 7, 11, 15 }
                                            });
                    break;
                case 2:
                    InitializeLevel(5, 5,   new List<int>() {
                                                1, 2, 3, 4, 3,
                                                0, 0, 0, 0, 0,
                                                0, 0, 0, 0, 0,
                                                0, 0, 0, 4, 0,
                                                1, 2, 0, 0, 0
                                            },
                                            new List<List<int>>() {
                                                new List<int> { 0, 5, 10, 15, 20 },
                                                new List<int> { 1, 6, 11, 16, 21 },
                                                new List<int> { 2, 7, 12, 17, 22, 23, 24, 19, 14, 9, 4 },
                                                new List<int> { 3, 8, 13, 18 }
                                            });
                    break;
                case 3:
                    InitializeLevel(5, 5,   new List<int>() {
                                                1, 0, 0, 0, 1,
                                                2, 0, 0, 0, 0,
                                                3, 0, 0, 3, 0,
                                                4, 0, 0, 4, 0,
                                                2, 0, 0, 0, 0
                                            },
                                            new List<List<int>>() {
                                                new List<int> { 0, 1, 2, 3, 4 },
                                                new List<int> { 5, 6, 7, 8, 9, 14, 19, 24, 23, 22, 21, 20 },
                                                new List<int> { 10, 11, 12, 13 },
                                                new List<int> { 15, 16, 17, 18 }
                                            });
                    break;
                case 4:
                    InitializeLevel(5, 5,   new List<int>() {
                                                1, 2, 0, 0, 3,
                                                0, 0, 4, 0, 0,
                                                0, 0, 5, 0, 0,
                                                0, 0, 0, 0, 0,
                                                1, 4, 5, 2, 3
                                            },
                                            new List<List<int>>() {
                                                new List<int> { 0, 5, 10, 15, 20 },
                                                new List<int> { 1, 2, 3, 8, 13, 18, 23 },
                                                new List<int> { 4, 9, 14, 19, 24 },
                                                new List<int> { 7, 6, 11, 16, 21 },
                                                new List<int> { 12, 17, 22 }
                                            });
                    break;
                case 5:
                    InitializeLevel(5, 5,   new List<int>() {
                                                1, 2, 3, 2, 4,
                                                0, 0, 0, 0, 0,
                                                0, 0, 3, 0, 0,
                                                1, 0, 0, 0, 0,
                                                4, 0, 0, 0, 0 
                                            },
                                            new List<List<int>>() {
                                                new List<int> { 0, 5, 10, 15 },
                                                new List<int> { 1, 6, 11, 16, 17, 18, 13, 8, 3 },
                                                new List<int> { 2, 7, 12 },
                                                new List<int> { 20, 21, 22, 23, 24, 19, 14, 9, 4 }
                                            });
                    break;
                case 6:
                    InitializeLevel(5, 5,   new List<int>() {
                                                0, 0, 0, 0, 1,
                                                0, 3, 0, 3, 2,
                                                0, 4, 2, 0, 0,
                                                1, 0, 0, 0, 4,
                                                5, 0, 0, 0, 5 
                                            },
                                            new List<List<int>>() {
                                                new List<int> { 4, 3, 2, 1, 0, 5, 10, 15 },
                                                new List<int> { 9, 14, 13, 12 },
                                                new List<int> { 6, 7, 8 },
                                                new List<int> { 11, 16, 17, 18, 19 },
                                                new List<int> { 20, 21, 22, 23, 24 }
                                            });
                    break;
                case 7:
                    InitializeLevel(5, 5,   new List<int>() {
                                                1, 0, 0, 0, 0,
                                                2, 3, 4, 5, 0,
                                                0, 0, 0, 0, 0,
                                                0, 3, 4, 5, 0,
                                                0, 2, 1, 0, 0
                                            },
                                            new List<List<int>>() {
                                                new List<int> { 0, 1, 2, 3, 4, 9, 14, 19, 24, 23, 22 },
                                                new List<int> { 5, 10, 15, 20, 21 },
                                                new List<int> { 6, 11, 16 },
                                                new List<int> { 7, 12, 17 },
                                                new List<int> { 8, 13, 18 }
                                            });
                    break;
                case 8:
                    InitializeLevel(5, 6,   new List<int>() {
                                                1, 0, 0, 0, 1,
                                                2, 0, 3, 0, 3,
                                                4, 0, 5, 0, 6,
                                                0, 0, 6, 0, 0,
                                                0, 0, 0, 5, 0,
                                                4, 2, 0, 0, 0
                                            },
                                            new List<List<int>>() {
                                                new List<int> { 0, 1, 2, 3, 4 },
                                                new List<int> { 5, 6, 11, 16, 21, 26 },
                                                new List<int> { 7, 8, 9 },
                                                new List<int> { 10, 15, 20, 25 },
                                                new List<int> { 12, 13, 18, 23 },
                                                new List<int> { 14, 19, 24, 29, 28, 27, 22, 17 }
                                            });
                    break;
                case 9:
                    InitializeLevel(6, 6,   new List<int>() {
                                                1, 0, 0, 0, 0, 1,
                                                2, 3, 0, 0, 0, 0,
                                                0, 4, 5, 0, 5, 3,
                                                0, 0, 0, 0, 0, 4,
                                                0, 6, 0, 0, 0, 6,
                                                0, 0, 0, 0, 0, 2
                                            },
                                            new List<List<int>>() {
                                                new List<int> { 0, 1, 2, 3, 4, 5 },
                                                new List<int> { 6, 12, 18, 24, 30, 31, 32, 33, 34, 35 },
                                                new List<int> { 7, 8, 9, 10, 11, 17 },
                                                new List<int> { 13, 19, 20, 21, 22, 23 },
                                                new List<int> { 14, 15, 16 },
                                                new List<int> { 25, 26, 27, 28, 29 }
                                            });
                    break;
                case 10:
                    InitializeLevel(6, 6,   new List<int>() {
                                                1, 0, 0, 0, 0, 2,
                                                3, 4, 5, 4, 0, 0,
                                                0, 0, 0, 0, 0, 0,
                                                0, 0, 5, 0, 0, 0,
                                                0, 0, 0, 0, 0, 0,
                                                0, 0, 0, 3, 1, 2
                                            },
                                            new List<List<int>>() {
                                                new List<int> { 0, 1, 2, 3, 4, 10, 16, 22, 28, 34 },
                                                new List<int> { 5, 11, 17, 23, 29, 35 },
                                                new List<int> { 6, 12, 18, 24, 30, 31, 32, 33 },
                                                new List<int> { 7, 13, 19, 25, 26, 27, 21, 15, 9 },
                                                new List<int> { 8, 14, 20 }
                                            });
                    break;
                case 11:
                    InitializeLevel(6, 6,   new List<int>() {
                                                0, 0, 0, 0, 0, 1,
                                                1, 2, 0, 0, 0, 3,
                                                4, 0, 0, 4, 0, 0,
                                                2, 0, 0, 0, 0, 0,
                                                3, 0, 0, 0, 0, 0,
                                                5, 0, 0, 0, 0, 5
                                            },
                                            new List<List<int>>() {
                                                new List<int> { 5, 4, 3, 2, 1, 0, 6 },
                                                new List<int> { 7, 8, 9, 10, 16, 22, 21, 20, 19, 18 },
                                                new List<int> { 11, 17, 23, 29, 28, 27, 26, 25, 24 },
                                                new List<int> { 12, 13, 14, 15 },
                                                new List<int> { 30, 31, 32, 33, 34, 35 }
                                            });
                    break;
                case 12:
                    InitializeLevel(6, 6,   new List<int>() {
                                                1, 2, 0, 0, 0, 2,
                                                0, 3, 0, 3, 4, 5,
                                                0, 6, 7, 0, 0, 0,
                                                0, 0, 4, 7, 0, 0,
                                                0, 0, 0, 0, 0, 0,
                                                1, 6, 5, 0, 0, 0
                                            },
                                            new List<List<int>>() {
                                                new List<int> { 0, 6, 12, 18, 24, 30 },
                                                new List<int> { 1, 2, 3, 4, 5 },
                                                new List<int> { 7, 8, 9 },
                                                new List<int> { 10, 16, 22, 28, 27, 26, 20 },
                                                new List<int> { 11, 17, 23, 29, 35, 34, 33, 32 },
                                                new List<int> { 13, 19, 25, 31 },
                                                new List<int> { 14, 15, 21 }
                                            },
                                            new List<int>() {
                                                0, 3, 3, 0, 0, 0,
                                                2, 0, 0, 0, 0, 4,
                                                2, 0, 0, 0, 0, 4,
                                                2, 0, 0, 0, 0, 4,
                                                2, 0, 0, 0, 0, 4,
                                                0, 0, 0, 1, 1, 0
                                            });
                    break;
                case 13:
                    InitializeLevel(6, 6,   new List<int>() {
                                                0, 0, 0, 2, 0, 3,
                                                0, 4, 0, 3, 0, 0,
                                                0, 0, 0, 0, 0, 0,
                                                0, 0, 0, 0, 0, 0,
                                                0, 0, 0, 0, 2, 0,
                                                1, 4, 1, 0, 0, 0
                                            },
                                            new List<List<int>>() {
                                                new List<int> { 30, 24, 18, 12, 6, 0, 1, 2, 8, 14, 20, 26, 32 },
                                                new List<int> { 3, 4, 10, 16, 22, 28 },
                                                new List<int> { 5, 11, 17, 23, 29, 35, 34, 33, 27, 21, 15, 9 },
                                                new List<int> { 7, 13, 19, 25, 31 }
                                            });
                    break;
                case 14:
                    InitializeLevel(6, 6,   new List<int>() {
                                                1, 2, 3, 0, 0, 0,
                                                0, 0, 4, 0, 5, 0,
                                                0, 0, 5, 0, 0, 0,
                                                0, 0, 0, 4, 0, 0,
                                                0, 0, 0, 0, 0, 3,
                                                1, 0, 0, 0, 0, 2
                                            },
                                            new List<List<int>>() {
                                                new List<int> { 0, 6, 12, 18, 24, 30 },
                                                new List<int> { 1, 7, 13, 19, 25, 31, 32, 33, 34, 35 },
                                                new List<int> { 2, 3, 4, 5, 11, 17, 23, 29 },
                                                new List<int> { 8, 9, 15, 21 },
                                                new List<int> { 10, 16, 22, 28, 27, 26, 20, 14 }
                                            });
                    break;
                case 15:
                    InitializeLevel(8, 7,   new List<int>() {
                                                1, -1, -1, 2, 0, 0, 0, 0,
                                                0, -1, -1, 3, 0, 0, 3, 0,
                                                0, -1, -1, -1, -1, -1, -1, 0,
                                                0, 4, 0, 0, -1, -1, -1, 0,
                                                0, -1, -1, 0, 5, 0, 5, 0,
                                                0, -1, -1, 0, 0, 0, 4, 0,
                                                0, 0, 0, 0, 0, 0, 1, 2
                                            },
                                            new List<List<int>>() {
                                                new List<int> { 0, 8, 16, 24, 32, 40, 48, 49, 50, 51, 52, 53, 54 },
                                                new List<int> { 3, 4, 5, 6, 7, 15, 23, 31, 39, 47, 55 },
                                                new List<int> { 11, 12, 13, 14 },
                                                new List<int> { 25, 26, 27, 35, 43, 44, 45, 46 },
                                                new List<int> { 36, 37, 38 }
                                            });
                    break;
                case 16:
                    InitializeLevel(6, 6,   new List<int>() {
                                                3, 0, 0, 0, 2, 1,
                                                0, 0, 4, 0, 0, 0,
                                                0, 3, 0, 0, 0, 0,
                                                0, 5, 0, 5, 0, 0,
                                                0, 2, 0, 0, 0, 0,
                                                0, 0, 0, 0, 4, 1
                                            },
                                            new List<List<int>>() {
                                                new List<int> { 5, 11, 17, 23, 29, 35 },
                                                new List<int> { 4, 10, 16, 22, 28, 27, 26, 25 },
                                                new List<int> { 0, 1, 2, 3, 9, 15, 14, 13 },
                                                new List<int> { 8, 7, 6, 12, 18, 24, 30, 31, 32, 33, 34 },
                                                new List<int> { 19, 20, 21 }
                                            });
                    break;
                case 17:
                    InitializeLevel(6, 6,   new List<int>() {
                                                0, 0, 1, 2, 0, 0,
                                                0, 3, 4, 0, 0, 0,
                                                0, 0, 0, 1, 0, 0,
                                                0, 0, 0, 5, 4, 0,
                                                0, 3, 0, 0, 5, 0,
                                                0, 0, 0, 2, 0, 0
                                            },
                                            new List<List<int>>() {
                                                new List<int> { 2, 1, 0, 6, 12, 18, 24, 30, 31, 32, 26, 20, 14, 15 },
                                                new List<int> { 3, 4, 5, 11, 17, 23, 29, 35, 34, 33 },
                                                new List<int> { 7, 13, 19, 25 },
                                                new List<int> { 8, 9, 10, 16, 22 },
                                                new List<int> { 21, 27, 28 }
                                            },
                                            new List<int>() {
                                                0, 3, 3, 0, 3, 0,
                                                2, 0, 0, 0, 0, 4,
                                                2, 0, 0, 0, 0, 0,
                                                0, 0, 0, 0, 0, 4,
                                                2, 0, 0, 0, 0, 4,
                                                0, 1, 0, 1, 1, 0
                                            });
                    break;
                case 18:
                    InitializeLevel(6, 6,   new List<int>() {
                                                1, 0, 0, 0, 0, 2,
                                                0, 0, 3, 4, 0, 0,
                                                3, 0, 0, 0, 0, 0,
                                                0, 0, 0, 5, 0, 0,
                                                0, 5, 0, 6, 1, 0,
                                                4, 6, 0, 0, 2, 0
                                            },
                                            new List<List<int>>() {
                                                new List<int> { 0, 1, 2, 3, 4, 10, 16, 22, 28 },
                                                new List<int> { 5, 11, 17, 23, 29, 35, 34 },
                                                new List<int> { 12, 6, 7, 8 },
                                                new List<int> { 9, 15, 14, 13, 19, 18, 24, 30 },
                                                new List<int> { 21, 20, 26, 25 },
                                                new List<int> { 27, 33, 32, 31 }
                                            });
                    break;
                case 19:
                    InitializeLevel(6, 6,   new List<int>() {
                                                1, 0, 0, 0, 0, 2,
                                                3, 0, 0, 0, 0, 4,
                                                0, 1, 0, 0, 5, 0,
                                                0, 2, 0, 4, 3, 0,
                                                0, 0, 0, 0, 0, 0,
                                                5, 0, 0, 0, 0, 0
                                            },
                                            new List<List<int>>() {
                                                new List<int> { 0, 1, 7, 13 },
                                                new List<int> { 5, 4, 3, 2, 8, 14, 20, 19 },
                                                new List<int> { 6, 12, 18, 24, 25, 26, 27, 28, 22 },
                                                new List<int> { 11, 10, 9, 15, 21 },
                                                new List<int> { 16, 17, 23, 29, 35, 34, 33, 32, 31, 30 }
                                            });
                    break;
                case 20:
                    InitializeLevel(6, 6,   new List<int>() {
                                                -1, 1, 0, 0, 0, 0,
                                                1, 0, 0, 0, 3, 0,
                                                4, 0, 0, 0, 0, 0,
                                                -1, 0, 0, 0, 0, -1,
                                                0, 0, 0, 0, 2, 3,
                                                4, 2, 0, 0, 0, 0
                                            },
                                            new List<List<int>>() {
                                                new List<int> { 1, 7, 6 },
                                                new List<int> { 31, 32, 26, 20, 14, 8, 2, 3, 4, 5, 11, 17, 16, 22, 28},
                                                new List<int> { 10, 9, 15, 21, 27, 33, 34, 35, 29 },
                                                new List<int> { 12, 13, 19, 25, 24, 30 }
                                            });
                    break;
                case 21:
                    InitializeLevel(6, 6,   new List<int>() {
                                                1, 0, 0, 0, 2, 3,
                                                0, 0, 0, 0, 4, 0,
                                                0, 0, 0, 0, 5, 0,
                                                0, 0, 0, 5, 4, 0,
                                                0, 0, 0, 0, 0, 0,
                                                1, 2, 3, 0, 0, 0
                                            },
                                            new List<List<int>>() {
                                                new List<int> { 0, 6, 12, 18, 24, 30 },
                                                new List<int> { 4, 3, 2, 1, 7, 13, 19, 25, 31 },
                                                new List<int> { 5, 11, 17, 23, 29, 35, 34, 33, 32 },
                                                new List<int> { 10, 9, 8, 14, 20, 26, 27, 28, 22 },
                                                new List<int> { 16, 15, 21 }
                                            });
                    break;
            }

            SaveLoadManager.instanceManager.SaveLevelDimensions(levelIndex, width, height);
            SaveLoadManager.instanceManager.SaveGridLevel(gridLevel, "gridLevel", levelIndex);
            SaveLoadManager.instanceManager.SaveAnswerLevel(answerLevel, "answerLevel", levelIndex);
            
            if (wallLevel != null)
            {
                SaveLoadManager.instanceManager.SaveGridLevel(wallLevel, "wallLevel", levelIndex);
            }
        }
    }

    private void InitializeLevel(int width, int height, List<int> gridLevel, List<List<int>> answerLevel, List<int> wallLevel = null)
    {
        this.width = width;
        this.height = height;
        this.gridLevel = gridLevel;
        this.answerLevel = answerLevel;
        this.wallLevel = wallLevel;
    }

    private void InitializeLevel(int width, int height, List<int> gridLevel, List<List<int>> answerLevel)
    {
        this.width = width;
        this.height = height;
        this.gridLevel = gridLevel;
        this.answerLevel = answerLevel;
        this.wallLevel = null;
    }
}
