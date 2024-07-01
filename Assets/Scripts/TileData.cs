using System.Collections.Generic;
using UnityEngine;

public class TileData : MonoBehaviour
{
    public int SomeCid { get; set; }
    public Tile SomeConnectionTile { get; set; }
    public List<Tile> SomeConnection { get; set; }
}