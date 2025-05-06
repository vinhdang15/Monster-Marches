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
    // MapDesignData,
    // MapProgressData,
    // WayPointData,
    // TowerData,
    // BulletData,
    // BulletEffectData,
    // UnitData,
    // SkillData,
    // EnemyWaveData
}

public enum UnitID
{
    Monster_1,
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

    EnemySpawnerManager,
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
    Arrow_1,
    Arrow_2,
    MagicBall_1,
    MagicBall_2,
    Bomb_1,
    Bomb_2,
}