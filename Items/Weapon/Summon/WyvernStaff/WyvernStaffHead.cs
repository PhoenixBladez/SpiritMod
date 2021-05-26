using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Summon.WyvernStaff
{
	public class WyvernStaffHead : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wyvern");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 3;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}
		public override void SetDefaults()
		{
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.minion = true;
			projectile.damage = 13;
			projectile.width = projectile.height = 22;
			projectile.netImportant = true;
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 9;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
			projectile.minionSlots = 0;

		}
		public bool attack;
		public int deathCounter;
		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			deathCounter--;
			if (deathCounter == 1)
				projectile.active = false;
			if (attack)
			{
				projectile.aiStyle = 121;
				aiType = 625;
			}
		}
	}
}