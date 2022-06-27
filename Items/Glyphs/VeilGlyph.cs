using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Glyphs
{
	public class VeilGlyph : GlyphBase, IGlowing
	{
		public static Microsoft.Xna.Framework.Graphics.Texture2D[] _textures;

		Microsoft.Xna.Framework.Graphics.Texture2D IGlowing.Glowmask(out float bias)
		{
			bias = GLOW_BIAS;
			return _textures[1];
		}

		public override GlyphType Glyph => GlyphType.Veil;
		public override Microsoft.Xna.Framework.Graphics.Texture2D Overlay => _textures[2];
		public override Color Color => new Color { PackedValue = 0x89cc3e };
		public override string ItemType => "item";
		public override string Effect => "Shielding Veil";
		public override string Addendum =>
			"After 8 seconds of not taking damage you gain Phantom Veil\n" +
			"This Veil will increase life regen and block the next attack";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Veil Glyph");
			Tooltip.SetDefault(
				"+5% attack speed\n" +
				"After 8 seconds of not taking damage you gain Phantom Veil\n" +
				"This Veil will increase life regen and block the next attack");
		}


		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 28;
			Item.value = Item.sellPrice(0, 2, 0, 0);
			Item.rare = ItemRarityID.Orange;

			Item.maxStack = 999;
		}

		public override bool CanApply(Item item)
		{
			return item.IsWeapon() || item.useStyle > 0 && item.mountType < 0 && item.shoot <= ProjectileID.None;
		}


		public static void Block(Player player)
		{
			player.immune = true;
			player.immuneTime = 80;
			if (player.longInvince)
				player.immuneTime += 40;
			for (int i = 0; i < player.hurtCooldowns.Length; i++) {
				player.hurtCooldowns[i] = player.immuneTime;
			}
			Vector2 center = player.Center;
			Vector2 scale = new Vector2((player.width >> 1) + 4f, (player.height >> 1) + 4f);
			for (int i = 0; i < 50; i++) {
				Vector2 offset = Main.rand.NextVec2CircularEven(1, 1);
				Dust dust = Dust.NewDustPerfect(center + scale * offset, 110, 3 * offset, 100);
				dust.scale *= 2.5f;
				dust.noGravity = true;
			}
			if (player.whoAmI == Main.myPlayer && Main.netMode == NetmodeID.Server) {
				ModPacket packet = SpiritMod.Instance.GetPacket(MessageType.Dodge, 2);
				packet.Write((byte)player.whoAmI);
				packet.Write((byte)1);
				packet.Send();
			}
		}
	}
}