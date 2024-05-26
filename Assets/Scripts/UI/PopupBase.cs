using UnityEngine;

public abstract class PopupBase : MonoBehaviour
{
    [SerializeField] protected UIPanel PanelType;

    protected bool isInitialized = false;
    public UIPanel UIPanelType => PanelType;

    public abstract void Initialize();
}
