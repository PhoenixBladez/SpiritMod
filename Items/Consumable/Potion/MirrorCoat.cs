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
			item.width = 20;
			item.height = 30;
			item.rare = ItemRarityID.Pink;
			item.maxStack = 30;

			item.useStyle = ItemUseStyleID.EatingUsing;
			item.useTime = item.useAnimation = 20;

			item.consumable = true;
			item.autoReuse = false;

			item.buffType = ModContent.BuffType<MirrorCoatBuff>();
			item.buffTime = 10800;

			item.UseSound = SoundID.Item3;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.CrystalShard, 2);
			recipe.AddIngredient(ItemID.Shiverthorn, 1);
			recipe.AddIngredient(ModContent.ItemType<MarbleChunk>(), 1);
			recipe.AddIngredient(ItemID.BottledWater, 1);
			recipe.AddTile(TileID.Bottles);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	public class MirrorCoatBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Mirror Coat");
			Description.SetDefault("Immunity to Stoned");
			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.buffImmune[BuffID.Stoned] = true;

		}
	}
}
