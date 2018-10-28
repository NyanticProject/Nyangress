using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Mapbox.Unity.Location;
using Firebase;
using Firebase.Unity.Editor;
using Firebase.Storage;
using Firebase.Database;
using System.Threading;
using System.Threading.Tasks;
using System;
using Mapbox.Examples;

public class CameraSystems : MonoBehaviour
{

    int width ;
    int height ;
    int fps = 30;
    Texture2D texture;
    WebCamTexture webcamTexture;
    Color32[] colors = null;
    [SerializeField]
    private RawImage m_displayUI = null;
    public byte[] png;
    public ImmediatePositionWithLocationProvider users;
    ILocationProvider location;
    IEnumerator Init()
    {
        while (true)
        {
            if (webcamTexture.width > 16 && webcamTexture.height > 16)
            {
                colors = new Color32[webcamTexture.width * webcamTexture.height];
                texture = new Texture2D(webcamTexture.width, webcamTexture.height, TextureFormat.RGBA32, false);
                m_displayUI.texture = texture;
                break;
            }
            yield return null;
        }
    }


    //DatabaseReference reference;
    // Use this for initialization
    DatabaseReference reference;
    StorageReference storage_ref;
    FirebaseStorage storage;

    // Use this for initialization
    void Start()
    {
        storage = FirebaseStorage.DefaultInstance;
        storage_ref = storage.GetReferenceFromUrl("gs://nyangress-220707.appspot.com");
        // Set up the Editor before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://nyangress-220707.firebaseio.com/");

        // Get the root reference location of the database.
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        if (Application.platform == RuntimePlatform.Android)
        {
            // Android
           // GetComponent<RectTransform>().Rotate(new Vector3(0, 0, -90));
            width = (int)gameObject.GetComponent<RectTransform>().sizeDelta.y;
            height = (int)gameObject.GetComponent<RectTransform>().sizeDelta.x;
        }
        else {
            width = (int)gameObject.GetComponent<RectTransform>().sizeDelta.x;
            height = (int)gameObject.GetComponent<RectTransform>().sizeDelta.y;
        }

        WebCamDevice[] devices = WebCamTexture.devices;
        webcamTexture = new WebCamTexture(devices[0].name, this.width, this.height, this.fps);
        m_displayUI.texture = webcamTexture;

        webcamTexture.Play();

        StartCoroutine(Init());
    }

    // Update is called once per frame
    void Update()
    {

            webcamTexture.GetPixels32(colors);
            texture.SetPixels32(colors);
            texture.Apply();
    }


    public void takeAPic(Button button)
    {
        //disable button function
        button.interactable = false;
        try
        {

            webcamTexture.GetPixels32(colors);
            texture.SetPixels32(colors);
            texture.Apply();
            png = texture.EncodeToPNG();

            //init datapackage
            DataPackage package = new DataPackage();

            //write some datas
            package.latitude = users.LocationProvider.CurrentLocation.LatitudeLongitude.x;
            package.longitude = users.LocationProvider.CurrentLocation.LatitudeLongitude.y;
            package.timeStamp = DateTime.Now.ToString("yyyyMMddhhmmss");

            //upload picture and get urls
            Firebase.Storage.StorageReference pngRef = storage_ref.Child("images").Child(package.timeStamp + ".png");

            // Upload the file to the path "images/rivers.jpg"
            pngRef.PutBytesAsync(png).ContinueWith((Task<StorageMetadata> task) =>
            {
                if (task.IsFaulted || task.IsCanceled)
                {
                    Debug.Log(task.Exception.ToString());
                    // Uh-oh, an error occurred!
                }
                else
                {
                    // Metadata contains file metadata such as size, content-type, and download URL.
                    StorageMetadata metadata = task.Result;
                    pngRef.GetDownloadUrlAsync().ContinueWith((Task<Uri> task2) =>
                    {
                        if (!task2.IsFaulted && !task2.IsCanceled)
                        {
                            Debug.Log("Download URL: " + task2.Result);
                            package.picURL = task2.Result.ToString();
                            string json = JsonUtility.ToJson(package);
                            //make location of containing data
                            string key = reference.Child("Cat").Push().Key;
                            reference.Child("Cat").Child(key).SetRawJsonValueAsync(json);
                        }
                    });
                }
            });

        }
        catch (Exception e)
        {
            throw;
        }
        finally {
            //eable button function
            button.interactable = true;
        }

    }
} 