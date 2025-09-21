using System;
using Zenject;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using Managers.UserManager;
using System.Collections.Generic;

namespace Managers.SaveManager
{
    public class SaveManager : ISaveManager
    {
        private readonly string _saveDataPath = Path.Combine(Application.persistentDataPath, "SaveData.json");

        [Inject] private IUserManager _userManager;

        public void Save()
        {
            var saveData = new SaveData()
            {
                UserData = new UserData
                {
                    LastUser = _userManager.CurrentUser,
                }
            };

            var json = JsonConvert.SerializeObject(saveData);

            if (!File.Exists(_saveDataPath))
            {
                using (File.Create(_saveDataPath))
                {
                }
            }

            File.WriteAllText(_saveDataPath, json);
        }

        public SaveData Load()
        {
            if (File.Exists(_saveDataPath))
            {
                var json = File.ReadAllText(_saveDataPath);
                try
                {
                    return JsonConvert.DeserializeObject<SaveData>(json);
                }
                catch (Exception e)
                {
                    Debug.LogError("Json doesn't deserialized: " + e.Message);
                    return GetDefaultSaveData();
                }
            }

            return GetDefaultSaveData();
        }

        private SaveData GetDefaultSaveData()
        {
            var lastUser = new User();
            return new SaveData
            {
                UserData = new UserData
                {
                    LastUser = lastUser,
                }
            };
        }
    }
}