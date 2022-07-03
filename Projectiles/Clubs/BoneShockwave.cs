using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using SpiritMod.Dusts;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace SpiritMod.Projectiles.Clubs
{
	public class BoneShockwave : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Axe Fire");
		}

		public override void SetDefaults()
		{
			Projectile.hostile = false;
			Projectile.width = 24;
			Projectile.height = 24;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.damage = 1;
			Projectile.penetrate = -1;
			Projectile.alpha = 255;
            Projectile.timeLeft = 3;
			Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.DamageType = DamageClass.Melee;
		}

		//projectile.ai[0]: how many more pillars. Each one is one less
		//projectile.ai[1]: 0: center, -1: going left, 1: going right
		bool activated = false;
		float startposY = 0;
		public override bool PreAI()
		{
			if (startposY == 0) {
				startposY = Projectile.position.Y;
                if (Main.tile[(int)Projectile.Center.X / 16, (int)(Projectile.Center.Y / 16)].BlockType == BlockType.Solid)
                {
                    Projectile.active = false;
                }
			}
			Projectile.velocity.X = 0;
			if (!activated) {
				Projectile.velocity.Y = 24;
			}
			else {
				Projectile.velocity.Y = -3;
				for (int i = 0; i < 5; i++)
				{
					int dust = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height * 2, DustType<Dusts.FloranClubDust>());
                    Main.dust[dust].scale *= Main.rand.NextFloat(.65f, .9f);
					//Main.dust[dust].velocity = Vector2.Zero;
					//Main.dust[dust].noGravity = true;
				}
				if (Projectile.timeLeft == 5 && Projectile.ai[0] > 0) {
					if (Projectile.ai[1] == -1 || Projectile.ai[1] == 0) {
						Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center.X - Projectile.width, startposY, 0, 0, ModContent.ProjectileType<BoneShockwave>(), Projectile.damage, Projectile.knockBack, Projectile.owner, Projectile.ai[0] - 1, -1);
					}
					if (Projectile.ai[1] == 1 || Projectile.ai[1] == 0) {
						Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center.X + Projectile.width, startposY, 0, 0, ModContent.ProjectileType<BoneShockwave>(), Projectile.damage, Projectile.knockBack, Projectile.owner, Projectile.ai[0] - 1, 1);
					}
				}
			}
			return false;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (oldVelocity.Y != Projectile.velocity.Y && !activated) {
				startposY = Projectile.position.Y;
				Projectile.velocity.Y = -2;
				activated = true;
				Projectile.timeLeft = 10;
			}
			return false;
		}
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			fallThrough = false;
			return true;
		}

	}
}