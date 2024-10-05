using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    [SerializeField] private Transform targetA, targetB;
    [SerializeField] private float moveSpeed = 2;
    
    private Transform currentTarget;
    // Start is called before the first frame update
    void Start()
    {
        currentTarget = targetA;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(transform.position == targetA.position)
        {
            currentTarget = targetB;
        }    
        
        if(transform.position == targetB.position)
        {
            currentTarget = targetA;
        }    
            
        transform.position = Vector2.MoveTowards(transform.position, currentTarget.position, moveSpeed * Time.deltaTime);
        
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player") && other.transform.position.y > transform.position.y)
        {
            other.transform.SetParent(transform);
        }
    }
    
    private void OnCollisionExit2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            other.transform.SetParent(null);
        }
    }
}
