using System;
using UnityEngine;

public class Collectible : MonoBehaviour
{
  public void OnCollisionEnter(Collision other)
  {
    if (other.collider.CompareTag("Player")) 
    {
      var controller = other.collider.GetComponent<PlayerController>();
      controller.SetCanHarvest(true);
    }
    
  }

  public void OnCollisionExit(Collision other)
  {
    
    if (other.collider.CompareTag("Player")) 
    {
      var controller = other.collider.GetComponent<PlayerController>();
      controller.SetCanHarvest(false);
    }
  }


  public void OnTriggerEnter(Collider other)
  {
    if (other.CompareTag("Player"))
    {
      var controller = other.GetComponent<PlayerController>();
      controller.SetCanHarvest(true);
    }
  }


  public void OnTriggerExit(Collider other)
  {
    if (other.CompareTag("Player"))
    {
      var controller = other.GetComponent<PlayerController>();
      controller.SetCanHarvest(false);
    }
  }
}