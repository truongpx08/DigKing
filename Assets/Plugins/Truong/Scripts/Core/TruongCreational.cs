using UnityEngine;

public class TruongCreational : TruongUtils
{
    protected void FlipGo(Component go)
    {
        if (go == null) return;
        if (go.gameObject == null) return;
        var goTransform = go.transform;
        var ls = goTransform.localScale;
        goTransform.localScale = new Vector3(ls.x * -1, ls.y, ls.z);
    }

    protected void EnableGo(Transform go)
    {
        if (go == null) return;
        EnableGo(go.gameObject);
    }

    protected void EnableGo(GameObject go)
    {
        if (go == null) return;
        go.SetActive(true);
    }

    protected void DisableGo(Transform go)
    {
        if (go == null) return;
        DisableGo(go.gameObject);
    }

    protected void EnableGo(Component go)
    {
        if (go == null) return;
        if (go.gameObject == null) return;
        go.gameObject.SetActive(true);
    }

    protected void DisableGo(GameObject go)
    {
        if (go == null) return;
        go.SetActive(false);
    }

    protected void DisableGo(Component go)
    {
        if (go == null) return;
        if (go.gameObject == null) return;
        go.gameObject.SetActive(false);
    }

    protected void SetName(string value)
    {
        this.name = value;
    }

    protected virtual void Disable()
    {
        this.gameObject.SetActive(false);
    }

    protected void DisableAllChildren()
    {
        for (int index = 0; index < this.transform.childCount; index++)
        {
            DisableGo(transform.GetChild(index));
        }
    }
}