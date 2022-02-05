using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.CascadeSet.Armor
{
	public class CascadeArmorPlayer : ModPlayer
	{
		internal float bubbleStrength = 0;
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
				damage = (int)(damage * (1 - (.75f * bubbleStrength)));
				PopBubble();
			}
		}

		public override void ModifyHitByProjectile(Projectile proj, ref int damage, ref bool crit)
		{
			if (bubbleStrength > 0f)
			{
				damage = (int)(damage * (1 - (.75f * bubbleStrength)));
				PopBubble();
			}
		}

		private void PopBubble()
		{
			int radius = (int)(300 * bubbleStrength);

			for (int i = 0; i < Main.maxNPCs; ++i)
			{
				NPC npc = Main.npc[i];
				if (npc.active && npc.CanBeChasedBy() && npc.DistanceSQ(player.Center) < radius * radius)
					npc.StrikeNPC((int)(30 * bubbleStrength), 3f * bubbleStrength, player.Center.X < npc.Center.X ? 1 : -1);
			}

			for (int i = 0; i < 8; ++i)
			{
				Vector2 vel = new Vector2(0, Main.rand.NextFloat(7f, 10f)).RotatedByRandom(MathHelper.Pi);
				Dust.NewDustPerfect(player.Center, Terraria.ID.DustID.t_Slime, vel, 0, default, Main.rand.NextFloat(1f, 2.5f));
			}

			Main.PlaySound(Terraria.ID.SoundID.Item86, player.Center);
			bubbleStrength = 0f;
		}

		public static readonly PlayerLayer Bubble = new PlayerLayer("SpiritMod", "CascadeBubbleShield", PlayerLayer.MiscEffectsBack, delegate (PlayerDrawInfo drawInfo)
		{
			if (drawInfo.shadow != 0f)
				return;

			Player drawPlayer = drawInfo.drawPlayer;
			Mod mod = SpiritMod.Instance;
			CascadeArmorPlayer modPlayer = drawPlayer.GetModPlayer<CascadeArmorPlayer>();

			if (modPlayer.bubbleStrength > 0f)
			{
				Texture2D texture = mod.GetTexture("Items/Sets/CascadeSet/Armor/BubbleShield");
				Texture2D outline = mod.GetTexture("Items/Sets/CascadeSet/Armor/BubbleShieldOutline");

				Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
				Vector2 drawPos = drawPlayer.Center - Main.screenPosition + new Vector2(0, drawPlayer.gfxOffY);

				float scale = modPlayer.bubbleStrength;
				float sin1 = 2.5f;
				float sinValue = (float)(Math.Cos((double)Main.GlobalTime % sin1 / sin1 * MathHelper.TwoPi) / (sin1 * 2) + (sin1 / 5));

				DrawData data = new DrawData(texture, drawPos, null, Color.White * scale * 0.125f, 0f, texture.Size() / 2f, scale * .4f + sinValue, SpriteEffects.None, 0);
				DrawData outlineData = new DrawData(outline, drawPos, null, Color.White * scale * 0.35f, 0f, texture.Size() / 2f, scale * .4f + sinValue, SpriteEffects.None, 0);

				Main.playerDrawData.Add(data);
				Main.playerDrawData.Add(outlineData);

			}
		});

		public override void ModifyDrawLayers(List<PlayerLayer> layers)
		{
			Bubble.visible = true;
			layers.Insert(0, Bubble);
			Bubble.visible = true;
			layers.Add(Bubble);
		}
	}
}
