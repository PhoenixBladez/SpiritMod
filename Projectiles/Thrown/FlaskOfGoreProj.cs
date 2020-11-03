using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Buffs;
using SpiritMod.Items;

namespace SpiritMod.Projectiles.Thrown
{
	public class FlaskOfGoreProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Flask Of Gore");
		}

		public override void SetDefaults()
		{
			projectile.width = 20;
			projectile.height = 20;

			projectile.aiStyle = 2;

			projectile.friendly = true;
			projectile.hostile = false;
			projectile.ranged = true;
			projectile.alpha = 0;
		}
		public override void Kill(int timeLeft)
		{
			Player player = Main.player[projectile.owner];
			Main.PlaySound(new Terraria.Audio.LegacySoundStyle(4, 19));
            Main.PlaySound(new Terraria.Audio.LegacySoundStyle(2, 107));
            for (int j = 0; j < 4; j++) {
				Gore.NewGore(new Vector2(projectile.position.X, projectile.position.Y), new Vector2(), Main.rand.Next(134, 137), 1);
			}
			if (Main.rand.Next(4) == 0 && !player.HasBuff(mod.BuffType("CrimsonSkullBuff")) && projectile.friendly) {
				int p = Item.NewItem((int)projectile.position.X, (int)projectile.position.Y - 20, projectile.width, projectile.height, mod.ItemType("CrimsonSkull"));
			}
		}
	}
}
