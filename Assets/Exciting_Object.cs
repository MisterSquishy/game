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
                objectRenderer = this.gameObject.GetComponent<SpriteRenderer>();
                objectRenderer.sortingOrder = playerRenderer.sortingOrder + 1;
                GameObject _character = null;
                _character = Character_Manager.GetClosestCharacterTypeWithRawTransform(this.transform, CharacterType.Scout, _character, float.PositiveInfinity);
                // Move the actual character of this gameobject closer to _character gameobject.
                this.transform.position = _character.transform.GetChild(0).position; //get the actual scout, not the scout manager
            }
            else
            {
                objectRenderer.sortingOrder = playerRenderer.sortingOrder - 1;
            }
        }
    }
}
