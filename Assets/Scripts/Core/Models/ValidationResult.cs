public struct ValidationResult
{
    public bool IsValid { get; }
    public EffectDirection? EffectDirection { get; }

    public ValidationResult(bool isValid, EffectDirection? effectDirection = null)
    {
        IsValid = isValid;
        EffectDirection = effectDirection;
    }

    public static ValidationResult Valid(EffectDirection? effectDirection = null) => new ValidationResult(true, effectDirection);
    public static ValidationResult Invalid(EffectDirection? effectDirection = null) => new ValidationResult(false, effectDirection);
}