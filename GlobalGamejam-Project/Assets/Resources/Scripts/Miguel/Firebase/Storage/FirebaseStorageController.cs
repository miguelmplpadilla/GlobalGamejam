using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Firebase.Storage;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Firebase.Extensions;
using SimpleFileBrowser;

public class FirebaseStorageController : MonoBehaviour
{
    public static FirebaseStorageController instance;
    
    private RawImage rawImage;

    private FirebaseStorage storage;
    private StorageReference storageReference;
    
    private void Awake()
    {
        instance = this;
        rawImage = GetComponent<RawImage>();
    }

    private void Start()
    {
        storage = FirebaseStorage.DefaultInstance;
        storageReference = storage.GetReferenceFromUrl("gs://justanormallife-9c9c1.appspot.com");

        /*FileBrowser.SetFilters(true, new FileBrowser.Filter("Images", ".jpg", ".png"),
            new FileBrowser.Filter("Text Files", ".txt", ".pdf"));

        FileBrowser.SetDefaultFilter(".txt");
        
        FileBrowser.SetExcludedExtensions(".lnk", ".tmp", ".zip", ".rar", ".exe");*/
    }

    public void UploadFile(byte[] bytes, string fileName)
    {
        var metadata = new MetadataChange();
        metadata.ContentType = "text/txt";
        
        StorageReference uploadRef = storageReference.Child("Partidas/"+fileName+".txt");

        Debug.Log("File Upload Started");
        
        uploadRef.PutBytesAsync(bytes, metadata).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError(task.Exception.ToString());
                return;
            }

            Debug.Log("File Uploaded Successfully");
        });
    }

    private IEnumerator ShowLoadDialog()
    {
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.FilesAndFolders, true, null, null,
            "Load Files and Folders", "Load");

        Debug.Log(FileBrowser.Success);

        if (!FileBrowser.Success) yield break;

        foreach (var result in FileBrowser.Result)
            Debug.Log(result);

        Debug.Log("File Selected");
        byte[] bytes = FileBrowserHelpers.ReadBytesFromFile(FileBrowser.Result[0]);
        
        UploadFile(bytes, "PruebaSubida");
    }

    public void ExecuteUploadFile()
    {
        StartCoroutine("ShowLoadDialog");
    }

    private void DownloadImage()
    {
        StorageReference image = storageReference.Child("Kojima_Quieres.png");

        image.GetDownloadUrlAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError(task.Exception);
                return;
            }

            StartCoroutine(LoadImage(Convert.ToString(task.Result)));
        });
    }

    private IEnumerator LoadImage(string mediaUrl)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(mediaUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(request.error);
            yield break;
        }

        rawImage.texture = ((DownloadHandlerTexture) request.downloadHandler).texture;
    }
}
