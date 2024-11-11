using UnityEngine;

public class TowerBase : MonoBehaviour
{
    string  towerType;
    int     level;
    string  bulletType;
    float   fireRate;
    float   rangeDetech;
    float   rangeRaycast;
    int     goldRequired;
    string  descriptions;
    
    public string   TowerType     { get => towerType; protected set => towerType = value; }
    public int      Level         { get => level; protected set => level = value; }
    public int      GoldRequired  { get => goldRequired; protected set => goldRequired = value; }
    public string   BulletType    { get => bulletType; protected set => bulletType = value; }
    public float    RangeDetect   { get => rangeDetech; protected set => rangeDetech = value; }
    public float    RangeRaycast  { get => rangeRaycast; protected set => rangeRaycast = value; }
    public float    FireRate      { get => fireRate; protected set => fireRate = value; }
    public string   Descriptions  { get => descriptions; protected set => descriptions = value; }

    public virtual void UpgradeTowerModel(TowerData _data) { }
}