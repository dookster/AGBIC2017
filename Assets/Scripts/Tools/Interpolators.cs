using UnityEngine;
using System.Collections;

public class Interpolators {
    
    public enum Type { NONE, SMOOTHSTEP, SMOOTHERSTEP, INSQUARED, OUTSQUARED, INCUBED, OUTCUBED, INSINE, OUTSINE }

    public static float interpolate(float t, Type type)
    {
        switch (type)
        {
            case Type.NONE:
                return t;
            case Type.SMOOTHSTEP:
                return SmoothStep(t);
            case Type.SMOOTHERSTEP:
                return SmootherStep(t);
            case Type.INSQUARED:
                return EaseInSquared(t);
            case Type.OUTSQUARED:
                return EaseOutSquared(t);
            case Type.INCUBED:
                return EaseInSquared(t);
            case Type.OUTCUBED:
                return EaseOutCubed(t);
            case Type.INSINE:
                return EaseInSine(t);
            case Type.OUTSINE:
                return EaseOutSine(t);
            default:
                return t;
        }
    }

    public static float SmoothStep(float t)
    {
        return t * t * (3f - 2f * t);
    }

    public static float SmootherStep(float t)
    {
        return t * t * t * (t * (6f * t - 15f) + 10f);
    }

    public static float EaseInSquared(float t)
    {
        return t * t;
    }

    public static float EaseOutSquared(float t)
    {
        return 1 - (1 - t) * (1 - t);
    }

    public static float EaseInCubed(float t)
    {
        return t * t * t;
    }

    public static float EaseOutCubed(float t)
    {
        return 1 - (1 - t) * (1 - t) * (1 - t);
    }

    public static float EaseOutSine(float t)
    {
        return Mathf.Sin(t * Mathf.PI / 2);
    }

    public static float EaseInSine(float t)
    {
        return -1 * Mathf.Cos(t * (Mathf.PI / 2)) + 1;
    }

    public static float EaseInOutSine(float t)
    {
        return -0.5f * (Mathf.Cos(Mathf.PI * t) - 1);
    }



}
