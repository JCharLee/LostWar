using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteraction
{
    string interactionPrompt { get; }
    float CastingTime { get; }
    Coroutine CastRoutine { get; }
    Coroutine MoveRoutine { get; }

    bool Action(PlayerInteraction interactor);
}
