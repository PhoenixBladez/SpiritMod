using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpiritMod.NPCs
{
	public class ForgottenOne : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Forgotten One");
			Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.DesertGhoul];
            NPCID.Sets.TrailCacheLength[npc.type] = 3;
            NPCID.Sets.TrailingMode[npc.type] = 0;
        }

		public override void SetDefaults()
		{
			npc.width = 36;
			npc.height = 44;
			npc.damage = 300;
			npc.defense = 0;
			npc.lifeMax = 40;
			npc.HitSound = SoundID.NPCHit3;
			npc.DeathSound = SoundID.NPCDeath6;
			npc.value = 5000f;
			npc.knockBackResist = .60f;
			npc.aiStyle = 3;
			aiType = NPCID.DesertGhoul;
			aiType = NPCID.DesertGhoul;
			animationType = NPCID.DesertGhoul;
			npc.lavaImmune = true;
			npc.buffImmune[BuffID.CursedInferno] = true;
			npc.buffImmune[BuffID.ShadowFlame] = true;
			npc.buffImmune[BuffID.Confused] = true;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return spawnInfo.player.ZoneUnderworldHeight && NPC.downedBoss3 ? 0.08f : 0f;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
            for (int k = 0; k < 20; k++)
            {
                Dust.NewDust(npc.position, npc.width, npc.height, 6, 2.5f * hitDirection, -2.5f, 117, default(Color), .6f);
            }
            if (npc.life <= 0)
			{
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/One1"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/One2"), 1f);
                for (int k = 0; k < 20; k++)
                {
                    Dust.NewDust(npc.position, npc.width, npc.height, 6, 2.5f * hitDirection, -2.5f, 117, default(Color), .6f);
                }
            }
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Vector2 drawOrigin = new Vector2(Main.npcTexture[npc.type].Width * 0.5f, (npc.height * 0.5f));
            for (int k = 0; k < npc.oldPos.Length; k++)
            {
                var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
                Vector2 drawPos = npc.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, npc.gfxOffY);
                Color color = npc.GetAlpha(lightColor) * (float)(((float)(npc.oldPos.Length - k) / (float)npc.oldPos.Length) / 2);
                spriteBatch.Draw(Main.npcTexture[npc.type], drawPos, new Microsoft.Xna.Framework.Rectangle?(npc.frame), color, npc.rotation, drawOrigin, npc.scale, effects, 0f);
            }
            return true;
        }
        public override void NPCLoot()
		{
			for (int k = 0; k < 10; k++)
			{
				int dust = Dust.NewDust(npc.position, npc.width, npc.height, 6);
				Main.dust[dust].noGravity = true;
			}
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("CarvedRock"), Main.rand.Next(1) + 2);
		}
	}
}