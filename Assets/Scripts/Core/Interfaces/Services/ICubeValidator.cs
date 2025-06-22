using UnityEngine;

public interface ICubeValidator
{
    ValidationResult ValidatePlacement(float localX, float localY, CubeColor color, Vector2 screenPosition);
}