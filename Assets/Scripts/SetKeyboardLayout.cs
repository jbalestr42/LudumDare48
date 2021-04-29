using UnityEngine;
using UnityEngine.InputSystem;

public class SetKeyboardLayout : MonoBehaviour {
    void Start()
    {
        //This checks if your computer's operating system is in the French language
        if (Application.systemLanguage == SystemLanguage.French) {
            //Outputs into console that the system is French
            Debug.Log("This system is in French. ");
        }
        //Otherwise, if the system is English, output the message in the console
        else if (Application.systemLanguage == SystemLanguage.English) {
            Debug.Log("This system is in English. ");
        }
    }
}