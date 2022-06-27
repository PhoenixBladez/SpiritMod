using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Summon.WyvernStaff
{
	public class WyvernStaffBody : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wyvern");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 9;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
			Main.projFrames[Projectile.type] = 3;
		}

		public override void SetDefaults()
		{
			Projectile.penetrate = 600;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.friendly = true;
			Projectile.damage = 13;
			//projectile.extraUpdates = 1;
			Projectile.width = Projectile.height = 20;
			Projectile.minion = true;
			Projectile.minionSlots = 0;
			Projectile.aiStyle = -1;
		}

		public override void AI()
		{
			if (Main.netMode != NetmodeID.MultiplayerClient)
			{
				if (!Main.projectile[(int)Projectile.ai[0]].active)
				{
					Projectile.timeLeft = 0;
					Projectile.active = false;
					Gore.NewGore(Projectile.Center, Projectile.velocity, Main.rand.Next(11, 13), 1f);
					// NetMessage.SendData(28, -1, -1, "", projectile.whoAmI, -1f, 0.0f, 0.0f, 0, 0, 0);
				}
			}

			if (Projectile.ai[0] < (double)Main.projectile.Length)
			{
				// We're getting the center of this projectile.
				Vector2 projectileCenter = new Vector2(Projectile.position.X + Projectile.width * 0.5f, Projectile.position.Y + Projectile.height * 0.5f);
				// Then using that center, we calculate the direction towards the 'parent projectile' of this projectile.
				float dirX = Main.projectile[(int)Projectile.ai[0]].position.X + (Main.projectile[(int)Projectile.ai[0]].width / 2f) - projectileCenter.X;
				float dirY = Main.projectile[(int)Projectile.ai[0]].position.Y + (Main.projectile[(int)Projectile.ai[0]].height / 2f) - projectileCenter.Y;
				// We then use Atan2 to get a correct rotation towards that parent projectile.
				Projectile.rotation = (float)Math.Atan2(dirY, dirX) + 1.57f;
				// We also get the length of the direction vector.
				float length = (float)Math.Sqrt(dirX * dirX + dirY * dirY);
				// We calculate a new, correct distance.
				float dist = (length - Projectile.width) / length;
				float posX = dirX * dist;
				float posY = dirY * dist;

				// Reset the velocity of this projectile, because we don't want it to move on its own
				Projectile.velocity = Vector2.Zero;
				// And set this projectiles position accordingly to that of this projectiles parent projectile.
				Projectile.position.X = Projectile.position.X + posX;
				Projectile.position.Y = Projectile.position.Y + posY;
				Projectile.spriteDirection = Main.projectile[(int)Projectile.ai[0]].spriteDirection;
				if (Main.projectile[(int)Projectile.ai[0]].type != ModContent.ProjectileType<WyvernStaffBody>() && Main.projectile[(int)Projectile.ai[0]].type != ModContent.ProjectileType<WyvernStaffHead>())
				{
					Main.NewText("Uh oh. Something went wrong. Report to Spirit Mod devs immediately: Wyvern Staff Improper Typing");
					Projectile.active = false;
				}
			}
			else
			{
				Main.NewText("Uh oh. Something went wrong. Report to Spirit Mod devs immediately: Wyvern Staff Projectile whoAmI OOB");
				Projectile.active = false;
			}
		}
	}
}