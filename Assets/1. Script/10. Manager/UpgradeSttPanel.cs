using TMPro;
using UnityEngine;

public class UpgradeSttPanel : UIElementBase
{
    [SerializeField] TextMeshProUGUI descriptionText;
    [SerializeField] TextMeshProUGUI towerDamageText;
    [SerializeField] TextMeshProUGUI spawnRateText;

    public void SetUpgradeSttText(TowerPresenter towerPresenter)
    {
        if(towerPresenter.towerModel.Level == 2) return;
        //int currentLevel = tower.currentLevel;
        descriptionText.text = towerPresenter.DescriptionUpgrade.ToString();
        towerDamageText.text = towerPresenter.TowerDamageUpgrade.ToString();
        spawnRateText.text = towerPresenter.TowerSpawnRateUpgrade.ToString() + "s";
        
    }

    public override void ShowInPos(Vector2 pos)
    {
        float offsetInXAxis = 6f;
        if(pos.x >= 0)
        {
            offsetInXAxis = Mathf.Abs(offsetInXAxis) * -1;
        }
        else
        {
            offsetInXAxis = Mathf.Abs(offsetInXAxis);
        }
        float x = pos.x + offsetInXAxis;
        float y = pos.y;
        transform.position = new Vector2(x, y);

        gameObject.SetActive(true);
    }
}
