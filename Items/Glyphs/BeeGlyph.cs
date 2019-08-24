using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Glyphs
{
	public class BeeGlyph : GlyphBase, Glowing
	{
		public static int _type;
		public static Microsoft.Xna.Framework.Graphics.Texture2D[] _textures;

		Microsoft.Xna.Framework.Graphics.Texture2D Glowing.Glowmask(out float bias)
		{
			bias = GLOW_BIAS;
			return _textures[1];
		}

		public override GlyphType Glyph => GlyphType.Bee;
		public override Microsoft.Xna.Framework.Graphics.Texture2D Overlay => _textures[2];
		public override Color Color => new Color { PackedValue = 0x5ca6eb };
		public override string Effect => "Honeyed";
		public override string Addendum =>
			"Drenches the user in honey\n"+
			"Attacks will release bees";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bee Glyph");
			Tooltip.SetDefault(
				"Drenches the user in honey\n"+
				"Attacks will release bees");
		}


		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 28;
			item.value = Item.sellPrice(0, 2, 0, 0);
			item.rare = 4;

			item.maxStack = 999;
		}


		public static void ReleaseBees(Player owner, NPC target, int damage)
		{
			if (owner.whoAmI != Main.myPlayer || !target.CanLeech())
				return;
			damage = owner.beeDamage((int)(damage * .4f));
			if (damage < 1)
				return;
			int count = Main.rand.Next(1, 2);
			for (int i = 0; i < count; i++)
			{
				Vector2 velocity = target.velocity;
				velocity.X += (float)Main.rand.Next(-35, 36) * 0.02f;
				velocity.Y += (float)Main.rand.Next(-35, 36) * 0.02f;
				Projectile.NewProjectile(target.Center, velocity, owner.beeType(), damage, owner.beeKB(0f), Main.myPlayer);
			}
		}
	}
}