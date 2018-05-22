using UnityEngine;
using System.Collections.Generic;

namespace TrollBridge
{

    public class Be_Scooter : MonoBehaviour
    {

        [Tooltip("The CharacterType to follow")]
        public CharacterType TypeToFollow;
        [Tooltip("How far the mob can see before aggroing on the CharacterType. " +
                    "Set to 0 for infinite distance.")]
        public float AggroDistance = 5f;
        [Tooltip("How close the mob can get before stopping.")]
        public float ComfortZoneEnd = 1.2f;
        [Tooltip("How close the type can get before the mob backs up.")]
        public float ComfortZoneStart = 0.8f;
        [Header("Object Speed")]
        [Tooltip("The speed at which the GameObject will be moving.  IF there is a Character script attached to this GameObject then this variable 'Speed' will be changed to the Character Component 'CurrentMoveSpeed'")]
        public float Speed = 1f;

        private Character character;
        private Character_Stats charStats;
        private List<Character> listCharacter = new List<Character>();
        private Exciting_Object Current_Exciting_Object;
        private Exciting_Object Current_Held_Object;
        private Exciting_Object Thing_In_Mouth;
        private int Excitement_Level = 2;
        private float True_Speed { get { return Speed * Excitement_Level/2; } }

        void Start()
        {
            // Get the Character component.
            character = GetComponentInParent<Character>();
            // Get the Character Stats component.
            charStats = character.GetComponentInChildren<Character_Stats>();
            if (charStats != null)
            {
                Speed = charStats.CurrentMoveSpeed; //some npcs have char defined in parent
            }
            if (AggroDistance == 0)
            {
                AggroDistance = float.PositiveInfinity;
            }
            Get_Leashed();
        }


        void Update()
        {
            // IF this character is able to move.

            if (character.CanMove)
            {
                // Get the list of all the characters.
                listCharacter = Character_Manager.GetCharactersByType(listCharacter, TypeToFollow);
                // IF the List of CharacterTypes is greater than 0.
                if (listCharacter.Count > 0)
                {
                    {
                        Look_Around();
                        if (Current_Exciting_Object != null)
                        {
                            Excitement_Level = (int) Current_Exciting_Object.ExcitementLevel;
                            Go_To_Exciting_Object();
                        } else
                        {
                            Excitement_Level = 2;
                            Wander();
                        }
                    }
                }
            }
        }

        void Wander()
        {
            // Create a GameObject variable.
            GameObject _character = null;
            // Get the closest GameObject with the CharacterType chosen and save it to _character.
            _character = Character_Manager.GetClosestCharacterType(character.transform, TypeToFollow, _character, AggroDistance);
            // IF the closest gameobject is not null.
            if (_character != null)
            {
                float distance = (transform.position - _character.GetComponent<Character>().characterEntity.transform.position).magnitude;
                if (distance > ComfortZoneEnd)
                {
                    // Move the actual character of this gameobject closer to _character gameobject.
                    character.characterEntity.transform.position =
                        Vector2.MoveTowards(transform.position, _character.GetComponent<Character>().characterEntity.transform.position, Time.deltaTime * True_Speed);
                }
                else if (distance < ComfortZoneStart)
                {
                    // Move the actual character of this gameobject further from _character gameobject.
                    character.characterEntity.transform.position =
                        Vector2.MoveTowards(transform.position, _character.GetComponent<Character>().characterEntity.transform.position, Time.deltaTime * True_Speed * -1);
                }
            }
        }

        void Get_Leashed()
        {
            //List<Character> heroList = new List<Character>();
            //HingeJoint2D leash = GetComponent<HingeJoint2D>();
            //leash.autoConfigureConnectedAnchor = true;

            // Create a GameObject variable.
            //GameObject _character = null;
            // Get the closest GameObject with the CharacterType chosen and save it to _character.
            //_character = Character_Manager.GetClosestCharacterType(character.transform, TypeToFollow, _character, AggroDistance);
            //leash.connectedBody = _character.GetComponent<Rigidbody2D>();
            //leash.enabled = true;
        }

        void Look_Around()
        {
            Vector3 start = transform.position;
            //this isn't stupid, right?
            int animation_direction = GetComponent<Animator>().GetInteger("Direction");
            Vector3 direction;
            switch (animation_direction) {
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
                    direction = Vector3.right;
                    break;
            }
            float distance = 10f;


            HashSet<RaycastHit2D> objectsInSight = new HashSet<RaycastHit2D>();
            for (float i = -45; i <= 45; i += 1)
            {
                Vector3 startOffset = Vector3.Scale(Quaternion.AngleAxis(i, Vector3.forward) * direction, new Vector3(0.5f, 0.5f, 0.5f));
                Debug.DrawRay(start + startOffset, Quaternion.AngleAxis(i, Vector3.forward) * direction * distance, Color.red);
                objectsInSight.UnionWith(Physics2D.RaycastAll(start + startOffset, Quaternion.AngleAxis(i, Vector3.forward) * direction, distance));
            }

            foreach (RaycastHit2D obj in objectsInSight)
            {
                Exciting_Object exciting_obj = (Exciting_Object)obj.collider.gameObject.GetComponent("Exciting_Object");
                if (Current_Exciting_Object == null || (exciting_obj != null && exciting_obj.ExcitementLevel > Current_Exciting_Object.ExcitementLevel))
                {
                    Current_Exciting_Object = exciting_obj;
                }
            }
        }

        void Go_To_Exciting_Object()
        {
            // Move the actual character of this gameobject closer to _character gameobject.
            character.characterEntity.transform.position =
                Vector2.MoveTowards(transform.position, Current_Exciting_Object.transform.position, Time.deltaTime * True_Speed);
            if (transform.position == Current_Exciting_Object.transform.position)
            {
                Thing_In_Mouth = Current_Exciting_Object;
                Current_Exciting_Object.IsInMouth = true;
                Current_Held_Object = Current_Exciting_Object;
                Current_Exciting_Object = null;
            }
        }
    }
}
