using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Bestiary;

namespace SpiritMod.NPCs.MycelialBotanist
{
	public class MycelialBotanist : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mycelial Botanist");
			Main.npcFrameCount[NPC.type] = 11;
		}

		int timer = 0;
		bool shooting = false;
		int frame = 0;

		public override void SetDefaults()
		{
			NPC.aiStyle = 3;
			NPC.lifeMax = 65;
			NPC.defense = 6;
			NPC.value = 65f;
			NPC.knockBackResist = 0.7f;
			NPC.width = 56;
			NPC.height = 50;
			NPC.damage = 15;
			NPC.lavaImmune = false;
			NPC.noTileCollide = false;
			NPC.alpha = 0;
			NPC.dontTakeDamage = false;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.Zombie76;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<Items.Banners.MyceliumBotanistBanner>();
			AIType = NPCID.Skeleton;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundMushroom,
				new FlavorTextBestiaryInfoElement("A gardener’s hobbies are not to be ridiculed. Get in the way of these naturalists, and you’ll face bloom and doom."),
			});
		}

		public override bool PreAI()
		{
			NPC.TargetClosest();

			if (shooting)
			{
				NPC.velocity.Y = 6;
				NPC.velocity.X *= 0.08f;
			}
			if (timer == 240)
			{
				shooting = true;
				timer = 0;
			}

			if (!shooting)
				timer++;

			if (NPC.velocity.X < 0f)
				NPC.spriteDirection = -1;
			else if (NPC.velocity.X > 0f)
				NPC.spriteDirection = 1;

			if (frame == 10 && NPC.frameCounter == 6)
			{
				SoundEngine.PlaySound(SoundID.Item1, NPC.Center);
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					Vector2 direction = Main.player[NPC.target].Center - NPC.Center;
					float distance = MathHelper.Clamp(direction.Length(), -250, 250);
					direction.Normalize();
					direction *= distance / 20;
					Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X + (NPC.direction * 12), NPC.Center.Y, direction.X, direction.Y, ModContent.ProjectileType<MyceliumHat>(), 15, 1, Main.myPlayer, NPC.whoAmI);
				}
			}

			if (Main.player[NPC.target].Center.X < NPC.Center.X)
				NPC.spriteDirection = -1;
			else
				NPC.spriteDirection = 1;
			return true;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) => (spawnInfo.SpawnTileType == TileID.MushroomGrass) && spawnInfo.SpawnTileY > Main.rockLayer ? 2f : 0f;

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 30; k++)
			{
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Rope, 2.5f * hitDirection, -2.5f, 0, Color.White, Main.rand.NextFloat(.3f, 1.1f));
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Harpy, 2.5f * hitDirection, -2.5f, 0, Color.White, Main.rand.NextFloat(.3f, 1.1f));
			}

			if (NPC.life <= 0)
			{
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("MycelialBotanistGore1").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("MycelialBotanistGore2").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("MycelialBotanistGore3").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("MycelialBotanistGore4").Type, 1f);
			}
		}
		
		public override void FindFrame(int frameHeight)
		{
			NPC.frameCounter++;

			if (!shooting)
			{
				if (NPC.frameCounter >= 7)
				{
					frame++;
					NPC.frameCounter = 0;
				}

				if (frame >= 5)
					frame = 0;
			}
			else
			{
				if (NPC.frameCounter >= 7)
				{
					frame++;
					NPC.frameCounter = 0;
				}

				if (frame >= 11)
				{
					shooting = false;
					frame = 0;
				}

				if (frame < 5)
					frame = 5;
			}
			NPC.frame.Y = frameHeight * frame;
		}
	}
}