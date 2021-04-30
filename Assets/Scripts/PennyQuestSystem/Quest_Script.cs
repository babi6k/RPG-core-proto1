using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest_Script 
{
    [SerializeField] List<QuestEvent> questEvents = new List<QuestEvent>();

    public Quest_Script() {}

    public QuestEvent AddQuestEvent(string name , string description )
    {
        QuestEvent questEvent = new QuestEvent(name,description);
        questEvents.Add(questEvent);
        return questEvent;
    }

    public void AddPath(string fromQuestEvent, string toQuestEvent)
    {
        QuestEvent from = FindQuestEvent(fromQuestEvent);
        QuestEvent to = FindQuestEvent(toQuestEvent);

        if (from != null && to != null)
        {
            QuestPath path = new QuestPath(from, to);
            from.GetPathList().Add(path);
        }
    }
    //Breadth first search
    public void BFS(string id, int orderNumber = 1)
    {
        QuestEvent thisEvent = FindQuestEvent(id);
        thisEvent.SetOrder(orderNumber);

        foreach(QuestPath path in thisEvent.GetPathList())
        {
            if (path.GetEndEvent().GetOrder() == -1)
            {
                BFS(path.GetEndEvent().GetId(), orderNumber + 1);
            }
        }

    }

    public void PrintPath()
    {
        foreach (QuestEvent questEvent in questEvents)
        {
            Debug.Log(questEvent.GetName() + " " + questEvent.GetOrder());
        }
    }

    private QuestEvent FindQuestEvent(string id)
    {
        foreach (QuestEvent questEvent in questEvents)
        {
            if (questEvent.GetId() == id)
            {
                return questEvent;
            }
        }
        return null;
    }
}
