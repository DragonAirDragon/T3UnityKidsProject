using System.Collections.Generic;
using UnityEngine;

public class CompositeCubeValidator : ICubeValidator
{
    private readonly List<ICubeValidator> _validators;

    public CompositeCubeValidator(IEnumerable<ICubeValidator> validators)
    {
        _validators = new List<ICubeValidator>(validators);
    }

    public void AddValidator(ICubeValidator validator)
    {
        _validators.Add(validator);
    }

    public void RemoveValidator(ICubeValidator validator)
    {
        _validators.Remove(validator);
    }

    public ValidationResult ValidatePlacement(float localX, float localY, CubeColor color, Vector2 screenPosition)
    {
        foreach (var validator in _validators)
        {
            var result = validator.ValidatePlacement(localX, localY, color, screenPosition);
            if (!result.IsValid)
                return result;
        }

        return ValidationResult.Valid();
    }
}