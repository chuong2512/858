using UnityEngine;


public static class MathExtentions
{
    /// <summary>
    /// Checks probablity over given percentage.
    /// </summary>
    /// <param name="percentage">Percentage</param>
    /// <returns></returns>
    public static bool Probability(int percentage)
    {
        int _random = Random.Range(0, 100);

        return _random <= percentage;
    }

    /// <summary>
    /// Checks probablity over given percentage.
    /// </summary>
    /// <param name="percentage">Percentage</param>
    /// <returns></returns>
    public static bool Probability(float percentage)
    {
        float _random = Random.Range(0, 100);

        return _random <= percentage;
    }
}

