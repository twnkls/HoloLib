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
 * Pick and Place Object
 * Sample script for attaching a gameobject on a hololens surface OR
 * attaching in front of the user ( when no surface found ) when selected.
 * 
 * @author  : Robin Kollau
 * @version : 1.0.1
 * @date    : 11 11 2016 
 * 
 */

using com.twnkls.HoloLib.Unity;
using com.twnkls.HoloLib.Misc;
public class PickAndPlaceObject : HoloObject
{

    /// <summary>
    /// Places this object on a surface.
    /// </summary>
    public override void Update()
    {
        //when user selects this object.
        if( _isSelected)
        {
            //Check if we can place this object on a surface...
            if(HoloUtils.PlaceOnSurface(this.transform) == false)
            {
                //if no surface to place, place this in front of user.
                HoloUtils.PlaceInFrontOfUser(this.transform, 5.0f);
            }  
        }
    }
}
