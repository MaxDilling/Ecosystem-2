using UnityEngine;

public class LivingEntity : MonoBehaviour {


    // State:

    public int colourMaterialIndex;
    public Species species;
    public Material material;

    public Coord coord;
    //
    [HideInInspector]
    public int mapIndex;
    [HideInInspector]
    public Coord mapCoord;

    protected bool dead = false;
    protected bool removed = false;

    // Try fix pb
    protected float amountRemaining = 1;
    protected float decay = 0.1f;       // decay in percent per second 
    //float consumeSpeed = 8;

    public virtual void Init (Coord coord) {
        this.coord = coord;
        transform.position = Environment.tileCentres[coord.x, coord.y];

        // Set material to the instance material
        var meshRenderer = transform.GetComponentInChildren<MeshRenderer> ();
        for (int i = 0; i < meshRenderer.sharedMaterials.Length; i++)
        {
            if (meshRenderer.sharedMaterials[i] == material) {
                material = meshRenderer.materials[i];
                break;
            }
        }
    }

    protected virtual void Kill (CauseOfDeath cause) {
        if (!dead) {
            dead = true;
        }
    }
    protected virtual void Remove () {
        if (!removed) {
            removed = true;
            Environment.RegisterDeath (this);
            Destroy (gameObject);
        }
    }


    public float Consume (float amount) {
        Kill(CauseOfDeath.Eaten); // make sure this entity is dead 

        float amountConsumed = Mathf.Max (0, Mathf.Min (amountRemaining, amount));
        amountRemaining -= amountConsumed;

        transform.localScale = Vector3.one * amountRemaining;

        if (amountRemaining <= 0) {
            Remove ();
        }

        return amountConsumed;
    }
}