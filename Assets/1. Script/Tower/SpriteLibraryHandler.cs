using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class SpriteLibraryHandler : MonoBehaviour
{
    private SpriteLibrary spriteLibrary;
    [SerializeField] List<SpriteLibraryAsset> spriteLibraryAssetList;

    private void Start()
    {
        Loadcomponents();
        SetDefaultSprite();
    }

    private void Loadcomponents()
    {
        spriteLibrary = GetComponent<SpriteLibrary>();
    }

    public void UpdateSprite(int towerLevel)
    {
        if (spriteLibraryAssetList[towerLevel - 1] != null)
        {
            spriteLibrary.spriteLibraryAsset = spriteLibraryAssetList[towerLevel - 1];
        }
    }

    public void SetDefaultSprite()
    {
        spriteLibrary.spriteLibraryAsset = spriteLibraryAssetList[0];
    }
}
