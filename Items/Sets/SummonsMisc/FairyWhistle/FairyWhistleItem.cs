using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.SummonsMisc.FairyWhistle
{
	public class FairyWhistleItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fairy Whistle");
			Tooltip.SetDefault("Calls a protective fairy that hovers around you");
		}

		public override void SetDefaults()
		{
			item.damage = 12;
			item.width = 22;
			item.height = 18;
			item.value = Item.sellPrice(0, 2, 0, 0);
			item.rare = ItemRarityID.White;
			item.mana = 12;
			item.knockBack = 2f;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.useTime = 30;
			item.useAnimation = 30;
			item.summon = true;
			item.noMelee = true;
			item.shoot = ModContent.ProjectileType<FairyMinion>();
			if(!Main.dedServ)
				item.UseSound = mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Whistle").WithPitchVariance(0.3f).WithVolume(1.2f);

			item.scale = 0.75f;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			player.itemRotation = 0;
			if (!Main.dedServ)
				Main.PlaySound(SoundID.Item44, player.Center);

			Projectile.NewProjectile(position, -Vector2.UnitY, type, damage, knockBack, player.whoAmI);
			return false;
		}

		public override Vector2? HoldoutOffset() => new Vector2(5, -2);

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddRecipeGroup(RecipeGroupID.Wood, 25);
			recipe.AddIngredient(ItemID.FallenStar, 3);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}