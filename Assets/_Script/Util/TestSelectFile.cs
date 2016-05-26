using UnityEngine;
using System.Collections;

public class TestSelectFile : MonoBehaviour
{
    public GameObject plane;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 100, 35), "OpenDialog"))
        {
            string fileName = ZGetOpenFileName.OpenSelectFile("All Files\0*.*\0Image Files\0*.JPG;*.PNG;*.BMP\0\0");
            Debug.Log(fileName);
            if (fileName != string.Empty)
            {
                StartCoroutine(WaitLoad(fileName));
            }
        }
    }

    IEnumerator WaitLoad(string fileName)
    {
        WWW wwwTexture = new WWW("file://" + fileName);

        Debug.Log(wwwTexture.url);

        yield return wwwTexture;

        plane.GetComponent<Renderer>().sharedMaterial.mainTexture = wwwTexture.texture;
    }
}
