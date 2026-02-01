using UnityEngine;

public class YLock : MonoBehaviour
{
    [SerializeField] Transform YPosition; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = new Vector3(YPosition.position.x + 197f,transform.position.y,transform.position.z);
    }
}
