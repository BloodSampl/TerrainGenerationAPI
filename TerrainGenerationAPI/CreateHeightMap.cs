using UnityEngine;
namespace TerrainGenerationAPI
{
  public class CreateHeightMap : MonoBehaviour
{
    [SerializeField] private int totalRectangleX;
    [SerializeField] private int totalRectangleZ;
    [SerializeField] private float highScale;
    [SerializeField] private Texture2D heightMap;

    int _totalVerticesX;
    int _totalVerticesZ;

    int _totalVertices = 0;

    private const int TotalIndeciesForEveryRect = 6;
    private int _totalIndecies;

    Vector3[] _vertices;
    int[] _indecies;

    MeshFilter _meshFilter;
    MeshRenderer _meshRenderer;
    [SerializeField] Material material;

    private void Start()
    {
        if (Camera.main != null) Camera.main.transform.LookAt(this.transform);
        _meshFilter = gameObject.AddComponent<MeshFilter>();
        _meshRenderer = gameObject.AddComponent<MeshRenderer>();

        _totalVerticesX = totalRectangleX + 1;
        _totalVerticesZ = totalRectangleZ + 1;
        _totalVertices = _totalVerticesX * _totalVerticesZ;

        _totalIndecies = totalRectangleX * totalRectangleZ * TotalIndeciesForEveryRect;

        _vertices = new Vector3[_totalVertices];

        CreateMesh();

        var mesh = _meshFilter.mesh;
        mesh.vertices = _vertices;
        mesh.triangles = _indecies;
        _meshRenderer.material = material;
    }

    private void CreateMesh()
    {
        for (int z = 0; z < _totalVerticesZ; z++)
        {
            for (int x = 0; x < _totalVerticesX; x++)
            {
                Color pixel = heightMap.GetPixel(-x, -z);
                int i = x + z * _totalVerticesX;
                _vertices[i] = new Vector3(-x, pixel.r * highScale, z);
            }
        }
        _indecies = new int[_totalIndecies];
        int currentIndex = 0;
        int currentRow = 1;

        for (int i = 0; i < _totalIndecies; i += 6)
        {
            _indecies[i + 0] = currentIndex;
            _indecies[i + 1] = currentIndex + 1;
            _indecies[i + 2] = currentIndex + 1 + _totalVerticesX;

            _indecies[i + 3] = currentIndex + 1 + _totalVerticesX;
            _indecies[i + 4] = currentIndex + _totalVerticesX;
            _indecies[i + 5] = currentIndex;

            currentIndex++;

            if (currentIndex < _totalVerticesX * currentRow - 1) continue;
            currentIndex++;
            currentRow++;
        }
    }
}
}