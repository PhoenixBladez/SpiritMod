using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace SpiritMod.Items.Sets.AvianDrops
{
	internal class AvianHook : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Avian Hook");
			Tooltip.SetDefault("Striking tiles allows the player to float briefly");
		}

		public override void SetDefaults()
		{
			item.expert = true;
			item.CloneDefaults(ItemID.AmethystHook);
			item.shootSpeed = 14f;
			item.expert = true;
			item.shoot = ProjectileType<AvianHookProjectile>();
		}
	}

	internal class AvianHookProjectile : ModProjectile
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("${ProjectileName.GemHookAmethyst}");

		public override void SetDefaults() => projectile.CloneDefaults(ProjectileID.GemHookAmethyst);

		public override bool? CanUseGrapple(Player player)
		{
			int hooksOut = 0;
			for (int l = 0; l < Main.maxProjectiles; l++)
			{
				if (Main.projectile[l].active && Main.projectile[l].owner == Main.myPlayer && Main.projectile[l].type == projectile.type)
					hooksOut++;
			}
			return hooksOut <= 3;
		}

		// Amethyst Hook is 300, Static Hook is 600
		public override float GrappleRange() => 380f;

		public override void NumGrappleHooks(Player player, ref int numHooks) => numHooks = 3;

		public override void GrappleRetreatSpeed(Player player, ref float speed) => speed = 14f;

		public override void GrapplePullSpeed(Player player, ref float speed)
		{
			player.AddBuff(BuffID.Featherfall, 120);
			speed = 13;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = GetTexture("SpiritMod/Items/Sets/AvianDrops/AvianHookChain");
			Vector2 center = projectile.Center;

			Vector2 offset = Main.player[projectile.owner].MountedCenter - center;
			bool doDraw = true;

			if (projectile.Center.HasNaNs() || offset.HasNaNs())
				doDraw = false;

			while (doDraw)
			{
				if (offset.Length() < texture.Height + 1)
					doDraw = false;
				else
				{
					center += Vector2.Normalize(offset) * texture.Height;
					offset = Main.player[projectile.owner].MountedCenter - center;
					Color color = projectile.GetAlpha(Lighting.GetColor((int)center.X / 16, (int)(center.Y / 16.0)));
					Main.spriteBatch.Draw(texture, center - Main.screenPosition, null, color, (float)Math.Atan2(offset.Y, offset.X) - 1.57f, texture.Size() / 2, 1f, SpriteEffects.None, 0f);
				}
			}
		}
	}
}
