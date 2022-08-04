using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Summon
{
	public class KnifeMinionProjectile : Minion
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Unbound Soul");
			Main.projFrames[Projectile.type] = 1;
			ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
			ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			Projectile.width = 24;
			Projectile.height = 32;
			Projectile.friendly = true;
			Projectile.minion = true;
			Projectile.minionSlots = 0f;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 18000;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.netImportant = true;

			Projectile.usesLocalNPCImmunity = true;
			//5a: projectile.localNPCHitCooldown = -1; // 1 hit per npc max
			Projectile.localNPCHitCooldown = 20; // o
		}

		public override void CheckActive()
		{
			MyPlayer mp = Main.player[Projectile.owner].GetModPlayer<MyPlayer>();

			if (Projectile.damage == 0) //This shouldn't happen
				Projectile.damage = (int)Main.player[Projectile.owner].GetDamage(Projectile.DamageType).ApplyTo(5);

			if (mp.Player.dead || !mp.rogueCrest)
				Projectile.active = false;

			if (mp.rogueCrest)
				Projectile.timeLeft = 2;
		}

		bool trailing = false;
		public override void Behavior()
		{
			Projectile.minionSlots = 0f;
			Projectile.rotation = Projectile.velocity.X * 0.25f;
			trailing = false;
			Projectile.tileCollide = false;
			Player player = Main.player[Projectile.owner];
			float num = Projectile.width * 1.1f;
			for (int i = 0; i < 1000; i++)
			{
				Projectile current = Main.projectile[i];
				if (i != Projectile.whoAmI && current.active && Projectile.owner == current.owner && Projectile.type == current.type && Math.Abs(Projectile.position.X - current.position.X) + Math.Abs(Projectile.position.Y - current.position.Y) < num)
				{
					if (Projectile.position.X < Main.projectile[i].position.X)
						Projectile.velocity.X -= 0.08f;
					else
						Projectile.velocity.X += 0.08f;

					if (Projectile.position.Y < Main.projectile[i].position.Y)
						Projectile.velocity.Y -= 0.08f;
					else
						Projectile.velocity.Y += 0.08f;
				}
			}

			Vector2 value = Projectile.position;
			float num21 = 500f;
			bool flag = false;
			Projectile.tileCollide = false;
			for (int j = 0; j < 200; j++)
			{
				NPC nPC = Main.npc[j];
				if (nPC.CanBeChasedBy(this, false))
				{
					float num3 = Vector2.Distance(nPC.Center, Projectile.Center);
					if ((num3 < num21 || !flag) && Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, nPC.position, nPC.width, nPC.height))
					{
						num21 = num3;
						value = nPC.Center;
						flag = true;
					}
				}
			}

			if (Vector2.Distance(player.Center, Projectile.Center) > (flag ? 1000f : 500f))
			{
				Projectile.ai[0] = 1f;
				Projectile.netUpdate = true;
			}

			if (Projectile.ai[0] == 1f)
				Projectile.tileCollide = false;

			if (flag && Projectile.ai[0] == 0f)
			{
				Vector2 value2 = value - Projectile.Center;
				if (value2.Length() > 200f)
				{
					value2.Normalize();
					Projectile.velocity = (Projectile.velocity * 20f + value2 * 6f) / 21f;
				}
				else
					Projectile.velocity *= (float)Math.Pow(0.97, 2.0);

				trailing = true;
				Projectile.tileCollide = false;
				Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X) + 1.57f;
				bool flag25 = false;
				int jim = 1;
				for (int index1 = 0; index1 < 200; index1++)
				{
					if (Main.npc[index1].CanBeChasedBy(Projectile, false) && Collision.CanHit(Projectile.Center, 1, 1, Main.npc[index1].Center, 1, 1))
					{
						float num23 = Main.npc[index1].position.X + (float)(Main.npc[index1].width / 2);
						float num24 = Main.npc[index1].position.Y + (float)(Main.npc[index1].height / 2);
						float num25 = Math.Abs(Projectile.position.X + (float)(Projectile.width / 2) - num23) + Math.Abs(Projectile.position.Y + (float)(Projectile.height / 2) - num24);
						if (num25 < 500f)
						{
							flag25 = true;
							jim = index1;
						}
					}
				}
				if (flag25)
				{
					float num1 = 68.5f;
					float num2 = Main.npc[jim].Center.X - Projectile.Center.X;
					float num3 = Main.npc[jim].Center.Y - Projectile.Center.Y;
					Vector2 direction5 = Main.npc[jim].Center - Projectile.Center;
					direction5.Normalize();
					Projectile.rotation = Projectile.DirectionTo(Main.npc[jim].Center).ToRotation() + 1.57f;
					float num4 = (float)Math.Sqrt(num2 * num2 + num3 * num3);
					float num5 = num1 / num4;
					float num6 = num2 * num5;
					float num7 = num3 * num5;
					int num8 = 10;
					if (Main.rand.Next(20) == 0)
					{
						Projectile.velocity.X = (Projectile.velocity.X * (num8 - 1) + num6) / num8;
						Projectile.velocity.Y = (Projectile.velocity.Y * (num8 - 1) + num7) / num8;
						Projectile.netUpdate = true;
					}
				}
			}
			else
			{
				if (!Collision.CanHitLine(Projectile.Center, 1, 1, player.Center, 1, 1))
					Projectile.ai[0] = 1f;

				float num4 = 6f;
				if (Projectile.ai[0] == 1f)
					num4 = 15f;

				Vector2 center = Projectile.Center;
				Vector2 vector = player.Center - center;
				Projectile.ai[1] = 3600f;
				Projectile.netUpdate = true;
				int num5 = 1;
				for (int k = 0; k < Projectile.whoAmI; k++)
				{
					if (Main.projectile[k].active && Main.projectile[k].owner == Projectile.owner && Main.projectile[k].type == Projectile.type)
						num5++;
				}
				vector.X -= (10 + num5 * 40) * player.direction;
				vector.Y -= 70f;
				float num6 = vector.Length();
				if (num6 > 200f && num4 < 9f)
					num4 = 9f;

				if (num6 < 100f && Projectile.ai[0] == 1f && !Collision.SolidCollision(Projectile.position, Projectile.width, Projectile.height))
				{
					Projectile.ai[0] = 0f;
					Projectile.netUpdate = true;
				}
				if (num6 > 2000f)
					Projectile.Center = player.Center;

				if (num6 > 48f)
				{
					vector.Normalize();
					vector *= num4;
					float num7 = 10f;
					Projectile.velocity = (Projectile.velocity * num7 + vector) / (num7 + 1f);
				}
				else
				{
					Projectile.direction = Main.player[Projectile.owner].direction;
					Projectile.velocity *= (float)Math.Pow(0.9, 2.0);
				}
			}
			if (Projectile.velocity.X > 0f)
				Projectile.spriteDirection = (Projectile.direction = -1);
			else if (Projectile.velocity.X < 0f)
				Projectile.spriteDirection = (Projectile.direction = 1);

			if (Projectile.ai[1] > 0f)
				Projectile.ai[1] += 1f;

			if (Projectile.ai[1] > 140f)
			{
				Projectile.ai[1] = 0f;
				Projectile.netUpdate = true;
			}
		}

		public override void SelectFrame() { }

		public override bool PreDraw(ref Color lightColor)
		{
			if (trailing)
			{
				Vector2 drawOrigin = new Vector2(TextureAssets.Projectile[Projectile.type].Value.Width * 0.5f, Projectile.height * 0.5f);
				for (int k = 0; k < Projectile.oldPos.Length; k++)
				{
					Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
					Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
					Main.spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, drawPos, null, color * .6f, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0f); ;
				}
			}

			return !trailing;
		}
	}
}