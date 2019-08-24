using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Summon
{
	public class ProbeMinion : ProbeINFO
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Probe");
			Main.projFrames[base.projectile.type] = 1;
			ProjectileID.Sets.MinionSacrificable[base.projectile.type] = true;
			ProjectileID.Sets.Homing[base.projectile.type] = true;
			ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
		}

		public override void SetDefaults()
		{
			projectile.netImportant = true;
			projectile.width = 18;
			projectile.height = 18;
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
			ProjectileID.Sets.LightPet[projectile.type] = true;
			Main.projPet[projectile.type] = true;
			projectile.magic = true;
			projectile.aiStyle = 66;
			aiType = 387;
			projectile.damage = 36;
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
				modPlayer.ProbeMinion = false;

			if (modPlayer.ProbeMinion)
				projectile.timeLeft = 2;

		}

		public override void SelectFrame()
		{
			projectile.frameCounter++;
			if (projectile.frameCounter >= 1)
			{
				projectile.frameCounter = 0;
				projectile.frame = (projectile.frame + 1) % 3;
			}
		}

	}
}