using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LevelButton : MonoBehaviour
    {
        public Action<int> SelectLevel;
        
        [SerializeField] 
        private int _levelNumber;
        [SerializeField] 
        private Sprite _selectedSprite;
        [SerializeField] 
        private Sprite _unSelectedSprite;

        private Button _button;
        public int LevelNumber
        {
            get => _levelNumber;
            set => _levelNumber = value;
        }

        private void Awake()
        {
            _button = gameObject.GetComponent<Button>();
            _button.onClick.AddListener(SelectLevelfc);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(SelectLevelfc);
        }

        private void SelectLevelfc()
        {
            SoundManager.Instance.PlayButtonPressedSound();
            if (gameObject.transform.GetChild(1).gameObject.activeInHierarchy) return;
            SelectLevel.Invoke(LevelNumber);
        }

        public void Selected()
        {
            gameObject.GetComponent<Image>().sprite = _selectedSprite;
        }
        public void Unselected()
        {
            gameObject.GetComponent<Image>().sprite = _unSelectedSprite;
        }
    }
}
