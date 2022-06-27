using SpiritMod.Buffs.Pet;
using SpiritMod.Projectiles.Pet;
using SpiritMod.Projectiles.DonatorItems;
using Terraria;
using Terraria.GameContent;
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
			Tooltip.SetDefault("Summons a faerie to protect you\n'Your enemies shall be harassed by luminous lances'");
			SpiritGlowmask.AddGlowMask(Item.type, "SpiritMod/Items/Pets/CagedMoonlight_Glow");
        }
        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Lighting.AddLight(Item.position, 0.08f, .38f, .28f);
			Texture2D texture;
			texture = TextureAssets.Item[Item.type].Value;
			spriteBatch.Draw
			(
				ModContent.Request<Texture2D>("SpiritMod/Items/Pets/CagedMoonlight_Glow"),
				new Vector2
				(
					Item.position.X - Main.screenPosition.X + Item.width * 0.5f,
					Item.position.Y - Main.screenPosition.Y + Item.height - texture.Height * 0.5f + 2f
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

			Item.damage = 0;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.shoot = ModContent.ProjectileType<HarpyPet>();
			Item.width = 16;
			Item.height = 30;
			Item.useAnimation = 20;
			Item.useTime = 20;
			Item.rare = ItemRarityID.Orange;
			Item.noMelee = true;
			Item.value = Item.sellPrice(0, 3, 50, 0);
			Item.buffType = ModContent.BuffType<HarpyPetBuff>();
		}

		public override void UseStyle(Player player, Rectangle heldItemFrame)
		{
			if (player.whoAmI == Main.myPlayer && player.itemTime == 0) {
				player.AddBuff(Item.buffType, 3600, true);
			}
		}

		public override bool CanUseItem(Player player)
		{
			return player.miscEquips[1].IsAir;
		}

		public override void AddRecipes()
		{

			Recipe modRecipe = CreateRecipe(1);
			modRecipe.AddIngredient(ModContent.ItemType<Items.Consumable.Potion.MoonJelly>());
			modRecipe.AddIngredient(ItemID.CrystalShard, 10);
			modRecipe.AddIngredient(ItemID.SoulofLight, 5);
			modRecipe.AddTile(TileID.MythrilAnvil);
			modRecipe.Register();
		}
	}
}