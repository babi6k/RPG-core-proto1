using System.Collections;
using UnityEngine;

public class QuestPath 
{
   [SerializeField] QuestEvent startEvent;
   [SerializeField] QuestEvent endEvent;

   public QuestPath(QuestEvent from, QuestEvent to)
   {
       startEvent = from;
       endEvent = to;
   }

   public QuestEvent GetStartEvent() { return startEvent; }
   public QuestEvent GetEndEvent() { return endEvent; }
}
