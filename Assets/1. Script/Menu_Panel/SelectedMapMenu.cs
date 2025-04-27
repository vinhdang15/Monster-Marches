using TMPro;
using UnityEngine;

public class SelectedMapMenu : MenuBase
{
    [SerializeField] TextMeshProUGUI mapName;
    [SerializeField] TextMeshProUGUI mapDescription;
    [SerializeField] SelectedMapIcon mapIcon;

    public void ShowSelectedMapMenu(MapModel mapModel)
    {
        SetMapValue(mapModel);
        base.Show();
    }

    public override void Hide()
    {
        base.Hide();
    }

    private void SetMapValue(MapModel mapModel)
    {
        mapName.text = mapModel.name.ToString();
        mapDescription.text = mapModel.Description.ToString();
        mapIcon.LoadSelectedMapIcon(mapModel.MapID.ToString());
        gameObject.SetActive(true);
    }

}
