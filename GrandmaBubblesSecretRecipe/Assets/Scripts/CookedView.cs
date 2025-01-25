using UnityEngine;
using UnityEngine.UI;

public class CookedView : MonoBehaviour
{
    [SerializeField]
    private Image barFillImage;

    [SerializeField]
    private Vector3 offset;
    private Cooked state;

    public void Bind(Cooked state)
    {
        this.state = state;
    }

    private void Update()
    {
        if (state == null)
        {
            Destroy(this.gameObject);
            return;
        }
        barFillImage.fillAmount = Mathf.Clamp01(state.cookValue);
        transform.position = state.transform.position + offset;
    }
}
