using TMPro;
using UnityEngine;

public class Startup : MonoBehaviour {
    public GameObject Canvas;
    public TMP_InputField EntitiesCount;
    
    [Header("Showcase")]
    public StarsSerializeShowcase starsSerializeShowcase;
    
    public void StartStarsShowCase() {
        starsSerializeShowcase.Create(int.Parse(EntitiesCount.text));
        Canvas.SetActive(false);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape) && !Canvas.activeSelf) {
            starsSerializeShowcase.OnDestroy();
            Canvas.SetActive(true);
        }
    }
}
