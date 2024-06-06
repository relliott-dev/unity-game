using UnityEngine;

namespace RDE
{
    /// <summary>
    /// 
    /// Manages the locomotion of a character in the game
    /// This includes handling reflection off impact and current speed for impact damage
    /// 
    /// @TODO:
    /// - Possible rewrite?
    /// - Possibly move object script here
    /// 
    /// </summary>
    public class CharacterLocomotionManager : MonoBehaviour
    {
        #region Variables

        [Header("Helper Variables")]
        protected Vector2 velocity;

        #endregion

        #region Locomotion Methods

        //Returns the current velocity
        public float CurrentSpeed()
        {
            return velocity.magnitude;
        }

        //Applies bounce back effect from objects
        public void ApplyReflection(Collision2D collision)
        {
            Vector2 normal = collision.contacts[0].normal;
            float speedAfterCollision = velocity.magnitude * 0.5f;
            velocity = Vector2.Reflect(velocity.normalized, normal) * speedAfterCollision;
        }

        //Applies bounce back effect from shooting
        public void ApplyRecoil(float recoilForce)
        {
            Vector2 recoilDirection = -transform.up;
            velocity += recoilDirection * recoilForce;
        }

        #endregion
    }
}