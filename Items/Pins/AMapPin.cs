using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Pins
{
	// Abstract class for a map pin.
	// Contains all the code needed for a map pin item to place, move, or remove map pins.
	// ColorName is the most important: it defines which map pin the item is associated with
	public abstract class AMapPin : ModItem
	{
		// The name of this pin's color (capitalized, single word, please)
		// Affects both the visible name and where it expects the item/map texture to be
		public abstract string PinName { get; }

		// The color of this map pin's placement text
		public abstract Color TextColor { get; }

		// How the pin's name appears in the tooltip description (formats to lowercase by default)
		public virtual string DescName => PinName.ToLower();

		public override string Texture => $"SpiritMod/Items/Pins/Textures/Pin{PinName}Item";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault($"Wayfinder's Pin ({PinName})");
			Tooltip.SetDefault($"Places or moves a {DescName} map pin\nRight Click to delete pin");
		}

		public override void SetDefaults()
		{
			item.width = 32;
			item.height = 32;
			item.useTime = 25;
			item.useAnimation = 25;
			item.useStyle = ItemUseStyleID.HoldingUp;
			item.noMelee = true;
			item.knockBack = 5;
			item.value = Item.buyPrice(silver: 50);
			item.rare = ItemRarityID.Green;
			item.autoReuse = false;
			item.shootSpeed = 0f;
		}

		public override bool AltFunctionUse(Player player) => true;

		public override bool UseItem(Player player)
		{
			Main.PlaySound(SoundID.Dig, (int)player.position.X, (int)player.position.Y);
			string text;
			if (player.altFunctionUse != 2) {
				text = "Location Pinned";
				ModContent.GetInstance<PinWorld>().SetPin(PinName, player.Center / 16);
			}
			else {
				text = "Pin Removed";
				ModContent.GetInstance<PinWorld>().RemovePin(PinName);
			}
			CombatText.NewText(
				new Rectangle((int)player.position.X, (int)player.position.Y - 10, player.width, player.height),
				TextColor,
				text
			);
			return true;
		}
	}
}