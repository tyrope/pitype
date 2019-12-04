using UnityEngine;

public class DontDestroyOnSceneChange : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
