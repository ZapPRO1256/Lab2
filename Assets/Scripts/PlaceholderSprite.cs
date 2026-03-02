using UnityEngine;

/// <summary>
/// Creates placeholder sprites with proper size and simple ship shapes for player, enemies, and bullets.
/// Uses pixelsPerUnit so sprites appear at visible scale in world space (camera ortho size 5).
/// </summary>
public static class PlaceholderSprite
{
    const int ShipSize = 32;
    const float ShipPixelsPerUnit = 24f;  // 32/24 ≈ 1.33 units world size
    const float BulletPixelsPerUnit = 24f;

    /// <summary>1x1 single-pixel sprite (e.g. for background tiles).</summary>
    public static Sprite Create(Color color)
    {
        var tex = new Texture2D(1, 1);
        tex.SetPixel(0, 0, color);
        tex.Apply();
        tex.filterMode = FilterMode.Point;
        return Sprite.Create(tex, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f), 100f);
    }

    /// <summary>Player ship: triangle pointing up (classic shooter shape).</summary>
    public static Sprite CreatePlayerShip(Color color)
    {
        var tex = new Texture2D(ShipSize, ShipSize);
        Color clear = new Color(0, 0, 0, 0);
        for (int y = 0; y < ShipSize; y++)
            for (int x = 0; x < ShipSize; x++)
                tex.SetPixel(x, y, clear);

        // Triangle: tip at top center, base at bottom. Slight margin.
        int tipY = ShipSize - 4;
        int baseY = 4;
        int centerX = ShipSize / 2;
        int halfBase = 12;
        for (int y = baseY; y <= tipY; y++)
        {
            float t = (float)(y - baseY) / (tipY - baseY);
            int halfWidth = (int)(halfBase * (1f - t) + 2 * t); // narrow at top, wide at base
            if (halfWidth < 1) halfWidth = 1;
            for (int x = centerX - halfWidth; x <= centerX + halfWidth; x++)
            {
                if (x >= 0 && x < ShipSize)
                    tex.SetPixel(x, y, color);
            }
        }

        tex.Apply();
        tex.filterMode = FilterMode.Bilinear;
        return Sprite.Create(tex, new Rect(0, 0, ShipSize, ShipSize), new Vector2(0.5f, 0.5f), ShipPixelsPerUnit);
    }

    /// <summary>Enemy ship: triangle pointing down, slightly wider.</summary>
    public static Sprite CreateEnemyShip(Color color)
    {
        var tex = new Texture2D(ShipSize, ShipSize);
        Color clear = new Color(0, 0, 0, 0);
        for (int y = 0; y < ShipSize; y++)
            for (int x = 0; x < ShipSize; x++)
                tex.SetPixel(x, y, clear);

        int tipY = 4;
        int baseY = ShipSize - 4;
        int centerX = ShipSize / 2;
        int halfBase = 12;
        for (int y = tipY; y <= baseY; y++)
        {
            float t = (float)(y - tipY) / (baseY - tipY);
            int halfWidth = (int)(2 + (halfBase - 2) * t);
            if (halfWidth < 1) halfWidth = 1;
            for (int x = centerX - halfWidth; x <= centerX + halfWidth; x++)
            {
                if (x >= 0 && x < ShipSize)
                    tex.SetPixel(x, y, color);
            }
        }

        tex.Apply();
        tex.filterMode = FilterMode.Bilinear;
        return Sprite.Create(tex, new Rect(0, 0, ShipSize, ShipSize), new Vector2(0.5f, 0.5f), ShipPixelsPerUnit);
    }

    /// <summary>Bullet: small elongated rectangle, visible in world space.</summary>
    public static Sprite CreateBullet(Color color)
    {
        const int w = 6;
        const int h = 16;
        var tex = new Texture2D(w, h);
        for (int y = 0; y < h; y++)
            for (int x = 0; x < w; x++)
                tex.SetPixel(x, y, color);
        tex.Apply();
        tex.filterMode = FilterMode.Bilinear;
        return Sprite.Create(tex, new Rect(0, 0, w, h), new Vector2(0.5f, 0.5f), BulletPixelsPerUnit);
    }
}
