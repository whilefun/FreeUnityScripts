using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
// MySingleton
// A very basic singleton that remains persistent across scenes
//
public class MySingleton : MonoBehaviour
{

	// the instance is private so no one outside can mess with it
	private static MySingleton _instance;
    
	// public accessor, so other scripts can call MySingleton.Instance
	public static MySingleton Instance {
		get { return _instance; }
	}


	void Awake()
	{

		// If instance already exists, it means there is another one of this class somewhere in the scene. Destroy this one so we will still only have a single instance.
		if (_instance != null)
		{

			Debug.LogWarning("MySingleton:: Duplicate MySingleton '" + this.gameObject.name + "', deleting duplicate instance.");
			Destroy(this.gameObject);

		}
		else
		{

			// If this was the first instance of the class, save the reference to the instance variable, and optionally move to DontDestroyOnLoad
			_instance = this;
			DontDestroyOnLoad(this.gameObject);

			// And if there's lots of stuff to initialize, optionally do that in another function
			if (!initialized)
			{
				
				initialized = true;
				initialize();
				
			}

		}

	}
	
	private void initialize()
	{
		// TODO
	}
		
}