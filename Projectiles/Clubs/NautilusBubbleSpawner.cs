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
	public class NautilusBubbleSpawner : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nautilus Bubble");
		}

		public override void SetDefaults()
		{
			Projectile.hostile = false;
			Projectile.width = 30;
			Projectile.height = 30;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.damage = 1;
			Projectile.penetrate = -1;
            Projectile.hide = true;
			Projectile.alpha = 255;
            Projectile.timeLeft = 75;
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
            if (startposY == 0)
            {
                startposY = Projectile.position.Y;
                if (Main.tile[(int)Projectile.Center.X / 16, (int)(Projectile.Center.Y / 16)].BlockType == BlockType.Solid)
                {
                    Projectile.active = false;
                }
            }
            Projectile.velocity.X = 0;
            if (!activated)
            {
                Projectile.velocity.Y = 24;
				if (Projectile.timeLeft < 58)
                {
                    Projectile.Kill();
                }
            }
            else
            {
                if (Projectile.timeLeft % 15 == 0)
                {
                    int proj = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center.X + Main.rand.Next(-15, 15), Projectile.Center.Y + 6, 0, Main.rand.NextFloat(-2f, -1f), ModContent.ProjectileType<NautilusBubbleProj>(), Projectile.damage / 4, Projectile.owner, 0, 0f);
                    Main.projectile[proj].scale = Main.rand.NextFloat(.8f, 1f);
                    Main.projectile[proj].timeLeft = Main.rand.Next(90, 110);
                }
            }
            return false;
		}
        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (oldVelocity.Y != Projectile.velocity.Y && !activated) {
				startposY = Projectile.position.Y;
				activated = true;
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