using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Buffs;
using SpiritMod.Items.Sets.ThrownMisc.FlaskofGore;

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
			Projectile.width = 20;
			Projectile.height = 20;

			Projectile.aiStyle = 2;

			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.alpha = 0;
		}

		public override void Kill(int timeLeft)
		{
			Player player = Main.player[Projectile.owner];
			SoundEngine.PlaySound(SoundID.NPCDeath19);
            SoundEngine.PlaySound(SoundID.Item107);
            for (int j = 0; j < 4; j++)
				Gore.NewGore(Projectile.GetSource_Death(), Projectile.position, new Vector2(), Main.rand.Next(134, 137), 1);
			if (Main.rand.Next(4) == 0 && !player.HasBuff(ModContent.BuffType<CrimsonSkullBuff>()) && Projectile.friendly)
				Item.NewItem(Projectile.GetSource_Death(), (int)Projectile.position.X, (int)Projectile.position.Y - 20, Projectile.width, Projectile.height, ModContent.ItemType<CrimsonSkull>());
		}
	}
}
