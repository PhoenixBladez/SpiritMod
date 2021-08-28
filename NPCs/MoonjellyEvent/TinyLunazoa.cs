using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Consumable;
using SpiritMod.NPCs.Boss.MoonWizard.Projectiles;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.MoonjellyEvent
{
	public class TinyLunazoa : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tiny Lunazoa");
			Main.npcFrameCount[npc.type] = 5;
		}

		public override void SetDefaults()
		{
			npc.width = 12;
			npc.height = 20;
			npc.damage = 0;
			npc.defense = 0;
			npc.lifeMax = 5;
			npc.HitSound = SoundID.NPCHit25;
			npc.DeathSound = SoundID.NPCDeath28;
            npc.buffImmune[BuffID.Poisoned] = true;
            npc.buffImmune[BuffID.Venom] = true;
            npc.value = 60f;
			Main.npcCatchable[npc.type] = true;
			npc.catchItem = (short)ModContent.ItemType<TinyLunazoaItem>();
			npc.knockBackResist = .45f;
			npc.aiStyle = 64;
            npc.scale = 1f;
			npc.noGravity = true;
            npc.noTileCollide = true;
			aiType = NPCID.Firefly;
		}
		public override bool? CanBeHitByProjectile(Projectile projectile) => !projectile.minion;

		public override void HitEffect(int hitDirection, double damage)
        {
            for (int k = 0; k < 15; k++)
            {
                Dust d = Dust.NewDustPerfect(npc.Center, 226, Vector2.One.RotatedByRandom(6.28f) * Main.rand.NextFloat(2), 0, default, 0.65f);
                d.noGravity = true;
            }
            if (npc.life <= 0) {
                int p = Projectile.NewProjectile(npc.Center.X, npc.Center.Y, Main.rand.NextFloat(-1.1f, 1.1f), Main.rand.NextFloat(-1.1f, 1.1f), ModContent.ProjectileType<JellyfishOrbiter>(), NPCUtils.ToActualDamage(7, 1.5f), 0.0f, Main.myPlayer, 0.0f, (float)npc.whoAmI);
                Main.projectile[p].scale = npc.scale;
                Main.projectile[p].timeLeft = Main.rand.Next(55, 75);
                for (int k = 0; k < 10; k++)
                {
                    Dust d = Dust.NewDustPerfect(npc.Center, 226, Vector2.One.RotatedByRandom(6.28f) * Main.rand.NextFloat(3), 0, default, 0.75f);
                    d.noGravity = true;
                }
            }
		}
		public override void AI()
        {
            npc.rotation = (float)Math.Atan2(npc.velocity.Y, npc.velocity.X) + 1.57f;
            Lighting.AddLight(new Vector2(npc.Center.X, npc.Center.Y), 0.075f * 2, 0.231f * 2, 0.255f * 2);

        }
        public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.15f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}

		public override void NPCLoot() => npc.DropItem(ItemID.Gel);

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame, drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, SpriteEffects.None, 0);
            return false;
        }

		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor) => Main.spriteBatch.Draw(mod.GetTexture("NPCs/MoonjellyEvent/TinyLunazoa_Glow"), npc.Center - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame, Color.White, npc.rotation, npc.frame.Size() / 2, npc.scale, SpriteEffects.None, 0);
	}
}
