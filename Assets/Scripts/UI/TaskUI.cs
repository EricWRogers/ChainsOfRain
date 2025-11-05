using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class TaskUI : MonoBehaviour
{
    public TMP_Text taskText;
    public List<string> tasks;
    private int currentTaskIndex = 0;
    private float time = 0f;

    public TaskUI instance;


    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        if (tasks.Count == 0) return;

        taskText.text = tasks[0];
    }
    public void NextTask()
    {
        currentTaskIndex++;
        if (currentTaskIndex >= tasks.Count) return;

        taskText.text = "<s>" + taskText.text + "</s>\n" + tasks[currentTaskIndex];

        
    }

}
