using SpiritMod.Buffs.Potion;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using SpiritMod.Items.Sets.MarbleSet;

namespace SpiritMod.Items.Consumable.Potion
{
	public class MirrorCoat : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mirror Coat");
			Tooltip.SetDefault("Immunity to Stoned");
		}


		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 30;
			Item.rare = ItemRarityID.LightRed;
			Item.maxStack = 30;

			Item.useStyle = ItemUseStyleID.EatFood;
			Item.useTime = Item.useAnimation = 20;

			Item.consumable = true;
			Item.autoReuse = false;

			Item.buffType = ModContent.BuffType<MirrorCoatBuff>();
			Item.buffTime = 10800;

			Item.UseSound = SoundID.Item3;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.CrystalShard, 2);
			recipe.AddIngredient(ItemID.Shiverthorn, 1);
			recipe.AddIngredient(ModContent.ItemType<MarbleChunk>(), 1);
			recipe.AddIngredient(ItemID.BottledWater, 1);
			recipe.AddTile(TileID.Bottles);
			recipe.Register();
		}
	}
	public class MirrorCoatBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mirror Coat");
			Description.SetDefault("Immunity to Stoned");
			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(Player player, ref int buffIndex) => player.buffImmune[BuffID.Stoned] = true;
	}
}
