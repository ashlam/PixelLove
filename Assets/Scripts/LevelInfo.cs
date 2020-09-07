using UnityEngine;
using System.Collections;

public class LevelInfo : MonoBehaviour
{

    public int levelID;
    public string levelName;
    public GameObject mainChequerObject;
    public float mainCameraSize = 10;
    public GameObject uiRoot;
    public ClassicType classicType = ClassicType.None;

    public Texture2D previewTexture;

    public LevelInfo nextLevel = null;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}