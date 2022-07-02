using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
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
			Item.damage = 10;
			Item.width = 22;
			Item.height = 18;
			Item.value = Item.sellPrice(0, 2, 0, 0);
			Item.rare = ItemRarityID.White;
			Item.mana = 12;
			Item.knockBack = 2f;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.DamageType = DamageClass.Summon;
			Item.noMelee = true;
			Item.shoot = ModContent.ProjectileType<FairyMinion>();
			if(!Main.dedServ)
				Item.UseSound = new SoundStyle("SpiritMod/Sounds/Whistle") with { PitchVariance = 0.3f, Volume = 1.2f };

			Item.scale = 0.75f;
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			player.itemRotation = 0;
			if (!Main.dedServ)
				SoundEngine.PlaySound(SoundID.Item44, player.Center);

			Projectile.NewProjectile(source, position, -Vector2.UnitY, type, damage, knockback, player.whoAmI);
			return false;
		}

		public override Vector2? HoldoutOffset() => new Vector2(5, -2);

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddRecipeGroup(RecipeGroupID.Wood, 25);
			recipe.AddIngredient(ItemID.FallenStar, 4);
			recipe.AddTile(TileID.WorkBenches);
			recipe.Register();
		}
	}
}