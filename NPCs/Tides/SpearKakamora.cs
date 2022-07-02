using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.Sets.TideDrops;

namespace SpiritMod.NPCs.Tides
{
	public class SpearKakamora : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Kakamora Lobber");
			Main.npcFrameCount[NPC.type] = 7;
		}

		public override void SetDefaults()
		{
			NPC.width = 52;
			NPC.height = 38;
			NPC.damage = 28;
			NPC.defense = 14;
			AIType = NPCID.SnowFlinx;
			NPC.aiStyle = 3;
			NPC.lifeMax = 110;
			NPC.knockBackResist = .3f;
			NPC.value = 200f;
			NPC.noTileCollide = false;
			NPC.HitSound = SoundID.NPCHit2;
			NPC.DeathSound = SoundID.NPCDeath1;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<Items.Banners.KakamoraThrowerBanner>();
		}

		int timer = 0;
		bool charging = false;
		int chargeDirection = -1; //-1 is left, 1 is right
		bool throwing = false;
		bool thrownCoconut = false;

		public override void AI()
		{
			if (NPC.wet)
			{
				NPC.noGravity = true;
				if (NPC.velocity.Y > -7)
					NPC.velocity.Y -= .085f;
				return;
			}
			else
				NPC.noGravity = false;

			NPC.TargetClosest(true);
			Player player = Main.player[NPC.target];

			var list2 = Main.projectile.Where(x => x.Hitbox.Intersects(NPC.Hitbox));
			foreach (var proj in list2)
			{
				if (proj.type == ModContent.ProjectileType<ShamanBolt>() && proj.active && NPC.life < NPC.lifeMax - 30)
				{
					NPC.life += 30;
					NPC.HealEffect(30, true);
					proj.active = false;
				}
				else if (proj.type == ModContent.ProjectileType<ShamanBolt>() && proj.active && NPC.life > NPC.lifeMax - 30)
				{
					NPC.life += NPC.lifeMax - NPC.life;
					NPC.HealEffect(NPC.lifeMax - NPC.life, true);
					proj.active = false;
				}
			}

			if (!throwing || charging)
				timer++;

			if (Main.rand.NextBool(100) && !charging && timer > 50)
			{
				if (!throwing)
				{
					throwing = true;
					NPC.frameCounter = 0;
				}
			}

			if (throwing)
			{
				NPC.aiStyle = -1;
				NPC.velocity.X = 0;

				if (player.position.X > NPC.position.X)
					NPC.spriteDirection = 1;
				else
					NPC.spriteDirection = -1;
			}

			if (timer == 300)
			{
				charging = true;
				SoundEngine.PlaySound(SoundID.NPCHit51, NPC.Center);
				NPC.velocity.X = 0;
				if (player.position.X > NPC.position.X)
				{
					chargeDirection = 1;
					NPC.spriteDirection = 1;
				}
				else
				{
					chargeDirection = -1;
					NPC.spriteDirection = -1;
				}
			}
			if (charging)
			{
				if (timer == 350)
				{
					NPC.velocity.X = 4 * chargeDirection;
					NPC.velocity.Y = -7;
				}
				if (timer < 350)
				{
					NPC.aiStyle = -1;
					NPC.velocity.X = -0.6f * chargeDirection;
				}
				else
				{
					NPC.aiStyle = 26;
					NPC.spriteDirection = NPC.direction;
				}
				if (Math.Abs(NPC.velocity.X) < 3 && timer > 350)
				{
					charging = false;
					NPC.aiStyle = 3;
					NPC.rotation = 0;
					timer = 0;
				}
				if ((chargeDirection == 1 && player.position.X < NPC.position.X) || (chargeDirection == -1 && player.position.X > NPC.position.X) && timer > 180)
				{
					NPC.rotation += 0.1f * NPC.velocity.X;
					NPC.velocity.Y = 5;
				}
			}
			else if (!throwing)
			{
				NPC.rotation = 0;
				NPC.aiStyle = 3;
				NPC.spriteDirection = NPC.direction;
				var list = Main.npc.Where(x => x.Hitbox.Intersects(NPC.Hitbox));
				foreach (var npc2 in list)
				{
					if (npc2.type == ModContent.NPCType<LargeCrustecean>() && NPC.Center.Y > npc2.Center.Y && npc2.active)
					{
						NPC.velocity.X = npc2.direction * 7;
						NPC.velocity.Y = -2;
						SoundEngine.PlaySound(new SoundStyle("SpiritMod/Sounds/Kakamora/KakamoraHit"), NPC.Center);
					}
				}
			}
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.AddCommon<CoconutGun>(50);
			npcLoot.AddCommon<TikiJavelin>(50);
		}

		public override void FindFrame(int frameHeight)
		{
			if (!throwing)
			{
				if (NPC.collideY)
				{
					if (charging)
						NPC.frameCounter += 0.1f;

					NPC.frameCounter += 0.2f;
					NPC.frameCounter %= 4;
					int frame = (int)NPC.frameCounter;
					NPC.frame.Y = frame * frameHeight;
				}
			}
			else
			{
				NPC.frameCounter += 0.3f;
				if (NPC.frameCounter >= 3f)
				{
					throwing = false;
					NPC.frameCounter = 0;
					NPC.aiStyle = 3;
					thrownCoconut = false;
				}
				if (NPC.frameCounter >= 1.3f && !thrownCoconut)
				{
					SoundEngine.PlaySound(SoundID.Item, (int)NPC.position.X, (int)NPC.position.Y, 1);
					Vector2 direction = Vector2.Normalize(Main.player[NPC.target].Center - NPC.Center) * 9f;
					float A = (float)Main.rand.Next(-50, 50) * 0.02f;
					float B = (float)Main.rand.Next(-50, 50) * 0.02f;
					Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X + (NPC.direction * 12), NPC.Center.Y, direction.X + A, direction.Y + B, ModContent.ProjectileType<CoconutHostile>(), NPC.damage / 2, 1, Main.myPlayer, 0, 0);
					thrownCoconut = true;
				}
				NPC.frameCounter %= 3;
				int frame = (int)NPC.frameCounter + 4;
				NPC.frame.Y = frame * frameHeight;
			}
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 10; k++)
			{
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.DynastyWood, 2.5f * hitDirection, -2.5f, 0, Color.White, 0.7f);
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.DynastyWood, 2.5f * hitDirection, -2.5f, 0, default, .34f);
			}

			if (NPC.life <= 0)
			{
				SoundEngine.PlaySound(new SoundStyle("SpiritMod/Sounds/Kakamora/KakamoraDeath"), NPC.Center);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/Kakamora_Gore").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/Kakamora_Gore1").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/Kakamora_GoreSpear").Type, 1f);
			}
		}
	}
}
