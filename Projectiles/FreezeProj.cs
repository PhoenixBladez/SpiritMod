using System;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles
{
	class FreezeProj : ModProjectile
	{
		public static int _type;

		public override string Texture => SpiritMod.EMPTY_TEXTURE;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Frost Ward");
		}

		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.timeLeft = 60;
			projectile.height = 200;
			projectile.width = 200;
			projectile.alpha = 255;
		}

		public override bool? CanCutTiles()
		{
			return false;
		}

		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			projectile.Center = player.Center;

			Rectangle hitbox = projectile.Hitbox;
			for (int i = 0; i < 200; i++)
			{
				NPC target = Main.npc[i];
				if (!target.friendly && target.catchItem == 0 && hitbox.Contains(target.Center.ToPoint()))
					target.AddBuff(Buffs.MageFreeze._type, 240);
			}
			if (Main.rand.Next(9) == 1)
			{
				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 187);
				Main.dust[dust].noGravity = true;
				Main.dust[dust].scale = 0.9f;
			}
		}
	}
}
