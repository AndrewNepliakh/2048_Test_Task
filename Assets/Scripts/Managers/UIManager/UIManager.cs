using System;
using Zenject;
using Managers;
using UnityEngine;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace UI
{
    public class UIManager : IUIManager
    {
        [Inject] private IAssetsManager _assetsManager;

        private Window _currentPopup;
        private Window _currentHUDWindow;
        private Canvas _mainCanvas;

        private readonly Dictionary<Type, IUIView> _popupsPool = new();
        private readonly Dictionary<Type, IUIView> _HUDPool = new();

        public void Init(Canvas mainCanvas)
        {
            _mainCanvas = mainCanvas;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        public async Task<T> ShowPopup<T>(UIViewArguments args = null) where T : Window
        {
            if (!_popupsPool.ContainsKey(typeof(T)))
            {
                var assetGo = await _assetsManager.InstantiateWithDi(
                    typeof(T).ToString(),
                    Vector3.one, 
                    Quaternion.identity,
                    _mainCanvas.transform);

                _currentPopup = assetGo.GetComponent<T>();
                _popupsPool.Add(typeof(T), _currentPopup);
                
                var rectTransform = _currentPopup.GetComponent<RectTransform>();
                
                SetRectTransform(rectTransform);

                _currentPopup.Show(args);
                _currentPopup.transform.SetAsLastSibling();
            }
            else
            {
                if (_popupsPool.TryGetValue(typeof(T), out var uiView))
                {
                    _currentPopup = uiView as Window;
                    
                    var rectTransform = _currentPopup.GetComponent<RectTransform>();
                
                    SetRectTransform(rectTransform);
                    
                    _currentPopup.Show(args);
                    _currentPopup.transform.SetAsLastSibling();
                }
                else
                {
                    throw new NullReferenceException($"UIManager's pool doesn't contain view of type {typeof(T)}");
                }
            }

            return (T)_currentPopup;
        }

        public async Task<T> ShowHUDWindow<T>(UIViewArguments args = null) where T : Window
        {
            if (_currentHUDWindow != null)
                _currentHUDWindow.Hide();

            if (!_HUDPool.ContainsKey(typeof(T)))
            {
                var assetGO = await _assetsManager.InstantiateWithDi(
                    typeof(T).ToString(),
                    Vector3.zero, 
                    Quaternion.identity,
                    _mainCanvas.transform);

                _currentHUDWindow = assetGO.GetComponent<T>();
                _HUDPool.Add(typeof(T), _currentHUDWindow);
                
                var rectTransform = _currentHUDWindow.GetComponent<RectTransform>();
                
                SetRectTransform(rectTransform);

                _currentHUDWindow.Show(args);
            }
            else
            {
                if (_HUDPool.TryGetValue(typeof(T), out var uiView))
                {
                    _currentHUDWindow = uiView as Window;
                    
                    var rectTransform = _currentHUDWindow.GetComponent<RectTransform>();
                
                    SetRectTransform(rectTransform);
                    
                    _currentHUDWindow.Show(args);
                }
                else
                {
                    throw new NullReferenceException($"UIManager's HUD pool doesn't contain view of type {typeof(T)}");
                }
            }

            return (T)_currentHUDWindow;
        }

        public void HideHUDWindow() => _currentHUDWindow?.Hide();
        
        public void HideCurrentPopup() => _currentPopup?.Hide();
        
        public void HidePopup<T>(T popup) where T : Window => popup.Hide();
        
        public void Dispose()
        {
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
            ClearPopups();
            ClearHUDs();
        }
        
        private void ClearPopups()
        {
            foreach (var popup in _popupsPool.Values)
            {
                if (popup != null)
                {
                    popup.Hide();
                    _assetsManager.ReleaseInstance(popup.GameObject);
                }
            }

            _popupsPool.Clear();
            _currentPopup = null;
        }
        
        private void ClearHUDs()
        {
            foreach (var hud in _HUDPool.Values)
            {
                if (hud != null)
                {
                    hud.Hide();
                    _assetsManager.ReleaseInstance(hud.GameObject);
                }
            }

            _HUDPool.Clear();
            _currentHUDWindow = null;
        }
        
        private void OnSceneUnloaded(Scene scene)
        {
            ClearPopups();
            ClearHUDs();
        }
        
        private void SetRectTransform(RectTransform rectTransform)
        {
            rectTransform.SetParent(_mainCanvas.transform, false);
            rectTransform.localPosition = Vector3.zero;
            rectTransform.localScale = Vector3.one;
            rectTransform.anchorMin = new Vector2(0.0f, 0.0f);
            rectTransform.anchorMax = new Vector2(1.0f, 1.0f);
            rectTransform.pivot = new Vector2(0.5f, 0.5f);
        }
    }
}
