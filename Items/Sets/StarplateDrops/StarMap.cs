using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.StarplateDrops
{
	public class StarMap : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Astral Map");
			Tooltip.SetDefault("Teleports you to the cursor location\n10 second cooldown");
			SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Sets/StarplateDrops/StarMap_Glow");
		}

		public override void SetDefaults()
		{
			item.damage = 0;
			item.noMelee = true;
			item.channel = true;
			item.rare = ItemRarityID.Pink;
			item.width = 42;
			item.height = 58;
			item.useTime = item.useAnimation = 12;
			item.UseSound = SoundID.Item8;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.expert = true;
			item.autoReuse = false;
			item.shootSpeed = 0f;
			item.noUseGraphic = true;
		}

		public override bool UseItem(Player player)
		{
			if (player.HasBuff(ModContent.BuffType<Buffs.AstralMapCooldown>()))
				return false;		
			else
				AstralTeleport(player);
			return true;
		}

		private void AstralTeleport(Player player)
		{
			if (!Collision.SolidCollision(Main.MouseWorld, player.width, player.height))
			{
				RunTeleport(player, Main.MouseWorld);
				player.AddBuff(ModContent.BuffType<Buffs.AstralMapCooldown>(), 600);
			}
		}

		private void RunTeleport(Player player, Vector2 pos)
		{
			player.Teleport(pos, 2, 0);
			player.velocity = Vector2.Zero;
			Main.PlaySound(SoundID.Item6, player.Center);
			DustHelper.DrawStar(player.Center, DustID.GoldCoin, pointAmount: 4, mainSize: 1.7425f, dustDensity: 6, dustSize: .65f, pointDepthMult: 3.6f, noGravity: true);
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Lighting.AddLight(item.position, 0.2f, .142f, .032f);

			Texture2D glow = ModContent.GetTexture(Texture + "_Glow");
			Texture2D outline = ModContent.GetTexture(Texture + "_Outline");
			float Timer = (float)Math.Sin(Main.GlobalTime * 3) / 2 + 0.5f;

			void DrawTex(Texture2D tex, float opacity, Vector2? offset = null) => spriteBatch.Draw(tex, item.Center + (offset ?? Vector2.Zero) - Main.screenPosition, null, Color.White * opacity, rotation, tex.Size() / 2, scale, SpriteEffects.None, 0);

			for (int i = 0; i < 6; i++)
			{
				Vector2 drawPos = Vector2.UnitX.RotatedBy((i / 6f) * MathHelper.TwoPi) * Timer * 6;
				DrawTex(glow, (1 - Timer) / 2, drawPos);
				DrawTex(outline, (1 - Timer) / 2, drawPos + (Vector2.UnitY * 2));
			}
			DrawTex(glow, (Timer / 5) + 0.5f);
			DrawTex(outline, (Timer / 5) + 0.5f, Vector2.UnitY * 2);
		}
	}
}
