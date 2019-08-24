using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Summon
{
	public class EnchantedPaladinsHammerMinion : EnchantedPaladinsHammerINFO
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Enchanted Paladin's Hammer Minion");
			Main.projFrames[base.projectile.type] = 21;
			ProjectileID.Sets.MinionSacrificable[base.projectile.type] = true;
			ProjectileID.Sets.Homing[base.projectile.type] = true;
			ProjectileID.Sets.LightPet[projectile.type] = true;
			ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
		}

		public override void SetDefaults()
		{
			projectile.netImportant = true;
			projectile.width = 44;
			projectile.height = 44;
			projectile.friendly = true;
			Main.projPet[projectile.type] = true;
			projectile.minion = true;
			projectile.netImportant = true;
			projectile.minionSlots = 1;
			projectile.penetrate = -1;
			projectile.timeLeft = 18000;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			inertia = 30f;
			Main.projPet[projectile.type] = true;
			projectile.magic = true;
			projectile.aiStyle = 66;
			aiType = 533;
			projectile.damage = 50;
			Lighting.AddLight((int)(projectile.Center.X / 16f), (int)(projectile.Center.Y / 16f), 1f, 1f, 10f);
		}

		public override bool MinionContactDamage()
		{
			return true;
		}

		public override void CheckActive()
		{
			Player player = Main.player[projectile.owner];
			MyPlayer modPlayer = (MyPlayer)player.GetModPlayer(mod, "MyPlayer");
			if (player.dead)
				modPlayer.EnchantedPaladinsHammerMinion = false;

			if (modPlayer.EnchantedPaladinsHammerMinion)
				projectile.timeLeft = 2;

		}

		public override void CreateDust()
		{
			Lighting.AddLight((int)(projectile.Center.X / 16f), (int)(projectile.Center.Y / 16f), 1f, 1f, 10f);
		}

		public override void SelectFrame()
		{
			projectile.frameCounter++;
			if (projectile.frameCounter >= 6)
			{
				projectile.frameCounter = 0;
				projectile.frame = (projectile.frame + 1) % 3;
			}
		}

	}
}