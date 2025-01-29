using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpResetTimer : MonoBehaviour
{
    public Material material;
    public bool canJump;
    public PlayerMovementGrappling playerMovement;
    private float isActive = 1f;

    private void Start()
    {
        if (material != null)
        {
            material.SetFloat("_IsActive", isActive);
        }
    }

    private void Update()
    {
        if (material != null)
        {
            // Obtener el valor actual del shader
            float shaderValue = material.GetFloat("_IsActive");

            // Si el shader está en 1, ponerlo en 0; si no, ponerlo en 1
            material.SetFloat("_IsActive", shaderValue == 1f ? 1f : 0f);
        }

        if (canJump && Input.GetKeyDown(KeyCode.Space))
        {
            playerMovement.rb.AddForce(transform.up * playerMovement.jumpForce, ForceMode.Impulse);
            canJump = false;
            //Destroy(gameObject);  // OPCIONAL -> Destruye el objeto al tocarlo
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verificar si el objeto que entró en el trigger es el jugador
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            canJump = true;
            StartCoroutine(ChangeShaderValue(0f, 1f)); // Cambia el valor del shader a 0 suavemente
            StartCoroutine(ResetJumpAfterDelay(2f)); // Inicia la corrutina para desactivar canJump
        }
    }

    private IEnumerator ResetJumpAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(ChangeShaderValue(1f, 1f)); // Cambia el valor del shader a 1 antes de destruir el objeto
        yield return new WaitForSeconds(1f);
        canJump = false;
        Destroy(gameObject); // Destruye el objeto después del tiempo establecido
    }

    private IEnumerator ChangeShaderValue(float targetValue, float duration)
    {
        if (material == null) yield break;

        float startValue = material.GetFloat("_IsActive");
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float newValue = Mathf.Lerp(startValue, targetValue, time / duration);
            material.SetFloat("_IsActive", newValue);
            yield return null;
        }
        material.SetFloat("_IsActive", targetValue);
    }
}
