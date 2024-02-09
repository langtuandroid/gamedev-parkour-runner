using UnityEngine;

namespace Game
{
    public class MovingTexturepr : MonoBehaviour
    {
        private Vector2 _moveDirectionpr;
        private Renderer _rendererpr;
        
        private void Start()
        {
            _moveDirectionpr = new Vector2(0f, -0.01f);
            _rendererpr = GetComponent<Renderer>();
        }
        private void Update()
        {
            _rendererpr.material.SetTextureOffset("_BaseMap", _moveDirectionpr + _rendererpr.material.GetTextureOffset("_BaseMap"));
        }
    }
}
