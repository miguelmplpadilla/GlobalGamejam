using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class DisassembleController : MonoBehaviour
{
    public PartController[] parts;
    public ScrewController[] screws;

    private void Start()
    {
        parts = FindObjectsOfType<PartController>();
        screws = FindObjectsOfType<ScrewController>();
    }

    public bool CheckDisassembled()
    {
        foreach (var obj in parts)
            if (obj.transform.parent == null || !obj.parentRotate.Equals(obj.transform.parent.gameObject))
                return false;

        foreach (var srew in screws)
            if (!srew.screwed) return false;
        
        EndGame();
        return true;
    }

    private async void EndGame()
    {
        await Task.Yield();
        Debug.Log("Object Fixed or Assembled");
    }
}
