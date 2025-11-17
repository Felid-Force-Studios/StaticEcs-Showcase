using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Mathematics;
using UnityEngine;

[BurstCompile]
public static class MathUtils {
    
    [BurstCompile]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void RGBToHSV(ref float4 rgbColor, out float H, out float S, out float V) {
        if (rgbColor.z > (double) rgbColor.y && rgbColor.z > (double) rgbColor.x) {
            RGBToHSVHelper(4f, rgbColor.z, rgbColor.x, rgbColor.y, out H, out S, out V);
        } else if (rgbColor.y > (double) rgbColor.x) {
            RGBToHSVHelper(2f, rgbColor.y, rgbColor.z, rgbColor.x, out H, out S, out V);
        } else {
            RGBToHSVHelper(0.0f, rgbColor.x, rgbColor.y, rgbColor.z, out H, out S, out V);
        }
    }

    [BurstCompile]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void RGBToHSVHelper(
        float offset,
        float dominantcolor,
        float colorone,
        float colortwo,
        out float H,
        out float S,
        out float V
    ) {
        V = dominantcolor;
        if (V != 0.0) {
            float num1 = colorone <= (double) colortwo ? colorone : colortwo;
            float num2 = V - num1;
            if (num2 != 0.0) {
                S = num2 / V;
                H = offset + (colorone - colortwo) / num2;
            } else {
                S = 0.0f;
                H = offset + (colorone - colortwo);
            }

            H /= 6f;
            if (H >= 0.0)
                return;
            ++H;
        } else {
            S = 0.0f;
            H = 0.0f;
        }
    }

    [BurstCompile]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void HSVToRGB(float H, float S, float V, out float4 result) {
        HSVToRGB(H, S, V, true, out result);
    }

    [BurstCompile]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void HSVToRGB(float H, float S, float V, bool hdr, out float4 result) {
        result = new float4(1.0f, 1.0f, 1.0f, 1.0f);
        if (S == 0.0) {
            result.x = V;
            result.y = V;
            result.z = V;
        } else if (V == 0.0) {
            result.x = 0.0f;
            result.y = 0.0f;
            result.z = 0.0f;
        } else {
            result.x = 0.0f;
            result.y = 0.0f;
            result.z = 0.0f;
            float num1 = S;
            float num2 = V;
            float f = H * 6f;
            int num3 = (int) Mathf.Floor(f);
            float num4 = f - num3;
            float num5 = num2 * (1f - num1);
            float num6 = num2 * (float) (1.0 - num1 * (double) num4);
            float num7 = num2 * (float) (1.0 - num1 * (1.0 - num4));
            switch (num3) {
                case -1:
                    result.x = num2;
                    result.y = num5;
                    result.z = num6;
                    break;
                case 0:
                    result.x = num2;
                    result.y = num7;
                    result.z = num5;
                    break;
                case 1:
                    result.x = num6;
                    result.y = num2;
                    result.z = num5;
                    break;
                case 2:
                    result.x = num5;
                    result.y = num2;
                    result.z = num7;
                    break;
                case 3:
                    result.x = num5;
                    result.y = num6;
                    result.z = num2;
                    break;
                case 4:
                    result.x = num7;
                    result.y = num5;
                    result.z = num2;
                    break;
                case 5:
                    result.x = num2;
                    result.y = num5;
                    result.z = num6;
                    break;
                case 6:
                    result.x = num2;
                    result.y = num7;
                    result.z = num5;
                    break;
            }

            if (!hdr) {
                result.x = Mathf.Clamp(result.x, 0.0f, 1f);
                result.y = Mathf.Clamp(result.y, 0.0f, 1f);
                result.z = Mathf.Clamp(result.z, 0.0f, 1f);
            }
        }
    }
}