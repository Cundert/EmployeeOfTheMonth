using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapScript : MonoBehaviour
{
    // Start is called before the first frame update
    public static MapScript instance;
    public List<Vector2> points;
    
    void loadPoints(){
		points = new List<Vector2>();
		Transform check = transform.GetChild(1);
		foreach(BoxCollider2D box in check.GetComponents<BoxCollider2D>()){
			Vector2 min = box.bounds.min, max = box.bounds.max;
			Vector2 c1 = new Vector2(min.x, max.y), c2 = new Vector2(max.x, min.y);
			points.Add(min + new Vector2(0.1f, 0));
			points.Add(min + new Vector2(-0.1f, 0));
			points.Add(min + new Vector2(0, 0.1f));
			points.Add(min + new Vector2(0, -0.1f));
			points.Add(min + new Vector2(0.1f, 0));
			points.Add(c1 + new Vector2(-0.1f, 0));
			points.Add(c1 + new Vector2(0, 0.1f));
			points.Add(c1 + new Vector2(0, -0.1f));
			points.Add(c1 + new Vector2(0.1f, 0));
			points.Add(max + new Vector2(-0.1f, 0));
			points.Add(max + new Vector2(0, 0.1f));
			points.Add(max + new Vector2(0, -0.1f));
			points.Add(max + new Vector2(0.1f, 0));
			points.Add(c2 + new Vector2(-0.1f, 0));
			points.Add(c2 + new Vector2(0, 0.1f));
			points.Add(c2 + new Vector2(0, -0.1f));
		}
	}
    
    void Start()
    {
        instance = this;
        loadPoints();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnDestroy(){
		instance = null;
	}
}
