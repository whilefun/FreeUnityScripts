using System.Collections.Generic;
using UnityEngine;

namespace Whilefun.Tools
{

#if UNITY_EDITOR

    //
    // WFGTerrainObject
    // A mostly empty class that allows us to find objects in a scene that should be moved by WFGTerrainObjectPlacementToolEditorWindow
    //
    [ExecuteAlways]
    public class WFGTerrainObject : MonoBehaviour
    {

        [SerializeField, Tooltip("The vertical offset this object will have from the terrain. Default is zero.")]
        private float _verticalOffset = 0.0f;
        public float VerticalOffset { get { return _verticalOffset; } }

        private Terrain myTerrain = null;
        private bool isOnTerrain = true;

        private Vector3 mostRecentSnapPosition = Vector2.zero;


        private void locateMyTerrain()
        {

            // We need to find the terrain closest to us. Start with 0th terrain, and only select other terrain if its closer than 0th one.
            Terrain[] allTerrains = Terrain.activeTerrains;
            
            // Note: We use y of our object, rather than terrain so that our distance calculation effectively ignores y delta between terrain and our object.
            Vector3 terrainCenterPosition = new Vector3(allTerrains[0].transform.position.x + allTerrains[0].terrainData.size.x / 2, transform.position.y, allTerrains[0].transform.position.z + allTerrains[0].terrainData.size.z / 2);
            float distanceToNearestTerrain = (terrainCenterPosition - transform.position).sqrMagnitude;
            int terrainIndex = 0;

            for (int i = 0; i < allTerrains.Length; i++)
            {

                // Note: We use y of our object, rather than terrain so that our distance calculation effectively ignores y delta between terrain and our object.
                terrainCenterPosition = new Vector3(allTerrains[i].transform.position.x + allTerrains[i].terrainData.size.x / 2, transform.position.y, allTerrains[i].transform.position.z + allTerrains[i].terrainData.size.z / 2);

                // Check to see if the next terrain is closer than the 0th terrain
                float tempDistance = (terrainCenterPosition - transform.position).sqrMagnitude;

                if (tempDistance < distanceToNearestTerrain)
                {

                    distanceToNearestTerrain = tempDistance;
                    terrainIndex = i;

                }

            }

            // Remember closest one
            myTerrain = allTerrains[terrainIndex];

            // But even if we have a nearest Terrain, we might still not be within the XZ boundary of said Terrain. So we need to check!
            Vector3 centerPosition = new Vector3(myTerrain.transform.position.x + myTerrain.terrainData.size.x / 2, myTerrain.transform.position.y, myTerrain.transform.position.z + myTerrain.terrainData.size.z / 2);
            float minX = centerPosition.x - (myTerrain.terrainData.size.x / 2.0f);
            float maxX = centerPosition.x + (myTerrain.terrainData.size.x / 2.0f);
            float minZ = centerPosition.z - (myTerrain.terrainData.size.z / 2.0f);
            float maxZ = centerPosition.z + (myTerrain.terrainData.size.z / 2.0f);


            if (transform.position.x < minX || transform.position.x > maxX || transform.position.z < minZ || transform.position.z > maxZ)
            {
                isOnTerrain = false;
            }
            else
            {
                isOnTerrain = true;
            }

        }


        public void ForceLocateTerrain()
        {
            locateMyTerrain();
        }


        public void SnapToTerrain()
        {

            if(myTerrain != null)
            {

                // If we moved, assume we may have moved to another terrain
                if (mostRecentSnapPosition.x != transform.position.x || mostRecentSnapPosition.z != transform.position.z)
                {
                    locateMyTerrain();
                }

                Vector3 refreshedPosition = transform.position;
                refreshedPosition.y = myTerrain.SampleHeight(refreshedPosition);
                transform.position = refreshedPosition + new Vector3(0.0f, _verticalOffset, 0.0f);

                mostRecentSnapPosition.x = transform.position.x;
                mostRecentSnapPosition.z = transform.position.z;

            }
            else
            {
                locateMyTerrain();
            }

        }

       

        void OnDrawGizmos()
        {
            
            if(myTerrain != null)
            {

                //Gizmos.color = Color.cyan;
                //Vector3 terrainCenter = new Vector3(myTerrain.transform.position.x + myTerrain.terrainData.size.x / 2, myTerrain.transform.position.y, myTerrain.transform.position.z + myTerrain.terrainData.size.z / 2);
                //Gizmos.DrawLine(transform.position, terrainCenter);

                // If we're off terrain, draw in red 
                if (isOnTerrain == false)
                {

                    Color c = Color.red;
                    c.a = 0.5f;
                    Gizmos.color = c;
                    Gizmos.DrawSphere(transform.position, 5.0f);

                }

            }

        }


    }

#endif

}
