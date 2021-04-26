using UnityEngine;

namespace GameDevTV.Saving
{
    /// <summary>
    /// A `System.Serializable` wrapper for the `Vector3` class.
    /// </summary>
    [System.Serializable]
    public class SerializableQuaternion
    {
         float x,y,z,w;

         /// <summary>
        /// Copy over the state from an existing Quaternion.
        /// </summary>
        public SerializableQuaternion(Quaternion quaternion)
        {
            x = quaternion.x;
            y = quaternion.y;
            z = quaternion.z;
            w = quaternion.w;
        }

        /// <summary>
        /// Create a Quaternion from this class' state.
        /// </summary>
        /// <returns></returns>
        public Quaternion ToQuaternion()
        {
            return new Quaternion(x,y,z,w);
        }

    }
}