using Microsoft.Xna.Framework;
using SpiritMod.Items.Sets.SpiritSet;
using Terraria;
using Terraria.ID;
using SpiritMod.Tiles.Block;
using System.Linq;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Spirit
{
	public class AncientDemon : ModNPC
	{
		int timer = 0;

		public override void SetStaticDefaults() => DisplayName.SetDefault("Ancient Specter");

		public override void SetDefaults()
		{
			NPC.width = 80;
			NPC.height = 80;
			NPC.damage = 32;
			NPC.defense = 19;
			NPC.lifeMax = 300;
			NPC.HitSound = SoundID.NPCHit2;
			NPC.DeathSound = SoundID.NPCDeath6;
			NPC.value = 460f;
			NPC.knockBackResist = .15f;
			NPC.aiStyle = 44;
			AIType = NPCID.FlyingAntlion;
			NPC.noGravity = true;
			NPC.noTileCollide = true;
		}

		public override void AI()
		{
			timer++;

			if (timer == 20)
			{
				NPC.noTileCollide = true;
				timer = 0;
			}

			Lighting.AddLight((int)((NPC.position.X + (NPC.width / 2f)) / 16f), (int)((NPC.position.Y + (NPC.height / 2f)) / 16f), 0f, 0.0675f, 0.250f);

			if (Main.rand.Next(150) == 5) //Fires desert feathers like a shotgun
			{
				Vector2 direction = Main.player[NPC.target].Center - NPC.Center;
				direction.Normalize();
				direction.X *= 6f;
				direction.Y *= 6f;

				int amountOfProjectiles = Main.rand.Next(1, 2);
				for (int i = 0; i < amountOfProjectiles; ++i)
				{
					float A = Main.rand.Next(-1, 1) * 0.01f;
					float B = Main.rand.Next(-1, 1) * 0.01f;
					Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center.X, NPC.Center.Y, direction.X + A, direction.Y + B, ModContent.ProjectileType<SpiritScythe>(), 30, 1, Main.myPlayer, 0, 0);
				}
			}
			NPC.spriteDirection = NPC.direction;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.Asphalt, 0f, 0f, 100, default, 1f);

			if (NPC.life <= 0)
			{
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, 13);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, 12);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, 11);
				NPC.position.X = NPC.position.X + (NPC.width / 2);
				NPC.position.Y = NPC.position.Y + (NPC.height / 2);
				NPC.width = 30;
				NPC.height = 30;
				NPC.position.X = NPC.position.X - (NPC.width / 2);
				NPC.position.Y = NPC.position.Y - (NPC.height / 2);
				for (int num621 = 0; num621 < 20; num621++)
				{
					int num622 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.Asphalt, 0f, 0f, 100, default, 2f);
					Main.dust[num622].velocity *= 3f;
					if (Main.rand.Next(2) == 0)
					{
						Main.dust[num622].scale = 0.5f;
						Main.dust[num622].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
					}
				}
				for (int num623 = 0; num623 < 40; num623++)
				{
					int num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.Asphalt, 0f, 0f, 100, default, 3f);
					Main.dust[num624].noGravity = true;
					Main.dust[num624].velocity *= 5f;
					num624 = Dust.NewDust(new Vector2(NPC.position.X, NPC.position.Y), NPC.width, NPC.height, DustID.Asphalt, 0f, 0f, 100, default, 2f);
					Main.dust[num624].velocity *= 2f;
				}
			}
		}
		private static int[] SpawnTiles = { };
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			Player player = spawnInfo.Player;
			if (!player.GetSpiritPlayer().ZoneSpirit)
				return 0f;
			if (SpawnTiles.Length == 0)
			{
				int[] Tiles = { ModContent.TileType<SpiritDirt>(), ModContent.TileType<SpiritStone>(), ModContent.TileType<SpiritGrass>(), ModContent.TileType<SpiritIce>() };
				SpawnTiles = Tiles;
			}
			return SpawnTiles.Contains(Main.tile[spawnInfo.SpawnTileX, spawnInfo.SpawnTileY].TileType) && player.position.Y / 16 >= Main.maxTilesY - 330 && player.GetSpiritPlayer().ZoneSpirit && !spawnInfo.PlayerSafe ? 2f : 0f;
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot) => npcLoot.AddCommon(ModContent.ItemType<SoulShred>(), 3);
	}
}