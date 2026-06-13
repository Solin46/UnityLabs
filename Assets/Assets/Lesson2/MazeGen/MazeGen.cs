using Unity.Collections;
using UnityEngine;
using System.Linq;

public class MazeGen : MonoBehaviour
{
    // настройки блоков лабиринта
    [SerializeField] private GameObject Tile;
    [SerializeField] private GameObject Wall;

    [SerializeField] private Vector2Int GridSize;

    // переменные для монет
    [Header("Coin Settings")]
    [SerializeField] private GameObject coinPrefab; 
    [Range(0f, 1f)] [SerializeField] private float coinSpawnChance = 0.3f; 

    // технические переменные генератора лабиринта (матрица, сдвиги, массивы направлений):
    private int[,] matrix;
    private Vector2 offsets;
    int nx, ny;
    Vector2Int dir;

    Vector2Int[] directions = {
        new Vector2Int(0, 1),
        new Vector2Int(1, 0),
        new Vector2Int(0, -1),
        new Vector2Int(-1, 0)
    };
    Quaternion[] rotations = {
        Quaternion.Euler(0, 0, 0),
        Quaternion.Euler(0, 90, 0),
        Quaternion.Euler(0, 180, 0),
        Quaternion.Euler(0, 270, 0)
    };
    int[] index = { 0, 1, 2, 3 };
    int[] powers = { 1, 2, 4, 8 };

    bool borderBool;
    void Awake()
    {
        GenerateMaze(); // вызов генерации
    }

    private void GenerateMaze()
    {
        matrix = new int[GridSize.x, GridSize.y];
        offsets = new Vector2(Tile.transform.localScale.x, Tile.transform.localScale.z);

        GenerateCell(0, 0);
    }

    private void GenerateCell(int x, int y) // метод построения лабиринта
    {
        if (x < 0 || y < 0 || x >= GridSize.x || y >= GridSize.y || matrix[x, y] > 0)
        {
            return;
        }

        matrix[x, y] = 16; 
        
        // создание плитки пола
        Vector3 tilePos = transform.position + new Vector3(x * offsets.x, 0, y * offsets.y);
        Instantiate(Tile, tilePos, transform.rotation).transform.parent = transform;

        TrySpawnCoin(tilePos); // спавн монетки на плитке

        int[] shuffledIndex = index.OrderBy(x => Random.value).ToArray(); 
        foreach (int ind in shuffledIndex)
        {
            dir = directions[ind];
            nx = x + dir.x;
            ny = y + dir.y;

            borderBool = nx < 0 || ny < 0 || nx >= GridSize.x || ny >= GridSize.y;

            if (borderBool || (matrix[nx, ny] > 0 && (((matrix[nx, ny] - 1) >> ((ind + 2) % 4)) % 2 == 1)))
            {
                Instantiate(Wall, transform.position + new Vector3(x * offsets.x + (offsets.x / 2f) * dir.x * 0.95f, Wall.transform.lossyScale.y / 2, y * offsets.y + (offsets.y / 2f) * dir.y * 0.95f), transform.rotation * rotations[ind]).transform.parent = transform;
            }
            else
            {
                if (matrix[nx, ny] == 0)
                {
                    matrix[x, y] -= powers[ind];
                    GenerateCell(nx, ny);
                }
            }
        }
    }

    // метод спавна монет
    private void TrySpawnCoin(Vector3 tilePosition)
    {
        if (tilePosition == transform.position) return; // защита от спавна монеты в месте появления игрока

        if (Random.value <= coinSpawnChance && coinPrefab != null) // генерирует число, по величине которого определяется, заспавнится ли здесь монетка (если оно проходит указанный порог)
        {
            Vector3 coinPosition = tilePosition + new Vector3(0f, 0.5f, 0f);
            GameObject spawnedCoin = Instantiate(coinPrefab, coinPosition, Quaternion.identity);
            spawnedCoin.transform.parent = transform; 
        }
    }
}