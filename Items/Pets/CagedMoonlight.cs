using SpiritMod.Buffs.Pet;
using SpiritMod.Projectiles.Pet;
using SpiritMod.Projectiles.DonatorItems;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace SpiritMod.Items.Pets
{
	public class CagedMoonlight : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Caged Moonlight");
			Tooltip.SetDefault("Summons a faerie to protect you\n'Yout enemies shall be harassed by luminous lances'");
			SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Pets/CagedMoonlight_Glow");
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Lighting.AddLight(item.position, 0.08f, .38f, .28f);
			Texture2D texture;
			texture = Main.itemTexture[item.type];
			spriteBatch.Draw
			(
				ModContent.GetTexture("SpiritMod/Items/Pets/CagedMoonlight_Glow"),
				new Vector2
				(
					item.position.X - Main.screenPosition.X + item.width * 0.5f,
					item.position.Y - Main.screenPosition.Y + item.height - texture.Height * 0.5f + 2f
				),
				new Rectangle(0, 0, texture.Width, texture.Height),
				Color.White,
				rotation,
				texture.Size() * 0.5f,
				scale,
				SpriteEffects.None,
				0f
			);
		}
		public override void SetDefaults()
		{

			item.damage = 0;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.shoot = ModContent.ProjectileType<HarpyPet>();
			item.width = 16;
			item.height = 30;
			item.useAnimation = 20;
			item.useTime = 20;
			item.rare = ItemRarityID.Orange;
			item.noMelee = true;
			item.value = Item.sellPrice(0, 3, 50, 0);
			item.buffType = ModContent.BuffType<HarpyPetBuff>();
		}

		public override void UseStyle(Player player)
		{
			if (player.whoAmI == Main.myPlayer && player.itemTime == 0) {
				player.AddBuff(item.buffType, 3600, true);
			}
		}

		public override bool CanUseItem(Player player)
		{
			return player.miscEquips[1].IsAir;
		}

		public override void AddRecipes()
		{

			ModRecipe modRecipe = new ModRecipe(mod);
			modRecipe.AddIngredient(ModContent.ItemType<Items.Consumable.Potion.MoonJelly>());
			modRecipe.AddIngredient(ItemID.CrystalShard, 10);
			modRecipe.AddIngredient(ItemID.SoulofLight, 5);
			modRecipe.AddTile(TileID.MythrilAnvil);
			modRecipe.SetResult(this, 1);
			modRecipe.AddRecipe();
		}
	}
}