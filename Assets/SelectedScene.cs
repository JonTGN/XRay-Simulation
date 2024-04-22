using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SelectedScene
{
    // use to hold value of selected scene so we can restart by just reloading scene and not having to track vars
    public static string sceneType { get; set; } = null;
}
