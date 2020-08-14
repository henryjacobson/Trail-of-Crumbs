using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayPassword : MonoBehaviour
{
    [SerializeField]
    private List<bool> charactersToShow;

    [SerializeField]
    private TMP_Text text;

    void Start()
    {
        string toShow = this.ParsePassword(FindObjectOfType<PasswordManager>().GetPassword());
        this.SetText(toShow);
    }

    private string ParsePassword(string password)
    {
        string result = "";
        for(int i = 0; i < password.Length && i < this.charactersToShow.Count; i++)
        {
            result += this.charactersToShow[i] ? password[i].ToString() : "";
        }
        return result;
    }

    private void SetText(string password)
    {
        this.text.text = password;
    }

    public string GetText()
    {
        return this.text.text;
    }
}
