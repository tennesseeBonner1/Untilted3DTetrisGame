using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JardGameboard : MonoBehaviour
{
    public GameObject bottomPlane;
    public GameObject NPlane, SPlane, WPlane, EPlane;

    public int gridSizeX, gridSizeY, gridSizeZ;
    public Transform[,,] theGrid;

    private void OnDrawGizmos()
    {
        if (bottomPlane != null)
        {
            // RESIZE BOTTOM PLANE
            Vector3 scaler = new Vector3((float) gridSizeX / 10, 1, (float) gridSizeZ / 10);
            bottomPlane.transform.localScale = scaler;
            
            // REPOSITION
            bottomPlane.transform.position = new Vector3(transform.position.x + (float) gridSizeX / 2,
                                                            transform.position.y,
                                                            transform.position.z + (float) gridSizeZ / 2);
            // RETILE MATERIAL
            bottomPlane.GetComponent<MeshRenderer>().sharedMaterial.mainTextureScale =
                new Vector2(gridSizeX, gridSizeZ);
        }
        
        if (NPlane != null)
        {
            // RESIZE NORTH PLANE
            Vector3 scaler = new Vector3((float) gridSizeX / 10, 1, (float) gridSizeY / 10);
            NPlane.transform.localScale = scaler;
            
            // REPOSITION
            NPlane.transform.position = new Vector3(transform.position.x + (float) gridSizeX / 2,
                transform.position.y + (float)gridSizeY / 2,
                transform.position.z + gridSizeZ);
            // RETILE MATERIAL
            NPlane.GetComponent<MeshRenderer>().sharedMaterial.mainTextureScale =
                new Vector2(gridSizeX, gridSizeY);
        }
        
        if (SPlane != null)
        {
            // RESIZE NORTH PLANE
            Vector3 scaler = new Vector3((float) gridSizeX / 10, 1, (float) gridSizeY / 10);
            SPlane.transform.localScale = scaler;
            
            // REPOSITION
            SPlane.transform.position = new Vector3(transform.position.x + (float) gridSizeX / 2,
                transform.position.y + (float)gridSizeY / 2,
                transform.position.z);
            // RETILE MATERIAL
            //SPlane.GetComponent<MeshRenderer>().sharedMaterial.mainTextureScale =
                //new Vector2(gridSizeX, gridSizeY);
        }
        
        if (EPlane != null)
        {
            // RESIZE NORTH PLANE
            Vector3 scaler = new Vector3((float) gridSizeZ / 10, 1, (float) gridSizeY / 10);
            EPlane.transform.localScale = scaler;
            
            // REPOSITION
            EPlane.transform.position = new Vector3(transform.position.x + gridSizeX,
                transform.position.y + (float)gridSizeY / 2,
                transform.position.z + (float)gridSizeZ / 2);
            // RETILE MATERIAL
            EPlane.GetComponent<MeshRenderer>().sharedMaterial.mainTextureScale =
                new Vector2(gridSizeZ, gridSizeY);
        }
        
        if (WPlane != null)
        {
            // RESIZE NORTH PLANE
            Vector3 scaler = new Vector3((float) gridSizeZ / 10, 1, (float) gridSizeY / 10);
            WPlane.transform.localScale = scaler;
            
            // REPOSITION
            WPlane.transform.position = new Vector3(transform.position.x,
                transform.position.y + (float)gridSizeY / 2,
                transform.position.z + (float)gridSizeZ / 2);
            // RETILE MATERIAL
            //WPlane.GetComponent<MeshRenderer>().sharedMaterial.mainTextureScale =
                //new Vector2(gridSizeZ, gridSizeY);
        }
    }
}
