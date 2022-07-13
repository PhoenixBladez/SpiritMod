using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Sets.EvilBiomeDrops.PesterflyCane;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Festerfly
{
	public class VileWasp : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pesterfly");
			Main.npcFrameCount[NPC.type] = 2;
			NPCID.Sets.TrailCacheLength[NPC.type] = 2;
			NPCID.Sets.TrailingMode[NPC.type] = 0;
		}

		public override void SetDefaults()
		{
			NPC.width = 22;
			NPC.height = 20;
			NPC.damage = 20;
			NPC.defense = 0;
			NPC.lifeMax = 10;
			NPC.HitSound = SoundID.NPCHit1; //Dr Man Fly
			NPC.DeathSound = SoundID.NPCDeath16;
			NPC.value = 10f;
			NPC.noGravity = true;
			NPC.noTileCollide = false;
			NPC.knockBackResist = .65f;
			NPC.aiStyle = 44;
			AIType = NPCID.FlyingAntlion;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheCorruption,
				new FlavorTextBestiaryInfoElement("These pests infest the innards of the abominable Festerfly. If you flex hard enough while one is biting you, it might explode."),
			});
		}

		public override void FindFrame(int frameHeight)
		{
			NPC.frameCounter += 0.15f;
			NPC.frameCounter %= Main.npcFrameCount[NPC.type];
			int frame = (int)NPC.frameCounter;
			NPC.frame.Y = frame * frameHeight;
		}

		public override void AI() => NPC.spriteDirection = NPC.direction;

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			Vector2 drawOrigin = new Vector2(TextureAssets.Npc[NPC.type].Value.Width * 0.5f, (NPC.height / Main.npcFrameCount[NPC.type]) * 0.5f);
			for (int k = 0; k < NPC.oldPos.Length; k++)
			{
				var effects = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
				Vector2 drawPos = NPC.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, NPC.gfxOffY);
				Color color = NPC.GetAlpha(drawColor) * (((NPC.oldPos.Length - k) / (float)NPC.oldPos.Length) / 2);
				spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, drawPos, new Microsoft.Xna.Framework.Rectangle?(NPC.frame), color, NPC.rotation, drawOrigin, NPC.scale, effects, 0f);
			}
			return true;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 30; k++)
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Plantera_Green, 2.5f * hitDirection, -2.5f, 0, Color.Purple, 0.3f);

			if (NPC.life <= 0)
			{
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Pesterfly5").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("Pesterfly6").Type, 1f);
			}
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot) => npcLoot.AddCommon<PesterflyCane>(30);
	}
}
