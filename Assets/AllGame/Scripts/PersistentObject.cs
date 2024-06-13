using UnityEngine;

public class PersistentObject : MonoBehaviour
{
    public LoadingScreenController loadingScreenController;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        if (loadingScreenController == null)
        {
            loadingScreenController = FindObjectOfType<LoadingScreenController>();
        }
    }
}
