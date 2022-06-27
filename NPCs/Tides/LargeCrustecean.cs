using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.Sets.TideDrops;

namespace SpiritMod.NPCs.Tides
{
	public class LargeCrustecean : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bubble Brute");
			Main.npcFrameCount[NPC.type] = 9;
		}

		public override void SetDefaults()
		{
			NPC.width = 80;
			NPC.height = 82;
			NPC.damage = 25;
			NPC.defense = 4;
			AIType = NPCID.WalkingAntlion;
			NPC.aiStyle = 3;
			NPC.lifeMax = 375;
			NPC.knockBackResist = .2f;
			NPC.value = 200f;
			NPC.noTileCollide = false;
			NPC.HitSound = SoundID.NPCHit18;
			NPC.DeathSound = SoundID.NPCDeath5;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<Items.Banners.BubbleBruteBanner>();
		}

		bool blocking = false;
		int blockTimer = 0;

		public override void AI()
		{
			NPC.TargetClosest(true);
			Player player = Main.player[NPC.target];
			blockTimer++;
			if (NPC.wet)
			{
				NPC.noGravity = true;
				if (NPC.velocity.Y > -7)
					NPC.velocity.Y -= .085f;
				return;
			}
			else
				NPC.noGravity = false;

			if (blockTimer == 200)
			{
				//   Main.PlaySound(SoundLoader.customSoundType, npc.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/Kakamora/KakamoraThrow"));
				NPC.frameCounter = 0;
				NPC.velocity.X = 0;
			}

			if (blockTimer > 250)
				blocking = true;

			if (blockTimer > 350)
			{
				blocking = false;
				blockTimer = 0;
				NPC.frameCounter = 0;
			}
			if (blocking)
			{
				NPC.aiStyle = 0;

				if (player.position.X > NPC.position.X)
					NPC.spriteDirection = 1;
				else
					NPC.spriteDirection = -1;
			}
			else
			{
				NPC.spriteDirection = NPC.direction;
				NPC.aiStyle = 3;
			}
		}

		public override void OnKill()
		{
			if (Main.rand.NextBool(15))
				Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ModContent.ItemType<PumpBubbleGun>());

			if (Main.rand.Next(3) != 0)
				Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ModContent.ItemType<TribalScale>(), Main.rand.Next(2) + 1);
		}

		public override void FindFrame(int frameHeight)
		{
			if ((NPC.collideY || NPC.wet) && !blocking)
			{
				NPC.frameCounter += 0.2f;
				NPC.frameCounter %= 6;
				int frame = (int)NPC.frameCounter;
				NPC.frame.Y = frame * frameHeight;
			}

			if (NPC.wet)
				return;

			if (blocking)
			{
				NPC.frameCounter += 0.05f;
				NPC.frameCounter = MathHelper.Clamp((float)NPC.frameCounter, 0, 2.9f);
				int frame = (int)NPC.frameCounter;
				NPC.frame.Y = (frame + 6) * frameHeight;
				if (NPC.frameCounter > 2 && blockTimer % 5 == 0)
				{
					SoundEngine.PlaySound(SoundID.Item, (int)NPC.position.X, (int)NPC.position.Y, 85);
					Projectile.NewProjectile(NPC.Center.X + (NPC.direction * 34), NPC.Center.Y - 4, NPC.direction * Main.rand.NextFloat(3, 6), 0 - Main.rand.NextFloat(1), ModContent.ProjectileType<LobsterBubbleSmall>(), NPC.damage / 2, 1, Main.myPlayer, 0, 0);
				}
			}
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 30; k++)
			{
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, 2.5f * hitDirection, -2.5f, 0, Color.White, 0.7f);
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Blood, 2.5f * hitDirection, -2.5f, 0, default, 1.14f);
			}

			if (NPC.life <= 0)
				for (int i = 1; i < 8; ++i)
					Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/LargeCrustacean/lobster" + i).Type, 1f);
		}
	}
}