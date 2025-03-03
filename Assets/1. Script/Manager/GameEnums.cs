using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnums : MonoBehaviour
{

}

public enum InitNameObject
{
    Camera,
    CanvasScreenSpace,
    CanvasWorldSpace,
    InitMenu,
    UpgradeMenu,
    CheckSymbol,
    PauseMenu,
    VictoryMenu,
    GameOverMenu,
    CurrentSttPanel,
    UpgradeSttPanel,
    GameSttPanel,

    SpawnEnemyManager,
    UnitPool,
    BulletPool,

    FPSCounter,
    EmptyPlotManager,
    BulletTowerManager,
    BarrackTowerManager,
    SoldierManager,
    BulletManager,
    EnemyManager,
    InputController,
    RaycastHandler,
    InputButtonHandler,
    TowerActionHandler,
    GamePlayManager,
    PanelManager,
    CautionManager
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

public enum BulletTypea
{
    Arrow1,
    Arrow2,
    MagicBall1,
    MagicBall2,
    Bomb1,
    Bomb2,
}