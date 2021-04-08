using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.Consumable.Food;
using Terraria.DataStructures;

namespace SpiritMod.NPCs.AntlionAssassin
{
	public class AntlionAssassin : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Antlion Assassin");
			Main.npcFrameCount[npc.type] = 6;
		}

		public override void SetDefaults()
		{
			npc.width = 22;
			npc.height = 32;
			npc.damage = 21;
			npc.defense = 8;
			npc.lifeMax = 74;
			npc.HitSound = SoundID.NPCHit32;
			npc.DeathSound = SoundID.NPCDeath6;
			npc.value = 329f;
			npc.knockBackResist = .45f;
			npc.aiStyle = 3;
			aiType = NPCID.SnowFlinx;
			banner = npc.type;
			bannerItem = ModContent.ItemType<Items.Banners.AntlionAssassinBanner>();
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{

			if (Main.tileSand[spawnInfo.spawnTileType])
				return SpawnCondition.OverworldDayDesert.Chance * 1.145f;
			return 0;
		}
		int timer;
		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 11; k++) {
				Dust.NewDust(npc.position, npc.width, npc.height, 85, hitDirection, -1f, 1, default(Color), .61f);
			}
			if (npc.life <= 0) {
				{
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/AntlionAssassin/Assassin1"), 1f);
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/AntlionAssassin/Assassin2"), 1f);
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/AntlionAssassin/Assassin3"), 1f);
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/AntlionAssassin/Assassin4"), 1f);
				}
				if (Main.LocalPlayer.GetSpiritPlayer().emptyAntlionScroll) {
					MyWorld.numAntlionsKilled++;
				}
				for (int k = 0; k < 11; k++) {
					Dust.NewDust(npc.position, npc.width, npc.height, 85, hitDirection, -1f, 1, default(Color), .61f);
				}
				int ing = Gore.NewGore(npc.position, npc.velocity, 825);
				Main.gore[ing].timeLeft = 30;
				int ing1 = Gore.NewGore(npc.position, npc.velocity, 826);
				Main.gore[ing1].timeLeft = 30;
				int ing2 = Gore.NewGore(npc.position, npc.velocity, 827);
				Main.gore[ing2].timeLeft = 30;
			}
		}

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.15f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}

		public override void AI()
		{
			npc.spriteDirection = npc.direction;
			npc.alpha++;
			timer++;
			if (timer >= 500) {
				for (int k = 0; k < 11; k++) {
					Dust.NewDust(npc.position, npc.width, npc.height, 85, npc.direction, -1f, 1, default(Color), .61f);
				}
				Main.PlaySound(SoundID.NPCKilled, (int)npc.position.X, (int)npc.position.Y, 6);
				int ing = Gore.NewGore(npc.position, npc.velocity, 825);
				Main.gore[ing].timeLeft = 130;
				int ing1 = Gore.NewGore(npc.position, npc.velocity, 826);
				Main.gore[ing1].timeLeft = 130;
				int ing2 = Gore.NewGore(npc.position, npc.velocity, 827);
				Main.gore[ing2].timeLeft = 130;
				npc.alpha = 0;
				timer = 0;
			}
		}

		public override void OnHitByItem(Player player, Item item, int damage, float knockback, bool crit)
		{
			for (int k = 0; k < 11; k++) {
				Dust.NewDust(npc.position, npc.width, npc.height, 85, npc.direction, -1f, 1, default(Color), .61f);
			}
			if (npc.alpha >= 220)
			{
				Main.PlaySound(SoundID.NPCKilled, (int)npc.position.X, (int)npc.position.Y, 6);
			}
			int ing = Gore.NewGore(npc.position, npc.velocity, 825);
			Main.gore[ing].timeLeft = 50;
			Main.gore[ing].scale = Main.rand.NextFloat(.5f, .9f);
			int ing1 = Gore.NewGore(npc.position, npc.velocity, 826);
			Main.gore[ing1].timeLeft = 50;
			Main.gore[ing].scale = Main.rand.NextFloat(.5f, .9f);
			int ing2 = Gore.NewGore(npc.position, npc.velocity, 827);
			Main.gore[ing2].timeLeft = 50;
			Main.gore[ing].scale = Main.rand.NextFloat(.5f, .9f);
			npc.alpha = 0;
			timer = 0;
			npc.alpha = 0;
		}
		public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
		{
			for (int k = 0; k < 11; k++) {
				Dust.NewDust(npc.position, npc.width, npc.height, 85, npc.direction, -1f, 1, default(Color), .61f);
			}
			if (npc.alpha >= 220)
			{
				Main.PlaySound(SoundID.NPCKilled, (int)npc.position.X, (int)npc.position.Y, 6);
			}
			int ing = Gore.NewGore(npc.position, npc.velocity, 825);
			Main.gore[ing].timeLeft = 130;
			int ing1 = Gore.NewGore(npc.position, npc.velocity, 826);
			Main.gore[ing1].timeLeft = 130;
			int ing2 = Gore.NewGore(npc.position, npc.velocity, 827);
			Main.gore[ing2].timeLeft = 130;
			npc.alpha = 0;
			timer = 0;
			npc.alpha = 0;
		}
		public override void NPCLoot()
		{
			if (Main.rand.NextBool(53)) {
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, 857);
            }
            if (Main.rand.NextBool(16))
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Hummus>());
            }
        }
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			SpriteEffects spriteEffects = SpriteEffects.None;
			if (npc.spriteDirection == 1)
				spriteEffects = SpriteEffects.FlipHorizontally;
			Vector2 vector2_3 = new Vector2((float) (Main.npcTexture[npc.type].Width / 2), (float) (Main.npcTexture[npc.type].Height / Main.npcFrameCount[npc.type] / 2));
			Microsoft.Xna.Framework.Color color12 = Lighting.GetColor((int) ((double) npc.position.X + (double) npc.width * 0.5) / 16, (int) (((double) npc.position.Y + (double) npc.height * 0.5) / 16.0));
			Main.spriteBatch.Draw(mod.GetTexture("NPCs/AntlionAssassin/AntlionAssassin_Glow"), new Vector2((float) ((double) npc.position.X - (double) Main.screenPosition.X + (double) (npc.width / 2) - (double) Main.npcTexture[npc.type].Width * (double) npc.scale / 2.0 + (double) vector2_3.X * (double) npc.scale), (float) ((double) npc.position.Y - (double) Main.screenPosition.Y + (double) npc.height - (double) Main.npcTexture[npc.type].Height * (double) npc.scale / (double) Main.npcFrameCount[npc.type] + 4.0 + (double) vector2_3.Y * (double) npc.scale) ), new Microsoft.Xna.Framework.Rectangle?(npc.frame), Microsoft.Xna.Framework.Color.White * .85f, npc.rotation, vector2_3, npc.scale, spriteEffects, 0.0f);
			if (npc.velocity.Y != 0f)
			{
				for (int index = 1; index < 10; ++index)
				{
					Microsoft.Xna.Framework.Color color2 = new Microsoft.Xna.Framework.Color(255 - index * 10, 110 - index * 10, 110 - index * 10, 110 - index * 10);
					Main.spriteBatch.Draw(mod.GetTexture("NPCs/AntlionAssassin/AntlionAssassin_Glow"), new Vector2((float) ((double) npc.position.X - (double) Main.screenPosition.X + (double) (npc.width / 2) - (double) Main.npcTexture[npc.type].Width * (double) npc.scale / 2.0 + (double) vector2_3.X * (double) npc.scale), (float) ((double) npc.position.Y - (double) Main.screenPosition.Y + (double) npc.height - (double) Main.npcTexture[npc.type].Height * (double) npc.scale / (double) Main.npcFrameCount[npc.type] + 4.0 + (double) vector2_3.Y * (double) npc.scale) ) - npc.velocity * (float) index * 0.5f, new Microsoft.Xna.Framework.Rectangle?(npc.frame), color2, npc.rotation, vector2_3, npc.scale, spriteEffects, 0.0f);
				}
			}
		}
	}
}
