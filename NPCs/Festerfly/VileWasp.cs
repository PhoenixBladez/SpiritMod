using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Sets.EvilBiomeDrops.PesterflyCane;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Festerfly
{
	public class VileWasp : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pesterfly");
			Main.npcFrameCount[npc.type] = 2;
			NPCID.Sets.TrailCacheLength[npc.type] = 2;
			NPCID.Sets.TrailingMode[npc.type] = 0;
		}

		public override void SetDefaults()
		{
			npc.width = 22;
			npc.height = 20;
			npc.damage = 20;
			npc.defense = 0;
			npc.lifeMax = 10;
			npc.HitSound = SoundID.NPCHit1; //Dr Man Fly
			npc.DeathSound = SoundID.NPCDeath16;
			npc.value = 10f;
			npc.noGravity = true;
			npc.noTileCollide = false;
			npc.knockBackResist = .65f;
			npc.aiStyle = 44;
			aiType = NPCID.FlyingAntlion;
		}

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.15f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}

		public override void AI() => npc.spriteDirection = npc.direction;

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(Main.npcTexture[npc.type].Width * 0.5f, (npc.height / Main.npcFrameCount[npc.type]) * 0.5f);
			for (int k = 0; k < npc.oldPos.Length; k++)
			{
				var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
				Vector2 drawPos = npc.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, npc.gfxOffY);
				Color color = npc.GetAlpha(lightColor) * (((npc.oldPos.Length - k) / (float)npc.oldPos.Length) / 2);
				spriteBatch.Draw(Main.npcTexture[npc.type], drawPos, new Microsoft.Xna.Framework.Rectangle?(npc.frame), color, npc.rotation, drawOrigin, npc.scale, effects, 0f);
			}
			return true;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 30; k++)
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.Plantera_Green, 2.5f * hitDirection, -2.5f, 0, Color.Purple, 0.3f);

			if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Pesterfly/Pesterfly5"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Pesterfly/Pesterfly6"), 1f);
			}
		}

		public override void NPCLoot()
		{
			if (Main.rand.NextBool(30))
				Item.NewItem(npc.getRect(), ModContent.ItemType<PesterflyCane>(), 1);
		}
	}
}
