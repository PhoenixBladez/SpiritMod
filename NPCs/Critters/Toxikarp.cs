using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Consumable.Fish;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;

namespace SpiritMod.NPCs.Critters
{
	public class Toxikarp : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Toxikarp");
			Main.npcFrameCount[npc.type] = 6;
		}

		public override void SetDefaults()
		{
			npc.width = 28;
			npc.height = 28;
			npc.damage = 40;
			npc.defense = 12;
			npc.lifeMax = 320;
			Main.npcCatchable[npc.type] = true;
			npc.catchItem = (short)ItemID.Prismite;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath22;
			npc.knockBackResist = 0f;
			npc.aiStyle = 16;
			npc.noGravity = true;
			npc.dontCountMe = true;
			npc.npcSlots = 0;
			aiType = NPCID.CorruptGoldfish;
		}
		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
        	target.AddBuff(BuffID.Poisoned, 1200);
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
			if (npc.life <= 0) {
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/ToxikarpGore"), 1f);
			}
			for (int k = 0; k < 11; k++) {
					Dust.NewDust(npc.position, npc.width, npc.height, 167, npc.direction, -1f, 1, default(Color), .61f);
				}		
		}
		public override void AI()
		{
			Lighting.AddLight((int)((npc.position.X + (float)(npc.width / 2)) / 16f), (int)((npc.position.Y + (float)(npc.height / 2)) / 16f), .170f * 1.5f, .064f* 1.5f, .289f* 1.5f);
			npc.spriteDirection = -npc.direction;
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
			GlowmaskUtils.DrawNPCGlowMask(spriteBatch, npc, ModContent.GetTexture("SpiritMod/NPCs/Critters/Toxikarp_Glow"));
		}
		public override void NPCLoot()
		{
			if (Main.rand.Next(2) == 1) {
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<RawFish>(), 1);
			}
    		if (Main.rand.Next(2) == 1) {
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Toxikarp, 1);
			}
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return spawnInfo.player.ZoneCorrupt && spawnInfo.water && Main.hardMode ? 0.005f : 0f;
		}

	}
}
