using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Consumable.Fish;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace SpiritMod.NPCs.Critters
{
	public class CrystalSerpent : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crystal Serpent");
			Main.npcFrameCount[npc.type] = 6;
		}

		public override void SetDefaults()
		{
			npc.width = 38;
			npc.height = 20;
			npc.damage = 55;
			npc.defense = 10;
			npc.lifeMax = 260;
			Main.npcCatchable[npc.type] = true;
			npc.catchItem = (short)ItemID.Prismite;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.knockBackResist = .35f;
			npc.aiStyle = 16;
			npc.noGravity = true;
			npc.dontCountMe = true;
			npc.npcSlots = 0;
			aiType = NPCID.CorruptGoldfish;
		}
		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if(Main.rand.NextBool(4)) {
				target.AddBuff(BuffID.BrokenArmor, 1200);
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
							 drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
			return false;
		}

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.15f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}


		public override void HitEffect(int hitDirection, double damage)
		{
    		Main.PlaySound(SoundID.DD2_WitherBeastHurt, npc.Center);
			if (npc.life <= 0) {
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/CrystalSerpentGore"), 1f);
			}
			for (int k = 0; k < 11; k++) {
					Dust.NewDust(npc.position, npc.width, npc.height, 112, npc.direction, -1f, 1, default(Color), .61f);
				}		
		}
		public override void AI()
		{
			Lighting.AddLight((int)((npc.position.X + (float)(npc.width / 2)) / 16f), (int)((npc.position.Y + (float)(npc.height / 2)) / 16f), .170f * 2f, .064f* 2f, .189f* 2f);
			npc.spriteDirection = -npc.direction;
        }
		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			GlowmaskUtils.DrawNPCGlowMask(spriteBatch, npc, ModContent.GetTexture("SpiritMod/NPCs/Critters/CrystalSerpent_Glow"));
		}
		public override void NPCLoot()
		{
			if (Main.rand.Next(2) == 1) {
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<RawFish>(), 1);
			}
    		if (Main.rand.Next(2) == 1) {
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.CrystalSerpent, 1);
			}
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return spawnInfo.player.ZoneHoly && spawnInfo.water && Main.hardMode ? 0.005f : 0f;
		}

	}
}
