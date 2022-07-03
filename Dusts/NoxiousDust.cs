using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Dusts
{
    public class NoxiousDust : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
			Texture2D texture = ModContent.Request<Texture2D>(Texture, ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

            dust.noGravity = true;
			dust.frame = new Rectangle(0, texture.Height / 3 * Main.rand.Next(3), texture.Width, texture.Height / 3);
			dust.noLight = true;
        }

        public override Color? GetAlpha(Dust dust, Color lightColor) => dust.color;

        public override bool Update(Dust dust)
        {
			dust.color = Lighting.GetColor((int)(dust.position.X / 16), (int)(dust.position.Y / 16)).MultiplyRGB(Color.Lerp(Color.Black * .34f, new Color(17, 145, 45), dust.scale/1f)) * 0.24f;
			dust.position += dust.velocity * 0.1f;
            dust.scale *= 0.992f;
			dust.velocity.Y -= Main.rand.NextFloat(1.5f, 2.1f) * dust.scale;
            dust.velocity *= 0.89f;

            if (dust.scale <= 0.2f) dust.active = false;
            return false;
        }
    }
}