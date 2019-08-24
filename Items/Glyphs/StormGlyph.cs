using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Glyphs
{
	public class StormGlyph : GlyphBase, Glowing
	{
		public static int _type;
		public static Microsoft.Xna.Framework.Graphics.Texture2D[] _textures;

		Microsoft.Xna.Framework.Graphics.Texture2D Glowing.Glowmask(out float bias)
		{
			bias = GLOW_BIAS;
			return _textures[1];
		}

		public override GlyphType Glyph => GlyphType.Storm;
		public override Microsoft.Xna.Framework.Graphics.Texture2D Overlay => _textures[2];
		public override Color Color => new Color { PackedValue = 0xc0b26d };
		public override string Effect => "Slicing Storms";
		public override string Addendum =>
			"Every third attack will cause a Slicing Gust\n"+
			"This Gust will knock enemies in the air and inflict Wind Burst\n"+
			"Enemies afflicted with Wind Burst receive amplified knockback";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Storm Glyph");
			Tooltip.SetDefault(
				"+3 Defense\n"+
				"Every third attack will cause a Slicing Gust\n"+
				"This Gust will knock enemies in the air and inflict Wind Burst\n"+
				"Enemies afflicted with Wind Burst receive amplified knockback");
		}


		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 28;
			item.value = Item.sellPrice(0, 2, 0, 0);
			item.rare = 2;

			item.maxStack = 999;
		}


		public static void WindBurst(MyPlayer player, Item item)
		{
			if (player.player.whoAmI != Main.myPlayer)
				return;

			if (player.stormStacks < 2)
			{
				player.stormStacks++;
				return;
			}
			player.stormStacks = 0;

			int damage = (int)(item.damage * player.player.GetDamageBoost());
			Vector2 position = player.player.MountedCenter;
			Vector2 velocity = Main.MouseWorld - position;
			float scale = 1 / velocity.Length();
			if (float.IsNaN(scale))
				velocity = new Vector2(player.player.direction, 0);
			else
				velocity *= scale;
			velocity *= 8f;
			Projectile.NewProjectile(position, velocity, Projectiles.SlicingGust._type, damage, 8f, Main.myPlayer);
		}
	}
}