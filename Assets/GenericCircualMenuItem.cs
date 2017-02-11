using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenericCircualMenuItem : MonoBehaviour {
    public Button myButton;
    public void HighlightButton(bool _show)
    {
        myButton.Select();
    }
}
