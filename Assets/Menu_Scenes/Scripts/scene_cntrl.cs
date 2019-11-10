using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class scene_cntrl : MonoBehaviour
{
    public string SceneName;
    public float DelayTimer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseUp()
    {
        Invoke("NextScene", DelayTimer);
    }

    void NextScene()
    {
        SceneManager.LoadScene(SceneName);
    }
}
