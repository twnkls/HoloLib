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
*  com.twnkls.HoloLib.Unity.HoloObject
*  
*  Base class for all UnityGameObjects that need Hololens interaction.
* 
*  Author : Robin Kollau
*  Version: 1.0.1
*  Date   : 10 08 2016 
* 
*/
namespace com.twnkls.HoloLib.Unity
{
    public class HoloObject : UnityEngine.MonoBehaviour, Interfaces.IHoloInteractible 
    {
        //protected
        protected bool _isFocussed    = false;
        protected bool _isHolded      = false;
        protected bool _isSelected    = false;

        // Use this for initialization
        public virtual void Start()
        {
            _isFocussed = false;
            _isHolded   = false;
            _isSelected = false;
        }


        // Update is called once per frame
        public virtual void Update(){ }

        //focussing.
        public virtual bool HasFocus
        {
            get
            {
                return _isFocussed;
            }
        }
        public virtual void OnFocusIn() { _isFocussed = true;  }
        public virtual void OnFocusOut(){ _isFocussed = false; }
        
        //selecting.
        public virtual void OnSelect() { _isSelected = !_isSelected;  }

        //holding.
        public virtual void OnHoldStart(UnityEngine.Ray head_ray){ _isHolded = true;  }
        public virtual void OnHoldEnd(UnityEngine.Ray head_ray)  { _isHolded = false; }

        //placing.
        public virtual void PlaceOnSurface() { Misc.HoloUtils.PlaceOnSurface(this.transform); }

        //following.
        public virtual void FollowTarget( UnityEngine.Transform target, float distance ) { Misc.HoloUtils.FollowTarget(this.transform, target, distance); }
    }
}

