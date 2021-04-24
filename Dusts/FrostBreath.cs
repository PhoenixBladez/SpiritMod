using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Dusts
{
    public class FrostBreath : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = true;
            dust.noLight = true;
        }

        public override Color? GetAlpha(Dust dust, Color lightColor) => dust.color;

        public override bool Update(Dust dust)
        {
            dust.color = Lighting.GetColor((int)(dust.position.X / 16), (int)(dust.position.Y / 16)).MultiplyRGB(new Color(156, 217, 255)) * 0.36f;
            dust.position += dust.velocity * 0.1f;
            dust.scale *= 0.992f;
            dust.velocity *= 0.97f;
            dust.rotation += 0.05f;
            
            if (dust.scale <= 0.2f) dust.active = false;
            return false;
        }
    }
}