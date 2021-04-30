using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestEvent
{
    public enum EventStatus {Waiting, Current , Done};
    //Waiting = not yet completed but can't be worked on cause there's a prerequiste event
    //Current = the one the player should be trying to achieve
    //Done = has been achieved

    [SerializeField] string name;
    [SerializeField] string description;
    [SerializeField] string id;
    [SerializeField] int order = -1;
    [SerializeField] EventStatus status;

    [SerializeField] List<QuestPath> pathList = new List<QuestPath>();

    public QuestEvent(string newName, string newDescription )
    {
        id = Guid.NewGuid().ToString();
        name = newName;
        description = newDescription;
        status = EventStatus.Waiting;
    }

     public void UpdateQuestEvent(EventStatus es)
    {
        status = es;
    }

    public string GetId(){ return id; }

    public string GetName(){ return name; }

    public int GetOrder() { return order; }

    public void SetOrder(int orderNumber)
    {
        order = orderNumber;
    }

    public List<QuestPath> GetPathList() { return pathList; }

}
