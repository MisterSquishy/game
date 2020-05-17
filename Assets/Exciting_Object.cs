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
        private Vector3 Velocity = new Vector3(0,0,0);

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
            else if (Velocity.magnitude > 0)
            {
                this.transform.position = this.transform.position + Velocity;
                Velocity = Vector3.Scale(Velocity, new Vector3(0.75f, 0.75f));

            }
        }

        public void pick_up(CharacterType characterType)
        {
            if (characterType.Equals(CharacterType.Scout))
            {
                IsInMouth = true;
            }
            else
            {
                IsInHand = true;
            }
        }

        public void drop_it()
        {
            IsInMouth = false;
        }

        public void throw_away(Vector3 direction)
        {
            if (IsInHand)
            {
                IsInHand = false;
                // todo is this a dumb way to scale this vector?
                Velocity = direction.normalized;
            }
        }
    }
}
