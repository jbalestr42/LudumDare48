using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public static class Tools {
    public static quaternion RotateTowards(quaternion from, quaternion to, float maxDegreesDelta)
    {
        float num = Angle(from, to);
        return num < float.Epsilon ? to : math.slerp(from, to, math.min(1f, maxDegreesDelta / num));
    }

    public static float Angle(this quaternion q1, quaternion q2)
    {
        var dot = math.dot(q1, q2);
        return !(dot > 0.999998986721039) ? (float)(math.acos(math.min(math.abs(dot), 1f)) * 2.0) : 0.0f;
    }

    public static float Radian(float3 from, float3 to)
    {
        // sqrt(a) * sqrt(b) = sqrt(a * b) -- valid for real numbers
        var denominator = math.sqrt(math.lengthsq(from) * math.lengthsq(to));
        if (denominator < float.Epsilon) return 0F;
        var d = math.clamp(math.dot(from, to) / denominator, -1F, 1F);
        return math.acos(d);
    }

    public static bool IsPointInPolygon(float2[] polygon, float2 testPoint)
    {
        bool result = false;
        int j = polygon.Length - 1;

        for (int i = 0; i < polygon.Length; i++) {
            if (polygon[i].y < testPoint.y && polygon[j].y >= testPoint.y || polygon[j].y < testPoint.y && polygon[i].y >= testPoint.y) {
                if (polygon[i].x + (testPoint.y - polygon[i].y) / (polygon[j].y - polygon[i].y) * (polygon[j].x - polygon[i].x) < testPoint.x) {
                    result = !result;
                }
            }
            j = i;
        }

        return result;
    }

    public static bool IsInScreen(float3 position, float3 cameraPosition, float2 size)
    {
        if (position.x > cameraPosition.x - size.x && position.x < cameraPosition.x + size.x &&
            position.z > cameraPosition.z - size.y && position.z < cameraPosition.z + size.y) {
            return true;
        }
        return false;
    }

    public static float4 ColorToFloat4(Color color)
    {
        return new float4(color.r, color.g, color.b, color.a);
    }

    public static float4 ColorToLinearFloat4(Color color)
    {
        return ColorToFloat4(color.linear);
    }

    public static float4[] GradientToFloat4Array(Gradient gradient, int sampleCount)
    {
        var colorArray = new float4[sampleCount];

        for (int i = 0; i < sampleCount; i++) {
            Color color = gradient.Evaluate(i / (float)sampleCount);
            colorArray[i] = Tools.ColorToLinearFloat4(color);
        }

        return colorArray;
    }
}