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
			Main.npcFrameCount[NPC.type] = 3;
		}
		
		public override void SetDefaults()
		{
			NPC.friendly = false;
			NPC.lifeMax = 250;
			NPC.defense = 20;
			NPC.value = 0;
			NPC.aiStyle = -1;
            NPC.knockBackResist = 0f;
			NPC.width = 24;
			NPC.height = 50;
			NPC.damage = 0;
			NPC.lavaImmune = true;
            NPC.noGravity = true;
			NPC.noTileCollide = true;
			NPC.HitSound = SoundID.NPCHit13;
			NPC.DeathSound = SoundID.NPCDeath2;
		}
		public override void AI()
		{
			NPC parent = Main.npc[(int)NPC.ai[0]];
			Player player = Main.player[parent.target];
			counter++;
			if (counter < ROTATETIME)
			{
				NPC.ai[1] += ROTATESPEED;
				radius += ROTATEEXPAND;
				NPC.Center = (parent.Center - new Vector2(0, 30)) + (NPC.ai[1].ToRotationVector2() * radius);
			}
			if (counter == ROTATETIME)
			{
				int distance = Main.rand.Next(300, 500);
				bool spotPicked = false;
				while (!spotPicked)
				{
					NPC.ai[3] = Main.rand.Next(360);
					double anglex = Math.Sin(NPC.ai[3] * (Math.PI / 180));
					double angley = Math.Cos(NPC.ai[3] * (Math.PI / 180));
					posToBe.X = player.Center.X + (int)(distance * anglex);
					posToBe.Y = player.Center.Y + (int)(distance * angley);
					if (!Main.tile[(int)(posToBe.X / 16), (int)(posToBe.Y / 16)].HasTile && !Main.tile[(int)(posToBe.X / 16), (int)(posToBe.Y / 16)].HasTile)
					{
						spotPicked = true;
					}
					NPC.netUpdate = true;
				}
			}
			if (counter > ROTATETIME)
			{
				Vector2 direction = posToBe - NPC.Center;
				float speed = (float)Math.Sqrt(direction.Length());
				direction.Normalize();
				NPC.velocity = direction * speed;
				if (speed < 2)
				{
					NPC.Center = posToBe;
					NPC.velocity = Vector2.Zero;
					if (counter % FIRINGSPEED == 0)
					{
						Vector2 launchDirection = player.Center - NPC.Center;
						launchDirection.Normalize();
						launchDirection = launchDirection.RotatedBy(Main.rand.NextFloat(-0.5f, 0.5f));
						launchDirection *= 10;
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, launchDirection, ModContent.ProjectileType<AzureJelly>(), 35, 4, 255, player.whoAmI);
					}
				}
			}
			if (counter > ROTATETIME + BURSTTIME)
			{
				for (int i = 0; i < 8; i++)
				{
					Vector2 launchDirection = player.Center - NPC.Center;
					launchDirection.Normalize();
					launchDirection = launchDirection.RotatedBy(Main.rand.NextFloat(-0.5f, 0.5f));
					launchDirection *= 15;
					Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, launchDirection, ModContent.ProjectileType<AzureJelly>(), 35, 4, 255, player.whoAmI);
				}
				NPC.active = false;
			}
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}
		public override void FindFrame(int frameHeight)
		{
			NPC.frameCounter += 0.15f;
			NPC.frame.Y = frameHeight * ((int)NPC.frameCounter % Main.npcFrameCount[NPC.type]);
		}
	}
}
