using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Tiles.Block;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.Sets.BriarDrops;
using SpiritMod.Items.Consumable.Food;

namespace SpiritMod.NPCs.Reach
{
	public class GrassVine : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Droseran Trapper");
			Main.npcFrameCount[npc.type] = 6;
		}

		public override void SetDefaults()
		{
			npc.width = 34;
			npc.height = 34;
			npc.damage = 22;
			npc.defense = 8;
			npc.buffImmune[BuffID.Poisoned] = true;
			npc.lifeMax = 54;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.value = 60f;
			npc.knockBackResist = 0f;
			npc.behindTiles = true;
			banner = npc.type;
			bannerItem = ModContent.ItemType<Items.Banners.DroseranTrapperBanner>();
		}

		public override void NPCLoot()
		{
			if (!Main.dayTime) {
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<EnchantedLeaf>());
			}
			if (Main.rand.NextBool(10)) {
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Vine);
			}
            if (Main.rand.NextBool(33))
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<CaesarSalad>());
            }
        }
        public override void AI()
		{
			npc.TargetClosest(true);
			float num3212 = 12f;
			Vector2 vector449 = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
			float num3211 = Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2) - vector449.X;
			float num3210 = Main.player[npc.target].position.Y - vector449.Y;
			float num3209 = (float)Math.Sqrt((double)(num3211 * num3211 + num3210 * num3210));
			num3209 = num3212 / num3209;
			num3211 *= num3209;
			num3210 *= num3209;
			bool flag223 = false;
			if (npc.directionY < 0) {
				npc.rotation = (float)(Math.Atan2((double)num3210, (double)num3211) + 1.57);
				flag223 = ((double)npc.rotation >= -1.2 && (double)npc.rotation <= 1.2);
				if ((double)npc.rotation < -0.8) {
					npc.rotation = -0.8f;
				}
				else if ((double)npc.rotation > 0.8) {
					npc.rotation = 0.8f;
				}
				if (npc.velocity.X != 0f) {
					npc.velocity.X = npc.velocity.X * 0.9f;
					if ((double)npc.velocity.X > -0.1 || (double)npc.velocity.X < 0.1) {
						npc.netUpdate = true;
						npc.velocity.X = 0f;
					}
				}
			}
			if (npc.ai[0] > 0f) {
				if (npc.ai[0] == 200f) {
					Main.PlaySound(SoundID.Item5, npc.position);
				}
				ref float reference = ref npc.ai[0];
				reference -= 1f;
			}
			if ((Main.netMode != NetmodeID.MultiplayerClient & flag223) && npc.ai[0] == 0f && Collision.CanHit(npc.position, npc.width, npc.height, Main.player[npc.target].position, Main.player[npc.target].width, Main.player[npc.target].height)) {
				npc.ai[0] = 200f;
				int num3205 = 10;
				int num3204 = 276;
				int num3203 = Projectile.NewProjectile(vector449.X, vector449.Y, num3211/2, num3210/2, num3204, num3205, 0f, Main.myPlayer, 0f, 0f);
				Main.projectile[num3203].timeLeft = 300;
				Main.projectile[num3203].friendly = false;
				NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, num3203, 0f, 0f, 0f, 0, 0, 0);
				npc.netUpdate = true;
			}
			try
			{
				int num3202 = (int)npc.position.X / 16;
				int num3201 = (int)(npc.position.X + (float)(npc.width / 2)) / 16;
				int num3200 = (int)(npc.position.X + (float)npc.width) / 16;
				int num3199 = (int)(npc.position.Y + (float)npc.height) / 16;
				bool flag222 = false;
				if (Main.tile[num3202, num3199] == null)
				{
					Tile[,] tile15 = Main.tile;
					int num3564 = num3202;
					int num3565 = num3199;
					Tile tile16 = new Tile();
					tile15[num3564, num3565] = tile16;
				}
				if (Main.tile[num3201, num3199] == null)
				{
					Tile[,] tile17 = Main.tile;
					int num3566 = num3202;
					int num3567 = num3199;
					Tile tile18 = new Tile();
					tile17[num3566, num3567] = tile18;
				}
				if (Main.tile[num3200, num3199] == null)
				{
					Tile[,] tile19 = Main.tile;
					int num3568 = num3202;
					int num3569 = num3199;
					Tile tile20 = new Tile();
					tile19[num3568, num3569] = tile20;
				}
				if (Main.tile[num3202, num3199].nactive() && Main.tileSolid[Main.tile[num3202, num3199].type])
					flag222 = true;
				if (Main.tile[num3201, num3199].nactive() && Main.tileSolid[Main.tile[num3201, num3199].type])
					flag222 = true;
				if (Main.tile[num3200, num3199].nactive() && Main.tileSolid[Main.tile[num3200, num3199].type])
					flag222 = true;

				if (flag222)
				{
					npc.noGravity = true;
					npc.noTileCollide = true;
					npc.velocity.Y = -0.2f;
				}
				else
				{
					npc.noGravity = false;
					npc.noTileCollide = false;
					if (Main.rand.Next(2) == 0)
					{
						Vector2 position55 = new Vector2(npc.position.X - 4f, npc.position.Y + (float)npc.height - 8f);
						int width36 = npc.width + 8;
						float speedY16 = npc.velocity.Y / 2f;
						int num3198 = Dust.NewDust(position55, width36, 24, DustID.Sand, 0f, speedY16, 0, default, 1f);
						Dust dust = Main.dust[num3198];
						dust.velocity.X = dust.velocity.X * 0.4f;
						dust.velocity.Y = dust.velocity.Y * -1f;
						if (Main.rand.Next(2) == 0)
						{
							Main.dust[num3198].noGravity = true;
							Dust dust46 = Main.dust[num3198];
							dust46.scale += 0.2f;
						}
					}
				}
			}
			catch { }
		}

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.15f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            Player player = spawnInfo.player;
            if (!(player.ZoneTowerSolar || player.ZoneTowerVortex || player.ZoneTowerNebula || player.ZoneTowerStardust) && ((!Main.pumpkinMoon && !Main.snowMoon)) && !Main.eclipse && (SpawnCondition.GoblinArmy.Chance == 0))
            {
                return spawnInfo.player.GetSpiritPlayer().ZoneReach && Main.tile[(int)(spawnInfo.spawnTileX), (int)(spawnInfo.spawnTileY)].type == ModContent.TileType<ReachGrassTile>() ? .9f : 0f;
            }
            return 0f;
        }
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D ring = mod.GetTexture("NPCs/Reach/GrassVine_Texture");
			Main.spriteBatch.Draw(ring, new Vector2(npc.position.X - Main.screenPosition.X + (float)(npc.width / 2), npc.position.Y - Main.screenPosition.Y + (float)npc.height + 7f), new Microsoft.Xna.Framework.Rectangle(0, 0, ring.Width, ring.Height), drawColor, (0f - npc.rotation) * 0.6f, new Vector2((float)(ring.Width / 2), (float)(ring.Height / 2)), 1.08f, SpriteEffects.None, 0f);

			var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
							 drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
			return false;
		}
		public override void HitEffect(int hitDirection, double damage)
		{
			int d = 3;
			int d1 = 3;
			for (int k = 0; k < 30; k++) {
				Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, Color.Purple, 0.3f);
				Dust.NewDust(npc.position, npc.width, npc.height, d1, 2.5f * hitDirection, -2.5f, 0, default, .34f);
			}
			if (npc.life <= 0) {
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/LeafGreen"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/LeafGreen"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/LeafHead"), 1f);
			}
		}
		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (Main.rand.Next(10) == 0 && Main.expertMode) {
				target.AddBuff(148, 2000);
			}
		}
	}
}
