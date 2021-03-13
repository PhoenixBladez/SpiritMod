using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.GameInput;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Input;

namespace SpiritMod.Items.Accessory.FlyingFishFin
{
	public class Flying_Fish_Fin : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Flying Fish Fin");
			Tooltip.SetDefault("Increases jump height\n5% increased movement speed");
		}
		public override void SetDefaults()
		{
			item.width = 26;
			item.height = 30;
			item.value = Item.sellPrice(silver: 10);
			item.rare = 1;
			item.accessory = true;
		}
		public override void UpdateEquip(Player player)
		{
			Player.jumpHeight = 70;
			Player.jumpSpeed = 3f;
			player.moveSpeed = player.moveSpeed + 0.05f;
		}	
	}
}