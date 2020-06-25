using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpiritMod.NPCs
{
	public class Teratoma : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Putroma");
            NPCID.Sets.TrailCacheLength[npc.type] = 4;
            NPCID.Sets.TrailingMode[npc.type] = 0;
        }

		public override void SetDefaults()
		{
			npc.width = 34;
			npc.height = 36;
			npc.damage = 20;
			npc.defense = 5;
			npc.lifeMax = 65;
			npc.HitSound = SoundID.NPCHit18;
			npc.DeathSound = SoundID.NPCDeath21;
			npc.value = 110f;
			npc.knockBackResist = 0.45f;
			npc.aiStyle = 3;
			aiType = NPCID.WalkingAntlion;
		}
		public override void AI()
        {
            npc.rotation += .06f * npc.velocity.X;
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
            spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame,
                             lightColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
            {
                Vector2 drawOrigin = new Vector2(Main.npcTexture[npc.type].Width * 0.5f, (npc.height) * 0.5f);
                for (int k = 0; k < npc.oldPos.Length; k++)
                {
                    Vector2 drawPos = npc.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, npc.gfxOffY);
                    Color color = npc.GetAlpha(lightColor) * (float)(((float)(npc.oldPos.Length - k) / (float)npc.oldPos.Length) / 2);
                    spriteBatch.Draw(Main.npcTexture[npc.type], drawPos, new Microsoft.Xna.Framework.Rectangle?(npc.frame), color, npc.rotation, drawOrigin, npc.scale, effects, 0f);
                }
            }
            return false;
        }
        public override void HitEffect(int hitDirection, double damage)
        {
            int d = 22;
            int d1 = 184;
            for (int k = 0; k < 30; k++)
            {
                Dust.NewDust(npc.position, npc.width, npc.height, d, 2.5f * hitDirection, -2.5f, 0, Color.White, 0.7f);
                Dust.NewDust(npc.position, npc.width, npc.height, d1, 2.5f * hitDirection, -2.5f, 0, Color.White, .34f);
            }
            if (Main.rand.Next(2) == 0)
            {
                Main.PlaySound(3, npc.Center, 19);
                int tomaProj;
                tomaProj = Main.rand.Next(new int[] { mod.ProjectileType("Teratoma1"), mod.ProjectileType("Teratoma2"), mod.ProjectileType("Teratoma3") });
                bool expertMode = Main.expertMode;
                Main.PlaySound(SoundID.Item20, npc.Center);
                int damagenumber = expertMode ? 12 : 17;
                int p = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, Main.rand.Next(-8, 8), Main.rand.Next(-4, 0), tomaProj, damagenumber, 1, Main.myPlayer, 0, 0);
                Main.projectile[p].friendly = false;
                Main.projectile[p].hostile = true;
            }
			if (npc.life <= 0)
            {
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Teratoma/Teratoma1"), Main.rand.NextFloat(.85f, 1.1f));
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Teratoma/Teratoma2"), Main.rand.NextFloat(.85f, 1.1f));
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Teratoma/Teratoma3"), Main.rand.NextFloat(.85f, 1.1f));
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Teratoma/Teratoma4"), Main.rand.NextFloat(.85f, 1.1f));
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Teratoma/Teratoma5"), Main.rand.NextFloat(.85f, 1.1f));
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Teratoma/Teratoma6"), Main.rand.NextFloat(.85f, 1.1f));
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Teratoma/Teratoma7"), Main.rand.NextFloat(.85f, 1.1f));
                Main.PlaySound(29, npc.Center, 9);
            }
        }
    }
}
