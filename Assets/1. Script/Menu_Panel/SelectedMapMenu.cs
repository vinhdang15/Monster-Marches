using TMPro;
using UnityEngine;

public class SelectedMapMenu : MenuBase
{
    [SerializeField] TextMeshProUGUI mapName;
    [SerializeField] TextMeshProUGUI mapDescription;

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
        gameObject.SetActive(true);
        mapName.text = mapModel.name.ToString();
        mapDescription.text = mapModel.Description.ToString();
    }

}
