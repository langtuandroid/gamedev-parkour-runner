using MainControllers;
using UnityEngine;

namespace UI
{
    public class UIPanel : MonoBehaviour
    {
        [SerializeField]
        private PanelType _panelType;

        public PanelType PanelType => _panelType;
    }
}