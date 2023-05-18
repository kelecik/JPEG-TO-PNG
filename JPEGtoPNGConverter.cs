using UnityEngine;
using UnityEditor;
using System.IO;

public class JpeGtoPNGConverter : MonoBehaviour
{
    [MenuItem("Assets/Convert to PNG and Delete")]
    private static void ConvertToPngAndDelete()
    {
        string selectedFilePath = AssetDatabase.GetAssetPath(Selection.activeObject);
        if (string.IsNullOrEmpty(selectedFilePath)) return;
        if (selectedFilePath.EndsWith(".jpg") || selectedFilePath.EndsWith(".jpeg"))
        {
            byte[] jpegBytes = File.ReadAllBytes(selectedFilePath);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(jpegBytes);
            byte[] pngBytes = texture.EncodeToPNG();
            string pngFileName = Path.GetFileNameWithoutExtension(selectedFilePath) + ".png";
            string pngFilePath = Path.Combine(Path.GetDirectoryName(selectedFilePath) ?? string.Empty, pngFileName);
            File.WriteAllBytes(pngFilePath, pngBytes);
            AssetDatabase.DeleteAsset(selectedFilePath);
            AssetDatabase.ImportAsset(pngFilePath);
            Object pngAsset = AssetDatabase.LoadAssetAtPath(pngFilePath, typeof(Object));
            Selection.activeObject = pngAsset;
        }
        else if (selectedFilePath.EndsWith(".png"))
        {
            Debug.LogWarning("Selected file is already a PNG file.");
        }
        else
        {
            Debug.LogWarning("Selected file is not a JPEG or PNG file.");
        }
    }
}