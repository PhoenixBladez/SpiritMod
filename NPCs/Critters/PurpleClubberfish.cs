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
	public class PurpleClubberfish : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Purple Clubberfish");
			Main.npcFrameCount[NPC.type] = 6;
		}

		public override void SetDefaults()
		{
			NPC.width = 40;
			NPC.height = 28;
			NPC.damage = 25;
			NPC.defense = 16;
			NPC.lifeMax = 165;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = .35f;
			NPC.aiStyle = 16;
			NPC.noGravity = true;
			NPC.dontCountMe = true;
			NPC.npcSlots = 0;
			AIType = NPCID.CorruptGoldfish;
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
				Gore.NewGore(NPC.position, NPC.velocity, Mod.Find<ModGore>("Gores/PurpleClubberfishGore").Type, 1f);
			}
			for (int k = 0; k < 11; k++) {
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.CorruptPlants, NPC.direction, -1f, 1, default, .61f);
				}		
		}
		public override void AI()
		{
			NPC.spriteDirection = -NPC.direction;
        }
		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			var effects = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, NPC.Center - Main.screenPosition + new Vector2(0, NPC.gfxOffY), NPC.frame,
							 drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effects, 0);
			return false;
		}
		public override void OnKill()
		{
			if (Main.rand.Next(2) == 1) {
				Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ModContent.ItemType<RawFish>(), 1);
			}
    		if (Main.rand.Next(2) == 1) {
				Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ItemID.PurpleClubberfish, 1);
			}
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return spawnInfo.Player.ZoneCorrupt && spawnInfo.Water ? 0.0075f : 0f;
		}

	}
}
