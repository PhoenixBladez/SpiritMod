using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Buffs;
using SpiritMod.Items.Sets.CryoliteSet;
using SpiritMod.Items.Consumable.Food;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Winterborn
{
	public class WinterbornMelee : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Winterborn");
			Main.npcFrameCount[npc.type] = 6;
		}

		public override void SetDefaults()
		{
			npc.width = 24;
			npc.height = 46;
			npc.damage = 30;
			npc.defense = 11;
			npc.lifeMax = 250;
			npc.HitSound = SoundID.NPCDeath15;
			npc.DeathSound = SoundID.NPCDeath6;
			npc.value = 189f;
			npc.buffImmune[BuffID.OnFire] = true;
			npc.buffImmune[BuffID.Frostburn] = true;
			npc.buffImmune[ModContent.BuffType<CryoCrush>()] = true;
			npc.knockBackResist = .07f;
			npc.aiStyle = 3;
			aiType = NPCID.ArmoredViking;
			banner = npc.type;
			bannerItem = ModContent.ItemType<Items.Banners.WinterbornBanner>();
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return NPC.downedBoss3 && ((spawnInfo.spawnTileY > Main.rockLayer && spawnInfo.player.ZoneSnow) || (spawnInfo.player.ZoneSnow && Main.raining && spawnInfo.player.ZoneOverworldHeight)) ? 0.12f : 0f;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			int d = 206;
			int d1 = 187;
			for (int k = 0; k < 5; k++) {
				{
					Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, default, 0.7f);
					Dust.NewDust(npc.position, npc.width, npc.height, d1, 2.5f * hitDirection, -2.5f, 0, default, 0.7f);
				}
			}
			if (npc.life <= 0) {
				{
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Winterborn/WinterbornGore1"), 1f);
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Winterborn/WinterbornGore2"), 1f);
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Winterborn/WinterbornGore2"), 1f);
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Winterborn/WinterbornGore3"), 1f);
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Winterborn/WinterbornGore3"), 1f);
				}
				npc.position.X = npc.position.X + (float)(npc.width / 2);
				npc.position.Y = npc.position.Y + (float)(npc.height / 2);
				npc.width = 30;
				npc.height = 30;
				npc.position.X = npc.position.X - (float)(npc.width / 2);
				npc.position.Y = npc.position.Y - (float)(npc.height / 2);
				for (int num621 = 0; num621 < 20; num621++) {
					int num622 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.Flare_Blue, 0f, 0f, 100, default, .8f);
					if (Main.rand.Next(2) == 0) {
						Main.dust[num622].scale = 0.35f;
					}
				}
				for (int num623 = 0; num623 < 40; num623++) {
					int num624 = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.BlueCrystalShard, 0f, 0f, 100, default, .43f);
					Main.dust[num624].noGravity = true;
					Main.dust[num624].velocity *= 3f;
				}
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
							 drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
			return false;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			GlowmaskUtils.DrawNPCGlowMask(spriteBatch, npc, mod.GetTexture("NPCs/Winterborn/WinterbornMelee_Glow"));
		}
		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.25f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			target.AddBuff(BuffID.Chilled, 300);
			if (Main.rand.Next(10) == 0) {
				target.AddBuff(BuffID.Frozen, 120);
			}
		}

		public override void AI()
		{
			Lighting.AddLight((int)((npc.position.X + (float)(npc.width / 2)) / 16f), (int)((npc.position.Y + (float)(npc.height / 2)) / 16f), 0.079f, 0.132f, .2f);

			npc.spriteDirection = npc.direction;
		}

		public override void NPCLoot()
		{
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<CryoliteOre>(), 2 + Main.rand.Next(3, 7));
			if (Main.rand.NextBool(16))
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Popsicle>());
            }
		}
	}
}
