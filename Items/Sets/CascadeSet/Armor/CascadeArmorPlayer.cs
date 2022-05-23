using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Dusts;
using SpiritMod.Players;
using SpiritMod.Utilities;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.CascadeSet.Armor
{
	public class CascadeArmorPlayer : ModPlayer
	{
		public const float MaxResist = 0.20f;

		internal float bubbleStrength = 0;
		internal float bubbleVisual = 0;
		internal bool setActive = false;

		public override void ResetEffects() => setActive = false;

		public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
		{
			if(setActive)
				bubbleStrength = MathHelper.Clamp(bubbleStrength += 0.125f, 0, 1);
		}

		public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
		{
			if(setActive)
				bubbleStrength = MathHelper.Clamp(bubbleStrength += 0.125f, 0, 1);
		}

		public override void ModifyHitByNPC(NPC npc, ref int damage, ref bool crit)
		{
			if (bubbleStrength > 0f)
			{
				damage = (int)(damage * (1 - (MaxResist * bubbleStrength)));
				PopBubble();
			}
		}

		public override void ModifyHitByProjectile(Projectile proj, ref int damage, ref bool crit)
		{
			if (bubbleStrength > 0f)
			{
				damage = (int)(damage * (1 - (MaxResist * bubbleStrength)));
				PopBubble();
			}
		}

		public override void PostUpdate()
		{
			if (setActive)
			{
				player.GetModPlayer<ExtraDrawOnPlayer>().DrawDict.Add(delegate (SpriteBatch sB) { DrawBubble(sB); }, ExtraDrawOnPlayer.DrawType.Additive);
				player.GetModPlayer<ExtraDrawOnPlayer>().DrawDict.Add(delegate (SpriteBatch sB) { DrawBubble(sB, true); }, ExtraDrawOnPlayer.DrawType.AlphaBlend);
			}

			else if (bubbleStrength > 0) //Kill bubble if armor piece unequipped
				PopBubble();

			if (bubbleVisual < bubbleStrength) //smooth transition for visual
				bubbleVisual = MathHelper.Lerp(bubbleVisual, bubbleStrength, 0.1f);

			bubbleVisual = Math.Min(bubbleVisual, bubbleStrength); //Cap visual strength at real strength value
		}

		private void PopBubble()
		{
			int radius = (int)(300 * bubbleStrength);

			for (int i = 0; i < 16; i++)
			{
				Vector2 vel = new Vector2(0, Main.rand.NextFloat(7f, 10f)).RotatedByRandom(MathHelper.Pi);
				Dust.NewDustPerfect(player.Center, ModContent.DustType<DarkWaterDust>(), vel, 0, default, Main.rand.NextFloat(.6f, 1.25f));

				Dust dust = Dust.NewDustPerfect(player.Center + new Vector2(Main.rand.Next(-50, 50), Main.rand.Next(0, 30)), ModContent.DustType<BubbleDust>(), vel, 0, Color.White, Main.rand.NextFloat(1f, 2.5f));
				dust.velocity = new Vector2(0, Main.rand.NextFloat(-2f, -.5f));
			}

			for (int i = 0; i < Main.maxNPCs; ++i)
			{
				NPC npc = Main.npc[i];
				if (npc.active && npc.CanBeChasedBy() && npc.DistanceSQ(player.Center) < radius * radius)
					npc.StrikeNPC(1, 3f * bubbleStrength, player.Center.X < npc.Center.X ? 1 : -1);
			}

			Main.PlaySound(new LegacySoundStyle(2, 54).WithPitchVariance(0.2f), player.Center);
			Main.PlaySound(new LegacySoundStyle(4, 3).WithPitchVariance(0.2f), player.Center);
			Main.PlaySound(new LegacySoundStyle(2, 112).WithPitchVariance(0.2f).WithVolume(.6f), player.Center);
			Main.PlaySound(SoundID.Item86, player.Center);

			bubbleStrength = 0f;
			bubbleVisual = 0f;
		}

		private void DrawBubble(SpriteBatch sB, bool outline = false)
		{
			if (player.shadow != 0f)
				return;

			Mod mod = SpiritMod.Instance;

			if (bubbleVisual > 0f)
			{
				string texturePath = "Items/Sets/CascadeSet/Armor/BubbleShield";
				if (outline)
					texturePath += "Outline";

				Texture2D texture = mod.GetTexture(texturePath);

				Vector2 drawPos = player.Center - Main.screenPosition + new Vector2(0, player.gfxOffY);

				float baseScale = EaseFunction.EaseCubicOut.Ease(bubbleVisual); //Quadratic out easing

				//Returns a value based on a sine function and the global timer, interpolated closer to 1 to reduce the total range of the sine function
				float GetSineTimer(float cyclesPerSecond, float lerpStrength = 0) => MathHelper.Lerp((float)Math.Sin(Main.GlobalTime / 6 * (MathHelper.TwoPi * cyclesPerSecond)), 1, lerpStrength);

				baseScale *= GetSineTimer(1f, 0.95f);
				float squishDelta = GetSineTimer(6) / 15;
				Vector2 scaleVector = new Vector2(1 + squishDelta, 1 - squishDelta) * baseScale;

				float opacity = outline ? 0.25f : 1f;
				Color lightColor = Lighting.GetColor((int)(player.Center.X / 16), (int)(player.Center.Y / 16));

				sB.Draw(texture, drawPos, null, lightColor * bubbleVisual * opacity, 0f, texture.Size() / 2f, scaleVector, SpriteEffects.None, 0);
			}
		}
	}
}
