public abstract class Singleton<T> : AutoMonoBehaviour where T : AutoMonoBehaviour
{
    public static T Instance => instance;
    private static T instance;

    protected override void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this as T;
        DontDestroyOnLoad(this.gameObject);
        base.Awake();
    }
}