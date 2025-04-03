using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CopyTileMap : MonoBehaviour
{
    [SerializeField] private Tilemap targetTilemap;
    [SerializeField] private Tilemap selfTilemap;
    [Range(0,1)] public float validationSlider;

    private void OnValidate()
    {
        selfTilemap.SetTilesBlock(targetTilemap.cellBounds, targetTilemap.GetTilesBlock(targetTilemap.cellBounds));
    }
}
