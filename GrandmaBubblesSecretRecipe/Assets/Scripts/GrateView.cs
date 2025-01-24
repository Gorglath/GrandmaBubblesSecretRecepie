using UnityEngine;
using UnityEngine.UI;

public class GrateView : MonoBehaviour
{
    [SerializeField]
    private Image barFillImage;

    [SerializeField]
    private Vector3 offset;

    private GrateState state;

    public void Bind(GrateState state)
    {
        this.state = state;
    }

    private void Update()
    {
        if(state == null)
        {
            Destroy(this.gameObject);
            return;
        }
        barFillImage.fillAmount = state.grateValue;
        transform.position = state.transform.position + offset;
    }
}
