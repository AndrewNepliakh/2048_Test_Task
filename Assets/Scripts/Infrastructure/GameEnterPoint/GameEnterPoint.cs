using Zenject;
using Managers;
using UnityEngine;
using Implementations.Game;
using Managers.SaveManager;
using Managers.UserManager;
using UnityEngine.SceneManagement;

public class GameEnterPoint : MonoBehaviour
{
    [Inject] private IGameManager _gameManager;
    [Inject] private ISaveManager _saveManager;
    [Inject] private IUserManager _userManager;

    private void Start()
    {
        var saveData = _saveManager.Load();
        _userManager.Init(saveData.UserData);
        _saveManager.Save();
        _gameManager.LoadScene(Constants.GameplaySceneAssetId, LoadSceneMode.Single);
    }
}