using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PasswordManager : MonoBehaviour
{
    [SerializeField]
    private string password = "";
    [SerializeField]
    private int randomPasswordLength = 4;

    void Awake()
    {
        if (this.password == "")
        {
            this.password = GetRandomPassword(this.randomPasswordLength);
        }
    }

    private static string GetRandomPassword(int length)
    {
        return ((int)Random.Range(0, Mathf.Pow(10, length - 1))).ToString("D" + length);
    }

    public bool VerifyPassword(string attempt)
    {
        return attempt == this.password;
    }

    public string GetPassword()
    {
        return this.password;
    }
}
