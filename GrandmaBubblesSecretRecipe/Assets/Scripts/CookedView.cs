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
        barFillImage.fillAmount = state.cookValue/2.0f;
        transform.position = state.transform.position + offset;
    }
}
