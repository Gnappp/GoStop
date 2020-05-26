using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameText : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (SignInManager.firebaseUser != null)
            GetComponent<Text>().text = SignInManager.firebaseUser.Email;
    }

    // Update is called once per frame
   
}
