using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPhotoTask
{
    void Reset () ;
    bool IsAllFinished () ;
    bool IsAnyFinished () ;
    void UpdateTask () ;
    void SetFinished ( int n ) ;
    void Back () ;
    void Next ();
}
