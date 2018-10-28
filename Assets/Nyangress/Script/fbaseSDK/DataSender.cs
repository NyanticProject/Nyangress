
using Firebase;
using Firebase.Storage;
using Firebase.Database;
using Firebase.Unity.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Mapbox.Unity.Location;
using Mapbox.Utils;
using UnityEngine;
using UnityEngine.UI;


public class DataPackage
{
    public string timeStamp = "";
    public double latitude = 0.0;
    public double longitude = 0.0;
    public string tags = "";
    public string picURL = "";
}
public class DataSender : MonoBehaviour
{
    //DatabaseReference reference;
    // Use this for initialization
    DatabaseReference reference;
    StorageReference storage_ref;
    FirebaseStorage storage;
    void Start()
    {
        storage = FirebaseStorage.DefaultInstance;
        storage_ref = storage.GetReferenceFromUrl("gs://nyangress-220707.appspot.com");
        //System update Script
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Set a flag here indiciating that Firebase is ready to use by your
                // application.
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });


        // Set up the Editor before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://nyangress-220707.firebaseio.com/");

        // Get the root reference location of the database.
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        

        // Points to the root reference
        
    }

    // Update is called once per frame
    public void Poster(Vector2d location, byte[] pic)
    {
        //init datapackage
        DataPackage package = new DataPackage();

        //write some datas
        package.latitude = location.x;
        package.longitude = location.y;
        package.timeStamp = DateTime.Now.ToString("yyyyMMddhhmmss");

        //upload picture and get urls
        Firebase.Storage.StorageReference pngRef = storage_ref.Child("images").Child(package.timeStamp+".png");

        // Upload the file to the path "images/rivers.jpg"
        pngRef.PutBytesAsync(pic)
          .ContinueWith((Task<StorageMetadata> task) => {
              if (task.IsFaulted || task.IsCanceled)
              {
                  Debug.Log(task.Exception.ToString());
          // Uh-oh, an error occurred!
      }
              else
              {
          // Metadata contains file metadata such as size, content-type, and download URL.
                 StorageMetadata metadata = task.Result;
                  pngRef.GetDownloadUrlAsync().ContinueWith((Task<Uri> task2) => {
                      if (!task2.IsFaulted && !task2.IsCanceled)
                      {
                          Debug.Log("Download URL: " + task2.Result);
                          // ... now download the file via WWW or UnityWebRequest.

                      }
                  });
              }
          });

    }


}
