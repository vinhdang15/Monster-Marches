using UnityEngine;

public class GameEnums : MonoBehaviour
{

}

public enum AddressLabel
{
    JSON,
    BackgroundImage,
    InstructionImage,
    MapImage,
    Unit,
    DecorObject,
}

public enum ImageName
{
    BackgroundIntro,
    BackgroundWorldMap,
    Instruction_1,
    Instruction_2
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
}

public enum BulletID
{
    Arrow_1,
    Arrow_2,
    MagicBall_1,
    MagicBall_2,
    Bomb_1,
    Bomb_2,
}

public enum BulletEffectType
{
    none,
    Fire_DoT,
    Slow,
    Magic_DoT,
    Bomb_1_AoE20,
    Bomb_2_AoE30,
}

public enum Unitype
{
    Soldier,
    Enemy,
}

public enum UnitID
{
    none,
    Soldier_1,
    Soldier_2,
    Enemy_C_1,
    Enemy_C_2,
    Enemy_B_1,
    Enemy_A_1,
}

public enum UnitSkill
{
    none,
    SelfHealing,
    Archery,
}