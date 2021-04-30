using UnityEngine;
using RPG.Movement;
using RPG.Attributes;
using System;
using UnityEngine.EventSystems;
using UnityEngine.AI;
using GameDevTV.Inventories;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        Health health;

        [System.Serializable]
        struct CursorMapping
        {
            public Texture2D texture;
            public Vector2 hotspot;
            public CursorType type; 
        }

        [SerializeField] CursorMapping[] cursorMappings = null;
        [SerializeField] float maxNevMeshProjectionDistance = 1f;
        [SerializeField] float raycastRadius = 1f;

        bool isDragginUI = false;
        Vector3 startingPosition;
        Rect ScreenWindow = new Rect(0,0,Screen.width, Screen.height);


        public bool IsDragging() { return isDragginUI;}
        public Vector3 GetStartPos() {return startingPosition;}

        private void Awake()
        {
            health = GetComponent<Health>();
            startingPosition = transform.position;
        }

        void Update()
        {
            if(!ScreenWindow.Contains(Input.mousePosition)) 
            {
                Cursor.SetCursor(null,Vector2.zero,CursorMode.Auto);
                return;
            }
            UseAbilities();
            if (InteractWithUI()) return;
            if (health.IsDead())
            {
                SetCursor(CursorType.None);
                return;
            }
            if (InteractWithComponent()) return;
            if (InteractWithMovement()) return;

            SetCursor(CursorType.None);
          
        }

        private void UseAbilities()
        {
           ActionStore actionStore = GetComponent<ActionStore>();
            for (int i = 0; i < 9; i++)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                {
                    actionStore.Use(i, gameObject);
                }
            }
        }

        private bool InteractWithUI()
        {
            if (Input.GetMouseButtonUp(0))
            {
                isDragginUI = false;
            }
            
            if(EventSystem.current.IsPointerOverGameObject())
            {
                if (Input.GetMouseButtonDown(0))
                {
                    isDragginUI = true;
                }
                SetCursor(CursorType.UI);
                return true;
            }
            if (isDragginUI)
            {
                return true;
            }

            return false;
        }

        private bool InteractWithComponent()
        {
            RaycastHit[] hits = RaycastAllSorted();
            foreach (RaycastHit hit in hits)
            {
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();
                foreach(IRaycastable raycastable in raycastables)
                {
                    if (raycastable.HandleRayCast(this))
                    {
                        SetCursor(raycastable.GetCursorType());
                        return true;
                    }
                }
            }
            return false;
         }

        private RaycastHit[] RaycastAllSorted()
        {
            //Get all hits
            RaycastHit[] hits = Physics.SphereCastAll(GetMouseRay(), raycastRadius);
            //Sort by distance
            //Build array of distances
            float[] distances = new float[hits.Length];
            for (int i = 0; i<distances.Length; i++)
            {
                distances[i] = hits[i].distance;
            }
            Array.Sort(distances, hits);

            return hits;
        }

        private bool InteractWithMovement()
        {
            
            Vector3 target;
            bool hasHit = RaycastNavMesh(out target);
            if (hasHit)
            {
                if (!GetComponent<Mover>().CanMoveTo(target)) return false;
              
                if (Input.GetMouseButton(0))
                {
                    GetComponent<Mover>().StartMoveAction(target, 1f);
                }
                SetCursor(CursorType.Movement);
                return true;
            }

            return false;

        }

        private bool RaycastNavMesh(out Vector3 target)
        {
            target = new Vector3();
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (!hasHit) return false;

            // Find nearest navmesh point
            NavMeshHit navMeshHit;
            bool hasCastToNavMesh = NavMesh.SamplePosition(
                hit.point, out navMeshHit, maxNevMeshProjectionDistance, NavMesh.AllAreas);
            if (!hasCastToNavMesh) return false;

            target = navMeshHit.position;
            return true;
        }

        private void SetCursor(CursorType type)
        {
            CursorMapping mapping = GetCursorMapping(type);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping(CursorType type)
        {
            foreach(CursorMapping mapping in cursorMappings)
            {
                if(mapping.type == type)
                {
                    return mapping;
                }
            }
            return cursorMappings[0];
        }

        public static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}
