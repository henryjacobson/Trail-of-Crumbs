using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PasswordManager : MonoBehaviour
{
    [SerializeField]
    private string password = "";
    [SerializeField]
    private int randomPasswordLength = 4;

    void Start()
    {
        if (this.password == "")
        {
            this.password = GetRandomPassword(this.randomPasswordLength);
        }
    }

    private static string GetRandomPassword(int length)
    {
        return ((int)Random.Range(0, Mathf.Pow(10, length))).ToString();
    }

    void Update()
    {

    }
}
