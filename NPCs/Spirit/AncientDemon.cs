using Microsoft.Xna.Framework;
using SpiritMod.Items.Sets.SpiritSet;
using SpiritMod.Items.Weapon.Swung;
using SpiritMod.Projectiles.Hostile;
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

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Specter");
		}

		public override void SetDefaults()
		{
			npc.width = 80;
			npc.height = 80;
			npc.damage = 32;
			npc.defense = 19;
			npc.lifeMax = 300;
			npc.HitSound = SoundID.NPCHit2;
			npc.DeathSound = SoundID.NPCDeath6;
			npc.value = 460f;
			npc.knockBackResist = .15f;
			npc.aiStyle = 44;
			aiType = NPCID.FlyingAntlion;
			npc.noGravity = true;
			npc.noTileCollide = true;
		}

		public override void AI()
		{
			timer++;

			if (timer == 20) {
				npc.noTileCollide = true;
				timer = 0;
			}
			Lighting.AddLight((int)((npc.position.X + (float)(npc.width / 2)) / 16f), (int)((npc.position.Y + (float)(npc.height / 2)) / 16f), 0f, 0.0675f, 0.250f);
			if (Main.rand.Next(150) == 5) //Fires desert feathers like a shotgun
			{
				Vector2 direction = Main.player[npc.target].Center - npc.Center;
				direction.Normalize();
				direction.X *= 6f;
				direction.Y *= 6f;

				int amountOfProjectiles = Main.rand.Next(1, 2);
				for (int i = 0; i < amountOfProjectiles; ++i) {
					float A = (float)Main.rand.Next(-1, 1) * 0.01f;
					float B = (float)Main.rand.Next(-1, 1) * 0.01f;
					Projectile.NewProjectile(npc.Center.X, npc.Center.Y, direction.X + A, direction.Y + B, ModContent.ProjectileType<SpiritScythe>(), 30, 1, Main.myPlayer, 0, 0);
				}
			}
			npc.spriteDirection = npc.direction;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.Asphalt, 0f, 0f, 100, default, 1f);

			if (npc.life <= 0) {
				Gore.NewGore(npc.position, npc.velocity, 13);
				Gore.NewGore(npc.position, npc.velocity, 12);
				Gore.NewGore(npc.position, npc.velocity, 11);
				npc.position.X = npc.position.X + (npc.width / 2);
				npc.position.Y = npc.position.Y + (npc.height / 2);
				npc.width = 30;
				npc.height = 30;
				npc.position.X = npc.position.X - (npc.width / 2);
				npc.position.Y = npc.position.Y - (npc.height / 2);
				for (int num621 = 0; num621 < 20; num621++) {
					int num622 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.Asphalt, 0f, 0f, 100, default, 2f);
					Main.dust[num622].velocity *= 3f;
					if (Main.rand.Next(2) == 0) {
						Main.dust[num622].scale = 0.5f;
						Main.dust[num622].fadeIn = 1f + Main.rand.Next(10) * 0.1f;
					}
				}
				for (int num623 = 0; num623 < 40; num623++) {
					int num624 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.Asphalt, 0f, 0f, 100, default, 3f);
					Main.dust[num624].noGravity = true;
					Main.dust[num624].velocity *= 5f;
					num624 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.Asphalt, 0f, 0f, 100, default, 2f);
					Main.dust[num624].velocity *= 2f;
				}
			}
		}
        private static int[] SpawnTiles = { };
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			Player player = spawnInfo.player;
            if (!player.GetSpiritPlayer().ZoneSpirit)
            {
                return 0f;
            }
            if (SpawnTiles.Length == 0)
            {
                int[] Tiles = { ModContent.TileType<SpiritDirt>(), ModContent.TileType<SpiritStone>(), ModContent.TileType<SpiritGrass>(), ModContent.TileType<SpiritIce>() };
                SpawnTiles = Tiles;
            }
            return SpawnTiles.Contains(Main.tile[spawnInfo.spawnTileX, spawnInfo.spawnTileY].type) && player.position.Y / 16 >= Main.maxTilesY - 330 && player.GetSpiritPlayer().ZoneSpirit && !spawnInfo.playerSafe ? 2f : 0f;
		}

		public override void NPCLoot()
		{
            if (Main.rand.Next(3) == 1)
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<SoulShred>(), Main.rand.Next(1) + 1);
        }

	}
}