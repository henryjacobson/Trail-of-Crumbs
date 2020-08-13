using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(TMP_InputField))]
public class PasswordScreen : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField inputField;

    void Start()
    {
        this.inputField = this.GetComponent<TMP_InputField>();
    }

    void Update()
    {
        this.inputField.caretPosition = this.inputField.text.Length;
    }
}
