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
 *  com.twnkls.com.twnkls.HoloLib.HoloHead
 *  
 *  Manages the head position and direction of the user.
 * 
 *  Author : Robin Kollau
 *  Version: 1.0.0
 *  Date   : 04 08 2016 
 * 
 */
namespace com.twnkls.HoloLib.Misc
{
    public class HoloHead
    {
        //privates.
        private UnityEngine.Vector3 _position, _direction;

        /// <summary>
        /// Constructor.
        /// </summary>
        public HoloHead() {
            _position  = UnityEngine.Vector3.zero;
            _direction = UnityEngine.Vector3.zero;

            //check for a main camera.
            if (UnityEngine.Camera.main == null)
                throw new System.Exception("[ com.twnkls.HoloLib.HoloHead ] - No camera tagged 'MainCamera'.");
        }

        /// <summary>
        /// Gets / Privately Sets head position.
        /// </summary>
        public UnityEngine.Vector3 Position
        {
            get
            {
                return _position;
            }
            private set
            {
                _position = value;
            }
        }

        /// <summary>
        /// Gets / Privately sets head direction.
        /// </summary>
        public UnityEngine.Vector3 Direction
        {
            get
            {
                return _direction;
            }
            private set
            {
                _direction = value;
            }
        }

        /// <summary>
        /// Updates position and direction.
        /// </summary>
        public void Update()
        {
            Position  = UnityEngine.Camera.main.transform.position;
            Direction = UnityEngine.Camera.main.transform.forward;
        }
    }
}

