using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "New Mail", menuName = "Mail")]
public class MailScriptableObject : ScriptableObject
{
    public string mailName;
    public float mailMass;
    public float mailSpeed;
    public int mailPoints;
}
