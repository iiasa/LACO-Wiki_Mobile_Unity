using UnityEngine;
using UnityEngine.UI;

public class LoadStringFromScriptExample : MonoBehaviour
{
    public Text text;

    //****** Here is how you get strings at runtime. ******
    private void Start ()
    {
        //Make sure to always load strings before you use them! By default, they are loaded in Awake() method of LocalizationSupport prefab.
        //You can load them manually by calling: LocalizationSupport.LoadStrings();
        text.text = LocalizationSupport.GetString("Procedural");
	}
}