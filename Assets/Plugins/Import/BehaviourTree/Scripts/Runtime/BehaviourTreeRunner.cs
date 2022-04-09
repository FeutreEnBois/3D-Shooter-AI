using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourTreeRunner : MonoBehaviour
{
    // The main behaviourTree asset
    public BehaviourTree tree;

    //Storage container object to hold game object subsystems
    public Context context;
    // Start is called before the first frame update
    void Start()
    {
        //context = go_context.GetComponent<Context>();
        Context _context = CreateBehaviourTreeContext(context);
        tree = tree.Clone();
        tree.Bind(_context);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (tree)
        {
            tree.Update();
        }
    }

    Context CreateBehaviourTreeContext(Context _context)
    {
        return _context.CreateFromGameObject(gameObject);
    }
}
