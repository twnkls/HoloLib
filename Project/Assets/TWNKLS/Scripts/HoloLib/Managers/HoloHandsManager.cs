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
 *  com.twnkls.HoloLib.HoloHandsManager
 *  
 *  Tracks user hands.
 * 
 *  Author : Robin Kollau
 *  Version: 1.0.0
 *  Date   : 10 11 2016 
 * 
 */
namespace com.twnkls.HoloLib.Managers
{
    public class HoloHandsManager
    {
        //delegates.
        public delegate void SourceEvent();
        public SourceEvent OnSourceDelegate;

        //privates.
        private System.Collections.Generic.Dictionary<uint, Misc.HoloHand> _foundHands = new System.Collections.Generic.Dictionary<uint, Misc.HoloHand>();

        //publics.
        public System.Collections.Generic.Dictionary<uint, Misc.HoloHand> FoundHands { get { return _foundHands; } }
        public int AmountFoundHands { get { return FoundHands.Count; } }

        /// <summary>
        /// Singleton constructor.
        /// </summary>
        private static HoloHandsManager _instance = null;
        private HoloHandsManager(){ }
        public static HoloHandsManager GetInstance()
        {
            if (_instance == null)
                _instance = new HoloHandsManager();
            return _instance;
        }

        /// <summary>
        /// Initializes this manager.
        /// </summary>
        public void Init()
        {
            //interaction manager.
            UnityEngine.VR.WSA.Input.InteractionManager.SourceDetected += OnSourceDetected;
            UnityEngine.VR.WSA.Input.InteractionManager.SourceLost     += OnSourceLost;
            UnityEngine.VR.WSA.Input.InteractionManager.SourcePressed  += OnSourcePressed;
            UnityEngine.VR.WSA.Input.InteractionManager.SourceReleased += OnSourceReleased;
            UnityEngine.VR.WSA.Input.InteractionManager.SourceUpdated  += OnSourceUpdated;
        }
    
        /// <summary>
        /// Fired when Interaction Manager detects a source of interaction.
        /// </summary>
        /// <param name="state">UnityEngine.VR.WSA.Input.InteractionSourceState</param>
        private void OnSourceDetected(UnityEngine.VR.WSA.Input.InteractionSourceState state)
        {
            //hand instance.
            Misc.HoloHand hand;

            //check if we alrdy stored this hand.
            if (_foundHands.ContainsKey(state.source.id) )
            {
                //get hand.
                _foundHands.TryGetValue(state.source.id, out hand);

                //update hand.
                hand.Update(state);
            }
            else
            {
                //create new hand.
                hand = new Misc.HoloHand();

                //update hand.
                hand.Update(state);

                //add to hands list.
                _foundHands.Add(hand.ID, hand);
            }

            //call the delegate.
            if(OnSourceDelegate != null)
                OnSourceDelegate();
        }

        /// <summary>
        /// Fired when Interaction Manager loses a source of interaction.
        /// </summary>
        /// <param name="state">UnityEngine.VR.WSA.Input.InteractionSourceState</param>
        private void OnSourceLost(UnityEngine.VR.WSA.Input.InteractionSourceState state)
        {
            //check if we alrdy stored this hand.
            if (_foundHands.ContainsKey(state.source.id))
            {
                //retrieve the hand entry.
                Misc.HoloHand hand;
                _foundHands.TryGetValue(state.source.id, out hand);

                //remove entry from list.
                _foundHands.Remove(state.source.id);

                //nullify the hand.
                hand = null;
            }
            

            //call the delegate.
            if (OnSourceDelegate != null)
                OnSourceDelegate();
        }

        /// <summary>
        /// Fired when an Interaction Source is pressed.
        /// </summary>
        /// <param name="state">UnityEngine.VR.WSA.Input.InteractionSourceState</param>
        private void OnSourcePressed(UnityEngine.VR.WSA.Input.InteractionSourceState state)
        {
            //check if we alrdy stored this hand.
            if (_foundHands.ContainsKey(state.source.id))
            {
                //hand instance.
                Misc.HoloHand hand;

                //get hand.
                _foundHands.TryGetValue(state.source.id, out hand);

                //update hand.
                hand.Update(state);
            }
        }

        /// <summary>
        /// Fired when an Interaction Source is released.
        /// </summary>
        /// <param name="state">UnityEngine.VR.WSA.Input.InteractionSourceState</param>
        private void OnSourceReleased(UnityEngine.VR.WSA.Input.InteractionSourceState state)
        {
            //check if we alrdy stored this hand.
            if (_foundHands.ContainsKey(state.source.id))
            {
                //hand instance.
                Misc.HoloHand hand;

                //get hand.
                _foundHands.TryGetValue(state.source.id, out hand);

                //update hand.
                hand.Update(state);
            }
        }


        /// <summary>
        /// Fired when an Interaction Source is updated.
        /// </summary>
        /// <param name="state">UnityEngine.VR.WSA.Input.InteractionSourceState</param>
        private void OnSourceUpdated(UnityEngine.VR.WSA.Input.InteractionSourceState state)
        {
            //check if we alrdy stored this hand.
            if (_foundHands.ContainsKey(state.source.id))
            {
                //hand instance.
                Misc.HoloHand hand;

                //get hand.
                _foundHands.TryGetValue(state.source.id, out hand);

                //update hand.
                hand.Update(state);
            }
        }
    }
}
