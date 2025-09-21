using Managers;
using UnityEngine;

namespace UI
{
    public interface IUIView
    {
        GameObject GameObject { get; }
        void Show(UIViewArguments arguments);
        void Hide();
    }
}