using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Consumable;
using SpiritMod.NPCs.Boss.MoonWizard.Projectiles;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.MoonjellyEvent
{
	public class TinyLunazoa : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tiny Lunazoa");
			Main.npcFrameCount[NPC.type] = 5;
		}

		public override void SetDefaults()
		{
			NPC.width = 12;
			NPC.height = 20;
			NPC.damage = 0;
			NPC.defense = 0;
			NPC.lifeMax = 5;
			NPC.HitSound = SoundID.NPCHit25;
			NPC.DeathSound = SoundID.NPCDeath28;
            NPC.buffImmune[BuffID.Poisoned] = true;
            NPC.buffImmune[BuffID.Venom] = true;
            NPC.value = 60f;
			Main.npcCatchable[NPC.type] = true;
			NPC.catchItem = (short)ModContent.ItemType<TinyLunazoaItem>();
			NPC.knockBackResist = .45f;
			NPC.aiStyle = 64;
            NPC.scale = 1f;
			NPC.noGravity = true;
            NPC.noTileCollide = true;
			AIType = NPCID.Firefly;
		}

		public override bool? CanBeHitByProjectile(Projectile projectile) => !projectile.minion;

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 15; k++)
			{
				Dust d = Dust.NewDustPerfect(NPC.Center, 226, Vector2.One.RotatedByRandom(6.28f) * Main.rand.NextFloat(2), 0, default, 0.65f);
				d.noGravity = true;
			}

			if (NPC.life <= 0)
			{
				int p = Projectile.NewProjectile(NPC.Center.X, NPC.Center.Y, Main.rand.NextFloat(-1.1f, 1.1f), Main.rand.NextFloat(-1.1f, 1.1f), ModContent.ProjectileType<JellyfishOrbiter>(), NPCUtils.ToActualDamage(15, 1.5f), 0.0f, Main.myPlayer, 0.0f, (float)NPC.whoAmI);
				Main.projectile[p].scale = NPC.scale;
				Main.projectile[p].timeLeft = Main.rand.Next(55, 75);
				for (int k = 0; k < 10; k++)
				{
					Dust d = Dust.NewDustPerfect(NPC.Center, 226, Vector2.One.RotatedByRandom(6.28f) * Main.rand.NextFloat(3), 0, default, 0.75f);
					d.noGravity = true;
				}
			}
		}

		public override void AI()
        {
            NPC.rotation = (float)Math.Atan2(NPC.velocity.Y, NPC.velocity.X) + 1.57f;
            Lighting.AddLight(new Vector2(NPC.Center.X, NPC.Center.Y), 0.075f * 2, 0.231f * 2, 0.255f * 2);

        }
        public override void FindFrame(int frameHeight)
		{
			NPC.frameCounter += 0.15f;
			NPC.frameCounter %= Main.npcFrameCount[NPC.type];
			int frame = (int)NPC.frameCounter;
			NPC.frame.Y = frame * frameHeight;
		}

		public override void OnKill() => NPC.DropItem(ItemID.Gel);

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, NPC.Center - Main.screenPosition + new Vector2(0, NPC.gfxOffY), NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, SpriteEffects.None, 0);
            return false;
        }

		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) => Main.spriteBatch.Draw(Mod.GetTexture("NPCs/MoonjellyEvent/TinyLunazoa_Glow"), NPC.Center - Main.screenPosition + new Vector2(0, NPC.gfxOffY), NPC.frame, Color.White, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, SpriteEffects.None, 0);
	}
}
