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
 * 
 *  com.twnkls.HoloLib.HoloHand
 *  
 *  Holds the data for a users hand thats in sight 
 *  of the Hololens
 * 
 *  Author : Robin Kollau
 *  Version: 1.0.0
 *  Date   : 10 11 2016 
 * 
 */
namespace com.twnkls.HoloLib.Misc
{
    public class HoloHand
    {
        //publics.
        public uint ID { get; set; }
        public bool IsPressed { get; set; }
        public UnityEngine.VR.WSA.Input.InteractionSourceKind KIND { get; private set; }
        
        //privates.
        private UnityEngine.Vector3 _position, _savedPosition, _deltaPosition, _velocity;

        /// <summary>
        /// Constructor
        /// </summary>
        public HoloHand()
        {
            this.KIND = UnityEngine.VR.WSA.Input.InteractionSourceKind.Hand;
            _position = UnityEngine.Vector3.zero;
            _savedPosition = UnityEngine.Vector3.zero;
            _deltaPosition = UnityEngine.Vector3.zero;
            _velocity = UnityEngine.Vector3.zero;
        }

        /// <summary>
        /// Gets / Sets position
        /// </summary>
        public UnityEngine.Vector3 Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
            }
        }

        /// <summary>
        /// Gets the delta position.
        /// </summary>
        public UnityEngine.Vector3 DeltaPosition
        {
            get
            {
                return _deltaPosition;
            }
        }

        /// <summary>
        /// Gets / Sets velocity
        /// </summary>
        public UnityEngine.Vector3 Velocity
        {
            get
            {
                return _velocity;
            }
            set
            {
                _velocity = value;
            }
        }

        /// <summary>
        /// Updates all properties
        /// </summary>
        /// <param name="state">UnityEngine.VR.WSA.Input.InteractionSourceState</param>
        public void Update(UnityEngine.VR.WSA.Input.InteractionSourceState state )
        {
            //Hand check.
            if (state.source.kind != UnityEngine.VR.WSA.Input.InteractionSourceKind.Hand)
                return;
                  
            //update all variables.
            this.ID        = state.source.id;
            this.IsPressed = state.pressed;

            //get the position.
            state.properties.location.TryGetPosition(out _position);
         
            
            //get the velocity.
            state.properties.location.TryGetVelocity(out _velocity);

            //get the delta position.
            _deltaPosition = _position - _velocity;
        }

        /// <summary>
        /// Sends a debug message with all properties and values.
        /// </summary>
        public void Debug()
        {
            string str = string.Format("[HoloHand] - ID:{0}, KIND:{1}, IS PRESSED:{2}, LOCATION:{3}, DELTA POSITION:{4}, VELOCITY:{5}", this.ID, this.KIND, this.IsPressed, this.Position, this.DeltaPosition, this.Velocity);
            UnityEngine.Debug.Log(str);
        }
    }
}
