using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.GranitechSet.GranitechSword
{
	public class GranitechSaberItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Technobrand");
			Tooltip.SetDefault("Swinging creates additional holographic blades");
		}

		public override void SetDefaults()
		{
			Item.DamageType = DamageClass.Melee;
			Item.damage = 60;
			Item.Size = new Vector2(88, 92);
			Item.useTime = Item.useAnimation = 18;
			Item.reuseDelay = 20;
			Item.knockBack = 5f;
			Item.shootSpeed = 1;
			Item.noUseGraphic = true;
			Item.noMelee = true;
			Item.autoReuse = true;
			Item.channel = true;
			Item.rare = ItemRarityID.LightRed;
			Item.value = Item.sellPrice(gold: 2);
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.shoot = ProjectileID.PurificationPowder;
		}

		public override Vector2? HoldoutOffset() => new Vector2(-6, 0);

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			SoundEngine.PlaySound(new SoundStyle("SpiritMod/Sounds/SwordSlash1") with { PitchVariance = 0.6f, Volume = 0.8f }, player.Center);

			type = ModContent.ProjectileType<GranitechSaberProjectile>();
			GranitechSaberPlayer modplayer = player.GetModPlayer<GranitechSaberPlayer>();

			Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, player.whoAmI, modplayer.SwingDirection);
			modplayer.SwingDirection *= -1;

			return false;
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI) => GlowmaskUtils.DrawItemGlowMaskWorld(spriteBatch, Item, ModContent.Request<Texture2D>(Texture + "_glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value, rotation, scale);

		public override void AddRecipes()
		{
			var recipe = CreateRecipe(1);
			recipe.AddIngredient(ModContent.ItemType<GranitechMaterial>(), 10);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}

	//Storing info on items is buggy iirc with usage from inventory, so just using a modplayer instead
	internal class GranitechSaberPlayer : ModPlayer
	{
		public int SwingDirection { get; set; } = 1;

		public override void PostUpdate()
		{
			if (Player.HeldItem.type != ModContent.ItemType<GranitechSaberItem>()) //Reset when held item changes
				SwingDirection = 1;
		}
	}
}