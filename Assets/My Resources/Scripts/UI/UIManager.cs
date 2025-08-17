using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

namespace Half_Measure.UI
{
    [RequireComponent(typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster))]
    [RequireComponent(typeof(EventSystem), typeof(InputSystemUIInputModule))]
    public class UIManager : Singleton<UIManager>
    {
        private readonly Dictionary<Type, UIBase> uiDictionary = new();
        private UIBase previousUI = null;
        private UIBase currentUI = null;

        private void Start()
        {
            OpenUI<UIMainMenu>();
        }

        protected override void LoadComponents()
        {
            if (uiDictionary.Count < transform.childCount) 
            {
                foreach (Transform child in transform)
                {
                    UIBase ui = child.GetComponent<UIBase>();
                    Type uiType = ui.GetType();
                    if (!uiDictionary.ContainsKey(uiType)) uiDictionary.Add(uiType, ui);
                }
            }
        }
        
        public T OpenUI<T>() where T : UIBase
        {
            Type uiType = typeof(T);
            previousUI = currentUI;
            currentUI = uiDictionary[uiType];
            previousUI?.Close();
            currentUI?.Open();
            return currentUI as T;
        }

        public T GetUI<T>() where T : UIBase => uiDictionary[typeof(T)] as T;

        public void BackUI()
        {
            currentUI?.Close();
            previousUI?.Open();
            currentUI = previousUI;
            previousUI = null;
        }
    }
}


