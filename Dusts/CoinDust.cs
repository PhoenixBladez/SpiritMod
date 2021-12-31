using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Dusts
{
    public class CoinDust : ModDust
    {
        public override void OnSpawn(Dust dust)
		{
			dust.frame = new Rectangle(0, Main.rand.NextBool() ? 0 : 13, 12, 13);
			dust.rotation = 0.1f;
			dust.noGravity = false;
            dust.noLight = false;
        }

        public override bool Update(Dust dust)
        {
            dust.position += dust.velocity;
            dust.velocity.Y += 0.2f;
            if (Main.tile[(int)dust.position.X / 16, (int)dust.position.Y / 16].active() && Main.tile[(int)dust.position.X / 16, (int)dust.position.Y / 16].collisionType == 1)
            {
                dust.velocity *= -0.85f;
            }

            dust.rotation = dust.velocity.ToRotation();
            dust.scale *= 0.98f;
            if (dust.scale < 0.2f)
            {
                dust.active = false;
            }
            return false;
        }
    }

}