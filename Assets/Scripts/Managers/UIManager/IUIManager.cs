using System;
using System.Threading.Tasks;
using UnityEngine;

namespace UI
{
    public interface IUIManager : IDisposable
    {
        void Init(Canvas mainCanvas);

        Task<T> ShowPopup<T>(UIViewArguments args = null) where T : Window;

        Task<T> ShowHUDWindow<T>(UIViewArguments args = null) where T : Window;

        void HideHUDWindow();

        void HideCurrentPopup();

        void HidePopup<T>(T popup) where T : Window;
    }
}