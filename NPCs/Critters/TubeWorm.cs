using SpiritMod.Items.Consumable;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.Bestiary;

namespace SpiritMod.NPCs.Critters
{
	public class TubeWorm : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tubeworm");
			Main.npcFrameCount[NPC.type] = 6;
			Main.npcCatchable[NPC.type] = true;
		}

		public override void SetDefaults()
		{
			NPC.dontCountMe = true;
			NPC.width = 10;
			NPC.height = 14;
			NPC.damage = 0;
			NPC.defense = 0;
			NPC.lifeMax = 5;
			NPC.HitSound = SoundID.NPCHit2;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.catchItem = (short)ModContent.ItemType<TubewormItem>();
			NPC.knockBackResist = 0f;
			NPC.aiStyle = 0;
			NPC.npcSlots = 0;
			NPC.alpha = 255;
			AIType = NPCID.WebbedStylist;
        }

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Ocean,
				new FlavorTextBestiaryInfoElement("Deep below the glimmering tides, down where the light won't shine, live some fascinating creatures, able to feast off of the smog exuded from hydrothermal vents."),
			});
		}

		bool hasPicked = false;
		int pickedType;

		public override void AI()
        {
			if (NPC.alpha > 0)
				NPC.alpha -= 5;

			if (!hasPicked)
            {
				NPC.scale = Main.rand.NextFloat(.6f, 1.15f);
				pickedType = Main.rand.Next(0, 4);
				hasPicked = true;
			}
        }

		public override void FindFrame(int frameHeight)
		{
			NPC.frameCounter += 0.18f;
			NPC.frameCounter %= Main.npcFrameCount[NPC.type];
			int frame = (int)NPC.frameCounter;
			NPC.frame.Y = frame * frameHeight;
			NPC.frame.X = 18 * pickedType;
			NPC.frame.Width = 18;
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(pickedType);
			writer.Write(hasPicked);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			pickedType = reader.ReadInt32();
			hasPicked = reader.ReadBoolean();
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			Vector2 drawOrigin = new Vector2(TextureAssets.Npc[NPC.type].Value.Width * 0.5f, (NPC.height * 0.5f));
			Vector2 drawPos = NPC.Center - screenPos + drawOrigin + new Vector2(-14, -12);
			Color color = !NPC.IsABestiaryIconDummy ? NPC.GetAlpha(drawColor) : Color.White;
			var effects = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, drawPos, NPC.frame, color, NPC.rotation, drawOrigin, NPC.scale, effects, 0f);
			return false;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
            if (NPC.life <= 0)
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("TubewormGore").Type, 1f);
        }
	}
}
