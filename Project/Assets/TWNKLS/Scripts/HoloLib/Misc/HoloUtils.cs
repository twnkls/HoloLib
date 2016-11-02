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
 *  com.twnkls.HoloLib.Utils
 *  
 *  Holds several utility functions
 * 
 *  Author : Robin Kollau
 *  Version: 1.0.0
 *  Date   : 18 08 2016 
 * 
 */
namespace com.twnkls.HoloLib.Misc
{
    public class HoloUtils
    {
        /// <summary>
        /// Place an object on the HoloSurface.
        /// </summary>
        public static void PlaceOnSurface(UnityEngine.Transform target)
        {
            UnityEngine.RaycastHit hit;
            if ( HoloUtils.FindPointOnSurface(50.0f, out hit))
            {
                target.position = hit.point;
                UnityEngine.Quaternion toQuat = UnityEngine.Quaternion.FromToRotation(UnityEngine.Vector3.back, hit.normal);
                toQuat.x = 0;
                toQuat.z = 0;
                target.rotation = UnityEngine.Quaternion.Lerp(target.rotation, toQuat, 0.5f);
            }
        }


        /// <summary>
        /// Lets the target follow the user with a certain distance.
        /// </summary>
        /// <param name="target">UnityEngine.Transform</param>
        /// <param name="user">UnityEngine.Camera</param>
        /// <param name="distance">float</param>
        public static void FollowTarget(UnityEngine.Transform subject, UnityEngine.Transform target, float distance)
        {
            //position.
            UnityEngine.Vector3 userPos = target.localPosition;
            UnityEngine.Vector3 toPos   = userPos + (target.forward * distance);
            subject.position            = UnityEngine.Vector3.Lerp(subject.position, toPos, 0.25f);

            //rotation.
            UnityEngine.Quaternion toQuat = target.localRotation;
            subject.rotation = toQuat;
        }


        /// <summary>
        /// Places an object in from of the user at X distance
        /// </summary>
        /// <param name="subject">UnityEngine.Transform</param>
        /// <param name="distance">float</param>
        public static void PlaceInFrontOfUser(UnityEngine.Transform subject, float distance)
        {
            //place window in front of user.
            UnityEngine.Transform user_transform = UnityEngine.Camera.main.transform;
            UnityEngine.Vector3 user_position    = user_transform.position;
            UnityEngine.Quaternion user_rotation = user_transform.rotation;
            UnityEngine.Vector3 toPos            = user_position + (user_transform.forward * distance);

            //set location and rotation.
            subject.position = toPos;
            subject.rotation = user_rotation;

            //check if a surface is present closer than distance,
            // if so... we place the subject on that surface.
            UnityEngine.RaycastHit hit;
            if ( HoloUtils.FindPointOnSurface(distance, out hit))
            {
                UnityEngine.Vector3 deltaPosition = user_transform.position - hit.point;
                if (hit.distance < distance)
                {
                    subject.position = hit.point;
                }
            }
        }



        /// <summary>
        /// Finds a point on the environment surface.
        /// </summary>
        /// <returns>True if the ray intersects with a Collider, otherwise false.</returns>
        public static bool FindPointOnSurface(float max_ray_length, out UnityEngine.RaycastHit hit)
        {
            return UnityEngine.Physics.Raycast( HoloGazeManager.GetInstance().Head.Position,
                                                HoloGazeManager.GetInstance().Head.Direction,
                                                out hit,
                                                max_ray_length,
                                                HoloSpatialMappingManager.PhysicsRaycastMask);
        }



        /// <summary>
        /// Returns the parent with certain component type.
        /// </summary>
        /// <param name="subject">UnityEngine.Transform</param>
        /// <returns>UnityEngine.Transform</returns>
        public static UnityEngine.Transform GetTopLvl<T>(UnityEngine.Transform subject)
        {
            UnityEngine.Transform s = subject;
            while (s.parent != null && s.GetComponent<T>() == null)
            {
                s = s.parent;
            }
            return s;
        }
    }
}
