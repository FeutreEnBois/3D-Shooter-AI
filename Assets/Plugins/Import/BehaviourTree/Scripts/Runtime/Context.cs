using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// The context is a shared object every node has access to.
// Commonly used components and subsytems should be stored here
public class Context
{
    public virtual Context CreateFromGameObject(GameObject gameObject)
    {
        // Fetch all commonly used components
        Context context = new Context();
        return context;
    }
}
