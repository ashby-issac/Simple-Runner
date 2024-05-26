using UnityEngine;

public abstract class PanelPopup : PopupBase
{
    public abstract void OnMainMenuClicked();

    public virtual void OnQuitClicked()
    {
        Application.Quit();
    }
}
