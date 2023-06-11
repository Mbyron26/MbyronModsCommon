namespace MbyronModsCommon;

public abstract class SingletonManager<T> : SingletonClass<T>, IManager where T : class, new() {
    public abstract bool IsInit { get; set; }

    public abstract void DeInit();
    public abstract void Init();
}

public interface IManager {
    bool IsInit { get; set; }
    void Init();
    void DeInit();
}

public abstract class SingletonClass<T> where T : class, new() {
    private static T instance;

    public static T Instance {
        get {
            if (instance is null) {
                instance = new T();
                InternalLogger.Log($"Creating singleton of type {typeof(T).Name}");
            }
            return instance;
        }
    }

    public static bool Exists => instance is not null;
    public static void Destory() {
        instance = null;
        InternalLogger.Log($"Destroyed singleton of type {typeof(T).Name}");
    }
}

public abstract class SingletonMod<T> {
    public static T Instance { get; set; }
}

public abstract class SingletonItem<T> {
    public static T Instance { get; set; }
}