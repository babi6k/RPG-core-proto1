using System;
using RPG.Core;

namespace RPG.Abilities.Helpers
{
public class LambdaAction : IAction
{
    Action activate;
    Action cancel;

    public LambdaAction(Action newActivate, Action newCancel = null)
    {
        activate = newActivate;
        cancel = newCancel;
    }

    public void Activate()
    {
        activate();
    }

    public void Cancel()
    {
       cancel();
    }
}
}
