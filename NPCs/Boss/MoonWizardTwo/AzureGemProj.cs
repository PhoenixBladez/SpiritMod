using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using SpiritMod.Buffs;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.NPCs.Boss.MoonWizardTwo.Projectiles;
using SpiritMod.Prim;
using SpiritMod.Utilities;

namespace SpiritMod.NPCs.Boss.MoonWizardTwo
{
	public class AzureGemProj : ModNPC
	{
		private const int ROTATETIME = 90;
		private const float ROTATEEXPAND = 1;
		private const float ROTATESPEED = 0.1f;
		private const int FIRINGSPEED = 50;
		private const int BURSTTIME = 299;

		private int counter;
		private float radius;
		private Vector2 posToBe = Vector2.Zero;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Azure Gem");
			Main.npcFrameCount[npc.type] = 3;
		}
		
		public override void SetDefaults()
		{
			npc.friendly = false;
			npc.lifeMax = 250;
			npc.defense = 20;
			npc.value = 0;
			npc.aiStyle = -1;
            npc.knockBackResist = 0f;
			npc.width = 24;
			npc.height = 50;
			npc.damage = 0;
			npc.lavaImmune = true;
            npc.noGravity = true;
			npc.noTileCollide = true;
			npc.HitSound = SoundID.NPCHit13;
			npc.DeathSound = SoundID.NPCDeath2;
		}
		public override void AI()
		{
			NPC parent = Main.npc[(int)npc.ai[0]];
			Player player = Main.player[parent.target];
			counter++;
			if (counter < ROTATETIME)
			{
				npc.ai[1] += ROTATESPEED;
				radius += ROTATEEXPAND;
				npc.Center = (parent.Center - new Vector2(0, 30)) + (npc.ai[1].ToRotationVector2() * radius);
			}
			if (counter == ROTATETIME)
			{
				int distance = Main.rand.Next(300, 500);
				bool spotPicked = false;
				while (!spotPicked)
				{
					npc.ai[3] = Main.rand.Next(360);
					double anglex = Math.Sin(npc.ai[3] * (Math.PI / 180));
					double angley = Math.Cos(npc.ai[3] * (Math.PI / 180));
					posToBe.X = player.Center.X + (int)(distance * anglex);
					posToBe.Y = player.Center.Y + (int)(distance * angley);
					if (!Main.tile[(int)(posToBe.X / 16), (int)(posToBe.Y / 16)].active() && !Main.tile[(int)(posToBe.X / 16), (int)(posToBe.Y / 16)].active())
					{
						spotPicked = true;
					}
					npc.netUpdate = true;
				}
			}
			if (counter > ROTATETIME)
			{
				Vector2 direction = posToBe - npc.Center;
				float speed = (float)Math.Sqrt(direction.Length());
				direction.Normalize();
				npc.velocity = direction * speed;
				if (speed < 2)
				{
					npc.Center = posToBe;
					npc.velocity = Vector2.Zero;
					if (counter % FIRINGSPEED == 0)
					{
						Vector2 launchDirection = player.Center - npc.Center;
						launchDirection.Normalize();
						launchDirection = launchDirection.RotatedBy(Main.rand.NextFloat(-0.5f, 0.5f));
						launchDirection *= 10;
						Projectile.NewProjectile(npc.Center, launchDirection, ModContent.ProjectileType<AzureJelly>(), 35, 4, 255, player.whoAmI);
					}
				}
			}
			if (counter > ROTATETIME + BURSTTIME)
			{
				for (int i = 0; i < 8; i++)
				{
					Vector2 launchDirection = player.Center - npc.Center;
					launchDirection.Normalize();
					launchDirection = launchDirection.RotatedBy(Main.rand.NextFloat(-0.5f, 0.5f));
					launchDirection *= 15;
					Projectile.NewProjectile(npc.Center, launchDirection, ModContent.ProjectileType<AzureJelly>(), 35, 4, 255, player.whoAmI);
				}
				npc.active = false;
			}
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}
		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.15f;
			npc.frame.Y = frameHeight * ((int)npc.frameCounter % Main.npcFrameCount[npc.type]);
		}
	}
}
