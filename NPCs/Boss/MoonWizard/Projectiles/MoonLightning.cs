using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Boss.MoonWizard.Projectiles
{
	public class MoonLightning : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Moon Lightning");
		}

		public override void SetDefaults()
		{
			Projectile.width = 10;
			Projectile.height = 10;
			Projectile.aiStyle = -1;
			Projectile.penetrate = 8;
            Projectile.hide = true;
			Projectile.timeLeft = 10;
			Projectile.tileCollide = false;
		}
        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            if (Main.rand.NextBool(8))
            {
                target.AddBuff(BuffID.Electrified, 180, true);
            }
        }
    }
}