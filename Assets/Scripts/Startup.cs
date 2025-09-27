using System;
using TMPro;
using UnityEngine;
#if ENABLE_IL2CPP
using Unity.IL2CPP.CompilerServices;
#endif

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

#if ENABLE_IL2CPP
namespace Unity.IL2CPP.CompilerServices {
    using System;

    internal enum Option {
        NullChecks = 1,
        ArrayBoundsChecks = 2,
        DivideByZeroChecks = 3
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = true)]
    internal class Il2CppSetOptionAttribute : Attribute {
        public Option Option { get; }
        public object Value { get; }

        public Il2CppSetOptionAttribute(Option option, object value) {
            Option = option;
            Value = value;
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false)]
    internal class Il2CppEagerStaticClassConstructionAttribute : Attribute { }
}
#endif
