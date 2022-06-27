using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Summon.WyvernStaff
{
	public class WyvernStaffHead : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wyvern");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 3;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}
		public override void SetDefaults()
		{
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.minion = true;
			Projectile.damage = 13;
			Projectile.width = Projectile.height = 22;
			Projectile.netImportant = true;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 9;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
			Projectile.minionSlots = 0;
		}

		public bool attack;
		public int deathCounter;

		public override void AI()
		{
			deathCounter--;
			if (deathCounter == 1)
			{
				SoundEngine.PlaySound(SoundID.NPCKilled, Projectile.Center, 8);
				Gore.NewGore(Projectile.Center, Projectile.velocity, Main.rand.Next(11, 13), 1f);
				Projectile.active = false;
			}

			if (attack)
			{
				Projectile.aiStyle = 121;
				AIType = 625;
			}
		}
	}
}