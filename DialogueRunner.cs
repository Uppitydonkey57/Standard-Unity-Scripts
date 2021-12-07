using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using System.ComponentModel;

public class DialogueRunner : MonoBehaviour
{
    private List<Line> que;

    [SerializeField] private TextMeshProUGUI text;

    private bool hasStarted;

    private bool skipLine;

    [SerializeField] private float dialogueTime;

    private bool runningDialogue = false;

    public UnityEvent onDialogueEnd;

    Key key;

    // Start is called before the first frame update
    void Start()
    {
        if (que == null)
            que = new List<Line>();

        key = new Key();
    }

    // Update is called once per frame
    void Update()
    {
        if (que.Count > 0 && !hasStarted && !runningDialogue)
        {
            StartCoroutine(RunLine(que[0]));
            que.RemoveAt(0);
            hasStarted = true;
            runningDialogue = true;
        }

        if (que.Count > 0 && !hasStarted && Input.GetButtonDown("Interact") && runningDialogue)
        {
            StartCoroutine(RunLine(que[0]));
            que.RemoveAt(0);
            hasStarted = true;
            runningDialogue = true;
        }

        if (que.Count <= 0 && !hasStarted && runningDialogue && Input.GetButtonDown("Interact"))
        {
            runningDialogue = false;
            onDialogueEnd.Invoke();
        }


        if (Input.GetButtonDown("Interact") && hasStarted && runningDialogue)
        {
            skipLine = true;
        }
    }

    public void PushLine(Line line)
    {
        if (que == null)
        {
            que = new List<Line>();
        }

        que.Add(line);
    }

    public void PushDialogue(Dialogue dialogue)
    {
        foreach (Line line in dialogue.dialogue)
        {
            que.Add(line);
        }
    }

    IEnumerator RunLine(Line line)
    {
        string[] commandCheck = line.dialogue.Split(key.commandStart);

        Debug.Log(commandCheck[0]);

        if (commandCheck.Length > 0 && commandCheck[0] == "")
        {
            key.ProcessCommand(line.dialogue, FindObjectOfType<GameMaster>()); //REMOVE GM LATER
            hasStarted = false;
            yield break;
        }

        text.text = "";

        int currentCharacter = 0;

        while (currentCharacter < line.dialogue.Length)
        {
            text.text += line.dialogue[currentCharacter];
            yield return new WaitForSeconds(dialogueTime * (line.speedMultiplier + 1));

            if (skipLine)
            {
                text.text = line.dialogue;
                skipLine = false;
                break;
            }

            currentCharacter++;
        }

        hasStarted = false;
    }
}

[System.Serializable]
public class Line
{
    public string dialogue;
    [Tooltip("0 is the default speed of your DialogueRunner.")][Range(-0.9f, 1f)]public float speedMultiplier;

    [HideInInspector] public UnityEvent actions;

    public Line(string dialogue, float speedMultiplier = 0f,  UnityEvent actions = null)
    {
        this.dialogue = dialogue;
        this.actions = actions;
        this.speedMultiplier = speedMultiplier;
    }
}

class Key
{
    public char commandStart = '/';

    public void ProcessCommand(string command)
    {
        string[] splitString = command.Split(commandStart, ' ');

        switch (splitString[1])
        {

        }
    }
}