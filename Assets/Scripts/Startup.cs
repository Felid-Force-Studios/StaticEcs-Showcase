using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Startup : MonoBehaviour {
    public GameObject Canvas;
    public TMP_InputField EntitiesCount;
    public Toggle IsBurst;
    public Toggle IsParallel;
    
    [Header("Showcase")]
    public StarsSerializeShowcase starsSerializeShowcase;
    
    public void StartStarsShowCase() {
        starsSerializeShowcase.Create(int.Parse(EntitiesCount.text), IsBurst.isOn, IsParallel.isOn);
        Canvas.SetActive(false);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape) && !Canvas.activeSelf) {
            starsSerializeShowcase.OnDestroy();
            Canvas.SetActive(true);
        }
    }
}
