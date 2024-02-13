using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class LevelSelectManager : MonoBehaviour
    {
        [SerializeField]
        private bool _IsUnlockAllLevelsfc;
        [SerializeField]
        private int _totalLevelsfc;
        [SerializeField]
        private Sprite selectedspritewp;
        [SerializeField]
        private ScrollRect MyScrollRectwp;
        [SerializeField]
        private GameObject _spawnScrollfc;
        [SerializeField]
        private GameObject _levelButtonPrefabfc;
        [SerializeField]
        private GameObject currentbuttonwp;

        private List<LevelButton> _allLevelButtons = new List<LevelButton>();
        private RectTransform MyScrollContentwp;
        private int _levelSelectedfc;

        private void Awake()
        {
            MyScrollContentwp = _spawnScrollfc.GetComponent<RectTransform>();
        }

        private void Start()
        {
            
            SoundManager.Instance.PlayButtonPressedSound();
            _levelSelectedfc = PlayerPrefs.GetInt("LevelProgression", 1);
            if (_IsUnlockAllLevelsfc == true)
            {
                _levelSelectedfc = _totalLevelsfc;
            }
            print(_levelSelectedfc);
            PlaceLevelsfc();
        }
         
        private void PlaceLevelsfc()
        {
            _allLevelButtons.Clear();
            for(int i = 1; i <= _totalLevelsfc; i++)
            {
                GameObject button = Instantiate(_levelButtonPrefabfc, _spawnScrollfc.transform);
                var buttonLevel = button.GetComponent<LevelButton>();
                _allLevelButtons.Add(buttonLevel);
                buttonLevel.LevelNumber = i;
                buttonLevel.SelectLevel += SelectLevel;
                button.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = i.ToString();
                if( i <= _levelSelectedfc)
                {
                    button.transform.GetChild(1).gameObject.SetActive(false);
                    if (i == _levelSelectedfc)
                    {
                        button.gameObject.GetComponent<Image>().sprite = selectedspritewp;
                        currentbuttonwp = button;
                    }
                }
            }
            SnapToCurrentOpenfc();
            //Invoke("SnapToCurrentOpenfc", 0.01f);
        }
        private void OnDestroy()
        {
            foreach (var button in _allLevelButtons)
            {
                button.SelectLevel -= SelectLevel;
            }
        }
        
        private void SnapToCurrentOpenfc()
        {
            Canvas.ForceUpdateCanvases();
            Vector3 contentPos = MyScrollContentwp.position;
            Vector3 buttonPos = currentbuttonwp.transform.position;
            MyScrollContentwp.anchoredPosition = MyScrollRectwp.transform.InverseTransformPoint(contentPos) 
                                                 - MyScrollRectwp.transform.InverseTransformPoint(buttonPos);
            MyScrollContentwp.anchoredPosition = new Vector2(0, MyScrollContentwp.anchoredPosition.y - 200f);
        }

        private void SelectLevel(int level)
        {
            foreach (var button in _allLevelButtons)
            {
                if (button.LevelNumber == level)
                {
                    button.Selected();
                }
                else
                {
                    button.Unselected();
                }
            }
            _levelSelectedfc = level;
        }
        
        public void LoadLevelpr()
        {
            SoundManager.Instance.PlayButtonPressedSound();
            int lastlevel = PlayerPrefs.GetInt("LevelProgression", 1);
            if (_levelSelectedfc <= lastlevel)
            {
                SceneManager.LoadScene(_levelSelectedfc);
            }
        }
    }
}
