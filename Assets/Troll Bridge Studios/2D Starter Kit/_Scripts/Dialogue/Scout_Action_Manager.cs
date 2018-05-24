using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TrollBridge {

	public class Scout_Action_Manager : Interaction_Area
    {
        private Be_Scooter _scoutManager;
        public override void Do_Interaction()
        {
            _scoutManager = this.GetComponentInParent<Be_Scooter>();
            if (_scoutManager.Current_Held_Object != null)
            {
                _scoutManager.Drop_It();
            }
        }

    }
}
