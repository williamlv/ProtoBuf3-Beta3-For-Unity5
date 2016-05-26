using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]

public class ZOpenFileName
{
    public int structSize = 0;
    public IntPtr dlgOwner = IntPtr.Zero;
    public IntPtr instance = IntPtr.Zero;
    public String filter = null;
    public String customFilter = null;
    public int maxCustFilter = 0;
    public int filterIndex = 0;
    public String file = null;
    public int maxFile = 0;
    public String fileTitle = null;
    public int maxFileTitle = 0;
    public String initialDir = null;
    public String title = null;
    public int flags = 0;
    public short fileOffset = 0;
    public short fileExtension = 0;
    public String defExt = null;
    public IntPtr custData = IntPtr.Zero;
    public IntPtr hook = IntPtr.Zero;
    public String templateName = null;
    public IntPtr reservedPtr = IntPtr.Zero;
    public int reservedInt = 0;
    public int flagsEx = 0;
}

public class ZGetOpenFileName
{
    [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    public static extern bool GetOpenFileName([In, Out] ZOpenFileName ofn);
    public static string OpenSelectFile([In] string defExt)
    {
        ZOpenFileName ofn = new ZOpenFileName();
        ofn.structSize = Marshal.SizeOf(ofn);
        ofn.filter = defExt;
        ofn.file = new string(new char[256]);
        ofn.maxFile = ofn.file.Length;
        ofn.fileTitle = new string(new char[64]);
        ofn.maxFileTitle = ofn.fileTitle.Length;
        ofn.initialDir = UnityEngine.Application.dataPath;//默认路径
        ofn.title = "Select File";
        ofn.defExt = defExt;
        //注意 一下项目不一定要全选 但是0x00000008项不要缺少
        ofn.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;//OFN_EXPLORER|OFN_FILEMUSTEXIST|OFN_PATHMUSTEXIST| OFN_ALLOWMULTISELECT|OFN_NOCHANGEDIR
        if (GetOpenFileName(ofn))
        {
            return ofn.file;
        }
        return string.Empty;
    }
}
/// Example
/*
 *"All Files\0*.*\0Image Files\0*.JPG;*.PNG;*.BMP\0\0"
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
*/
