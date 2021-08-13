using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.StarjinxEvent.Comets
{
	public class SmallComet : ModNPC
    {
		public override sealed bool CloneNewInstances => true;

		protected virtual string Size => "Small";
		protected virtual float BeamScale => 0.75f;

		ref NPC Parent => ref Main.npc[(int)npc.ai[0]];
		ref float TimerOffset => ref npc.ai[1];
		ref float SpinMomentum => ref npc.ai[2];
		ref float RotationOffset => ref npc.ai[3];

		public float initialDistance = 0f;

		public override void SetStaticDefaults() => DisplayName.SetDefault("Small Starjinx Comet");

		public override void SetDefaults()
        {
            npc.aiStyle = -1;
            npc.lifeMax = 10;
            npc.defense = 0;
            npc.value = 0f;
            aiType = 0;
            npc.knockBackResist = 0f;
            npc.width = 36;
            npc.height = 38;
            npc.damage = 0;
            npc.lavaImmune = false;
            npc.noTileCollide = false;
            npc.noGravity = true;
            npc.HitSound = SoundID.NPCHit7;
            npc.DeathSound = SoundID.Item89;
            npc.alpha = 255;
            for (int i = 0; i < BuffLoader.BuffCount; i++)
                npc.buffImmune[i] = true;
        }

        public override void ModifyHitByItem(Player player, Item item, ref int damage, ref float knockback, ref bool crit)
        {
            crit = false;
            if (player.HeldItem.pick > 0)
                damage = 5;
            else
                damage = 1;
        }

        public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            Player player = Main.player[projectile.owner];
            crit = false;
            if (player.HeldItem.pick > 0)
                damage = 5;
            else
                damage = 1;
        }

        float sinCounter;
        float sinIncrement = 0;

		private bool spinPlateau = false;

        public override void AI()
        {
            if (npc.alpha > 0)
                npc.alpha -= 10;
            else
                npc.alpha = 0;

            if (RotationOffset == 1000)
			{
				RotationOffset = npc.AngleTo(Parent.Center);
				initialDistance = npc.Distance(Parent.Center);
				npc.position = Parent.Center + new Vector2(0, initialDistance).RotatedBy(RotationOffset);
			}
			else
			{
				Vector2 pos = new Vector2(0, initialDistance * (float)(1 + Math.Sin(sinCounter) * 0.05f));
				npc.position = Parent.Center + pos.RotatedBy(RotationOffset);
				npc.rotation = RotationOffset;
				RotationOffset += SpinMomentum;
			}

            if (sinIncrement == 0)
                sinIncrement = Main.rand.NextFloat(0.025f, 0.035f);

			if (!spinPlateau)
			{
				SpinMomentum += 0.00005f;
				if (SpinMomentum > 0.005f && Main.rand.NextBool(10))
					spinPlateau = true;
			}

            sinCounter += sinIncrement;
            npc.TargetClosest(true);

            if (!Parent.active || Parent.type != ModContent.NPCType<StarjinxMeteorite>())
                npc.active = false;

            if (Main.rand.Next(200) == 0)
                Gore.NewGorePerfect(npc.Center, (Parent.Center - npc.Center) / 45, mod.GetGoreSlot("Gores/StarjinxGore"), 1);
        }

        public static void DrawDustLine(Vector2 pos1, Vector2 pos2)
        {
            Vector2 range = pos1 - pos2;
            Vector2 cometRange = range;
            cometRange.Normalize();
            cometRange *= 40;
            range -= cometRange;
            for (int i = 0; i < 5; i++)
            {
                float alpha = Main.rand.NextFloat();
                int chosenDust = Main.rand.Next(2) == 0 ? 159 : 164;
                Dust dust = Main.dust[Dust.NewDust(pos2 + (range * alpha), 0, 0, chosenDust)];
                dust.noGravity = true;
                dust.noLight = false;
                dust.velocity = range * 0.014f;
                dust.scale = 1.8f - (alpha * 1.5f);
                dust.fadeIn += dust.scale / 3 * 2;
            }
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            for (int k = 0; k < 7; k++)
            {
                Dust.NewDust(npc.position, npc.width, npc.height, 159, 2.5f * hitDirection, -2.5f, 0, default, 0.6f);
                Dust.NewDust(npc.position, npc.width, npc.height, 164, 2.5f * hitDirection, -2.5f, 0, default, 1.25f);
                Dust.NewDust(npc.position, npc.width, npc.height, 159, 2.5f * hitDirection, -2.5f, 0, default, 0.85f);
            }
            if (npc.life <= 0)
                Main.PlaySound(SoundID.Item14, npc.position);
        }

        public float Timer => Main.GlobalTime + TimerOffset;

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            var center = new Vector2(Main.npcTexture[npc.type].Width / 2, (Main.npcTexture[npc.type].Height / Main.npcFrameCount[npc.type] / 2));
            float cos = (float)Math.Cos(Timer % 2.4f / 2.4f * MathHelper.TwoPi) / 2f + 0.5f;

			DrawBeam(spriteBatch);

            SpriteEffects effect = (npc.spriteDirection == 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			Color baseCol = new Color(127 - npc.alpha, 127 - npc.alpha, 127 - npc.alpha, 0).MultiplyRGBA(Color.White);
            Color col = npc.GetAlpha(baseCol) * (1f - cos);

            Main.spriteBatch.Draw(Main.npcTexture[npc.type], npc.Center - Main.screenPosition, null, npc.GetAlpha(Color.White), npc.rotation, center, 1, SpriteEffects.None, 0f);

            for (int i = 0; i < 6; i++)
            {
                var vector29 = npc.Center + (i / 4 * MathHelper.TwoPi + npc.rotation).ToRotationVector2() * (4f * cos + 2f) - Main.screenPosition + new Vector2(0, npc.gfxOffY) - npc.velocity * i;
                spriteBatch.Draw(ModContent.GetTexture($"SpiritMod/NPCs/StarjinxEvent/Comets/{Size}CometGlow"), vector29, npc.frame, col, npc.rotation, npc.frame.Size() / 2f, npc.scale, effect, 0f);
            }
            return false;
        }

		private void DrawBeam(SpriteBatch b)
		{
			Texture2D beam = mod.GetTexture("Textures/Medusa_Ray");
			float rotation = npc.DirectionTo(Main.npc[(int)npc.ai[0]].Center).ToRotation();
			float fluctuate = (float)Math.Sin(Timer * 1.2f) / 6 + 0.125f;

			Color color = SpiritMod.StarjinxColor(Timer * 0.8f);
			color = Color.Lerp(color, Color.Transparent, fluctuate * 2);

			Rectangle rect = new Rectangle(0, 0, beam.Width, beam.Height);
			Vector2 scale = new Vector2((1 - fluctuate) * npc.Distance(Main.npc[(int)npc.ai[0]].Center) / beam.Width, 1) * 0.75f;
			Vector2 offset = new Vector2(100 * scale.X, 0).RotatedBy(rotation);
			b.Draw(beam, npc.Center - Main.screenPosition + offset / 2, new Rectangle?(rect), npc.GetAlpha(color), rotation, rect.Size() / 2, scale, SpriteEffects.None, 0);
		}

		public override bool CheckDead()
		{
			(Parent.modNPC as StarjinxMeteorite).updateCometOrder = true;
			return true;
		}

		public override bool CheckActive()
        {
			if (Parent.active && Parent.type == ModContent.NPCType<StarjinxMeteorite>())
			{
				return false;
			}
            return true;
        }
    }
}