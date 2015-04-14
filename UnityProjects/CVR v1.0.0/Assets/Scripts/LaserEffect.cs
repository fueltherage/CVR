using UnityEngine;
using System.Collections;

public class LaserEffect : MonoBehaviour {

  
    public float lineLife = 2.0f;
    public Vector2 scrollSpeed;
    Color originalColor;
    LineRenderer lineRend;
    
	// Use this for initialization
	void Start () {   
        
        lineRend = GetComponent<LineRenderer>();
        originalColor = lineRend.material.GetColor("_TintColor");
	}

    float elapsedTime = 0;
    float currentTime; 
	// Update is called once per frame
	void Update () {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            elapsedTime += Time.deltaTime;
            Color col = originalColor;
            col.a = Mathf.Lerp(originalColor.a, 0.0f, 1 / (lineLife / elapsedTime));
            lineRend.material.SetColor("_TintColor", col);
            Vector2 offSet = new Vector2(elapsedTime * scrollSpeed.x, elapsedTime * scrollSpeed.y);
            lineRend.material.SetTextureOffset("_MainTex", offSet);    
            lineRend.enabled = true;
        }
        else lineRend.enabled = false;
	}
    void StartEffect()
    {
        lineRend.enabled = true;
        elapsedTime = 0;
        currentTime = lineLife;
    }
    public IEnumerator lineTimer()
    {
        yield return new WaitForSeconds(lineLife);
        lineRend.enabled = false;
    }   
    public void go(Vector3 Start, Vector3 End)
    {
        StartEffect();
        lineRend.SetPosition(0, Start);
        lineRend.SetPosition(1, End);
        
    }
}
