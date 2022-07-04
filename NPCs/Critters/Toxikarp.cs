using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Consumable.Fish;
using Terraria;
using Terraria.GameContent;
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
			Main.npcFrameCount[NPC.type] = 6;
		}

		public override void SetDefaults()
		{
			NPC.width = 28;
			NPC.height = 28;
			NPC.damage = 40;
			NPC.defense = 12;
			NPC.lifeMax = 320;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath22;
			NPC.knockBackResist = 0f;
			NPC.aiStyle = 16;
			NPC.noGravity = true;
			NPC.dontCountMe = true;
			NPC.npcSlots = 0;
			AIType = NPCID.CorruptGoldfish;
		}
		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
        	target.AddBuff(BuffID.Poisoned, 1200);
		}
		public override void FindFrame(int frameHeight)
		{
			NPC.frameCounter += 0.15f;
			NPC.frameCounter %= Main.npcFrameCount[NPC.type];
			int frame = (int)NPC.frameCounter;
			NPC.frame.Y = frame * frameHeight;
		}


		public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life <= 0) {
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("ToxikarpGore").Type, 1f);
			}
			for (int k = 0; k < 11; k++) {
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Plantera_Green, NPC.direction, -1f, 1, default, .61f);
				}		
		}
		public override void AI()
		{
			Lighting.AddLight((int)((NPC.position.X + (float)(NPC.width / 2)) / 16f), (int)((NPC.position.Y + (float)(NPC.height / 2)) / 16f), .10f * 1.5f, .064f* 1.5f, .289f* 1.5f);
			NPC.spriteDirection = -NPC.direction;
        }
		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			var effects = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, NPC.Center - Main.screenPosition + new Vector2(0, NPC.gfxOffY), NPC.frame,
							 drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effects, 0);
			return false;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			GlowmaskUtils.DrawNPCGlowMask(spriteBatch, NPC, ModContent.Request<Texture2D>("SpiritMod/NPCs/Critters/Toxikarp_Glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value);
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.AddCommon(ItemID.Toxikarp, 2);
			npcLoot.AddCommon<RawFish>(2);
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return spawnInfo.Player.ZoneCorrupt && spawnInfo.Water && Main.hardMode ? 0.005f : 0f;
		}

	}
}
