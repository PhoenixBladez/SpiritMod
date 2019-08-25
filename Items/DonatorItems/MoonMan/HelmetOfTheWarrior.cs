using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.DonatorItems.MoonMan
{
	[AutoloadEquip(EquipType.Head)]
	class HelmetOfTheWarrior : ModItem
	{
		public static readonly int _type;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Helmet of the Warrior");
			Tooltip.SetDefault("~Donator Item~");
		}

		public override void SetDefaults()
		{
			item.width = 16;
			item.height = 20;

			item.value = Item.sellPrice(0, 0, 50, 0);
			item.rare = 3;

			item.defense = 8;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ChestplateOfTheWarrior._type && legs.type == BootsOfTheWarrior._type;
		}

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "Warrior's Will\nslowly raises defense as health lowers";
			player.statDefense += 10 - 10 * player.statLife / player.statLifeMax2;
		}

		public override void AddRecipes()
		{
			var recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.IronBar, 8);
			recipe.AddIngredient(ItemID.SilverBar, 8);
			recipe.AddIngredient(ItemID.MeteoriteBar, 8);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
