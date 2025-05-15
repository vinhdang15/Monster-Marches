using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnums : MonoBehaviour
{

}

public enum AddressLabel
{
    JSON,
    MapImage,
    Unit,
    DecorObject,
}

public enum InitNameObject
{
    CanvasWorldSpace,
}

public enum TowerType
{
    ArcherTower = 0,
    MageTower = 1,
    CannonTower = 2,
    Barrack = 3,
    TowerSell,
    TowerUpgrade,
}

public enum BulletType
{
    Arrow_1,
    Arrow_2,
    MagicBall_1,
    MagicBall_2,
    Bomb_1,
    Bomb_2,
}