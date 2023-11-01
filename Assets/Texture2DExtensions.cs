using UnityEngine;

public static class Texture2DExtensions
{
    public static Texture2D DrawCircle(this Texture2D tex, Color _color, int x, int y, int radius = 3)
    {
        if (tex == null)
        {
            return null;
        }

        #region preventOverflowTiling
        if (x + radius > tex.width)
        {
            x = tex.width - radius;
        }
        else if (x - radius < 0)
        {
            x = radius;
        }

        if (y + radius > 0)
        {
            y = radius * -1;
        }
        else if (y - radius < tex.height * -1)
        {
            y = (tex.height - radius) * -1;
        }
        #endregion

        float radiusSqr = radius * radius;

        for (int u = x - radius; u < x + radius + 1; u++)
            for (int v = y - radius; v < y + radius + 1; v++)
                if ((x - u) * (x - u) + (y - v) * (y - v) < radiusSqr)
                    tex.SetPixel(u, v, _color);

        tex.Apply();

        return tex;
    }
}
