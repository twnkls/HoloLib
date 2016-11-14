/*
MIT License

Copyright(c) 2016 twnkls | augmented reality

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

/**
 * Drag and Drop Object
 * Sample script for dragging and dropping a gameobject.
 * 
 * @author  : Robin Kollau
 * @version : 1.0.0
 * @date    : 14 11 2016 
 * 
 */
using com.twnkls.HoloLib.Unity;
using UnityEngine;

public class DragAndDropObject : HoloObject {

    /// <summary>
    /// Override OnManipulate to add movement placement.
    /// </summary>
    /// <param name="movement">Vector3</param>
    public override void OnManipulate(Vector3 movement)
    {
        //call base.
        base.OnManipulate(movement);

        //define a scalefactor.
        float scaleFactor       = 2.0f;

        //implement movement based on manipulation.
        this.transform.position = this.ManipulateStartPosition + (movement*scaleFactor);
    }
}
