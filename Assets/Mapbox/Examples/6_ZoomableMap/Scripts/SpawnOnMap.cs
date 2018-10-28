namespace Mapbox.Examples
{
	using UnityEngine;
	using Mapbox.Utils;
	using Mapbox.Unity.Map;
	using Mapbox.Unity.MeshGeneration.Factories;
	using Mapbox.Unity.Utilities;
    using Mapbox.Unity.Location;
	using System.Collections.Generic;
    using Firebase.Database;
    using Firebase.Unity.Editor;
    using Firebase;
    using System;
    using System.Threading.Tasks;
    using System.Threading;


    public class locationData {
        public double lat = 0;
        public double lot = 0;
        public string urls = "";
        public string tags = "";

    }
	public class SpawnOnMap : MonoBehaviour
	{
		[SerializeField]
		AbstractMap _map;

		[SerializeField]
		[Geocode]
		string[] _locationStrings;
		Vector2d[] _locations;

		[SerializeField]
		float _spawnScale = 100f;

		[SerializeField]
		GameObject _markerPrefab;

		List<GameObject> _spawnedObjects;
        public ImmediatePositionWithLocationProvider provider;

        DatabaseReference reference = null;
        List<locationData> locations = new List<locationData>();
        private void Awake()
        {
            FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://nyangress-220707.firebaseio.com/");
            reference = FirebaseDatabase.DefaultInstance.RootReference;
        }
        async void Start()
		{
            
             await reference.Child("Cat").GetValueAsync().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    // Handle the error...
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result;
                    Debug.Log("has Child " + snapshot.HasChildren + " number : " + snapshot.ChildrenCount);
                    IEnumerator<DataSnapshot> nameList = snapshot.Children.GetEnumerator();

                    while (nameList.MoveNext())
                    {
                        Debug.Log("Load Start");
                        try
                        {
                            DataSnapshot data = nameList.Current;
                            locationData dump = new locationData();
                            dump.lat = Convert.ToDouble(data.Child("latitude").Value.ToString());
                            dump.lot = Convert.ToDouble(data.Child("longitude").Value.ToString());
                            dump.tags = data.Child("tags").Value.ToString();
                            dump.urls = data.Child("picURL").Value.ToString();
                            locations.Add(dump);
                        }
                        catch (Exception e)
                        {
                            Debug.Log("ERROR in load data");
                        }
                        finally {
                            //nothing to do 
                        }
                    }

                    Debug.Log("Load completed");
                    _locations = new Vector2d[_locationStrings.Length];
                    // _spawnedObjects = new List<GameObject>();
                    Debug.Log("Load makers");
                    
                }
            });

            foreach (locationData current in locations)
            {
                Vector2d targetPoint = new Vector2d(current.lat, current.lot);
                var instance = Instantiate(_markerPrefab);
                instance.transform.localPosition = _map.GeoToWorldPosition(targetPoint, true);
                instance.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
                // _spawnedObjects.Add(instance);
            }
            Debug.Log("Done");

        }

        private void Update()
        {


        }

        /*
        void LateUpdate()
		{
			int count = _spawnedObjects.Count;
			for (int i = 0; i < count; i++)
			{
				var spawnedObject = _spawnedObjects[i];
				var location = _locations[i];
				spawnedObject.transform.localPosition = _map.GeoToWorldPosition(location, true);
				spawnedObject.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
			}
		}*/
	}
}