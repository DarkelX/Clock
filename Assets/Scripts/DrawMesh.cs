using UnityEngine;

namespace Scripts
{
    public class DrawMesh : MonoBehaviour
    {
        private Mesh mesh;

        private MeshFilter meshFilter;

        private void Start()
        {
            Initialization();
        }

        private void Initialization()
        {
            meshFilter = GetComponent<MeshFilter>();
            mesh = new Mesh();
            meshFilter.mesh = mesh;
            DrawTriangle();
        }

        private void DrawTriangle()
        {
            var verticesArray = new Vector3[3];
            var trianglesArray = new int[3];
            
            verticesArray[0] = new Vector3(0, 1, 0);
            verticesArray[1] = new Vector3(-1, 0, 0);
            verticesArray[2] = new Vector3(1, 0, 0);
            
            trianglesArray[0] = 0;
            trianglesArray[1] = 1;
            trianglesArray[2] = 2;
            
            mesh.vertices = verticesArray;
            mesh.triangles = trianglesArray;
        }
#if UNITY_EDITOR
        private void OnValidate()
        {
            Initialization();
        }
#endif
    }
}