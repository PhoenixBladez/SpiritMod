using SpiritMod.Items.Consumable;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpiritMod.NPCs.Critters
{
	public class Crinoid : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crinoid");
			Main.npcFrameCount[NPC.type] = 6;
		}

		public override void SetDefaults()
		{
			NPC.dontCountMe = true;
			NPC.width = 22;
			NPC.height = 22;
			NPC.damage = 0;
			NPC.defense = 0;
			NPC.lifeMax = 30;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			Main.npcCatchable[NPC.type] = true;
			NPC.catchItem = (short)ModContent.ItemType<CrinoidItem>();
			NPC.knockBackResist = 0f;
			NPC.aiStyle = 0;
			NPC.npcSlots = 0;
			NPC.alpha = 255;
			AIType = NPCID.WebbedStylist;
        }
		public bool hasPicked = false;
		int pickedType;
		public override void AI()
        {
			if (NPC.alpha > 0)
            {
				NPC.alpha -= 5;
            }
			if (!hasPicked)
            {
				NPC.scale = Main.rand.NextFloat(.6f, 1f);
				pickedType = Main.rand.Next(0, 3);
				hasPicked = true;

			}
        }

		public override void FindFrame(int frameHeight)
		{
			NPC.frameCounter += 0.22f;
			NPC.frameCounter %= Main.npcFrameCount[NPC.type];
			int frame = (int)NPC.frameCounter;
			NPC.frame.Y = frame * frameHeight;
			NPC.frame.X = 46 * pickedType;
			NPC.frame.Width = 46;
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
			Vector2 drawPos = NPC.Center - Main.screenPosition + drawOrigin + new Vector2(-26, -18);
			Color color = NPC.GetAlpha(drawColor);
			var effects = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, drawPos, NPC.frame, color, NPC.rotation, drawOrigin, NPC.scale, effects, 0f);
			return false;
		}
		public override void HitEffect(int hitDirection, double damage)
		{
            if (NPC.life <= 0)
            {
				for (int i = 0; i < 6; i++)
				{
					if (pickedType == 0)
					{
						Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("PinkCrinoid1").Type, Main.rand.NextFloat(.5f, 1.2f));
						Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("PinkCrionid2").Type, Main.rand.NextFloat(.5f, 1.2f));
					}
					if (pickedType == 1)
					{
						Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("RedCrinoid1").Type, Main.rand.NextFloat(.5f, 1.2f));
						Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("RedCrinoid2").Type, Main.rand.NextFloat(.5f, 1.2f));
					}
					if (pickedType == 2)
					{
						Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("YellowCrinoid1").Type, Main.rand.NextFloat(.5f, 1.2f));
						Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("YellowCrionid2").Type, Main.rand.NextFloat(.5f, 1.2f));
					}

				}
            }
        }
	}
}
