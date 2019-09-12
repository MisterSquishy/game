using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TrollBridge
{
    public class Exciting_Object : MonoBehaviour
    {

        [Tooltip("How exciting this object is to Scout (1-10)")]
        public float ExcitementLevel = 0f;
        public bool CanBePickedUp = false;
        public bool CanBeChewed = false;
        public bool IsInMouth = false;
        public bool IsInHand = false;
        private SpriteRenderer playerRenderer;
        private SpriteRenderer objectRenderer;

        // Use this for initialization
        void Start()
        {
            playerRenderer = Character_Manager.GetPlayer().GetComponent<SpriteRenderer>();
            objectRenderer = GetComponent<SpriteRenderer>();
        }

        // Update is called once per frame
        void Update()
        {
            if (IsInMouth)
            {
                GameObject _scout = null; //cleanup why do we have to pass null to this function?
                _scout = Character_Manager.GetClosestCharacterTypeWithRawTransform(this.transform, CharacterType.Scout, _scout, float.PositiveInfinity);
                this.transform.position = _scout.transform.GetChild(0).position; //get the actual scout, not the scout manager
            }
            else if (IsInHand)
            {
                this.transform.position = Character_Manager.GetPlayer().transform.position;
            }
        }

        public void throw_away()
        {
            if (IsInHand)
            {
                IsInHand = false;
                this.transform.position = new Vector3(this.transform.position.x + 10, this.transform.position.y + 10);
            }
        }
    }
}
