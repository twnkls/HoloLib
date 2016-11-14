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
 *  com.twnkls.HoloLib.Unity.HoloCursor
 *  
 *  places the attached gameobject on gazed target point
 * 
 *  Author : Robin Kollau
 *  Version: 1.0.1
 *  Date   : 11 11 2016 
 * 
 */
namespace com.twnkls.HoloLib.Unity
{
    public class HoloCursor : UnityEngine.MonoBehaviour
    {
        // Update is called once per frame
        void Update()
        {
            //Check if GazeManager has hit with ray..
           if(Managers.HoloGazeManager.GetInstance().GazeHit.HasHit )
            {
                //set this cursor.
                this.transform.localPosition = Managers.HoloGazeManager.GetInstance().GazeHit.RayHit.point;
                this.transform.localRotation = UnityEngine.Quaternion.FromToRotation(UnityEngine.Vector3.up, Managers.HoloGazeManager.GetInstance().GazeHit.RayHit.normal);
            }
            else
            {
                //set this cursor.
                Misc.HoloUtils.PlaceInFrontOfUser(this.transform, 10.0f);
                this.transform.localRotation = UnityEngine.Quaternion.FromToRotation(UnityEngine.Vector3.up, UnityEngine.Camera.main.transform.forward );
            }
        }
    }
}

