using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace TrollBridge
{
    [RequireComponent(typeof(RectTransform))]
    public abstract class Interaction_Area : MonoBehaviour
    {
        /// Set to true if you want to see the area that the Player can interact with the NPC
		public bool showAreaInScene = false;
        /// The Collider2D that represents the range for the Interaction to happen.
        public Collider2D rangeCollider;
        /// Sets the color of the Collider2D
        public Color areaColor = Color.black;

        // The bool to let us know we are currently in a transition.
        protected bool isInTransition = false;
        // The Player State.
        protected Player_Manager _playerManager;
        // The Character component on the main object.
        protected Character chara;


        protected virtual void Awake()
        {
            // IF there is a mainObject.
            if (GetComponentInParent<Character>() != null)
            {
                // The Character component.
                chara = GetComponentInParent<Character>();
            }
            // Check to make sure the user has the scripts working correctly.
            DebugCheck();
        }

        void DebugCheck()
        {
            // IF user has the show area in scene 
            if (showAreaInScene && (rangeCollider == null))
            {
                Grid.helper.DebugErrorCheck(70, this.GetType(), gameObject);
            }
        }

        void OnDrawGizmos()
        {
            // This is used for Scene view.
            if (showAreaInScene && rangeCollider != null)
            {
                // IF we have a CircleCollider2D,
                // ELSE IF we have a BoxCollider2D.
                if (rangeCollider.GetType() == typeof(CircleCollider2D))
                {
                    // Display the Circle Collider on the scene view.
                    SceneCircleCollider(rangeCollider.GetComponent<CircleCollider2D>(), areaColor);
                }
                else if (rangeCollider.GetType() == typeof(BoxCollider2D))
                {
                    // Display the Box Collider on the scene view.
                    SceneBoxCollider(rangeCollider.GetComponent<BoxCollider2D>(), areaColor);
                }
            }
        }

        protected virtual void OnTriggerEnter2D(Collider2D coll)
        {
            // Attempt to grab the Player_Manager script in this gameobjects parent.
            Player_Manager _player = coll.GetComponentInParent<Player_Manager>();
            // IF the colliding object doesnt have the Player Manager script.
            if (_player == null)
            {
                return;
            }
            // IF the colliding object's tag isn't Player.
            if (coll.tag != "Player")
            {
                return;
            }
            // Assign the Player Manager script.
            _playerManager = _player;
            // We add this Script to our player state list.
            _playerManager.ListOfInteractionAreas.Add(this);
        }

        protected virtual void OnTriggerExit2D(Collider2D coll)
        {
            // Attempt to grab the Player_Manager script
            Player_Manager _player = coll.GetComponentInParent<Player_Manager>();
            // IF the colliding object doesnt have the Player Manager script.
            if (_player == null)
            {
                return;
            }
            // IF the colliding object's tag isn't Player.
            if (coll.tag != "Player")
            {
                return;
            }
            // IF the players closest action key dialogue is this gameobject.
            if (_playerManager.ClosestInteractionArea == this)
            {
                // Set the closest action key dialogue to null.
                _playerManager.ClosestInteractionArea = null;
                // Set the bool to show if this is an action key dialogue to false.
                _playerManager.IsActionKeyDialogued = false;
            }
            // Unfreeze the player (in case we froze them).
            _playerManager.CanMove = true;
            // IF a Character script exists.
            if (chara != null)
            {
                // Make this GameObject not be able to move.
                chara.CanMove = true;
                // Let everything know that this GameObject has or has not a running dialogue.
                chara.isActionKeyDialogueRunning = false;
                // Let everything know who the focus of this Dialogue is.
                chara.actionKeyFocusTarget = null;
            }
            // We remove this script from our player state list.
            _playerManager.ListOfInteractionAreas.Remove(this);
        }

        // Used for displaying collider information on the Scene View.
        private void SceneCircleCollider(CircleCollider2D coll, Color areaColor)
        {
            #if UNITY_EDITOR
                // Set the color.
                UnityEditor.Handles.color = areaColor;
                // Get the offset.
                Vector3 offset = coll.offset;
                // Get the position of the collider gameobject.
                Vector3 discCenter = coll.transform.position;
                // Scaling incase the gameobject has been scaled.
                float scale;
                // IF the x scale is larger than the y scale.
                if (transform.lossyScale.x > transform.lossyScale.y)
                {
                    // Make scale the size of the x.
                    scale = transform.lossyScale.x;
                }
                else
                {
                    // Make scale the size of the y.
                    scale = transform.lossyScale.y;
                }
                // Draw the Disc on the Scene View.
                UnityEditor.Handles.DrawWireDisc(discCenter + offset, Vector3.back, coll.radius * scale);
            #endif
        }

        // Used for displaying collider information on the Scene View.
        private void SceneBoxCollider(BoxCollider2D coll, Color areaColor)
        {
            // Set the color.
            Gizmos.color = areaColor;
            // Get the offset.
            Vector3 offset = coll.offset;
            // Get the position of the collider gameobject.
            Vector3 boxCenter = coll.transform.position;
            // Draw the Box on the Scene View.
            Gizmos.DrawWireCube(boxCenter + offset, new Vector2(coll.size.x * transform.lossyScale.x, coll.size.y * transform.lossyScale.y));
        }

        public abstract void Do_Interaction();
    }
}
