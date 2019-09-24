using UnityEngine;

public class ParticleShootCollide : MonoBehaviour
{
    void OnParticleCollision(GameObject other)
    {
        other.SendMessage("DestroyAsteroid");
    }
}
