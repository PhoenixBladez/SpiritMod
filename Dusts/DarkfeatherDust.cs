using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
namespace SpiritMod.Dusts
{
	public class DarkfeatherDust : ModDust
	{
		public override void OnSpawn(Dust dust)
		{
            dust.noGravity = true;
        }
        public override Color? GetAlpha(Dust dust, Color lightColor)
        {
            return new Color(129, 227, 118, 100);
        }
        public override bool Update(Dust dust)
        {
            float num180 = dust.scale;
            if (num180 > 1f)
            {
                num180 = 1f;
            }
            Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), num180, num180 * 0.1f, num180 * 0.8f);
            {
                dust.scale -= 0.005f;
                Dust dust24 = dust;
                dust24.velocity *= 0.9f;
                Dust expr_1CD9_cp_0 = dust;
                expr_1CD9_cp_0.velocity.X = expr_1CD9_cp_0.velocity.X + (float)Main.rand.Next(-10, 11) * 0.02f;
                Dust expr_1D00_cp_0 = dust;
                expr_1D00_cp_0.velocity.Y = expr_1D00_cp_0.velocity.Y + (float)Main.rand.Next(-10, 11) * 0.02f;
                if (Main.rand.Next(5) == 0)
                {
                    int num179 = Dust.NewDust(dust.position, 4, 4, dust.type, 0f, 0f, 0, default(Color), 1f);
                    Main.dust[num179].noGravity = true;
                    Main.dust[num179].scale = dust.scale * 2.5f;
                }
            }
            return false;
		}
	}
}
