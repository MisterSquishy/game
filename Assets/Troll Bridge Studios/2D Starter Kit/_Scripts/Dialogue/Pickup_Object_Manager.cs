using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TrollBridge {

	public class Pickup_Object_Manager : Interaction_Area
    {
        public override void Do_Interaction()
        {
            if (_playerManager.Current_Held_Object != null)
            {
                _playerManager.Pick_Up_Object(this.GetComponentInParent<Exciting_Object>());
            }
        }

    }
}
