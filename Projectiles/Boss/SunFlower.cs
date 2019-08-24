using System;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Projectiles.Boss
{
	public class SunFlower : ModNPC
	{
		int timer = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Solar Flower");
		}

		public override void SetDefaults()
		{
			npc.width = 38;
			npc.height = 26;
			npc.damage = 18;
			npc.defense = 0;
			npc.lifeMax = 120;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.value = 60f;
			npc.knockBackResist = .1f;
			npc.aiStyle = 0;
			aiType = NPCID.BoundGoblin;
			animationType = NPCID.BoundGoblin;
		}

		public override void AI()
		{
			Player player = Main.player[npc.target];
			npc.velocity *= 0.95f;
			float num801 = npc.position.X + (float)(npc.width / 2) - player.position.X - (float)(player.width / 2);
			float num802 = npc.position.Y + (float)npc.height - 59f - player.position.Y - (float)(player.height / 2);
			float num803 = (float)Math.Atan2((double)num802, (double)num801) + MathHelper.PiOver2;
			if (num803 < 0f)
				num803 += (2 * MathHelper.Pi);
			else if (num803 > (2 * MathHelper.Pi))
				num803 -= (2 * MathHelper.Pi);

			float num804 = 0.1f;
			if (npc.rotation < num803)
			{
				if (num803 - npc.rotation > MathHelper.Pi)
					npc.rotation -= num804;
				else
					npc.rotation += num804;
			}
			else if (npc.rotation > num803)
			{
				if (npc.rotation - num803 > MathHelper.Pi)
					npc.rotation += num804;
				else
					npc.rotation -= num804;
			}

			if (npc.rotation > num803 - num804 && npc.rotation < num803 + num804)
				npc.rotation = num803;

			if (npc.rotation < 0f)
				npc.rotation += 2 * MathHelper.Pi;
			else if (npc.rotation > 2 * MathHelper.Pi)
				npc.rotation -= 6.283f;

			if (npc.rotation > num803 - num804 && npc.rotation < num803 + num804)
				npc.rotation = num803;

			timer++;
			bool expertMode = Main.expertMode;
			if (timer == 100 || timer == 300 || timer == 600)
			{
				Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 91);
				Vector2 direction = Main.player[npc.target].Center - npc.Center;
				direction.Normalize();
				direction.X *= 5f;
				direction.Y *= 5f;

				int amountOfProjectiles = 1;
				for (int i = 0; i < amountOfProjectiles; ++i)
				{
					float A = (float)Main.rand.Next(-200, 200) * 0.05f;
					float B = (float)Main.rand.Next(-200, 200) * 0.05f;
					int damage = expertMode ? 6 : 12;
					Projectile.NewProjectile(npc.Center, direction,
						mod.ProjectileType("SolarBeamHostile"), damage, 0, Main.myPlayer);
				}
			}
			else if (timer == 700)
				timer = 0;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
			{
				npc.position.X = npc.position.X + (float)(npc.width / 2);
				npc.position.Y = npc.position.Y + (float)(npc.height / 2);
				npc.width = 30;
				npc.height = 30;
				npc.position.X = npc.position.X - (float)(npc.width / 2);
				npc.position.Y = npc.position.Y - (float)(npc.height / 2);
				for (int num621 = 0; num621 < 20; num621++)
				{
					int num622 = Dust.NewDust(npc.position, npc.width, npc.height,
						3, 0f, 0f, 100, default(Color), 2f);
					Main.dust[num622].velocity *= 3f;
					if (Main.rand.Next(2) == 0)
					{
						Main.dust[num622].scale = 0.5f;
						Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
					}
				}
			}
		}

	}
}