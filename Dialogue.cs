using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Diaogue", menuName = "Dialogue")]
public class Dialogue : ScriptableObject
{
    public Line[] dialogue;
}