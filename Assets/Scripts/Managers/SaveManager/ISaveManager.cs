namespace Managers.SaveManager
{
    public interface ISaveManager
    {
        void Save();
        SaveData Load();
    }
}