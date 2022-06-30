using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Glyphs
{
	public class StormGlyph : GlyphBase, IGlowing
	{
		public static Texture2D[] _textures;

		Texture2D IGlowing.Glowmask(out float bias)
		{
			bias = GLOW_BIAS;
			return _textures[1];
		}

		public override GlyphType Glyph => GlyphType.Storm;
		public override Texture2D Overlay => _textures[2];
		public override Color Color => new Color { PackedValue = 0xc0b26d };
		public override string Effect => "Slicing Storms";

		public override string Addendum =>
			"Every third attack will cause a Slicing Gust\n" +
			"This Gust will knock enemies in the air and inflict Wind Burst\n" +
			"Enemies afflicted with Wind Burst receive amplified knockback";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Storm Glyph");
			Tooltip.SetDefault(
				"+3 Defense\n" +
				"Every third attack will cause a Slicing Gust\n" +
				"This Gust will knock enemies in the air and inflict Wind Burst\n" +
				"Enemies afflicted with Wind Burst receive amplified knockback");
		}

		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 28;
			Item.value = Item.sellPrice(0, 2, 0, 0);
			Item.rare = ItemRarityID.Green;
			Item.maxStack = 999;
		}

		public static void WindBurst(MyPlayer player, Item item)
		{
			if (player.Player.whoAmI != Main.myPlayer)
				return;

			if (player.stormStacks < 2) {
				player.stormStacks++;
				return;
			}
			player.stormStacks = 0;

			int damage = (int)(item.damage * player.Player.GetDamageBoost());
			Vector2 position = player.Player.MountedCenter;
			Vector2 velocity = Main.MouseWorld - position;
			float scale = 1 / velocity.Length();

			if (float.IsNaN(scale))
				velocity = new Vector2(player.Player.direction, 0);
			else
				velocity *= scale;

			velocity *= 8f;
			Projectile.NewProjectile(player.Player.GetSource_ItemUse(item), position, velocity, ModContent.ProjectileType<SlicingGust>(), damage, 8f, Main.myPlayer);
		}
	}
}