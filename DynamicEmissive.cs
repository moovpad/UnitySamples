using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class DynamicEmissive : MonoBehaviour
{
    new Renderer renderer; // new hides the parent <renderer> property.
    Material material;
    Color emissionColor;

    void Start()
    {
        // Gets access to the renderer and material components as we need to
        // modify them during runtime.
        renderer = GetComponent<Renderer>();
        material = renderer.material;

        // Gets the initial emission colour of the material, as we have to store
        // the information before turning off the light.
        emissionColor = material.GetColor("_EmissionColor");

        // Start a coroutine to toggle the light on / off.
        StartCoroutine(Toggle());
    }

    IEnumerator Toggle()
    {
        bool toggle = false;
        while (true)
        {
            yield return new WaitForSeconds(1f);
            Activate(toggle, Random.Range(0.5f, 2f));
            toggle = !toggle;
        }
    }

    // Call this method to turn on or turn off emissive light.
    public void Activate(bool on, float intensity = 1f)
    {
        if (on)
        {

            // Enables emission for the material, and make the material use
            // realtime emission.
            material.EnableKeyword("_EMISSION");
            material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;

            // Update the emission color and intensity of the material.
            material.SetColor("_EmissionColor", emissionColor * intensity);

            // Makes the renderer update the emission and albedo maps of our material.
            RendererExtensions.UpdateGIMaterials(renderer);

            // Inform Unity's GI system to recalculate GI based on the new emission map.
            DynamicGI.SetEmissive(renderer, emissionColor * intensity);
            DynamicGI.UpdateEnvironment();

        }
        else
        {

            material.DisableKeyword("_EMISSION");
            material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.EmissiveIsBlack;

            material.SetColor("_EmissionColor", Color.black);
            RendererExtensions.UpdateGIMaterials(renderer);

            DynamicGI.SetEmissive(renderer, Color.black);
            DynamicGI.UpdateEnvironment();

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
