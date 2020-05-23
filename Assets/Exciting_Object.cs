using UnityEngine;

namespace TrollBridge
{
    public class Exciting_Object : MonoBehaviour
    {

        [Tooltip("How exciting this object is to Scout (1-10)")]
        public float ExcitementLevel = 0f;
        public bool CanBePickedUp = false;
        public bool CanBeChewed = false;
        private Vector3 Velocity = new Vector3(0,0,0);

        // Update is called once per frame
        void Update()
        {
            if (Velocity.magnitude > 0)
            {
                this.transform.position = this.transform.position + Velocity;
                Velocity = Vector3.Scale(Velocity, new Vector3(0.75f, 0.75f));

            }
        }

        public void pick_up(GameObject gameObject)
        {
            Character holding_character = this.GetComponentInParent<Character>();
            if (holding_character == null) // no stealing!
            {
                this.transform.SetParent(gameObject.transform);
            }
        }

        public void drop_it()
        {
            this.transform.SetParent(null);
        }

        public void throw_away()
        {
            Character holding_character = this.GetComponentInParent<Character>();
            if (holding_character != null)
            {
                Vector3 direction;
                switch (holding_character.CharacterAnimator.GetInteger("Direction"))
                {
                    case 1:
                        direction = Vector3.up;
                        break;
                    case 2:
                        direction = Vector3.left;
                        break;
                    case 3:
                        direction = Vector3.down;
                        break;
                    default:
                        direction = Vector3.right; //todo 8 directions!!
                        break;
                }
                this.Velocity = direction.normalized;
                drop_it();
            }
        }
    }
}
