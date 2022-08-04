using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Consumable.Fish;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Terraria.GameContent.Bestiary;

namespace SpiritMod.NPCs.Critters
{
	public class Rockfish : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rockfish");
			Main.npcFrameCount[NPC.type] = 6;
		}

		public override void SetDefaults()
		{
			NPC.width = 48;
			NPC.height = 28;
			NPC.damage = 0;
			NPC.defense = 12;
			NPC.lifeMax = 55;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = .35f;
			NPC.aiStyle = 16;
			NPC.noGravity = true;
			NPC.npcSlots = 0;
			AIType = NPCID.Goldfish;
			NPC.dontCountMe = true;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Caverns,
				new FlavorTextBestiaryInfoElement("Undeniably too tough to eat. For bludgeoning, however, these natural disasters have quite the potential."),
			});
		}

		public override void AI()
		{
			Player target = Main.player[NPC.target];
			if (NPC.DistanceSQ(target.Center) < 65 * 65 && target.wet && NPC.wet)
			{
				Vector2 vel = NPC.DirectionFrom(target.Center) * 4.5f;
				NPC.velocity = vel;
				NPC.rotation = NPC.velocity.X * .06f;
				if (target.position.X > NPC.position.X)
				{
					NPC.spriteDirection = -1;
					NPC.direction = -1;
					NPC.netUpdate = true;
				}
				else if (target.position.X < NPC.position.X)
				{
					NPC.spriteDirection = 1;
					NPC.direction = 1;
					NPC.netUpdate = true;
				}
			}
		}

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			var effects = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, NPC.Center - screenPos + new Vector2(0, NPC.gfxOffY), NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effects, 0);
			return false;
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
			SoundEngine.PlaySound(SoundID.DD2_WitherBeastHurt, NPC.Center);
			if (NPC.life <= 0)
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("RockfishGore").Type, 1f);
			for (int k = 0; k < 11; k++)
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Stone, NPC.direction, -1f, 1, Color.Black, .61f);
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.AddCommon<RawFish>(2);
			npcLoot.AddCommon(ItemID.Rockfish, 2);
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo) => spawnInfo.Player.ZoneRockLayerHeight && spawnInfo.Water ? 0.009f : 0f;
	}
}
