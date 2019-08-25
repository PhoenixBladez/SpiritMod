using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace SpiritMod.Items.DonatorItems
{
	class CloakOfTheDesertKing : ModItem
	{
		public static readonly int _type;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cloak of the Desert King");
			Tooltip.SetDefault("Summons a killer bunny \n~Donator Item~");
		}
		public override void SetDefaults()
		{
            item.width = 26;
            item.height = 28;
            item.value = Item.sellPrice(0, 1, 50, 0);
            item.rare = 4;
            item.mana = 8;
            item.damage = 36;
            item.knockBack = 1;
            item.useStyle = 1;
            item.useTime = 30;
            item.useAnimation = 30;
            item.summon = true;
            item.noMelee = true;
            item.buffType = RabbitOfCaerbannogBuff._type;
			item.shoot = Projectiles.DonatorItems.RabbitOfCaerbannog._type;
            item.UseSound = SoundID.Item44;
        }
		public override void AddRecipes()
		{
			var recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.CrimsonCloak);
			recipe.AddIngredient(ItemID.SoulofNight, 10);
			recipe.AddTile(TileID.Loom);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
