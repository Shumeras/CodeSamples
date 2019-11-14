using UnityEngine;

public class UnitySingleton<T> : MonoBehaviour 
    where T : MonoBehaviour
{
    public static T _instance;

    public static T instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = Object.FindObjectOfType((typeof(T))) as T;
                if(!_instance)
                {
                    Debug.LogError("There is no instance of " + typeof(T).Name + " created;");
                    return null;
                }
            }

            return _instance;
        }

        private set {}
    }

}
