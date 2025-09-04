using Pathfinder.Graph;
using UnityEngine;

public class GraphCamera : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private GraphView graphView;
    [SerializeField] private Vector2 offset = new(-.5f, -.5f);
    
    private void Start()
    {
        int sizeX = graphView.Graph.GetSize().X;
        int sizeY = graphView.Graph.GetSize().Y;
        
        transform.position = new Vector3(sizeX / 2f + offset.x, sizeY / 2f + offset.y, -10);
        cam.orthographic = true;
        cam.orthographicSize = Mathf.Max(sizeY, sizeX) / 2f;
    }
}
